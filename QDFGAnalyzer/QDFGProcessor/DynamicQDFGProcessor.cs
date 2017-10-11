using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;

using de.tum.i22.monitor.malware.applications;
using de.tum.i22.monitor.malware.algorithms;
using de.tum.i22.monitor.malware.detection.algorithms;
using de.tum.i22.monitor.structures;

using NLog;
using MathNet.Numerics.Statistics;

using QDFGGraphManager.Model;
using QDFGGraphManager.EventLogProcessor;
using QDFGGraphManager.GraphBuilder;
using QDFGGraphManager.QDFGStatistics;
using QDFGGraphManager.QDFGProcessor;

namespace QDFGGraphManager.QDFGProcessor
{
    internal sealed class DynamicQDFGProcessor
    {
        private static Logger logger = LogManager.GetLogger("LogProcessor");
        private static Logger memoryLogger = LogManager.GetLogger("MemoryLog");

        System.Diagnostics.Process currentProcess = System.Diagnostics.Process.GetCurrentProcess();
        long totalBytesOfMemoryUsed = 0;

        /// <summary>
        /// Generate Dynamic Graph from Log File. Optionally generate Statistics and DataModel.
        /// </summary>
        /// <param name="logFile">Full Path to the Log File.</param>
        /// <param name="computeStats">true if Statistics and Model should be generated; otherwise, false.</param>
        internal void generateQDFG(String logFile, bool computeStats)
        {
            try
            {
                if (!Directory.Exists(Settings.outputDirectory + "\\" + Utility.IO.getFileNameFromPath(logFile)))
                {
                    // Create Output folder
                    DirectoryInfo di = Utility.IO.createOutputDirectory(Utility.IO.getFileNameFromPath(logFile));

                    if (di != null)
                    {
                        logger.Info(" --- Generating QDFG for Log <" + logFile + ">, Output Directory: \"" + di.Name + "\" --- ");

                        // Generate & Output static Graph
                        DFTGraph g = Utility.Graph.generateGraphFromCSV(logFile);
                        String serializedGraph = g.serializeGraphML();
                        Utility.Graph.writeGraphToFile(serializedGraph, di.Name + "\\StaticGraph_" + Utility.IO.getFileNameFromPath(logFile) + ".graphml");

                        // Generate Dynamic Graph from Log
                        processLogDynamic(logFile, di, computeStats);
                    }
                    else
                    {
                        logger.Fatal("Failed to Create output directory for log: " + Utility.IO.getFileNameFromPath(logFile));
                    }
                }

                else // skip previously processed log files
                {
                    logger.Fatal("Already processed. (" + Settings.outputDirectory + "\\" + Utility.IO.getFileNameFromPath(logFile) + ")");
                }
            }

            catch (Exception e)
            {
                logger.Fatal(logFile + ", failed to generate QDFG, " + e.Message);
            }
        }

        /// <summary>
        /// Creates Output Directories and verifies that log contains Nodes of interest.
        /// </summary>
        /// <returns>true if all is well; false otherwise.</returns>
        private bool preprocessLog(EventLog eventLog, String logPath, String outPath)
        {
            if (!eventLog.containsMalwareProcess() && !eventLog.containsGoodwareProcess())
            {
                logger.Fatal("Log Contains no Proccess of interest. Skipping [" + logPath + "]");
                return false;
            }

            else
            {
                try
                {
                    if (!Directory.Exists(Settings.outputDirectory + "\\" + outPath))
                        Directory.CreateDirectory(Settings.outputDirectory + "\\" + outPath);

                    eventLog.writeToFile(outPath + "\\originalLog.txt");

                    if (eventLog.containsGoodwareProcess())
                    {
                        logger.Info("Log Contains Goodware.");
                    }

                    if (eventLog.containsMalwareProcess())
                    {
                        logger.Info("Log Contains Malware.");
                    }

                    return true;
                }

                catch (IOException)
                {
                    logger.Fatal("Failed to Create Output Directory for dynamic Log: " + Settings.outputDirectory + "\\" + outPath);
                    return false;
                }
            }
        }

        /// <summary>
        /// Actual Implementation of the DynamicLogProcessor.
        /// Generate Dynamic Graph from Log File. Optionally generate Statistics and DataModel.
        /// </summary>
        /// <param name="logPath">Full Path to the Log File.</param>
        /// <param name="diOutput">DirectoryInfo of the output Directory.</param>
        /// <param name="computeStats">true if Statistics and Model should be generated; otherwise, false.</param>
        private void processLogDynamic(String logPath, DirectoryInfo diOutput, bool computeStats)
        {
            // Reset the external EventProcessor (LibDFT)
            EventProcessor ep = new EventProcessor();
            ep.freeResources();

            logger.Info("Processing Log for Dynamics: " + logPath);

            // Init EventLog, Statistics & Metrics
            EventLog eventLog = new EventLog(logPath);
            QDFGStatCollection stats = new QDFGStatCollection();
            CombinedMetrics metrics = new CombinedMetrics();

            // Subpath for the Output Directory of this particular dynamic Log
            String outPath = diOutput.Name + "\\dynamicLog";

            // Pre-process log file (Create Output Directories, Verify Log contains Nodes of interest)
            if (!preprocessLog(eventLog, logPath, outPath))    
                return;

            // Memory Check
            checkMemoryCount(logPath + ", Stage: init");

            // Create Transformer for Event Log; Used to perform operations on EventLog
            EventLogTransformer transformer = new EventLogTransformer(eventLog);
            
            // Fix inconsistent time stamps & transform the absolute time representation into a relative one
            logger.Info("Fixing Time Info in Log: " + logPath);
            transformer.fixDates();
            //eventLog.writeToFile(outPath + "\\sortedLog.txt");

            // Memory Check
            checkMemoryCount(logPath + ", Stage: Fix Dates");

            // Split Log into multiple Logs, each covering an increasing Interval (i.e. last Log is equivalent to original Log)
            logger.Info("Splitting Log additively: " + logPath);
            // List<EventLog> splitMergedLogs = transformer.splitAndMerge(Settings.eventSplitCount);
            List<EventLog> splitLogs = transformer.splitLogByTimeAdditive(Settings.timeStepMS);
            int numSamples = splitLogs.Count; // Number of Logs = Number of Intervals = Number of Samples for Features
            logger.Info("Split Done. Split into " + numSamples + " Logs.");

            // Memory Check
            checkMemoryCount(logPath + ", Stage: Split Logs");

            // Only start advanced Processing of Interval Logs, if we have a sufficient Number of Samples
            if (numSamples >= Settings.MIN_SAMPLES)
            {
                Dictionary<string, string> processedIDs = new Dictionary<string, string>();
                IEnumerable<DFTNode> verts = new List<DFTNode>();
                DFTGraph lastGraph = new DFTGraph();
                long start = -1;
                long end = splitLogs.Count + 10;
                bool monitoredFound = false;

                // For each Interval (partial) Log
                for (int i = 0; i < splitLogs.Count; i++)
                {
                    // Memory Check
                    checkMemoryCount(logPath + ", Stage: Partial Log " + i);

                    if (monitoredFound || splitLogs[i].containsMonitoredProcess())
                    {
                        logger.Debug("Contains Monitored " + i);
                        if (start == -1)
                        {
                            start = i;
                            monitoredFound = true;
                        }

                        logger.Info("Generating SubGraph " + i + "/" + splitLogs.Count);

                        // TODO Investigate possible "Bug" in EventProcessor
                        // At this point, somehow Graphs or Nodes/Edges that were generated in
                        // previous calls of this method can leak state into this call. 
                        // This can be verified by calling generateGraphFromString 
                        // multiple times for different Logs and directly outputting the graph given by 
                        // the EventProcessor.generateGraphFromString(<someLog>.ToString()) Method. 

                        // One can sometimes observe nodes in the graph that are not present in any event of "<someLog>".
                        // Further investigation shows that these nodes were however present in Logs previously
                        // processed by the EventProcessor. Therefore it seems like the EventProcessor
                        // is not working with a comepleteley fresh or cleared state when calling generateGraphFromString
                        // multiple times. Even though one would not expect that from a static Method. 
                        // The problem seems to be solvable by creating a new instance of EventProcessor and (possibly?)
                        // calling ep.freeRessources(). However this quickly leads to the next problem...
                        
                        //  The ressources (memory) used by the EventProcessor are seemingly not garbage collected,
                        // when the reference is nulled. This might be due to how Large Object Heap Collection works.
                        // Processing multiple logs in a single EventProcessor instance
                        // is not possible due to the "shared state" problem mentioned above. However, creating a new instance of
                        // EventProcessor every time we need to process a log, is not a good solution either, because
                        // memory from expired instances of EventProcessor is apparently not released.

                        // Therefore one is forced to restart the program for each Log (frees memory) and also 
                        // choose a time step that leads to a number of partial Logs that fit into memory. Smaller time step
                        // will result in more partial Logs and higher memory requirements (since we need to create 
                        // a new EventProcessor instance for each partial log.) Calling ep.freeRessources() for
                        // each partial Log does not seem to do anything, neither does nulling the EventProcessor object.)

                        // Debug: Output the Log for the current Interval
                        // File.WriteAllText(Settings.outputDirectory + "\\" + outPath + "\\input-" + i + ".txt", splitMergedLogs[i].ToString());

                        /* ????????????????????????????????????????????????????????????????????????????? */
                        /* ????????????????????????????????????????????????????????????????????????????? */
                        /* When exactly are we supposed to call this ? */
                        ep = new EventProcessor();
                        ep.freeResources();

                        // generate QDFG for this time instant (step / snapshot)
                        lastGraph = EventProcessor.generateGraphFromString(splitLogs[i].ToString());
                        DFTGraph workingGraph = new DFTGraph(lastGraph);
                        // Debug Output graphical representation of the Graph for the current Interval
                        //File.WriteAllText(Settings.outputDirectory + "\\" + outPath + "\\input-" + i + ".graphml",lastGraph.serializeGraphML());

                        // if stat collection is enabled, add stats for this time instant to the collection
                        if (computeStats)
                        {
                            metrics.decorate(workingGraph);
                            stats.addStats(workingGraph, i);
                            foreach (DFTNode n in workingGraph.Vertices)
                            {
                                if (n.nameFTR.Contains("malware"))
                                {
                                    logger.Debug("Node Features For " + n.nameFTR);
                                    foreach (DFTNodeFeature f in n.nodeFeatures)
                                    {
                                        logger.Debug(f.name);
                                    }
                                    logger.Debug("End of Node Features.");
                                }
                            }
                        }


                        verts = lastGraph.Vertices;

                        foreach (DFTNode n in verts)
                        {
                            if (!processedIDs.ContainsKey(n.nameFTR))
                            {
                                /* Somehow this information persists through separate method calls...
                                 * See advanced problem description above.
                                 * DFTNodeAttribute startTime = new DFTNodeAttribute();
                                 * startTime.name = "start";
                                 * startTime.value = i.ToString();
                                 * n.nodeProperties.Add(startTime);
                                 */

                                // Workaround: Pass a dictionary containing the relevant time information to the GEXF Engine
                                processedIDs.Add(n.nameFTR, i.ToString());
                                logger.Debug("ADDED NEW NODE: " + n.nameFTR + " -> " + i.ToString());
                            }
                        }

                        logger.Debug("IDs in Dict: " + processedIDs.Count);
                        //splitMergedLogs[i].writeToFile(outPath + "\\SplitAndMergedLog-" + i + ".txt");
                        //String gSerialized = g.serializeGraphML();
                        //Utility.writeGraphToFile(gSerialized, outPath + "\\SplitAndMergedLog-" + i + ".graphml");
                    }
                }
                GEXFWriter.writeGraph(lastGraph, outPath + "\\dynamic.gexf", true, start, end, processedIDs);
                File.WriteAllText(Settings.outputDirectory + "\\" + outPath + "\\final" + ".graphml", lastGraph.serializeGraphML());
                ep = new EventProcessor();
                ep.freeResources();
                logger.Warn("Wrote Dynamic Graph for: " + logPath);

                if (computeStats)
                {
                    String statsFile = Settings.outputDirectory + "\\" + diOutput.Name + "\\" + "stats.txt";
                    stats.writeStatsToFile(statsFile);
                    logger.Warn("Wrote Stats for: " + logPath + " to file: " + statsFile);
                }

                if (Settings.generateModel)
                {
                    ModelBuilder.addModelData(stats, diOutput.Name);
                }
            }

            else
            {
                logger.Fatal(logPath + ": Not Enough Active Samples (" + numSamples + ", MIN: " + Settings.MIN_SAMPLES + ")");
            }
        }

        private void printMetrics(DFTGraph g)
        {
            foreach (DFTNode n in g.Vertices)
            {
                if (n.nameFTR.Contains("malware") || n.nameFTR.Contains("goodware"))
                {
                    printMetrics(n);
                }
            }
        }

        private void printMetrics(DFTNode n)
        {
            Console.WriteLine("Features & Attributes for Node " + n.nameFTR);
            Console.WriteLine("    Features:");
            foreach (DFTNodeFeature f in n.nodeFeatures)
            {
                Console.WriteLine(f.name + ", " + f.value);
            }
            Console.WriteLine("    Attributes:");
            foreach (DFTNodeAttribute a in n.nodeProperties)
            {
                Console.WriteLine(a.name + ", " + a.value);
            }
        }

        private bool checkMemoryCount(String message)
        {
            this.currentProcess = System.Diagnostics.Process.GetCurrentProcess();
            this.totalBytesOfMemoryUsed = currentProcess.PrivateMemorySize64;
            memoryLogger.Info(message);
            double memUsedGB = (double)totalBytesOfMemoryUsed / 1000000000.0;
            memoryLogger.Info(message + ": Memory Used {0}, ~GB: {1}", totalBytesOfMemoryUsed, memUsedGB.ToString("0.00"));
            if (Settings.MEM_MAX_BYTES_ALLOWED >= totalBytesOfMemoryUsed)
                return true;

            memoryLogger.Fatal(message + ": Used memory exceeds MEM_MAX_BYTES_ALLOWED (" + Settings.MEM_MAX_BYTES_ALLOWED + ").");
            throw new AllowedMemoryExceededException("Used memory exceeds MEM_MAX_BYTES_ALLOWED (" + Settings.MEM_MAX_BYTES_ALLOWED + ").");
        }
    }
}
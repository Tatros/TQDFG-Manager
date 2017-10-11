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
    public sealed class StaticQDFGProcessor
    {
        private static Logger logger = LogManager.GetLogger("LogProcessor");
        private static Logger memoryLogger = LogManager.GetLogger("MemoryLog");

        System.Diagnostics.Process currentProcess = System.Diagnostics.Process.GetCurrentProcess();
        long totalBytesOfMemoryUsed = 0;

        /// <summary>
        /// Generate Static Graph from Log File. Optionally generate Statistics and DataModel.
        /// </summary>
        /// <param name="logFile">Full Path to the Log File.</param>
        /// <param name="computeStats">true if Statistics and Model should be generated; otherwise, false.</param>
        public void generateQDFG(String logFile, bool computeStats)
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

                        /*
                        // Generate & Output static Graph
                        DFTGraph g = Utility.Graph.generateGraphFromCSV(logFile);
                        String serializedGraph = g.serializeGraphML();
                        Utility.Graph.writeGraphToFile(serializedGraph, di.Name + "\\StaticGraph_" + Utility.IO.getFileNameFromPath(logFile) + ".graphml");
                        */

                        // Generate Dynamic Graph from Log
                        processLogStatic(logFile, di, computeStats);
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
                logger.Fatal(e.Message);
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
                    /*
                    if (!Directory.Exists(Settings.outputDirectory + "\\" + outPath))
                        Directory.CreateDirectory(Settings.outputDirectory + "\\" + outPath);

                    eventLog.writeToFile(outPath + "\\originalLog.txt");
                    */

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
                    logger.Fatal("Failed to Create Output Directory for static Log: " + Settings.outputDirectory + "\\" + outPath);
                    return false;
                }
            }
        }

        /// <summary>
        /// Actual Implementation of the StaticLogProcessor.
        /// Generate Graph from Log File. Optionally generate Statistics and DataModel.
        /// </summary>
        /// <param name="logPath">Full Path to the Log File.</param>
        /// <param name="diOutput">DirectoryInfo of the output Directory.</param>
        /// <param name="computeStats">true if Statistics and Model should be generated; otherwise, false.</param>
        private void processLogStatic(String logPath, DirectoryInfo diOutput, bool computeStats)
        {
            Settings.MIN_SAMPLES = 1; // Static Logs only consist of 1 sample (the final state of the graph)
            Settings.timeStepMS = -1; // Time is not considered for static graphs.

            // Reset the external EventProcessor (LibDFT)
            EventProcessor ep = new EventProcessor();
            ep.freeResources();

            logger.Info("Processing Log (Static): " + logPath);

            // Init EventLog, Statistics & Metrics
            EventLog eventLog = new EventLog(logPath);
            QDFGStatCollection stats = new QDFGStatCollection();
            CombinedMetrics metrics = new CombinedMetrics();

            // Subpath for the Output Directory of this particular dynamic Log
            String outPath = diOutput.Name + "\\staticLog";

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

            DFTGraph g = EventProcessor.generateGraphFromString(eventLog.ToString());
            Utility.Graph.writeGraphToFile(g.serializeGraphML(), diOutput.Name + "\\StaticGraphG_" + Utility.IO.getFileNameFromPath(logPath) + ".graphml");
            logger.Warn("Wrote Static Graph for: " + logPath);
            DFTGraph workingGraph = new DFTGraph(g);

            // if stat collection is enabled, add stats for this time instant to the collection
            if (computeStats)
            {
                logger.Info("Decorating graph: " + logPath);
                metrics.decorate(workingGraph);
                stats.addStats(workingGraph, 0);

                // logger.Info("Metrics for " + logPath);
                // printMetrics(workingGraph);
            }


            ep = new EventProcessor();
            ep.freeResources();

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
            logger.Info("Features & Attributes for Node " + n.nameFTR);
            logger.Info("    Features:");
            foreach (DFTNodeFeature f in n.nodeFeatures)
            {
                logger.Info(f.name + ", " + f.value);
            }
            logger.Info("    Attributes:");
            foreach (DFTNodeAttribute a in n.nodeProperties)
            {
                logger.Info(a.name + ", " + a.value);
            }
        }

        private bool checkMemoryCount(String message)
        {
            this.currentProcess = System.Diagnostics.Process.GetCurrentProcess();
            this.totalBytesOfMemoryUsed = currentProcess.PrivateMemorySize64;
            memoryLogger.Info(message);
            double memUsedGB = (double)totalBytesOfMemoryUsed / 1000000000.0;
            memoryLogger.Info("Memory Used: {0}, ~GB: {1}", totalBytesOfMemoryUsed, memUsedGB.ToString("0.00"));
            if (Settings.MEM_MAX_BYTES_ALLOWED >= totalBytesOfMemoryUsed)
                return true;

            memoryLogger.Fatal("Used memory exceeds MEM_MAX_BYTES_ALLOWED (" + Settings.MEM_MAX_BYTES_ALLOWED + ").");
            throw new AllowedMemoryExceededException("Used memory exceeds MEM_MAX_BYTES_ALLOWED (" + Settings.MEM_MAX_BYTES_ALLOWED + ").");
        }
    }
}
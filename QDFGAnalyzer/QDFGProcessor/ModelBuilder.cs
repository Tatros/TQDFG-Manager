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

namespace QDFGGraphManager.QDFGProcessor
{
    internal static class ModelBuilder
    {
        private static Logger logger = LogManager.GetLogger("ModelBuilder");

        public static void addModelData(QDFGStatCollection stats, String name)
        {
            Console.WriteLine("Model Data Name: " + name);
            // Classifier String for Node
            String classifier = "malware";

            // List of relevant Nodes (goodware/malware)
            List<DFTNode> relevantNodes = stats.getNodesByKeyword("malware");

            if (!(relevantNodes.Count > 0))
            {
                relevantNodes = stats.getNodesByKeyword("goodware");
                classifier = "goodware";
            }

            if (!(relevantNodes.Count > 0))
            {
                logger.Fatal(name + ": FAIL - Unable to add model data, Graph contains no malware or goodware Nodes.");
                return;
            }

            // List holding all Model Data, one for each relevant Node (i.e. goodware/malware processes)
            List<ModelData> modelData = new List<ModelData>();

            // Iterate over all relevant Nodes
            foreach (DFTNode n in relevantNodes)
            {
                if (n.typeEnum == DFTNodeType.PROCESS) // Node must be a process
                {
                    // Compute the Model Data for this Node
                    ModelData m = computeModelDataForNode(n, stats);

                    if (m != null)
                    {
                        // Add Model Data to the List
                        modelData.Add(m);
                        logger.Debug(name + ": OK. Model Data added.");
                        // DEBUG Output
                        // Console.WriteLine(m.getStatisticsReadableString());
                        // printMetrics(n);
                    }
                }
            }

            if (modelData.Count > 0)
            {
                // Path of the Model Data File
                
                String modelDataPath = Path.Combine(Settings.outputDirectory, name, Settings.ModelDataFile); //Settings.outputDirectory + "\\" + name + "\\" + Settings.ModelDataFile;
                String timeDataPath = Path.Combine(Settings.outputDirectory, name, Settings.TimeDataFile); //Settings.outputDirectory + "\\" + name + + "\\" + Settings.TimeDataFile;
                

                /*
                int count = 2;
                while (File.Exists(timeDataPath))
                {
                    Console.WriteLine("!!!!   FILE EXISTS: " + timeDataPath);
                    timeDataPath = Settings.outputDirectory + "\\" + "TimeData" + count + ".csv";
                    count++;
                }*/

                // logger.Fatal("Path: " + timeDataPath);
                try
                {
                    StringBuilder mdString = new StringBuilder();
                    StringBuilder tdString = new StringBuilder();

                    // If Model Data File does not exist, create it and add Header Line
                    if (!File.Exists(modelDataPath))
                    {
                        File.AppendAllText(modelDataPath, modelData.First().getHeaderString(Settings.MODEL_DATA_TYPE) + "\r\n");
                    }

                    // Construct Model Data String
                    foreach (ModelData m in modelData)
                    {
                        mdString.AppendLine(m.getModelDataString(Settings.MODEL_DATA_TYPE, classifier));

                        // Construct Time Data String
                        String line = TimeData.getStringTimeData(Settings.TIME_DATA_TYPE, m);
                        tdString.AppendLine(line);
                        //logger.Fatal(line);
                    }

                    // Append the constructed String to Model Data File
                    File.AppendAllText(modelDataPath, mdString.ToString());
                    File.AppendAllText(timeDataPath, tdString.ToString());
                }

                catch (ArgumentException e)
                {
                    logger.Fatal(e.Message);
                }

                catch (IOException e)
                {
                    logger.Fatal(e.Message);
                }
            }

            else
            {
                logger.Fatal(name + ": FAIL - no malware or goodware processes or insufficient samples.");
            }
        }

        // Construct Model Data from Node and Statistics
        public static ModelData computeModelDataForNode(DFTNode n, QDFGStatCollection stats)
        {
            // The Model Data Object
            ModelData modelData = null;

            // Lists holding certain Statistics for each time instance (sampled data)
            Dictionary<String, List<Double>> timedFeatureList = new Dictionary<String, List<Double>>();

            // Obtain the Collection of Vertex Statistics for this Node
            VertexStatCollection nodeStats = stats.getVertexStatCollectionByID(n.node_id);

            if (nodeStats != null) // Make sure we were able to obtain the Vertex Statistics for this Node
            {
                // Obtain Vertex Stats (of this Node), for all recorded Time Instances
                Dictionary<int, VertexStats> vertexStatsByTime = nodeStats.getAllStats();

                // Iterate over all recorded Time Instances
                foreach (VertexStats vs in vertexStatsByTime.Values)
                {
                    // Iterate over all Features within the FeatureSet of this Vertex Stats Object
                    foreach (Feature feature in vs.FeatureSet.Values)
                    {
                        /*
                        if (feature.Value >= 0) // Filter negative feature Values
                        {*/
                        // Get the assigned List for this feature
                        if (timedFeatureList.ContainsKey(feature.Name))
                        {
                            // Add Feature Value for this time Instance to the List
                            timedFeatureList[feature.Name].Add(feature.Value);
                        }

                        else // No List found for this feature
                        {
                            // Create new List for this Feature
                            timedFeatureList.Add(feature.Name, new List<Double>());
                            // Add Feature Value for this time Instance to the List
                            timedFeatureList[feature.Name].Add(feature.Value);
                        }
                        //}
                    }
                }

                // Create the Model Data Object using the time sampled Data
                modelData = new ModelData(n.nameFTR, timedFeatureList);
            }

            else // nodeStats == null
            {
                logger.Fatal("Unable to obtain Vertex Statistics Collection for Node " + n.nameFTR + "; Therefore, no Model Data was produced.");
                return null;
            }

            // Evaluate Model Data Constraints
            foreach (KeyValuePair<String, List<Double>> kvp in modelData.TimedFeatureCollection)
            {
                if (kvp.Value.Count < Settings.MIN_SAMPLES)
                {
                    logger.Fatal(modelData.Name + ": Model Data for does not contain enough samples for Statistic <" + kvp.Key + ">. Samples: " + kvp.Value.Count + ", Minimum: " + Settings.MIN_SAMPLES);
                    return null;
                }
            }

            return modelData;
        }
    }
}

using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using de.tum.i22.monitor.malware.applications;
using de.tum.i22.monitor.structures;

using NLog;

namespace QDFGGraphManager.Utility
{
    internal static class Graph
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();




        public static void writeGraphToFile(String serializedGraph, String filename)
        {
            File.WriteAllText(Settings.outputDirectory + "\\" + filename, serializedGraph);
        }

        public static DFTGraph generateGraphFromCSV(String pathToEventLogCSV)
        {
            if (File.Exists(pathToEventLogCSV))
            {
                //Console.WriteLine("Generate Graph: " + pathToEventLogCSV);
                DFTGraph graph = EventProcessor.generateGraphFromCSV(pathToEventLogCSV);
                return graph;
            }
            else
            {
                throw new ArgumentException("Unable to generate Graph, No Event Log found at: " + pathToEventLogCSV);
            }
            
        }

        public static DFTGraph generateGraphFromString(String log)
        {
            DFTGraph graph = EventProcessor.generateGraphFromString(log);
            return graph;
        }

        public static String getNodeString(DFTNode n)
        {
            return ("ID <" + n.node_id + ">, Type <" + n._nodeType + ">, Name <" + n.nameFTR + ">");
        }

        public static String getNodeNameSanitized(DFTNode n)
        {
            String name = n.nameFTR;
            
            int index = name.LastIndexOf('/');
            if (index > 0)
            {
                name = n.nameFTR.Substring(index);
            }
            else
            {
                name = n.nameFTR.Substring(1);
            }


            if (name.Length > 1)
            {
                name = name.Replace('>', '_');
                name = name.Replace('/', '_');

                name = n._nodeType + name;
                logger.Debug("Generated Name:  " + name);
                return name;
            }
            return "";
        }

        public static DFTNode getPythonNode(DFTGraph g)
        {
            DFTNode root = g.getNodeByName("P>python.exe");
            return root;
        }

        public static List<DFTNode> getMalwareNodes(DFTGraph g)
        {
            return getNodesByString(g, "malware");
        }

        public static List<DFTNode> getGoodwareNodes(DFTGraph g)
        {
            return getNodesByString(g, "goodware");
        }

        private static List<DFTNode> getNodesByString(DFTGraph g, String value)
        {
            List<DFTNode> nodes = new List<DFTNode>();
            foreach (DFTNode n in g.Vertices)
            {
                if (n.nameFTR.Contains(value))
                {
                    nodes.Add(n);
                }
            }

            return nodes;
        }

        public static DFTGraph getReachableGraphPython(DFTGraph g)
        {
            DFTNode root = g.getNodeByName("P>python.exe");
            return g.getReachabilityGraphFWD(root);
        }

        public static List<DFTEdge> getAllIncomingEdges(DFTGraph g, DFTNode n)
        {
            List<DFTEdge> inEdges = g.InEdges(n).ToList();
            return inEdges;
        }

        public static List<DFTEdge> getAllOutgoingEdges(DFTGraph g, DFTNode n)
        {
            List<DFTEdge> outEdges = g.OutEdges(n).ToList();
            return outEdges;
        }



        public static DFTGraph getReachableGraphFWD(DFTGraph g, DFTNode n)
        {
            return g.getReachabilityGraphFWD(n);
        }

        public static DFTGraph getReachableGraphBWD(DFTGraph g, DFTNode n, DFTNodeType type)
        {
            return g.getReachabilityGraphBWD(n, false, type);
        }

        public static void printAllNodesToDebug(DFTGraph g)
        {
            foreach (DFTNode node in g.Vertices)
            {
                logger.Debug(Utility.Graph.getNodeString(node));
            }
        }

        public static void printAllNodesToInfo(DFTGraph g)
        {
            foreach (DFTNode node in g.Vertices)
            {
                logger.Info(Utility.Graph.getNodeString(node));
            }
        }
    }
}

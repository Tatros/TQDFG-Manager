using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using de.tum.i22.monitor.malware.applications;
using de.tum.i22.monitor.structures;

using NLog;

namespace QDFGGraphManager.QDFGStatistics
{
    internal sealed class QDFGStatCollection
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        // maps node id's to their stat collection objects
        private Dictionary<int, VertexStatCollection> vertexStatCollection;
        private List<DFTNode> nodes;

        public QDFGStatCollection()
        {
            vertexStatCollection = new Dictionary<int, VertexStatCollection>();
            nodes = new List<DFTNode>();
        }

        public void addStats(DFTGraph g, int time)
        {
            // iterate over all nodes in the (partial) graph
            foreach (DFTNode n in g.Vertices)
            {

                // if node is not yet recognized, create it with its initial stats
                if (!vertexStatCollection.ContainsKey(n.node_id))
                {
                    vertexStatCollection.Add(n.node_id, new VertexStatCollection(n.node_id, n.nameFTR, n.typeEnum, getVertexStatsFromNode(g, n), time));
                    nodes.Add(n);
                }


                else // if node is already recognized, obtain its stats collection 
                     // and update it with a new VertexStats object for this time instance
                {
                    VertexStatCollection nodeStatCollection = vertexStatCollection[n.node_id];

                    if (nodeStatCollection.getVertexStatsForTime(time) != null)
                    {
                        logger.Fatal("Time Stats for Node ID " + n.node_id + " (" + n.nameFTR + ") already exist at time " + time);
                    }
                    
                    else
                    {
                        nodeStatCollection.addVertexStatsForTime(time, getVertexStatsFromNode(g, n));
                    }
                }
            }
        }

        private VertexStats getVertexStatsFromNode(DFTGraph g, DFTNode n)
        {
            /* In Degree */
            int inDeg = g.InDegree(n);
            /* Out Degree */
            int outDeg = g.OutDegree(n);

            /* Size of Edge Data */
            long sizeSumIncoming;
            long sizeSumOutgoing;
            computeStatsDataSize(g, n, out sizeSumIncoming, out sizeSumOutgoing);

            /* Centrality */
            int centralityCount;
            int totalNodes = g.Vertices.Count();
            double centralityRelative;
            computeStatsCentrality(g, n, totalNodes, out centralityCount, out centralityRelative);

            /* Closeness Centrality */


            return new VertexStats(n.node_id, n.nameFTR, inDeg, outDeg, sizeSumIncoming, sizeSumOutgoing, centralityCount, centralityRelative, totalNodes, n.nodeFeatures);
        }

        private static void computeStatsCentrality(DFTGraph g, DFTNode n, int totalNodes, out int centralityCount, out double centralityRelative)
        {
            // Compute Centrality of Node
            // Centrality of node n: The number of nodes for which there exists a path p, such that p ends in node n,
            //                       in the directed graph, divided by the total number of nodes in the graph
            centralityCount = 0;
            foreach (DFTNode node in g.Vertices)
            {
                if (g.getConnectingEdges(node, n).Count > 0)
                {
                    // Console.WriteLine("There exists a path from node " + node.nameFTR + ", to " + n.nameFTR);
                    centralityCount++;
                }
            }
            centralityRelative = ((double)centralityCount / (double)totalNodes) * 100;
            // Console.WriteLine("Total Centrality Count for node " + n.nameFTR + ": " + centralityCount + ", Relative: " + centralityRelative + " (" + centralityCount + "/" + g.Vertices.Count() + ")");
        }

        private static void computeStatsDataSize(DFTGraph g, DFTNode n, out long sizeSumIncoming, out long sizeSumOutgoing)
        {
            // compute inData/outData (sum of weights of incoming/outgoing edges)
            // Console.WriteLine("Node: " + n.nameFTR + ", inDeg: " + g.InDegree(n).ToString());
            List<DFTEdge> predEdges = g.getPredecessorEdges(n);
            List<DFTEdge> succEdges = g.getSuccessorEdges(n);

            sizeSumIncoming = 0;
            double sizeSumIncomingRel = 0;
            sizeSumOutgoing = 0;
            double sizeSumOutgoingRel = 0;
            int i = 1;
            foreach (DFTEdge e in predEdges)
            {
                // Console.WriteLine("Pred (" + i + "/" + predEdges.Count + "): " + e._name + ", size: " + e._size + ", Relative: " + e._inSizePercentage + ",    (" + e.Source.nameFTR + "->" + e.Target.nameFTR + ")");
                sizeSumIncoming += e._size;
                sizeSumIncomingRel += e._inSizePercentage;
                i++;
            }

            i = 0;
            foreach (DFTEdge e in succEdges)
            {
                // Console.WriteLine("Succ (" + i + "/" + succEdges.Count + "): " + e._name + ", size: " + e._size + ", Relative: " + e._outSizePercentage + ",    (" + e.Source.nameFTR + "->" + e.Target.nameFTR + ")");
                sizeSumOutgoing += e._size;
                sizeSumOutgoingRel += e._outSizePercentage;
                i++;
            }

            // Console.WriteLine("Sum Incoming: " + sizeSumIncoming + ", Percentage: " + sizeSumIncomingRel);
            // Console.WriteLine("Sum Outgoing: " + sizeSumOutgoing + ", Percentage: " + sizeSumIncomingRel);
            // Console.WriteLine("\n");
        }

        public void writeStatsToFile(String outPath)
        {
            try
            {
                File.WriteAllText(outPath, this.ToString());
            }

            catch (DirectoryNotFoundException)
            {
                logger.Fatal("Stat Output Failed. Could not find output Directory: " + outPath);
            }
        }

        public VertexStatCollection getVertexStatCollectionByID(int nodeID)
        {
            if (vertexStatCollection.ContainsKey(nodeID))
            {
                return vertexStatCollection[nodeID];
            }

            return null;
        }

        public VertexStatCollection getVertexStatCollectionByNodeName(String nodeName)
        {
            foreach (VertexStatCollection v in vertexStatCollection.Values)
            {
                if (v.NodeName == nodeName)
                {
                    return v;
                }
            }

            return null;
        }

        public List<DFTNode> getNodesByKeyword(String keyword)
        {
            List<DFTNode> nodesKWD = new List<DFTNode>();

            foreach (DFTNode n in Nodes)
            {
                if (n.nameFTR.Contains(keyword))
                {
                    nodesKWD.Add(n);
                }
            }

            return nodesKWD;
        }

        public bool isInCollection(String nodeName)
        {
            foreach (DFTNode n in Nodes)
            {
                if (n.nameFTR == nodeName)
                {
                    return true;
                }
            }

            return false;
        }

        public bool isInCollection(int nodeID)
        {
            foreach (DFTNode n in Nodes)
            {
                if (n.node_id == nodeID)
                {
                    return true;
                }
            }

            return false;
        }

        public List<DFTNode> Nodes
        {
            get { return this.nodes; }
        }

        public override string ToString()
        {
            // Construct Header
            StringBuilder header = new StringBuilder();
            header.Append("nodeID,nodeName,time,");
            
            // List of Feature Names
            List<String> featureNames = this.vertexStatCollection.Values.First().getAllStats().Values.First().FeatureSet.Keys.ToList();
            
            // Iterate over Names and append to Header
            for (int i = 0; i < (featureNames.Count - 1); i++)
            {
                header.Append(featureNames[i] + ",");
            }

            // Append Last Name
            header.Append(featureNames.Last() + "\r\n");

            // Construct Value Set
            StringBuilder values = new StringBuilder();
            foreach (int nodeID in this.vertexStatCollection.Keys)
            {
                VertexStatCollection nodeStatCollection = vertexStatCollection[nodeID];
                Dictionary<int, VertexStats> stats = nodeStatCollection.getAllStats();

                foreach (KeyValuePair<int, VertexStats> statPair in stats)
                {
                    values.Append(nodeID + "," + statPair.Value.NodeName + "," + statPair.Key + ",");

                    foreach (Feature feature in statPair.Value.FeatureSet.Values)
                    {
                        if (feature != statPair.Value.FeatureSet.Values.Last())
                            values.Append(feature.ValueAsString + ",");
                        else
                            values.Append(feature.ValueAsString + "\r\n");
                    }
                }
            }

            return header.ToString() + values.ToString();
        }
    }
}

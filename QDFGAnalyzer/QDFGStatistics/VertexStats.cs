using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using de.tum.i22.monitor.structures;

using NLog;

namespace QDFGGraphManager.QDFGStatistics
{
    internal sealed class VertexStats
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        private int nodeID;
        private String name;
        private int numFeatures;

        // Feature Data
        private double inSizeRatioProcessNodeFTR = -1.0;
        private double inSizeRatioFileNodeFTR = -1.0;
        private double inSizeRatioSocketNodeFTR = -1.0;
        private double outSizeRatioProcessNodeFTR = -1.0;
        private double outSizeRatioFileNodeFTR = -1.0;
        private double outSizeRatioSocketNodeFTR = -1.0;
        private double inEdgeCompressionNodeFTR = -1.0;
        private double outEdgeCompressionNodeFTR = -1.0;
        private double sizeStdDevNodeFTR = -1.0;
        private double sizeEntNodeFTR = -1.0;
        private double boundedPageRankNormalizedGraphFTR = -1.0;
        private double boundClosenessSizeCentralityNormalizedGraphFTR = -1.0;
        private double boundBetweennessSizeCentralityGraphFTR = -1.0;
        private double pageRankNormalizedGraphFTR = -1.0;
        private double closenessSizeCentralityNormalizedGraphFTR = -1.0;
        private double betweennessSizeCentralityGraphFTR = -1.0;

        Dictionary<String, Feature> featureSet;

        public VertexStats(int nodeID, String nodeName, int inDegree, int outDegree, long inData, long outData, int centralityCount, double centralityRelative, int numNodes, List<DFTNodeFeature> features)
        {
            this.name = nodeName;
            this.nodeID = nodeID;
            this.numFeatures = features.Count;

            featureSet = new Dictionary<string, Feature>();

            if (Settings.USE_OWN_STATS)
            {
                // this.inDeg = inDegree;
                featureSet.Add("inDeg", new Feature("inDeg", inDegree));
                // this.outDeg = outDegree;
                featureSet.Add("outDeg", new Feature("outDeg", outDegree));
                // this.inData = inData;
                featureSet.Add("inData", new Feature("inData", inData));
                // this.outData = outData;
                featureSet.Add("outData", new Feature("outData", outData));
                // this.centralityCount = centralityCount;
                featureSet.Add("centrality", new Feature("centrality", centralityCount));
                // this.centralityRelative = centralityRelative;
                featureSet.Add("centralityRelative", new Feature("centralityRelative", centralityRelative));
                // this.numNodes = numNodes;
                featureSet.Add("numNodes", new Feature("numNodes", numNodes));
            }

            parseFeatures(features);
        }

        public Feature getFeature(String featureName)
        {
            if (featureSet.ContainsKey(featureName))
            {
                return featureSet[featureName];
            }

            throw new ArgumentException("No Feature with name <" + featureName + "> exists for Vertex Stats of Node <" + this.name + ">.");
        }

        public Dictionary<String, Feature> FeatureSet
        {
            get { return this.featureSet; }
        }

        private void parseFeatures(List<DFTNodeFeature> features)
        {
            foreach (DFTNodeFeature feature in features)
            {
                switch (feature.name)
                {
                    case "inSizeRatioProcessNodeFTR":
                    {
                        inSizeRatioProcessNodeFTR = Double.Parse(feature.value.ToString());
                        featureSet.Add("inSizeRatioProcessNode", new Feature("inSizeRatioProcessNode", Double.Parse(feature.value.ToString())));
                        break;
                    }
                    case "inSizeRatioFileNodeFTR":
                    {
                        inSizeRatioFileNodeFTR = Double.Parse(feature.value.ToString());
                        featureSet.Add("inSizeRatioFileNode", new Feature("inSizeRatioFileNode", Double.Parse(feature.value.ToString())));
                        break;
                    }
                    case "inSizeRatioSocketNodeFTR":
                    {
                        inSizeRatioSocketNodeFTR = Double.Parse(feature.value.ToString());
                        featureSet.Add("inSizeRatioSocketNode", new Feature("inSizeRatioSocketNode", Double.Parse(feature.value.ToString())));
                        break;
                    }
                    case "outSizeRatioProcessNodeFTR":
                    {
                        outSizeRatioProcessNodeFTR = Double.Parse(feature.value.ToString());
                        featureSet.Add("outSizeRatioProcessNode", new Feature("outSizeRatioProcessNode", Double.Parse(feature.value.ToString())));
                        break;
                    }
                    case "outSizeRatioFileNodeFTR":
                    {
                        outSizeRatioFileNodeFTR = Double.Parse(feature.value.ToString());
                        featureSet.Add("outSizeRatioFileNode", new Feature("outSizeRatioFileNode", Double.Parse(feature.value.ToString())));
                        break;
                    }
                    case "outSizeRatioSocketNodeFTR":
                    {
                        outSizeRatioSocketNodeFTR = Double.Parse(feature.value.ToString());
                        featureSet.Add("outSizeRatioSocketNode", new Feature("outSizeRatioSocketNode", Double.Parse(feature.value.ToString())));
                        break;
                    }
                    case "inEdgeCompressionNodeFTR":
                    {
                        inEdgeCompressionNodeFTR = Double.Parse(feature.value.ToString());
                        featureSet.Add("inEdgeCompression", new Feature("inEdgeCompression", Double.Parse(feature.value.ToString())));
                        break;
                    }
                    case "outEdgeCompressionNodeFTR":
                    {
                        outEdgeCompressionNodeFTR = Double.Parse(feature.value.ToString());
                        featureSet.Add("outEdgeCompression", new Feature("outEdgeCompression", Double.Parse(feature.value.ToString())));
                        break;
                    }
                    case "sizeStdDevNodeFTR":
                    {
                        sizeStdDevNodeFTR = Double.Parse(feature.value.ToString());
                        featureSet.Add("sizeStdDev", new Feature("sizeStdDev", Double.Parse(feature.value.ToString())));
                        break;
                    }
                    case "sizeEntNodeFTR":
                    {
                        sizeEntNodeFTR = Double.Parse(feature.value.ToString());
                        featureSet.Add("sizeEnt", new Feature("sizeEnt", Double.Parse(feature.value.ToString())));
                        break;
                    }
                    case "boundedPageRankNormalizedGraphFTR":
                    {
                        boundedPageRankNormalizedGraphFTR = Double.Parse(feature.value.ToString());
                        featureSet.Add("boundedPageRankNormalized", new Feature("boundedPageRankNormalized", Double.Parse(feature.value.ToString())));
                        break;
                    }
                    case "boundClosenessSizeCentralityNormalizedGraphFTR":
                    {
                        boundClosenessSizeCentralityNormalizedGraphFTR = Double.Parse(feature.value.ToString());
                        featureSet.Add("boundClosenessSizeCentralityNormalized", new Feature("boundClosenessSizeCentralityNormalized", Double.Parse(feature.value.ToString())));
                        break;
                    }
                    case "boundBetweennessSizeCentralityGraphFTR":
                    {
                        boundBetweennessSizeCentralityGraphFTR = Double.Parse(feature.value.ToString());
                        featureSet.Add("boundBetweennessSizeCentrality", new Feature("boundBetweennessSizeCentrality", Double.Parse(feature.value.ToString())));
                        break;
                    }
                    case "pageRankNormalizedGraphFTR":
                    {
                        pageRankNormalizedGraphFTR = Double.Parse(feature.value.ToString());
                        featureSet.Add("pageRankNormalized", new Feature("pageRankNormalized", Double.Parse(feature.value.ToString())));
                        break;
                    }
                    case "closenessSizeCentralityNormalizedGraphFTR":
                    {
                        closenessSizeCentralityNormalizedGraphFTR = Double.Parse(feature.value.ToString());
                        featureSet.Add("closenessSizeCentralityNormalized", new Feature("closenessSizeCentralityNormalized", Double.Parse(feature.value.ToString())));
                        break;
                    }
                    case "betweennessSizeCentralityGraphFTR":
                    {
                        betweennessSizeCentralityGraphFTR = Double.Parse(feature.value.ToString());
                        featureSet.Add("betweennessSizeCentrality", new Feature("betweennessSizeCentrality", Double.Parse(feature.value.ToString())));
                        break;
                    }
                    default:
                    {
                        logger.Fatal("Feature <" + feature.name + "> with value <" + feature.value.ToString() + "> is unrecognized.");
                        break;
                    }
                }
            }
        }

        public String NodeName
        {
            get { return this.name; }
        }

        public int NodeID
        {
            get { return this.nodeID; }
        }

        /*
        public int CentralityCount
        {
            get { return this.centralityCount; }
        }

        public double CentralityRelative
        {
            get { return this.centralityRelative; }
        }

        public long InData
        {
            get { return this.inData; }
        }

        public long OutData
        {
            get { return this.outData; }
        }

        public int InDegree
        {
            get { return this.inDeg; }
        }

        public int OutDegree
        {
            get { return this.outDeg; }
        }

        public Double InSizeRatioProcessNode
        {
            get { return this.inSizeRatioProcessNodeFTR; }
        }

        public Double InSizeRatioFileNode
        {
            get { return this.inSizeRatioFileNodeFTR; }
        }

        public Double InSizeRatioSocketNode
        {
            get { return this.inSizeRatioSocketNodeFTR; }
        }

        public Double OutSizeRatioProcessNode
        {
            get { return this.outSizeRatioProcessNodeFTR; }
        }

        public Double OutSizeRatioFileNode
        {
            get { return this.outSizeRatioFileNodeFTR; }
        }

        public Double OutSizeRatioSocketNode
        {
            get { return this.outSizeRatioSocketNodeFTR; }
        }

        public Double InEdgeCompression
        {
            get { return this.inEdgeCompressionNodeFTR; }
        }

        public Double OutEdgeCompression
        {
            get { return this.outEdgeCompressionNodeFTR; }
        }

        public Double SizeStdDev
        {
            get { return this.sizeStdDevNodeFTR; }
        }

        public Double SizeEnt
        {
            get { return this.sizeEntNodeFTR; }
        }

        public Double BoundedPageRankNormalized
        {
            get { return this.boundedPageRankNormalizedGraphFTR; }
        }

        public Double BoundClosenessSizeCentralityNormalized
        {
            get { return this.boundClosenessSizeCentralityNormalizedGraphFTR; }
        }

        public Double BoundBetweennessSizeCentrality
        {
            get { return this.boundBetweennessSizeCentralityGraphFTR; }
        }

        public Double PageRankNormalized
        {
            get { return this.pageRankNormalizedGraphFTR; }
        }

        public Double ClosenessSizeCentralityNormalized
        {
            get { return this.closenessSizeCentralityNormalizedGraphFTR; }
        }

        public Double BetweennessSizeCentrality
        {
            get { return this.betweennessSizeCentralityGraphFTR; }
        }
        */
    }
}

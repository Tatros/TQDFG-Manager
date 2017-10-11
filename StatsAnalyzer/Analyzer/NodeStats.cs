using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StatsAnalyzer
{
    class NodeStats
    {
        public enum Property
        {
            inSizeRatioProcessNode,
            inSizeRatioFileNode,
            inSizeRatioSocketNode,
            outSizeRatioProcessNode,
            outSizeRatioFileNode,
            outSizeRatioSocketNode,
            inEdgeCompression,
            outEdgeCompression,
            sizeStdDev,
            sizeEnt,
            boundedPageRankNormalized,
            boundClosenessSizeCentralityNormalized,
            boundBetweennessSizeCentrality,
            pageRankNormalized,
            closenessSizeCentralityNormalized,
            betweennessSizeCentrality
            /*
            InDegree,
            OutDegree,
            InData,
            OutData,
            CentraliyCount,
            CentralityRelative,
            Centrality
             * */
        };

        int nodeID;
        String nodeName;
        int inDegree;
        int outDegree;
        int time;
        long inData;
        long outData;
        int centralityCount;
        double centralityRelative;
        int totalNodes;

        public NodeStats(int nodeID, String nodeName, int inDeg, int outDeg, long inData, long outData, int centralityCount, double centralityRelative, int totalNodes, int time)
        {
            this.nodeID = nodeID;
            this.nodeName = nodeName;
            this.inDegree = inDeg;
            this.outDegree = outDeg;
            this.inData = inData;
            this.outData = outData;
            this.time = time;
            this.centralityCount = centralityCount;
            this.centralityRelative = centralityRelative;
            this.totalNodes = totalNodes;
        }

        public int CentralityCount
        {
            get { return this.centralityCount; }
        }

        public double CentralityRelative
        {
            get { return this.centralityRelative; }
        }

        public int TotalNodes
        {
            get { return this.totalNodes; }
        }

        public int NodeID
        {
            get { return this.nodeID; }
        }

        public String NodeName
        {
            get { return this.nodeName; }
        }

        public int InDegree
        {
            get { return this.inDegree; }
        }

        public int OutDegree
        {
            get { return this.outDegree; }
        }

        public long InData
        {
            get { return this.inData; }
        }

        public long OutData
        {
            get { return this.outData; }
        }

        public override string ToString()
        {
            return ("time: " + time + "\r\ntotal Node Count: " + this.totalNodes + "\r\ninDegree: " + this.inDegree + "\r\noutDegree: " + this.outDegree + "\r\ninData: " + this.inData + "\r\noutData: " + this.outData + "\r\nCentralityCount: " + this.centralityCount + "\r\nCentralityRelative: " + this.centralityRelative +"%");
        }
    }
}

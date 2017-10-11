using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using de.tum.i22.monitor.structures;

namespace QDFGGraphManager.QDFGStatistics
{
    // Groups Vertex Stats for a certain Node by Time
    internal sealed class VertexStatCollection
    {
        int nodeID;
        String nodeName;
        DFTNodeType type;

        // Maps Time Instances to their Vertex Stats
        private Dictionary<int, VertexStats> stats;

        public VertexStatCollection(int nodeID, String nodeName, DFTNodeType type, VertexStats initialStats, int initialTime)
        {
            this.nodeID = nodeID;
            this.nodeName = nodeName;
            this.type = type;

            stats = new Dictionary<int, VertexStats>();
            stats.Add(initialTime, initialStats);
        }

        // Get the Vertex Stats for a certain Time Instance
        public VertexStats getVertexStatsForTime(int time)
        {
            if (stats.ContainsKey(time))
            {
                return stats[time];
            }

            else return null;
        }

        // Get the Vertex Stats for all Time Instances
        public Dictionary<int, VertexStats> getAllStats()
        {
            return this.stats;
        }

        public void addVertexStatsForTime(int time, VertexStats stats)
        {
            this.stats.Add(time, stats);
        }

        public String NodeName
        {
            get { return this.nodeName; }
        }

        public int NodeID
        {
            get { return this.nodeID; }
        }

        public DFTNodeType Type
        {
            get { return this.type; }
        }
    }
}

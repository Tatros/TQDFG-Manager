using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace StatsAnalyzer.Model
{
    interface IModel
    {
        MODEL Type { get; }
        String Name { get; }
        String NumberFormat { get; set; }
        String Separator { get; set; }
        Double MissingValue { get; set; }
        String MissingValueIdentifier { get; set; }
        DataTable DataTable { get; }
        DataTable FeatureTable { get; }
        DataTable ClassifiedFeatureTable { get; }
        int NumSamples { get; set; }
        List<String> getFeatureNames(String nodeName);
        List<String> getFeatureNames();
        Double getFeatureValue(String nodeName, String featureName);
        void saveToFile(String path);
        INode getNode(String nodeName);
        Model.NodeType getNodeType(String nodeName);
        List<INode> getNodes();
        event EventHandler ModelChanged;
    }
}

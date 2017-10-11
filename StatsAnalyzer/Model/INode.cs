using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StatsAnalyzer.Model
{
    interface INode
    {
        String Name { get; }
        int ID { get; }
        // private List<Tuple<String, Double>> features;

        Double getFeatureValue(String featureName);
        List<String> getFeatureNames();
        String ToString();
        bool isMalware();
        bool isGoodware();

    }
}

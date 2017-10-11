using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StatsAnalyzer.Model
{
    class GenericNode : INode
    {
        private String _name;
        private int _id;
        private Dictionary<String, Double> _featureValues;
        public enum NodeType { MALWARE, GOODWARE, UNKNOWN };

        public GenericNode(String name, int nodeID, Dictionary<String, Double> featureValues)
        {
            this._name = name;
            this._id = nodeID;
            this._featureValues = featureValues;
        }

        public String Name
        {
            get { return this._name; }
        }

        public int ID
        {
            get { return this._id; }
        }

        public List<String> getFeatureNames()
        {
            return this._featureValues.Keys.ToList();
        }

        public Double getFeatureValue(String featureName)
        {
            if (_featureValues.ContainsKey(featureName))
            {
                return _featureValues[featureName];
            }
            else
            {
                throw new ArgumentException("No feature with name <" + featureName + "> exists for node <" + Name + ">.");
            }
        }

        public List<Double> getFeatureValueList()
        {
            return this._featureValues.Values.ToList();
        }

        public double[] getFeatureValueArray()
        {
            return this._featureValues.Values.ToList().ToArray();
        }

        public bool isMalware()
        {
            return this._name.ToLower().Contains("malware") ? true : false;
        }

        public bool isGoodware()
        {
            return this._name.ToLower().Contains("goodware") ? true : false;
        }

        public override string ToString()
        {
            return this.Name;
        }
    }
}

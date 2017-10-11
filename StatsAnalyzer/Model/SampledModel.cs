using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StatsAnalyzer.Model
{
    class SampledModel : GenericAbstractModel, IModel
    {
        public SampledModel(Analyzer analyzer, String numberFormat = "0.0", double missingValue = -1.0, String missingValueIdentifier = "?", String separator = ";", int numSamples = -1)
        {
            this._analyzer = analyzer;
            _nodes = new List<GenericNode>();
            _featureNames = new List<string>();
            _numberFormat = numberFormat;
            _missingValue = missingValue;
            _separator = separator;
            _missingValueIdentifier = missingValueIdentifier;
            _numSamples = numSamples;
            _type = MODEL.SAMPLED;

            if (_numSamples <= 0)
            {
                _numSamples = analyzer.getMaxOverallSamples();
                Console.WriteLine("Max Overall Samples: " + _numSamples);
            }

            populateModel();
        }

        private void addFeatureNames(String nodeName, String baseFeature)
        {
            if (_nodes.Count == 0)
            {
                for (int i = 0; i < _numSamples; i++)
                {
                    String featureName = baseFeature + "_" + i;
                    if (!_featureNames.Contains(featureName))
                        _featureNames.Add(featureName);
                }
            }

            else
            {
                for (int i = 0; i < _numSamples; i++)
                {
                    String featureName = baseFeature + "_" + i;
                    if (!_featureNames.Contains(featureName))
                        throw new ArgumentException("Node <" + nodeName + "> provides inconsistent feature <" + baseFeature + ">");
                }
            }
        }

        private List<Tuple<String, Double>> generateSampledFeatures(String nodeName, String baseFeature, List<Double> featureValues)
        {
            List<Tuple<String, Double>> sampledNodeFeatures = new List<Tuple<string, double>>();

            if (!isValidSampleSet(featureValues))
            {
                for (int i = 0; i < _numSamples; i++)
                {
                    sampledNodeFeatures.Add(Tuple.Create<String, Double>(baseFeature + "_" + i, _missingValue));
                }
                
                addFeatureNames(nodeName, baseFeature);
            }
            else
            {
                for (int i = 0; i < _numSamples; i++)
                {
                    if (i < featureValues.Count)
                    {
                        sampledNodeFeatures.Add(Tuple.Create<String, Double>(baseFeature + "_" + i, featureValues[i]));
                    }
                    else
                    {
                        sampledNodeFeatures.Add(Tuple.Create<String, Double>(baseFeature + "_" + i, _missingValue));
                    }
                }

                addFeatureNames(nodeName, baseFeature);
            }

            return sampledNodeFeatures;
        }

        private void populateModel()
        {
            this._featureNames.Clear();
            this._nodes.Clear();

            List<String> baseFeatures = _analyzer.getBaseFeatures();
            List<String> nodeNames = _analyzer.getNodeNames();

            Dictionary<String, List<double>> nodeStats = new Dictionary<string,List<double>>();
            int nextID = 0;
            foreach (String nodeName in nodeNames)
            {
                nodeStats.Clear();
                nodeStats = new Dictionary<string, List<double>>(_analyzer.getAllStatsForNode(nodeName));


                Dictionary<String, Double> sampledNodeFeatures = new Dictionary<string,double>();

                foreach (String baseFeature in baseFeatures)
                {
                    if (!nodeStats.ContainsKey(baseFeature))
                        throw new ArgumentException("Node <" + nodeName + "> does not provide data for base feature <" + baseFeature + ">.");

                    List<Double> featureValues = nodeStats[baseFeature];
                    generateSampledFeatures(nodeName, baseFeature, featureValues).ForEach(tuple =>
                        {
                            if (!sampledNodeFeatures.ContainsKey(tuple.Item1))
                            {
                                sampledNodeFeatures.Add(tuple.Item1, tuple.Item2);
                            }
                        });
                }

                if (_nodes.Where(n => n.Name == nodeName).Count() > 0)
                    throw new ArgumentException("Duplicate Node found for Node <" + nodeName + ">.");
                else
                {
                    _nodes.Add(new GenericNode(nodeName, nextID, sampledNodeFeatures));
                    nextID++;
                }
            }
        }

        public override String Name
        {
            get { return "Sampled"; }
        }

        public override int NumSamples
        {
            get
            {
                return _numSamples;
            }
            set
            {
                _numSamples = value; populateModel(); raiseModelChanged();
            }
        }
    }
}

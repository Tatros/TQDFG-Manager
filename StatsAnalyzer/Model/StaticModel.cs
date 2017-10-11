using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StatsAnalyzer.Model
{
    class StaticModel : GenericAbstractModel, IModel
    {
        private DECISION_SAMPLE _decisionSample;
        public enum DECISION_SAMPLE { FIRST_VALUE, LAST_VALUE, MEDIAN_INDEX, MEDIAN_VALUE };

        public StaticModel(Analyzer analyzer, String numberFormat = "0.0", double missingValue = -1.0, String missingValueIdentifier = "?", String separator = ";", DECISION_SAMPLE decisionSample = DECISION_SAMPLE.LAST_VALUE)
        {
            this._analyzer = analyzer;
            _nodes = new List<GenericNode>();
            _featureNames = new List<string>();
            _numberFormat = numberFormat;
            _missingValue = missingValue;
            _separator = separator;
            _missingValueIdentifier = missingValueIdentifier;
            _decisionSample = decisionSample;
            _type = MODEL.STATIC;

            populateModel();
        }

        private void addFeatureName(String nodeName, String baseFeature)
        {
            if (_nodes.Count == 0)
            {
                if (!_featureNames.Contains(baseFeature))
                    _featureNames.Add(baseFeature);
            }

            else
            {
                if (!_featureNames.Contains(baseFeature))
                    throw new ArgumentException("Node <" + nodeName + "> provides inconsistent feature <" + baseFeature + ">");
            }
        }

        private Tuple<String, Double> generateStaticFeature(String nodeName, String baseFeature, List<Double> featureValues)
        {
            Tuple<String, Double> staticFeature;

            if (!isValidSampleSet(featureValues))
            {
                staticFeature = Tuple.Create<String, Double>(baseFeature, _missingValue);
                addFeatureName(nodeName, baseFeature);
            }
            else
            {
                switch (_decisionSample)
                {
                    case DECISION_SAMPLE.FIRST_VALUE:
                        {
                            staticFeature = Tuple.Create<String, Double>(baseFeature, featureValues.First());
                            break;
                        }
                    case DECISION_SAMPLE.LAST_VALUE:
                        {
                            staticFeature = Tuple.Create<String, Double>(baseFeature, featureValues.Last());
                            break;
                        }
                    case DECISION_SAMPLE.MEDIAN_INDEX:
                        {
                            int medianIndex = (int) Math.Round((double)featureValues.Count * 0.5, 0);
                            staticFeature = Tuple.Create<String, Double>(baseFeature, featureValues[medianIndex]);
                            break;
                        }
                    case DECISION_SAMPLE.MEDIAN_VALUE:
                        {
                            if (featureValues.Count > 2)
                            {
                                List<Double> sortedValues = new List<Double>(featureValues);
                                sortedValues.Sort();
                                int size = sortedValues.Count;
                                int middleIndex = size / 2;
                                double medianValue = (size % 2 != 0) ? (double)sortedValues[middleIndex] : ((double)sortedValues[middleIndex] + (double)sortedValues[middleIndex - 1]) / 2;
                                staticFeature = Tuple.Create<String, Double>(baseFeature, medianValue);
                            }

                            else
                            {
                                staticFeature = Tuple.Create<String, Double>(baseFeature, featureValues.First());
                            }

                            break;
                        }
                    default:
                        {
                            throw new NotImplementedException("Static feature extraction for decision sample <" + _decisionSample.ToString() + "> is not yet implemented.");
                        }
                }

                addFeatureName(nodeName, baseFeature);
            }

            return staticFeature;
        }

        private void populateModel()
        {
            this._featureNames.Clear();
            this._nodes.Clear();

            
            List<String> baseFeatures = _analyzer.getBaseFeatures();
            List<String> nodeNames = _analyzer.getNodeNames();

            int nextID = 0;
            Dictionary<String, List<double>> nodeStats = new Dictionary<string,List<double>>();
            foreach (String nodeName in nodeNames)
            {
                nodeStats.Clear();
                nodeStats = new Dictionary<string,List<double>>(_analyzer.getAllStatsForNode(nodeName));


                Dictionary<String, Double> staticNodeFeatures = new Dictionary<string,double>();

                foreach (String baseFeature in baseFeatures)
                {
                    if (!nodeStats.ContainsKey(baseFeature))
                        throw new ArgumentException("Node <" + nodeName + "> does not provide data for base feature <" + baseFeature + ">.");

                    List<Double> featureValues = nodeStats[baseFeature];
                    Tuple<String, Double> staticFeature = generateStaticFeature(nodeName, baseFeature, featureValues);

                    if (!staticNodeFeatures.ContainsKey(staticFeature.Item1))
                    {
                        staticNodeFeatures.Add(staticFeature.Item1, staticFeature.Item2);
                    }
                }

                if (_nodes.Where(n => n.Name == nodeName).Count() > 0)
                    throw new ArgumentException("Duplicate Node found for Node <" + nodeName + ">.");
                else
                {
                    _nodes.Add(new GenericNode(nodeName, nextID, staticNodeFeatures));
                    nextID++;
                }
            }
        }

        public DECISION_SAMPLE DecisionSample
        {
            get { return _decisionSample; }
            set { _decisionSample = value; populateModel(); raiseModelChanged(); }
        }

        public override int NumSamples
        {
            get { return _numSamples; }
            set { _numSamples = 1; }
        }

        public override String Name
        {
            get { return "Static"; }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics.Statistics;
using StatsAnalyzer;

namespace StatsAnalyzer.Model
{
    class StatisticalModel : GenericAbstractModel, IModel
    {
        public StatisticalModel(Analyzer analyzer, String numberFormat = "0.0", double missingValue = -1.0, String missingValueIdentifier = "?", String separator = ";")
        {
            this._analyzer = analyzer;
            _nodes = new List<GenericNode>();
            _featureNames = new List<string>();
            _numberFormat = numberFormat;
            _missingValue = missingValue;
            _separator = separator;
            _missingValueIdentifier = missingValueIdentifier;
            _numSamples = -1; // does not apply to statistical model
            _type = MODEL.STATISTICAL;

            populateModel();
        }

        private void addStatisticalFeatureNames(String nodeName, String baseFeature)
        {
            if (_nodes.Count == 0)
            {
                if (!_featureNames.Contains(baseFeature + "_Min"))
                    _featureNames.Add(baseFeature + "_Min");
                if (!_featureNames.Contains(baseFeature + "_Max"))
                    _featureNames.Add(baseFeature + "_Max");
                if (!_featureNames.Contains(baseFeature + "_Mean"))
                    _featureNames.Add(baseFeature + "_Mean");
                if (!_featureNames.Contains(baseFeature + "_StdDev"))
                    _featureNames.Add(baseFeature + "_StdDev");
                if (!_featureNames.Contains(baseFeature + "_Variance"))
                    _featureNames.Add(baseFeature + "_Variance");
                if (!_featureNames.Contains(baseFeature + "_Skewness"))
                    _featureNames.Add(baseFeature + "_Skewness");
                if (!_featureNames.Contains(baseFeature + "_Kurtosis"))
                    _featureNames.Add(baseFeature + "_Kurtosis");
                if (!_featureNames.Contains(baseFeature + "_LastValue"))
                    _featureNames.Add(baseFeature + "_LastValue");
                if (!_featureNames.Contains(baseFeature + "_FirstValue"))
                    _featureNames.Add(baseFeature + "_FirstValue");
                if (!_featureNames.Contains(baseFeature + "_MedianValue"))
                    _featureNames.Add(baseFeature + "_MedianValue");
                if (!_featureNames.Contains(baseFeature + "_MedianIndex"))
                    _featureNames.Add(baseFeature + "_MedianIndex");
            }

            else
            {
                if (!_featureNames.Contains(baseFeature + "_Min"))
                    throw new ArgumentException("Node <" + nodeName + "> provides inconsistent feature <" + baseFeature + ">");
                if (!_featureNames.Contains(baseFeature + "_Max"))
                    throw new ArgumentException("Node <" + nodeName + "> provides inconsistent feature <" + baseFeature + ">");
                if (!_featureNames.Contains(baseFeature + "_Mean"))
                    throw new ArgumentException("Node <" + nodeName + "> provides inconsistent feature <" + baseFeature + ">");
                if (!_featureNames.Contains(baseFeature + "_StdDev"))
                    throw new ArgumentException("Node <" + nodeName + "> provides inconsistent feature <" + baseFeature + ">");
                if (!_featureNames.Contains(baseFeature + "_Variance"))
                    throw new ArgumentException("Node <" + nodeName + "> provides inconsistent feature <" + baseFeature + ">");
                if (!_featureNames.Contains(baseFeature + "_Skewness"))
                    throw new ArgumentException("Node <" + nodeName + "> provides inconsistent feature <" + baseFeature + ">");
                if (!_featureNames.Contains(baseFeature + "_Kurtosis"))
                    throw new ArgumentException("Node <" + nodeName + "> provides inconsistent feature <" + baseFeature + ">");
                if (!_featureNames.Contains(baseFeature + "_LastValue"))
                    throw new ArgumentException("Node <" + nodeName + "> provides inconsistent feature <" + baseFeature + ">");
                if (!_featureNames.Contains(baseFeature + "_FirstValue"))
                    throw new ArgumentException("Node <" + nodeName + "> provides inconsistent feature <" + baseFeature + ">");
                if (!_featureNames.Contains(baseFeature + "_MedianValue"))
                    throw new ArgumentException("Node <" + nodeName + "> provides inconsistent feature <" + baseFeature + ">");
                if (!_featureNames.Contains(baseFeature + "_MedianIndex"))
                    throw new ArgumentException("Node <" + nodeName + "> provides inconsistent feature <" + baseFeature + ">");
            }
        }

        private List<Tuple<String, Double>> generateStatisticalFeatures(String nodeName, String baseFeature, List<Double> featureValues)
        {
            List<Tuple<String, Double>> statisticalNodeFeatures = new List<Tuple<string, double>>();

            if (!isValidSampleSet(featureValues))
            {
                statisticalNodeFeatures.Add(Tuple.Create<String,Double>(baseFeature + "_Min", MissingValue));
                statisticalNodeFeatures.Add(Tuple.Create<String,Double>(baseFeature + "_Max", MissingValue));
                statisticalNodeFeatures.Add(Tuple.Create<String,Double>(baseFeature + "_Mean", MissingValue));
                statisticalNodeFeatures.Add(Tuple.Create<String,Double>(baseFeature + "_StdDev", MissingValue));
                statisticalNodeFeatures.Add(Tuple.Create<String,Double>(baseFeature + "_Variance", MissingValue));
                statisticalNodeFeatures.Add(Tuple.Create<String,Double>(baseFeature + "_Skewness", MissingValue));
                statisticalNodeFeatures.Add(Tuple.Create<String,Double>(baseFeature + "_Kurtosis", MissingValue));
                statisticalNodeFeatures.Add(Tuple.Create<String, Double>(baseFeature + "_LastValue", MissingValue));
                statisticalNodeFeatures.Add(Tuple.Create<String, Double>(baseFeature + "_FirstValue", MissingValue));
                statisticalNodeFeatures.Add(Tuple.Create<String, Double>(baseFeature + "_MedianValue", MissingValue));
                statisticalNodeFeatures.Add(Tuple.Create<String, Double>(baseFeature + "_MedianIndex", MissingValue));

                addStatisticalFeatureNames(nodeName, baseFeature);
            }
            else
            {
                DescriptiveStatistics featureStatistics = new DescriptiveStatistics(featureValues);

                
                statisticalNodeFeatures.Add(Tuple.Create<String,Double>(baseFeature + "_Min", getSanitizedStatisticsValue(featureStatistics.Minimum)));
                statisticalNodeFeatures.Add(Tuple.Create<String,Double>(baseFeature + "_Max", getSanitizedStatisticsValue(featureStatistics.Maximum)));
                statisticalNodeFeatures.Add(Tuple.Create<String,Double>(baseFeature + "_Mean", getSanitizedStatisticsValue(featureStatistics.Mean)));
                statisticalNodeFeatures.Add(Tuple.Create<String,Double>(baseFeature + "_StdDev", getSanitizedStatisticsValue(featureStatistics.StandardDeviation)));
                statisticalNodeFeatures.Add(Tuple.Create<String,Double>(baseFeature + "_Variance", getSanitizedStatisticsValue(featureStatistics.Variance)));
                statisticalNodeFeatures.Add(Tuple.Create<String, Double>(baseFeature + "_Skewness", getSanitizedStatisticsValue(featureStatistics.Skewness)));
                statisticalNodeFeatures.Add(Tuple.Create<String,Double>(baseFeature + "_Kurtosis", getSanitizedStatisticsValue(featureStatistics.Kurtosis)));
                statisticalNodeFeatures.Add(Tuple.Create<String, Double>(baseFeature + "_LastValue", getSanitizedStatisticsValue(getDecisionSample(StaticModel.DECISION_SAMPLE.LAST_VALUE, featureValues))));
                statisticalNodeFeatures.Add(Tuple.Create<String, Double>(baseFeature + "_FirstValue", getSanitizedStatisticsValue(getDecisionSample(StaticModel.DECISION_SAMPLE.FIRST_VALUE, featureValues))));
                statisticalNodeFeatures.Add(Tuple.Create<String, Double>(baseFeature + "_MedianValue", getSanitizedStatisticsValue(getDecisionSample(StaticModel.DECISION_SAMPLE.MEDIAN_VALUE, featureValues))));
                statisticalNodeFeatures.Add(Tuple.Create<String, Double>(baseFeature + "_MedianIndex", getSanitizedStatisticsValue(getDecisionSample(StaticModel.DECISION_SAMPLE.MEDIAN_INDEX, featureValues))));
                addStatisticalFeatureNames(nodeName, baseFeature);
            }

            return statisticalNodeFeatures;
        }

        private double getSanitizedStatisticsValue(double value)
        {
            if (!Double.IsNaN(value) && !Double.IsInfinity(value))
                return value;
            else
                return -1.0;
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
                nodeStats = new Dictionary<string, List<double>>(_analyzer.getAllStatsForNode(nodeName));


                Dictionary<String, Double> statisticalNodeFeatures = new Dictionary<string,double>();

                foreach (String baseFeature in baseFeatures)
                {
                    if (!nodeStats.ContainsKey(baseFeature))
                        throw new ArgumentException("Node <" + nodeName + "> does not provide data for base feature <" + baseFeature + ">.");

                    List<Double> featureValues = nodeStats[baseFeature];
                    generateStatisticalFeatures(nodeName, baseFeature, featureValues).ForEach(tuple =>
                        {
                            if (!statisticalNodeFeatures.ContainsKey(tuple.Item1))
                            {
                                statisticalNodeFeatures.Add(tuple.Item1, tuple.Item2);
                            }
                        });
                }

                if (_nodes.Where(n => n.Name == nodeName).Count() > 0)
                    throw new ArgumentException("Duplicate Node found for Node <" + nodeName + ">.");
                else
                {
                    _nodes.Add(new GenericNode(nodeName, nextID, statisticalNodeFeatures));
                    nextID++;
                }
            }
        }

        public override String Name
        {
            get { return "Statistical"; }
        }

        public override int NumSamples
        {
            get
            {
                return _numSamples;
            }
            set
            {
                _numSamples = -1;
            }
        }
    }
}

using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Accord.Statistics.Analysis;

namespace StatsAnalyzer.MachineLearning
{
    public enum VotingScheme { NONE, MAJORITY_VOTE, ADDITIVE_PREDICTIONS };
    internal class CVResultVoter
    {
        List<CrossValidationResult> _cvResults;
        Dictionary<string, NodeClassification> _totalClassification;
        ConfusionMatrix _finalConfusionMatrix = null;

        public CVResultVoter(List<CrossValidationResult> cvResults)
        {
            this._cvResults = cvResults;

            _finalConfusionMatrix = CrossValidationResult.getAggregatedConfusionMatrix(_cvResults);
            _totalClassification = CrossValidationResult.mergeNodeClassificationsSum(_cvResults);
        }

        internal ReceiverOperatingCharacteristic getROC()
        {
            List<Tuple<int, double>> classificationSet = new List<Tuple<int,double>>();
            foreach (var kvp in _totalClassification)
            {
                kvp.Value.RawPredictions.ForEach(prediction => {
                    classificationSet.Add(new Tuple<int, double>(kvp.Value.ActualClass, prediction));
                });
            }

            List<int> expectedValues = new List<int>();
            List<double> predictedValues = new List<double>();

            classificationSet.ForEach(tuple => {
                expectedValues.Add(tuple.Item1);
                predictedValues.Add(tuple.Item2);
            });

            // Accord.Statistics.Analysis.RocAreaMethod method = RocAreaMethod.DeLong;
            ReceiverOperatingCharacteristic roc = new ReceiverOperatingCharacteristic(expectedValues.ToArray(), predictedValues.ToArray());
            return roc;
        }

        internal SchemeMajorityVote getMajorityVote()
        {
            SchemeMajorityVote scheme = new SchemeMajorityVote(_totalClassification);
            return scheme;
        }

        internal SchemeSumPredictions getSumPredictions()
        {
            SchemeSumPredictions scheme = new SchemeSumPredictions(_totalClassification);
            return scheme;
        }

        public ConfusionMatrix AggregatedConfusionMatrix
        {
            get { return this._finalConfusionMatrix; }
        }
    }
}

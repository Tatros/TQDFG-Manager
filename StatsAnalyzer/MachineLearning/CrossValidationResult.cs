using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using Accord.Statistics.Analysis;

namespace StatsAnalyzer.MachineLearning
{
    internal class CrossValidationResult
    {
        ConcurrentDictionary<string, NodeClassification> _nodeClassifications;
        ConcurrentBag<ConfusionMatrix> _confusionMatrices;

        internal CrossValidationResult()
        {
            this._nodeClassifications = new ConcurrentDictionary<string, NodeClassification>();
            this._confusionMatrices = new ConcurrentBag<ConfusionMatrix>();
        }

        internal void AddOrUpdateClassification(String nodeName, double prediction, int actualClass)
        {
            // Update Node Classifications
            NodeClassification classificationInfo = new NodeClassification(nodeName, prediction, actualClass);
            _nodeClassifications.AddOrUpdate(nodeName, classificationInfo,
                (key, existingValue) =>
                {
                    existingValue.addPrediction(prediction);
                    return existingValue;
                });
        }

        internal void AddConfusionMatrix(ConfusionMatrix m)
        {
            this._confusionMatrices.Add(m);
        }

        internal ConcurrentDictionary<string, NodeClassification> Classifications
        {
            get { return _nodeClassifications; }
        }

        internal ConcurrentBag<ConfusionMatrix> ConfusionMatrices
        {
            get { return _confusionMatrices; }
        }

        internal static ConfusionMatrix getAggregatedConfusionMatrix(List<CrossValidationResult> cvResults)
        {
            int count = 1;
            
            List<ConfusionMatrix> aggregatedMatrices = new List<ConfusionMatrix>();
            foreach (var cvResult in cvResults)
            {
                aggregatedMatrices.Add(ConfusionMatrix.Combine(cvResult.ConfusionMatrices.ToArray()));
                Console.WriteLine("Run " + count + " - Matrix Count: " + cvResult.ConfusionMatrices.Count);
                Console.WriteLine("Run " + count + " - Classification Count: " + cvResult.Classifications.Count);
                count++;
            }

            ConfusionMatrix finalConfusionMatrix = ConfusionMatrix.Combine(aggregatedMatrices.ToArray());
            return finalConfusionMatrix;
        }

        internal static Dictionary<string, NodeClassification> mergeNodeClassificationsSum(List<CrossValidationResult> cvResults)
        {
            Dictionary<string, NodeClassification> nodeClassifications = new Dictionary<string, NodeClassification>();

            foreach (var cvResult in cvResults)
            {
                foreach (var key in cvResult.Classifications.Keys)
                {
                    int actualClass = cvResult.Classifications[key].ActualClass;
                    double predictionSum = cvResult.Classifications[key].RawPredictions.Sum();


                    if (!nodeClassifications.ContainsKey(key))
                    {
                        // Add Key
                        nodeClassifications.Add(key, new NodeClassification(key, predictionSum, actualClass));
                    }

                    else
                    {
                        // Key exists
                        nodeClassifications[key].addPrediction(predictionSum);
                    }
                }
            }
            return nodeClassifications;
        }

        /* Basic Merge of all Predictions */
        /*
        internal static Dictionary<string, NodeClassification> mergeNodeClassifications(List<CrossValidationResult> cvResults)
        {
            Dictionary<string, NodeClassification> nodeClassifications = new Dictionary<string, NodeClassification>();

            foreach (var cvResult in cvResults)
            {
                foreach (var key in cvResult.Classifications.Keys)
                {
                    if (!nodeClassifications.ContainsKey(key))
                    {
                        // Add Key
                        nodeClassifications.Add(key, cvResult.Classifications[key]);
                    }

                    else
                    {
                        // Key exists
                        foreach (double prediction in cvResult.Classifications[key].RawPredictions)
                        {
                            nodeClassifications[key].addPrediction(prediction);
                        }
                    }
                }
            }
            return nodeClassifications;
        }*/

        internal void merge(CrossValidationResult otherResult)
        {

        }
    }
}

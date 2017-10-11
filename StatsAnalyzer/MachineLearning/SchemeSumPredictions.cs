using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Accord.Statistics.Analysis;

namespace StatsAnalyzer.MachineLearning
{
    internal class SchemeSumPredictions
    {
        DataTable _tableDetails;
        DataTable _tableSummary = null;
        Dictionary<string, NodeClassification> _nodeClassifications;
        ConfusionMatrix _baseMatrix;
        ConfusionMatrix _voterMatrix;
        List<int> baseExpected = new List<int>();
        List<int> basePredicted = new List<int>();
        List<double> basePredictedConf = new List<double>();
        List<int> voterExpected = new List<int>();
        List<int> voterPredicted = new List<int>();
        List<double> voterPredictedConf = new List<double>();

        double offset = -0.0;
        int _numPredictionsPerNode;

        int Samples = 0;
        int AP = 0;
        int AN = 0;
        int FP = 0;
        int FN = 0;
        int TP = 0;
        int TN = 0;

        int VSamples = 0;
        int VAP = 0;
        int VAN = 0;
        int VFP = 0;
        int VFN = 0;
        int VTP = 0;
        int VTN = 0;

        double rocAreaVoter = 0.0;
        double rocAreaBase = 0.0;

        public int NumPredictionsPerNode
        {
            get { return _numPredictionsPerNode; }
            set { _numPredictionsPerNode = value; }
        }

        int _numPredictions;

        public int NumPredictions
        {
            get { return _numPredictions; }
            set { _numPredictions = value; }
        }

        int _numNodes;

        public int NumNodes
        {
            get { return _numNodes; }
            set { _numNodes = value; }
        }

        public int NumSamples
        {
            get { return this._voterMatrix.Samples; }
        }

        public double ROCAreaBase
        {
            get { return this.rocAreaBase; }
        }

        public double ROCAreaVoter
        {
            get { return this.rocAreaVoter; }
        }

        public double Sensitivity
        {
            get { return this._voterMatrix.Sensitivity; }
        }

        public double FalsePositiveRate
        {
            get { return this._voterMatrix.FalsePositiveRate; }
        }

        public double FalseDiscoveryRate
        {
            get { return this._voterMatrix.FalseDiscoveryRate; }
        }

        public double Accuracy
        {
            get { return this._voterMatrix.Accuracy; }
        }

        public double PositivePredictiveValue
        {
            get { return this._voterMatrix.PositivePredictiveValue; }
        }

        public double Precision
        {
            get { return this._voterMatrix.Precision; }
        }

        public double Recall
        {
            get { return this._voterMatrix.Recall; }
        }

        public double FScore
        {
            get { return this._voterMatrix.FScore; }
        }

        public double Specificity
        {
            get { return this._voterMatrix.Specificity; }
        }

        public int ActualPositives
        {
            get { return this._voterMatrix.ActualPositives; }
        }

        public int ActualNegatives
        {
            get { return this._voterMatrix.ActualNegatives; }
        }

        public int TruePositives
        {
            get { return this._voterMatrix.TruePositives; }
        }

        public int TrueNegatives
        {
            get { return this._voterMatrix.TrueNegatives; }
        }

        public int FalsePositives
        {
            get { return this._voterMatrix.FalsePositives; }
        }

        public int FalseNegatives
        {
            get { return this._voterMatrix.FalseNegatives; }
        }

        internal SchemeSumPredictions(Dictionary<string, NodeClassification> nodeClassifications)
        {
            this._nodeClassifications = nodeClassifications;
            init();
            compute(nodeClassifications);
        }

        private void init()
        {

            _tableDetails = new DataTable();
            _tableDetails.Columns.Add("Node", typeof(String));
            _tableDetails.Columns.Add("Total Predictions", typeof(String));
            _tableDetails.Columns.Add("Correct Predictions", typeof(String));
            _tableDetails.Columns.Add("Wrong Predictions", typeof(String));

            for (int i = 0; i < _nodeClassifications.First().Value.RawPredictions.Count; i++)
                _tableDetails.Columns.Add("C" + i, typeof(String));

            _tableDetails.Columns.Add("Majority Vote", typeof(String));
            _tableDetails.Columns.Add("Sum Vote", typeof(String));
            _tableDetails.Columns.Add("Mean Prediction", typeof(String));
            _tableDetails.Columns.Add("Prediction Variance", typeof(String));
            _tableDetails.Columns.Add("Prediction Sum", typeof(String));
        }

        private void compute(Dictionary<string, NodeClassification> nodeClassifications)
        {
            baseExpected = new List<int>();
            basePredicted = new List<int>();
            basePredictedConf = new List<double>();
            voterExpected = new List<int>();
            voterPredicted = new List<int>();
            voterPredictedConf = new List<double>();

            int predictionCount = nodeClassifications.First().Value.RawPredictions.Count;

            foreach (var kvp in nodeClassifications)
            {
                double sum = kvp.Value.RawPredictions.Sum();

                // Voter
                VSamples++;
                voterExpected.Add(kvp.Value.ActualClass);
                voterPredictedConf.Add(sum);

                if (sum >= 0)
                {
                    voterPredicted.Add(1);
                }
                else
                {
                    voterPredicted.Add(-1);
                }

                if (kvp.Value.ActualClass >= 0) // malware
                {
                    VAP++;

                    if (sum >= 0) // malware predicted
                        VTP++;
                    else // goodware predicted
                        VFN++;
                }
                else if (kvp.Value.ActualClass < 0)
                {
                    VAN++;

                    if (sum < 0) // goodware predicted
                        VTN++;
                    else // malware predicted
                        VFP++;
                }


                // sum += offset;
                String sumVote = ((sum >= 0 && kvp.Value.ActualClass > 0) || (sum < 0 && kvp.Value.ActualClass < 0)) ? "Success" : "Fail";
                String majorityVote = (kvp.Value.CorrectPredictions > kvp.Value.FalsePredictions) ? "Success" : "Fail";
                
                String variancePrediction = Accord.Statistics.Tools.Variance(kvp.Value.RawPredictions.ToArray()).ToString();
                String meanPrediction = Accord.Statistics.Tools.Mean(kvp.Value.RawPredictions.ToArray()).ToString();
                String sumPredictions = sum.ToString();


                foreach (double prediction in kvp.Value.RawPredictions)
                {
                    baseExpected.Add(kvp.Value.ActualClass);
                    basePredictedConf.Add(prediction);
                    
                    if (prediction >= 0)
                        basePredicted.Add(1);
                    else
                        basePredicted.Add(-1);

                    Samples++;

                    if (kvp.Value.ActualClass > 0) // malware
                    {
                        AP++;

                        if (prediction >= 0) // predicted as malware
                            TP++;
                        else // predicted as goodware
                            FN++;
                    }

                    else if (kvp.Value.ActualClass < 0) // goodware
                    {
                        AN++;

                        if (prediction >= 0) // predicted as malware
                            FP++;
                        else // predicted as goodware
                            TN++;
                    }
                }
                
                /*
                addItem(kvp.Key, 
                    kvp.Value.TotalPredictions.ToString(), 
                    kvp.Value.CorrectPredictions.ToString(), 
                    kvp.Value.FalsePredictions.ToString(),
                    kvp.Value.RawPredictions[0].ToString(),
                    kvp.Value.RawPredictions[1].ToString(),
                    majorityVote,
                    sumVote,
                    meanPrediction,
                    variancePrediction,
                    sumPredictions);*/
                List<object> vals = new List<object>();
                vals.Add(kvp.Key);
                vals.Add(kvp.Value.TotalPredictions.ToString());
                vals.Add(kvp.Value.CorrectPredictions.ToString());
                vals.Add(kvp.Value.FalsePredictions.ToString());

                for (int i = 0; i < kvp.Value.RawPredictions.Count; i++)
                    vals.Add(kvp.Value.RawPredictions[i].ToString());

                vals.Add(majorityVote);
                vals.Add(sumVote);
                vals.Add(meanPrediction);
                vals.Add(variancePrediction);
                vals.Add(sumPredictions);

                // DataRow n = new DataRow();
                _tableDetails.Rows.Add(vals.ToArray());

                bool sumVoteSuccess = ((sum >= 0 && kvp.Value.ActualClass > 0) || (sum < 0 && kvp.Value.ActualClass < 0)) ? true : false;

                this._numNodes++;
                this._numPredictionsPerNode = kvp.Value.RawPredictions.Count;
                this._numPredictions += kvp.Value.RawPredictions.Count;
            }

            // calculate confusion matrix statistics
            //_Sensitivity = ((double)_NumTruePositive / (double)(_NumTruePositive + _NumFalseNegative));
            //_Specificity = ((double)_NumTrueNegative / (double)(_NumTrueNegative + _NumFalsePositive));
            //_Precision = ((double)_NumTruePositive / (double)(_NumTruePositive + _NumFalsePositive));
            //_FPR = ((double)_NumFalsePositive / (double)(_NumFalsePositive + _NumTrueNegative));
            //_Accuracy = (((double)(_NumTruePositive + _NumTrueNegative)) / ((double)(_numNodes)));
            //_FDR = ((double)_NumFalsePositive / (double)(_NumFalsePositive + _NumTruePositive));

            _baseMatrix = new ConfusionMatrix(basePredicted.ToArray<int>(), baseExpected.ToArray<int>(), 1, -1);
            _voterMatrix = new ConfusionMatrix(voterPredicted.ToArray<int>(), voterExpected.ToArray<int>(), 1, -1);

            ReceiverOperatingCharacteristic rocBase = new ReceiverOperatingCharacteristic(baseExpected.ToArray(), basePredictedConf.ToArray());
            ReceiverOperatingCharacteristic rocVoter = new ReceiverOperatingCharacteristic(voterExpected.ToArray(), voterPredictedConf.ToArray());
            rocBase.Compute(100);
            rocVoter.Compute(100);
            rocAreaBase = rocBase.Area;
            rocAreaVoter = rocVoter.Area;

            // Utility.writeToConsole<int>(voterExpected.ToArray<int>());
        }

        internal void showDetails()
        {
            FormDataView<string> f = new FormDataView<string>(_tableDetails);
            f.Show();
        }

        internal void showSummary()
        {
            if (_tableSummary == null)
            {
                _tableSummary = new DataTable();
                _tableSummary.Columns.Add("Category");
                _tableSummary.Columns.Add("Samples");
                _tableSummary.Columns.Add("AP");
                _tableSummary.Columns.Add("AN");
                _tableSummary.Columns.Add("TP");
                _tableSummary.Columns.Add("FP");
                _tableSummary.Columns.Add("TN");
                _tableSummary.Columns.Add("FN");
                _tableSummary.Columns.Add("Predictions/Node", typeof(String));
                _tableSummary.Columns.Add("AP-CM");
                _tableSummary.Columns.Add("AN-CM");
                _tableSummary.Columns.Add("TP-CM");
                _tableSummary.Columns.Add("FP-CM");
                _tableSummary.Columns.Add("TN-CM");
                _tableSummary.Columns.Add("FN-CM");
                _tableSummary.Columns.Add("Sensitivity");
                _tableSummary.Columns.Add("FPR");
                _tableSummary.Columns.Add("Precision");
                _tableSummary.Columns.Add("AUC");
                _tableSummary.Columns.Add("FScore");
                //_tableSummary.Columns.Add("Num Predictions", typeof(String));
                //_tableSummary.Columns.Add("Correct", typeof(String));
                //_tableSummary.Columns.Add("False", typeof(String));
                //_tableSummary.Columns.Add("Total Votes (Nodes)", typeof(String));
                //_tableSummary.Columns.Add("Correct Votes", typeof(String));
                //_tableSummary.Columns.Add("False Votes", typeof(String));
                //_tableSummary.Columns.Add("Sensitivity (TPR)", typeof(String));
                //_tableSummary.Columns.Add("Specificity (TNR)", typeof(String));
                //_tableSummary.Columns.Add("Precision (PPV)", typeof(String));
                //_tableSummary.Columns.Add("Accuracy", typeof(String));
                //_tableSummary.Columns.Add("FPR", typeof(String));
                //_tableSummary.Columns.Add("FDR", typeof(String));



                _tableSummary.Rows.Add("Base",
                    Samples, AP, AN, TP, FP, TN, FN, 
                    _numPredictionsPerNode.ToString(),
                    _baseMatrix.ActualPositives,
                    _baseMatrix.ActualNegatives, 
                    _baseMatrix.TruePositives, 
                    _baseMatrix.FalsePositives, 
                    _baseMatrix.TrueNegatives, 
                    _baseMatrix.FalseNegatives,
                    _baseMatrix.Sensitivity,
                    _baseMatrix.FalsePositiveRate,
                    _baseMatrix.Precision,
                    rocAreaBase,
                    _baseMatrix.FScore);

                _tableSummary.Rows.Add("Voter",
                    VSamples, VAP, VAN, VTP, VFP, VTN, VFN,
                    _numPredictionsPerNode.ToString(),
                    _voterMatrix.ActualPositives,
                    _voterMatrix.ActualNegatives,
                    _voterMatrix.TruePositives,
                    _voterMatrix.FalsePositives,
                    _voterMatrix.TrueNegatives,
                    _voterMatrix.FalseNegatives,
                    _voterMatrix.Sensitivity,
                    _voterMatrix.FalsePositiveRate,
                    _voterMatrix.Precision,
                    rocAreaVoter,
                    _voterMatrix.FScore);
                //_tableSummary.Rows.Add(VSamples, VAP, VAN, VTP, VFP, VTN, VFN, _numPredictionsPerNode.ToString(), _voterMatrix.ActualNegatives, _voterMatrix.ActualPositives, _voterMatrix.TruePositives, _voterMatrix.FalsePositives, _voterMatrix.TrueNegatives, _voterMatrix.FalseNegatives);
            }

            FormDataView<string> f = new FormDataView<string>(_tableSummary);
            f.Show();
        }

        internal ReceiverOperatingCharacteristic getROC()
        {
            // Accord.Statistics.Analysis.RocAreaMethod method = RocAreaMethod.DeLong;
            ReceiverOperatingCharacteristic roc = new ReceiverOperatingCharacteristic(voterExpected.ToArray(), voterPredictedConf.ToArray());
            return roc;
        }

        internal void addItem(String nodeName, 
            String totalPredictions, 
            String correctPredictions, 
            String falsePredictions, 
            String class1Pred,
            String class2Pred,
            String majorityVote,
            String sumVote,
            String meanPrediction,
            String variancePrediction,
            String sumPredictions)
        {
            _tableDetails.Rows.Add(nodeName, totalPredictions, correctPredictions, falsePredictions, class1Pred, class2Pred, majorityVote, sumVote, meanPrediction, variancePrediction, sumPredictions);
        }

        internal DataTable MajorityVoteDetails
        {
            get { return _tableDetails; }
        }

        internal DataTable SummaryTable
        {
            get { return this._tableSummary; }
        }

        internal DataTable DetailsTable
        {
            get { return this._tableDetails; }
        }
    }
}

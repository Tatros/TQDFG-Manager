using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Accord.Statistics.Analysis;

namespace StatsAnalyzer.MachineLearning
{
    internal partial class CVResultDialog : Form
    {
        List<CrossValidationResult> _cvResults;
        CVResultVoter _voter;
        List<String> availableSchemes;
        VotingScheme _scheme = VotingScheme.MAJORITY_VOTE;
        KernelSVMConfiguration _svmConfig;
        MachineLearning.MODE _cMode = MODE.STATIC_LINEAR;
        String _featureModel;
        long _timeElapsedMS = 0;
        long _memoryUsedBytes = 0;
        int _numRuns = 1;
        int _numFolds = 5;

        internal CVResultDialog(List<CrossValidationResult> cvResults, KernelSVMConfiguration svmConfig, MachineLearning.MODE cMode = MODE.STATIC_LINEAR, String featureModel = "multiple", long memoryUsedBytes = 0, long timeElapsedMS = 0, int numRuns = 1, int numFolds = 5)
        {
            InitializeComponent();
            this._cvResults = cvResults;
            this._voter = new CVResultVoter(cvResults);
            this._svmConfig = svmConfig;
            this._cMode = cMode;
            this._featureModel = featureModel;
            this._timeElapsedMS = timeElapsedMS;
            this._memoryUsedBytes = memoryUsedBytes;

            if (svmConfig != null)
            {
                this._numFolds = svmConfig.CrossValidationNumFolds;
                this._numRuns = svmConfig.CrossValidationNumRuns;
            }
            else
            {
                this._numFolds = numFolds;
                this._numRuns = numRuns;
            }
            initForm();
        }

        private void initForm()
        {
            availableSchemes = new List<string>();
            Enum.GetNames(typeof(VotingScheme)).ToList().ForEach(scheme =>
                { 
                    cbVotingScheme.Items.Add(scheme); 
                    availableSchemes.Add(scheme); 
                });

            if (cbVotingScheme.Items.Count > 0)
            {
                try
                {
                    _scheme = (VotingScheme)Enum.Parse(typeof(VotingScheme), cbVotingScheme.Items[0].ToString(), false);
                    cbVotingScheme.SelectedItem = _scheme.ToString();
                }
                catch (ArgumentException)
                {
                    MessageBox.Show("Invalid Voting Scheme.");
                    _scheme = (VotingScheme)Enum.Parse(typeof(VotingScheme), availableSchemes.First(), false);
                    cbVotingScheme.SelectedItem = _scheme.ToString();
                }
            }
        }

        private void buttonShowGeneric_Click(object sender, EventArgs e)
        {
            FormDataView<double> f = new FormDataView<double>(new[] { _voter.AggregatedConfusionMatrix });
            f.Show();
        }

        private void buttonDetails_Click(object sender, EventArgs e)
        {
            switch (_scheme)
            {
                case VotingScheme.MAJORITY_VOTE:
                    {
                        _voter.getMajorityVote().showDetails();
                        break;
                    }
                case VotingScheme.ADDITIVE_PREDICTIONS:
                    {
                        _voter.getSumPredictions().showDetails();
                        break;
                    }
                default:
                    {
                        break;
                    }
            }
        }

        private void cbVotingScheme_SelectedIndexChanged(object sender, EventArgs e)
        {
            String value = cbVotingScheme.SelectedItem.ToString();

            
            VotingScheme scheme;

            if (Enum.TryParse<VotingScheme>(value, out scheme))
            {
                this._scheme = scheme;
            }
            else
            {
                MessageBox.Show("Invalid Voting Scheme. Please select one of the provided voters.");
            }
        }

        private void buttonSummary_Click(object sender, EventArgs e)
        {
            switch (_scheme)
            {
                case VotingScheme.MAJORITY_VOTE:
                    {
                        _voter.getMajorityVote().showSummary();
                        break;
                    }
                case VotingScheme.ADDITIVE_PREDICTIONS:
                    {
                        _voter.getSumPredictions().showSummary();
                        break;
                    }
                default:
                    {
                        break;
                    }
            }
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void buttonROC_Click(object sender, EventArgs e)
        {
            ReceiverOperatingCharacteristic roc = null;
            int numPoints = -1;
            string numPointsStr = "";
            Utility.InputBox("ROC Points", "How many points should be used?", ref numPointsStr);
            if (numPointsStr != "")
                if (!Int32.TryParse(numPointsStr, out numPoints))
                {
                    MessageBox.Show("Your input was invalid. Please try again.");
                    return;
                }


            switch (_scheme)
            {
                case VotingScheme.NONE:
                    {
                        roc = _voter.getROC();
                        break;
                    }
                case VotingScheme.MAJORITY_VOTE:
                    {
                        roc = _voter.getMajorityVote().getROC();
                        break;
                    }
                case VotingScheme.ADDITIVE_PREDICTIONS:
                    {
                        roc = _voter.getSumPredictions().getROC();
                        break;
                    }
                default:
                    {
                        break;
                    }
            }

            if (roc != null)
            {
                roc.Compute(numPoints);
                FormDataView<double> f = new FormDataView<double>(roc);
                f.Show();
            }
            else
            {
                if (MessageBox.Show(this, "This voter does not offer ROC computation.\nWould you like to compute a general (non-vote) ROC from the data?", "ROC Computation", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
                {
                    roc = _voter.getROC();
                    roc.Compute(numPoints);
                    FormDataView<double> f = new FormDataView<double>(roc);
                    f.Show();
                }
            }
        }

        private String getExportString(String delimiter, String note, VotingScheme votingScheme)
        {
            StringBuilder s = new StringBuilder();

            String featureModel = this._featureModel;
            String kernel = _cMode.ToString();
            if (_svmConfig != null)
                kernel = this._svmConfig.Kernel.ToString();
            
            ConfusionMatrix cm = null;
            ReceiverOperatingCharacteristic roc = null;

            SchemeSumPredictions sumPredictions = null;
            SchemeMajorityVote majorityVote = null;

            switch (votingScheme)
            {
                case VotingScheme.NONE:
                    {
                        cm = _voter.AggregatedConfusionMatrix;
                        roc = _voter.getROC();
                        break;
                    }
                case VotingScheme.ADDITIVE_PREDICTIONS:
                    {
                        sumPredictions = _voter.getSumPredictions();
                        cm = null;
                        roc = sumPredictions.getROC();
                        break;
                    }
                case VotingScheme.MAJORITY_VOTE:
                    {
                        majorityVote = _voter.getMajorityVote();
                        cm = null;
                        roc = majorityVote.getROC();
                        break;
                    }
            }

            if (votingScheme == VotingScheme.NONE && roc != null && cm != null)
            {
                roc.Compute(100);
                s.Append(featureModel + delimiter);
                s.Append(votingScheme + delimiter);
                s.Append(kernel + delimiter);
                s.Append(note + delimiter);
                s.Append(this._numRuns + delimiter);
                s.Append(this._numFolds + delimiter);
                s.Append(this._timeElapsedMS + delimiter);
                s.Append(this._memoryUsedBytes + delimiter);
                s.Append(Utility.formatNumber(Utility.BytesToGB(this._memoryUsedBytes)) + delimiter);
                s.Append(cm.Samples + delimiter);
                s.Append(Utility.formatNumber(roc.Area) + delimiter);
                s.Append(Utility.formatNumber(cm.Sensitivity) + delimiter);
                s.Append(Utility.formatNumber(cm.Specificity) + delimiter);
                s.Append(Utility.formatNumber(cm.FalsePositiveRate) + delimiter);
                s.Append(Utility.formatNumber(cm.FalseDiscoveryRate) + delimiter);
                s.Append(Utility.formatNumber(cm.Accuracy) + delimiter);
                s.Append(Utility.formatNumber(cm.PositivePredictiveValue) + delimiter);
                s.Append(Utility.formatNumber(cm.Precision) + delimiter);
                s.Append(Utility.formatNumber(cm.Recall) + delimiter);
                s.Append(Utility.formatNumber(cm.FScore) + delimiter);
                s.Append(cm.ActualPositives + delimiter);
                s.Append(cm.ActualNegatives + delimiter);
                s.Append(cm.TruePositives + delimiter);
                s.Append(cm.TrueNegatives + delimiter);
                s.Append(cm.FalsePositives + delimiter);
                s.Append(cm.FalseNegatives);
            }

            else if (votingScheme == VotingScheme.ADDITIVE_PREDICTIONS && sumPredictions != null && roc != null)
            {
                roc.Compute(100);
                s.Append(featureModel + delimiter);
                s.Append(votingScheme + delimiter);
                s.Append(kernel + delimiter);
                s.Append(note + delimiter);
                s.Append(this._numRuns + delimiter);
                s.Append(this._numFolds + delimiter);
                s.Append(this._timeElapsedMS + delimiter);
                s.Append(this._memoryUsedBytes + delimiter);
                s.Append(Utility.formatNumber(Utility.BytesToGB(this._memoryUsedBytes)) + delimiter);
                s.Append(sumPredictions.NumSamples + delimiter);
                s.Append(Utility.formatNumber(sumPredictions.ROCAreaVoter) + delimiter);
                s.Append(Utility.formatNumber(sumPredictions.Sensitivity) + delimiter);
                s.Append(Utility.formatNumber(sumPredictions.Specificity) + delimiter);
                s.Append(Utility.formatNumber(sumPredictions.FalsePositiveRate) + delimiter);
                s.Append(Utility.formatNumber(sumPredictions.FalseDiscoveryRate) + delimiter);
                s.Append(Utility.formatNumber(sumPredictions.Accuracy) + delimiter);
                s.Append(Utility.formatNumber(sumPredictions.PositivePredictiveValue) + delimiter);
                s.Append(Utility.formatNumber(sumPredictions.Precision) + delimiter);
                s.Append(Utility.formatNumber(sumPredictions.Recall) + delimiter);
                s.Append(Utility.formatNumber(sumPredictions.FScore) + delimiter);
                s.Append(sumPredictions.ActualPositives + delimiter);
                s.Append(sumPredictions.ActualNegatives + delimiter);
                s.Append(sumPredictions.TruePositives + delimiter);
                s.Append(sumPredictions.TrueNegatives + delimiter);
                s.Append(sumPredictions.FalsePositives + delimiter);
                s.Append(sumPredictions.FalseNegatives);
            }

            else if (votingScheme == VotingScheme.MAJORITY_VOTE && majorityVote != null && roc != null)
            {
                roc.Compute(100);
                s.Append(featureModel + delimiter);
                s.Append(votingScheme + delimiter);
                s.Append(kernel + delimiter);
                s.Append(note + delimiter);
                s.Append(this._numRuns + delimiter);
                s.Append(this._numFolds + delimiter);
                s.Append(this._timeElapsedMS + delimiter);
                s.Append(this._memoryUsedBytes + delimiter);
                s.Append(Utility.formatNumber(Utility.BytesToGB(this._memoryUsedBytes)) + delimiter);
                s.Append(majorityVote.NumSamples + delimiter);
                s.Append(Utility.formatNumber(majorityVote.ROCAreaVoter) + delimiter);
                s.Append(Utility.formatNumber(majorityVote.Sensitivity) + delimiter);
                s.Append(Utility.formatNumber(majorityVote.Specificity) + delimiter);
                s.Append(Utility.formatNumber(majorityVote.FalsePositiveRate) + delimiter);
                s.Append(Utility.formatNumber(majorityVote.FalseDiscoveryRate) + delimiter);
                s.Append(Utility.formatNumber(majorityVote.Accuracy) + delimiter);
                s.Append(Utility.formatNumber(majorityVote.PositivePredictiveValue) + delimiter);
                s.Append(Utility.formatNumber(majorityVote.Precision) + delimiter);
                s.Append(Utility.formatNumber(majorityVote.Recall) + delimiter);
                s.Append(Utility.formatNumber(majorityVote.FScore) + delimiter);
                s.Append(majorityVote.ActualPositives + delimiter);
                s.Append(majorityVote.ActualNegatives + delimiter);
                s.Append(majorityVote.TruePositives + delimiter);
                s.Append(majorityVote.TrueNegatives + delimiter);
                s.Append(majorityVote.FalsePositives + delimiter);
                s.Append(majorityVote.FalseNegatives);
            }

            return s.ToString();
        }

        private void buttonExport_Click(object sender, EventArgs e)
        {
            String delimiter = ",";
            String note = "";
            if (Utility.InputBox("Optional Note", "Note Text", ref note) == DialogResult.OK)
            {
                //header
                String header = "FeatureModel" + delimiter
                    + "VotingScheme" + delimiter
                    + "Kernel" + delimiter
                    + "note" + delimiter
                    + "runs" + delimiter
                    + "folds" + delimiter
                    + "runtime" + delimiter
                    + "memoryBytes" + delimiter
                    + "memoryGB" + delimiter
                    + "NumSamples" + delimiter
                    + "AUC" + delimiter
                    + "Sensitivity" + delimiter
                    + "Specificity" + delimiter
                    + "FPR" + delimiter
                    + "FDR" + delimiter
                    + "Accuracy" + delimiter
                    + "PPV" + delimiter
                    + "Precision" + delimiter
                    + "Recall" + delimiter
                    + "FScore" + delimiter
                    + "AP" + delimiter
                    + "AN" + delimiter
                    + "TP" + delimiter
                    + "TN" + delimiter
                    + "FP" + delimiter
                    + "FN";

                if (!System.IO.File.Exists("ResultLog.csv"))
                    System.IO.File.WriteAllText("ResultLog.csv", header + "\n");
                
                System.IO.File.AppendAllText("ResultLog.csv", getExportString(delimiter, note, VotingScheme.NONE) + "\n");
                System.IO.File.AppendAllText("ResultLog.csv", getExportString(delimiter, note, VotingScheme.ADDITIVE_PREDICTIONS) + "\n");
                System.IO.File.AppendAllText("ResultLog.csv", getExportString(delimiter, note, VotingScheme.MAJORITY_VOTE) + "\n\n");
            }
        }


    }
}

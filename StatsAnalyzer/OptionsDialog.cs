using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using StatsAnalyzer.Model;

namespace StatsAnalyzer
{
    public partial class OptionsDialog : Form
    {
        IModel model;

        internal OptionsDialog(IModel model)
        {
            InitializeComponent();
            this.Text = "Model Configuration <" + model.Type.ToString() + ">";
            setupControls(model.Type);

            setControlText(this.tbMissingValue, model.MissingValue.ToString());
            setControlText(this.tbMissingValueIdentifier, model.MissingValueIdentifier);
            setControlText(this.tbNumberFormat, model.NumberFormat);
            setControlText(this.tbNumSamples, model.NumSamples.ToString());
            setControlText(this.tbSeparator, model.Separator);
            if (model.Type == MODEL.STATIC)
                setControlText(this.cbDecisionSample, ((StaticModel)model).DecisionSample.ToString());

            this.model = model;
        }

        private void setControlText(Control c, String value)
        {
            if (c.Enabled)
                c.Text = value;
        }

        private void setupControls(MODEL modelType)
        {
            cbDecisionSample.Enabled = false;
            label6.Enabled = false;
            tbNumSamples.Enabled = false;
            label5.Enabled = false;

            switch (modelType)
            {
                case MODEL.STATIC:
                    {
                        Enum.GetNames(typeof(StaticModel.DECISION_SAMPLE)).ToList().ForEach(item =>
                        {
                            cbDecisionSample.Items.Add(item);
                        });

                        cbDecisionSample.Enabled = true;
                        label6.Enabled = true;
                        break;
                    }
                case MODEL.SAMPLED:
                    {
                        tbNumSamples.Enabled = true;
                        label5.Enabled = true;
                        break;
                    }
                default:
                    {
                        break;
                    }
            }
        }

        private void validateInput()
        {
            String sNewMissingValue = tbMissingValue.Text;
            String sNewMissingValueIdentifier = tbMissingValueIdentifier.Text;
            String sNewNumberFormat = tbNumberFormat.Text;
            String sNewNumSamples = tbNumSamples.Text;
            String sNewSeparator = tbSeparator.Text;

            if (tbMissingValue.Enabled)
            {
                Double newMissingValue;
                if (!Double.TryParse(sNewMissingValue, out newMissingValue))
                {
                    throw new ArgumentException("Invalid setting for [missing value]: Cannot parse <" + sNewMissingValue + "> as Double.");
                }
            }

            if (tbMissingValueIdentifier.Enabled)
            {
                /*
                if (sNewMissingValueIdentifier.Length != 1)
                {
                    throw new ArgumentException("Invalid setting for [missing value identifier]: Value must be a single char.");
                }*/
            }

            if (tbSeparator.Enabled)
            {
                if (sNewSeparator.Length != 1)
                {
                    throw new ArgumentException("Invalid setting for [separator]: Value must be a single char.");
                }
            }

            if (tbNumberFormat.Enabled)
            {
                /*
                List<string> supportedFormats = new List<string> { "F0", "F1", "F2", "F3" };
                String supportedFormatsStr = "(";
                supportedFormats.ForEach(item => supportedFormatsStr += item);
                supportedFormatsStr += ")";
                if (!supportedFormats.Contains(sNewNumberFormat))
                {
                    throw new ArgumentException("Invalid setting for [number format]: Value must be one of the following " + supportedFormatsStr);
                }
                */

                if (!System.Text.RegularExpressions.Regex.Match(sNewNumberFormat, "^0.0{0,10}$").Success)
                {
                    throw new ArgumentException("Invalid setting for [number format]: Number format must match ^0.0{0,10}$, e.g. '0.000'.");
                }

                if (cbDecisionSample.Enabled)
                {
                    if (!Enum.GetNames(typeof(StaticModel.DECISION_SAMPLE)).Contains(cbDecisionSample.Text))
                    {
                        throw new ArgumentException("Invalid setting for [decision sample]: Please select a valid value from the dropdown control.");
                    }
                }
                
            }

            if (tbNumSamples.Enabled)
            {
                int newNumSamples;
                if (!Int32.TryParse(sNewNumSamples, out newNumSamples))
                {
                    throw new ArgumentException("Invalid setting for [num samples]: Cannot parse <" + sNewNumSamples + "> as Int.");
                }
                else
                {
                    if (newNumSamples <= 0 || newNumSamples > 10000)
                    {
                        throw new ArgumentException("Invalid setting for [num samples]: Value must be in interval [1, 10000].");
                    }
                }
            }
        }

        private void updateModel()
        {
            if (tbMissingValue.Enabled)
                this.model.MissingValue = Double.Parse(tbMissingValue.Text);
            if (tbMissingValueIdentifier.Enabled)
                this.model.MissingValueIdentifier = tbMissingValueIdentifier.Text;
            if (tbNumberFormat.Enabled)
                this.model.NumberFormat = tbNumberFormat.Text;
            if (tbNumSamples.Enabled)
                this.model.NumSamples = Int32.Parse(tbNumSamples.Text);
            if (tbSeparator.Enabled)
                this.model.Separator = tbSeparator.Text;
            if (cbDecisionSample.Enabled)
            {
                if (model.Type == MODEL.STATIC)
                {
                    ((StaticModel)model).DecisionSample = (StaticModel.DECISION_SAMPLE) Enum.Parse(typeof(StaticModel.DECISION_SAMPLE), cbDecisionSample.Text);
                }
            }
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            try
            {
                validateInput();
                updateModel();
                this.Close();
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using StatsAnalyzer.MachineLearning;
using StatsAnalyzer.Model;
using Accord.Statistics.Analysis;

namespace StatsAnalyzer
{
    internal partial class OptionsPCADialog : Form
    {
        private PCAConfiguration _configuration;
        private IModel _model;

        AnalysisMethod _method;
        String _numberFormat;

        private String _separator;
        private String _missingValueIdentifier;
        private String _forcedLabel;
        private int _forcedNumComponents;
        private bool _useForcedLabel;
        private bool _useForcedNumComponents;

        List<String> _activeFeatures;

        internal OptionsPCADialog(PCAConfiguration configuration, IModel model)
        {
            InitializeComponent();
            _model = model;
            loadConfiguration(configuration);
        }

        private void loadConfiguration(PCAConfiguration configuration)
        {
            _configuration = configuration;
            _method = _configuration.Method;
            _separator = _configuration.PCAExportSeparator;
            _missingValueIdentifier = _configuration.PCAExportMissingValueIdentifier;
            _forcedLabel = _configuration.PCAExportForcedLabel;
            _forcedNumComponents = _configuration.PCAExportForcedNumComponents;
            _useForcedLabel = _configuration.PCAExportUseForcedLabel;
            _useForcedNumComponents = _configuration.PCAExportUseForcedNumComponents;
            _numberFormat = _configuration.PCAExportNumberFormat;
            _activeFeatures = _configuration.ActiveFeatures;

            setFormValues();
        }

        private void setFormValues()
        {
            _configuration.getAvailableAnalysisMethods().ForEach(method => this.cbAnalysisMethod.Items.Add(method));

            this.cbAnalysisMethod.SelectedItem = this._method.ToString();
            this.tbSeparator.Text = this._separator;
            this.tbMissingValueIdentifier.Text = this._missingValueIdentifier;
            this.tbForcedLabel.Text = this._forcedLabel;
            this.tbForcedComponents.Text = this._forcedNumComponents.ToString();
            this.tbNumberFormat.Text = this._numberFormat;
            this.checkForcedComponents.Checked = this._useForcedNumComponents;
            this.checkForcedLabel.Checked = this._useForcedLabel;
            
            this.tbForcedLabel.Enabled = this.checkForcedLabel.Checked ? true : false;
            this.tbForcedComponents.Enabled = this.checkForcedComponents.Checked ? true : false;

            _model.getFeatureNames().ForEach(feature => {
                if (_activeFeatures.Contains(feature))
                    this.lbActiveFeatures.Items.Add(feature);
                else
                    this.lbIgnoredFeatures.Items.Add(feature);
            });
        }

        private void validateInput()
        {
            // Analysis Method
            if (cbAnalysisMethod.Enabled)
            {
                if (!_configuration.getAvailableAnalysisMethods().Contains(cbAnalysisMethod.Text))
                {
                    throw new ArgumentException("Invalid setting for [analysis method]: Please select a valid value from the dropdown control.");
                }

                else
                {
                    try
                    {
                        _method = (AnalysisMethod)Enum.Parse(typeof(AnalysisMethod), cbAnalysisMethod.Text);
                    }
                    catch (Exception)
                    {
                        throw new ArgumentException("Invalid setting for [analysis method]: Please select a valid value from the dropdown control.");
                    }
                }
            }

            // Number Format
            if (tbNumberFormat.Enabled)
            {
                if (!System.Text.RegularExpressions.Regex.Match(tbNumberFormat.Text, "^0.0{0,10}$").Success)
                {
                    throw new ArgumentException("Invalid setting for [number format]: Number format must match ^0.0{0,10}$, e.g. '0.000'.");
                }

                else
                {
                    _numberFormat = tbNumberFormat.Text;
                }
            }

            // Missing Value Identifier
            if (tbMissingValueIdentifier.Enabled)
            {
                if (tbMissingValueIdentifier.Text.Length != 1)
                {
                    throw new ArgumentException("Invalid setting for [missing value identifier]: Value must be a single char.");
                }
                else
                {
                    _missingValueIdentifier = tbMissingValueIdentifier.Text;
                }
            }

            // Separator
            if (tbSeparator.Enabled)
            {
                if (tbSeparator.Text.Length != 1)
                {
                    throw new ArgumentException("Invalid setting for [separator]: Value must be a single char.");
                }
                else
                {
                    _separator = tbSeparator.Text;
                }
            }

            // Forced Label
            if (tbForcedLabel.Enabled)
            {
                if (tbForcedLabel.Text.Length < 1)
                {
                    throw new ArgumentException("Invalid setting for [label]: Label cannot be empty.");
                }
                else
                {
                    _forcedLabel = tbForcedLabel.Text;
                }
            }

            // Num Forced Components
            if (tbForcedComponents.Enabled)
            {
                int numComponents;
                if (tbForcedComponents.Text.Length < 1)
                {
                    throw new ArgumentException("Invalid setting for [# forced components]: Please provide the number of components.");
                }
                else if (!Int32.TryParse(tbForcedComponents.Text, out numComponents))
                {
                    throw new ArgumentException("Invalid setting for [# forced components]: Number must be a positive integer.");
                }
                else if (numComponents <= 0)
                {
                    throw new ArgumentException("Invalid setting for [# forced components]: Number must be a positive integer.");
                }
                else
                {
                    _forcedNumComponents = numComponents;
                }
            }

            // Use Forced Label / Components
            _useForcedLabel = (checkForcedLabel.Checked) ? true : false;
            _useForcedNumComponents = (checkForcedComponents.Checked) ? true : false;

            // Active Features
            if (_activeFeatures.Count < 1)
                throw new ArgumentException("Invalid setting for [active features]: You must select at least one active feature.");
        }

        private void updateConfiguration()
        {
            _configuration.Method = _method;
            _configuration.PCAExportSeparator = _separator;
            _configuration.PCAExportMissingValueIdentifier = _missingValueIdentifier;
            _configuration.PCAExportNumberFormat = _numberFormat;
            _configuration.PCAExportUseForcedLabel = _useForcedLabel;
            _configuration.PCAExportUseForcedNumComponents = _useForcedNumComponents;
            _configuration.PCAExportForcedLabel = _forcedLabel;
            _configuration.PCAExportForcedNumComponents = _forcedNumComponents;
            _configuration.ActiveFeatures = _activeFeatures;

            //_activeFeatures.ForEach(feature => Console.WriteLine(feature + ", "));
        }

        private bool save()
        {
            try
            {
                validateInput();
                updateConfiguration();
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            if (save())
                this.Close();
        }

        private void checkForcedLabel_CheckedChanged(object sender, EventArgs e)
        {
            tbForcedLabel.Enabled = this.checkForcedLabel.Checked ? true : false;
        }

        private void checkForcedComponents_CheckedChanged(object sender, EventArgs e)
        {
            tbForcedComponents.Enabled = this.checkForcedComponents.Checked ? true : false;
        }

        private void buttonMarkIgnored_Click(object sender, EventArgs e)
        {
            Utility.swapItemBetweenListBox(lbActiveFeatures, lbIgnoredFeatures);
            refreshActiveFeatures();
        }
        private void buttonMarkActive_Click(object sender, EventArgs e)
        {
            Utility.swapItemBetweenListBox(lbIgnoredFeatures, lbActiveFeatures);
            refreshActiveFeatures();
        }

        private void refreshActiveFeatures()
        {
            this._activeFeatures.Clear();
            lbActiveFeatures.Items.OfType<String>().ToList().ForEach(feature => _activeFeatures.Add(feature));
        }
    }
}

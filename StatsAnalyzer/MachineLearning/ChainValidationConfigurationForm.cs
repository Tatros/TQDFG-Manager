using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StatsAnalyzer.MachineLearning
{
    public partial class ChainValidationConfigurationForm : Form
    {
        ChainValidationConfiguration _configuartion;
        List<String> _availableModes;
        MODE _validationMode;

        internal ChainValidationConfigurationForm(ChainValidationConfiguration configuration)
        {
            InitializeComponent();
            _configuartion = configuration;
            initForm();
            loadConfiguration();
        }

        private void loadConfiguration()
        {
            this._validationMode = _configuartion.ChainValidationMode;
            this.cbValidationMode.SelectedItem = this._validationMode.ToString();
        }

        private void initForm()
        {
            _availableModes = new List<string>();
            Enum.GetNames(typeof(MODE)).ToList().ForEach(mode =>
            {
                cbValidationMode.Items.Add(mode);
                _availableModes.Add(mode);
            });

            if (cbValidationMode.Items.Count > 0)
            {
                try
                {
                    _validationMode = (MODE)Enum.Parse(typeof(MODE), cbValidationMode.Items[0].ToString(), false);
                    cbValidationMode.SelectedItem = _validationMode.ToString();
                }
                catch (ArgumentException)
                {
                    MessageBox.Show("Invalid Voting Scheme.");
                    _validationMode = (MODE)Enum.Parse(typeof(MODE), _availableModes.First(), false);
                    cbValidationMode.SelectedItem = _validationMode.ToString();
                }
            }
        }

        private void save()
        {
            _configuartion.ChainValidationMode = this._validationMode;
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            this.save();
            this.Close();
        }

        private void cbValidationMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            String value = cbValidationMode.SelectedItem.ToString();


            MODE mode;

            if (Enum.TryParse<MODE>(value, out mode))
            {
                this._validationMode = mode;
            }
            else
            {
                MessageBox.Show("Invalid Validation Mode. Please select one of the provided Modes.");
            }
        }
    }
}

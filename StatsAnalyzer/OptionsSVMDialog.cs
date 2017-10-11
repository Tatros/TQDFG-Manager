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
using Accord.Statistics.Kernels;
using Accord.Math;
using AForge;
using StatsAnalyzer.Model;


namespace StatsAnalyzer
{
    internal partial class OptionsSVMDialog : Form
    {
        private KernelSVMConfiguration _configuration;
        private IModel _model;

        // kernel properties
        private Kernel _kernel = Kernel.GAUSSIAN;
        private double _gaussianSigma;
        private int _polyDegree;
        private double _polyConst;
        private double _laplacianSigma;
        private double _sigmoidAlpha;
        private double _sigmoidConst;

        // optionals
        private double _optDouble1;
        private double _optDouble2;
        private int _optInt1;
        private int _optInt2;

        // smo properties
        private double _complexity;
        private double _tolerance;
        private bool _useComputedWeights;
        private bool _useHeuristicalComplexity;
        private double _weightPositiveClass;
        private double _weightNegativeClass;

        // cross validation
        private int _numFolds;
        private int _numRuns;

        List<String> _activeFeatures;
        

        internal OptionsSVMDialog(KernelSVMConfiguration configuration, IModel model)
        {
            InitializeComponent();
            _model = model;
            loadConfiguration(configuration);
        }

        private void loadConfiguration(KernelSVMConfiguration configuration)
        {
            _configuration = configuration;
            _activeFeatures = _configuration.ActiveFeatures;

            // kernel
            this._kernel = configuration.Kernel;
            this._gaussianSigma = configuration.GaussianSigma;
            this._polyConst = configuration.PolynomialConstant;
            this._polyDegree = configuration.PolynomialDegree;
            this._laplacianSigma = configuration.LaplacianSigma;
            this._sigmoidAlpha = configuration.SigmoidAlpha;
            this._sigmoidConst = configuration.SigmoidConstant;
            
            // optionals
            this._optDouble1 = configuration.OptDouble1;
            this._optDouble2 = configuration.OptDouble2;
            this._optInt1 = configuration.OptInt1;
            this._optInt2 = configuration.OptInt2;

            // smo
            this._complexity = configuration.Complexity;
            this._tolerance = configuration.Tolerance;
            this._weightNegativeClass = configuration.WeightNegativeClass;
            this._weightPositiveClass = configuration.WeightPositiveClass;
            this._useComputedWeights = configuration.UseComputedWeights;
            this._useHeuristicalComplexity = configuration.UseHeuristicalComplexity;

            // cv
            this._numFolds = configuration.CrossValidationNumFolds;
            this._numRuns = configuration.CrossValidationNumRuns;

            setFormValues();
        }

        private void setFormValues()
        {
            _configuration.getAvailableKernels().ForEach(kernel => this.cbKernel.Items.Add(kernel));

            // kernel
            this.cbKernel.SelectedItem = this._kernel.ToString();
            this.tbGaussianSigma.Value  = (decimal)this._gaussianSigma;
            this.tbPolyConstant.Value   = (decimal)this._polyConst;
            this.tbPolyDegree.Value     = (decimal)this._polyDegree;
            this.tbLaplacianSigma.Value = (decimal)this._laplacianSigma;
            this.tbSigmoidAlpha.Value   = (decimal)this._sigmoidAlpha;
            this.tbSigmoidConst.Value   = (decimal)this._sigmoidConst;

            // optionals
            this.optDouble1.Value       = (decimal)this._optDouble1;
            this.optDouble2.Value       = (decimal)this._optDouble2;
            this.optInt1.Value          = (decimal)this._optInt1;
            this.optInt2.Value          = (decimal)this._optInt2;

            // smo
            this.tbComplexity.Value     = (decimal)this._complexity;
            this.tbTolerance.Value      = (decimal)this._tolerance;
            this.tbWeightsNegative.Value = (decimal)this._weightNegativeClass;
            this.tbWeightsPositive.Value = (decimal)this._weightPositiveClass;

            setUseHeuristics(this._useHeuristicalComplexity);
            setUseComputedWeights(this._useComputedWeights);

            // cv
            this.tbNumFolds.Value       = (decimal)this._numFolds;
            this.tbNumRuns.Value        = (decimal)this._numRuns;

            // features
            _model.getFeatureNames().ForEach(feature =>
            {
                if (_activeFeatures.Contains(feature))
                    this.lbActiveFeatures.Items.Add(feature);
                else
                    this.lbIgnoredFeatures.Items.Add(feature);
            });
        }

        private void refreshActiveFeatures()
        {
            this._activeFeatures.Clear();
            String exp = "";
            lbActiveFeatures.Items.OfType<String>().ToList().ForEach(feature => 
                {
                    _activeFeatures.Add(feature);

                    if (exp != "")
                        exp = exp + "|" + feature.ToString();
                    else
                        exp = feature.ToString();
                });
            
            tbExpression.Text = exp;
        }

        private void refreshKernels()
        {
            switch (this._kernel)
            {
                case Kernel.GAUSSIAN:
                    {
                        groupGaussianKernel.Enabled = true;
                        groupLaplacianKernel.Enabled = false;
                        groupPolyKernel.Enabled = false;
                        groupSigmoidKernel.Enabled = false;
                        break;
                    }
                case Kernel.LAPLACIAN:
                    {
                        groupGaussianKernel.Enabled = false;
                        groupLaplacianKernel.Enabled = true;
                        groupPolyKernel.Enabled = false;
                        groupSigmoidKernel.Enabled = false;
                        break;
                    }
                case Kernel.POLYNOMIAL:
                    {
                        groupGaussianKernel.Enabled = false;
                        groupLaplacianKernel.Enabled = false;
                        groupPolyKernel.Enabled = true;
                        groupSigmoidKernel.Enabled = false;
                        break;
                    }
                case Kernel.SIGMOID:
                    {
                        groupGaussianKernel.Enabled = false;
                        groupLaplacianKernel.Enabled = false;
                        groupPolyKernel.Enabled = false;
                        groupSigmoidKernel.Enabled = true;
                        break;
                    }
                default:
                    {
                        groupGaussianKernel.Enabled = false;
                        groupLaplacianKernel.Enabled = false;
                        groupPolyKernel.Enabled = false;
                        groupSigmoidKernel.Enabled = false;
                        break;
                    }
            }
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

        private void cbKernel_SelectedIndexChanged(object sender, EventArgs e)
        {
            Kernel k;
            if (Enum.TryParse<Kernel>(this.cbKernel.Text, true, out k))
            {
                this._kernel = k;
                this.refreshKernels();
            }
            else
            {
                groupGaussianKernel.Enabled = false;
                groupLaplacianKernel.Enabled = false;
                groupPolyKernel.Enabled = false;
                groupSigmoidKernel.Enabled = false;
            }
        }

        private void buttonEstimate_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(this, "This action will first save the configuration. Do you wish to Continue?", "Save required", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                if (save())
                {
                    double[,] sourceMatrix = _model.FeatureTable.ToMatrix<double>(_activeFeatures.ToArray());
                    double[][] inputs = sourceMatrix.ToArray();
                    DoubleRange range;

                    if (groupGaussianKernel.Enabled)
                    {
                        Gaussian gaussian = Gaussian.Estimate(inputs, inputs.Length, out range);
                        tbGaussianSigma.Value = (decimal)gaussian.Sigma;
                    }

                    if (groupPolyKernel.Enabled)
                    {
                    }

                    if (groupSigmoidKernel.Enabled)
                    {
                        Sigmoid sigmoid = Sigmoid.Estimate(inputs, inputs.Length, out range);

                        if (sigmoid.Alpha < (double)Decimal.MaxValue && sigmoid.Alpha > (double)Decimal.MinValue)
                            tbSigmoidAlpha.Value = (decimal)sigmoid.Alpha;

                        if (sigmoid.Constant < (double)Decimal.MaxValue && sigmoid.Constant > (double)Decimal.MinValue)
                            tbSigmoidConst.Value = (decimal)sigmoid.Constant;
                    }

                    if (groupLaplacianKernel.Enabled)
                    {
                        Laplacian laplacian = Laplacian.Estimate(inputs, inputs.Length, out range);
                        tbLaplacianSigma.Value = (decimal)laplacian.Sigma;
                    }
                }
                else
                {
                    MessageBox.Show("Failed to fully save the configuration.\nTherefore configuration properties can not be estimated.");
                }
            }
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            if (save())
                this.Close();
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

        private void validateInput()
        {
            // Kernel
            if (cbKernel.Enabled)
            {
                if (!_configuration.getAvailableKernels().Contains(cbKernel.Text))
                {
                    throw new ArgumentException("Invalid setting for [Kernel]: Please select a valid value from the dropdown control.");
                }

                else
                {
                    try
                    {
                        _kernel = (Kernel)Enum.Parse(typeof(Kernel), cbKernel.Text);
                    }
                    catch (Exception)
                    {
                        throw new ArgumentException("Invalid setting for [Kernel]: Please select a valid value from the dropdown control.");
                    }
                }
            }

            // Optionals
            double tempDouble;
            int tempInt;

            if (!Double.TryParse(optDouble1.Text, out tempDouble))
            {
                throw new ArgumentException("Invalid setting for [Opt Double1]: Not a number.");
            }
            else
            {
                _optDouble1 = tempDouble;
            }

            if (!Double.TryParse(optDouble2.Text, out tempDouble))
            {
                throw new ArgumentException("Invalid setting for [Opt Double2]: Not a number.");
            }
            else
            {
                _optDouble2 = tempDouble;
            }

            if (!Int32.TryParse(optInt1.Text, out tempInt))
            {
                throw new ArgumentException("Invalid setting for [Opt Int1]: Not a number.");
            }
            else
            {
                _optInt1 = tempInt;
            }

            if (!Int32.TryParse(optInt2.Text, out tempInt))
            {
                throw new ArgumentException("Invalid setting for [Opt Int2]: Not a number.");
            }
            else
            {
                _optInt2 = tempInt;
            }

            
            // Gaussian Sigma
            if (tbGaussianSigma.Enabled)
            {
                double sigma;
                if (!Double.TryParse(tbGaussianSigma.Text, out sigma))
                {
                    throw new ArgumentException("Invalid setting for [Gaussian Sigma]: Not a number.");
                }
                else
                {
                    _gaussianSigma = sigma;
                }
            }


            // Laplacian Sigma
            if (tbLaplacianSigma.Enabled)
            {
                double sigma;
                if (!Double.TryParse(tbLaplacianSigma.Text, out sigma))
                {
                    throw new ArgumentException("Invalid setting for [Laplacian Sigma]: Not a number.");
                }
                else
                {
                    _laplacianSigma = sigma;
                }
            }

            // Sigmoid Alpha
            if (tbSigmoidAlpha.Enabled)
            {
                double alpha;
                if (!Double.TryParse(tbSigmoidAlpha.Text, out alpha))
                {
                    throw new ArgumentException("Invalid setting for [Sigmoid Alpha]: Not a number.");
                }
                else
                {
                    _sigmoidAlpha = alpha;
                }
            }

            // Sigmoid Const
            if (tbSigmoidConst.Enabled)
            {
                double constant;
                if (!Double.TryParse(tbSigmoidConst.Text, out constant))
                {
                    throw new ArgumentException("Invalid setting for [Sigmoid Constant]: Not a number.");
                }
                else
                {
                    _sigmoidConst = constant;
                }
            }

            // Polynomial Constant
            if (tbPolyConstant.Enabled)
            {
                double polyConst;
                if (!Double.TryParse(tbPolyConstant.Text, out polyConst))
                {
                    throw new ArgumentException("Invalid setting for [Polynomial Constant]: Not a number.");
                }
                else
                {
                    _polyConst = polyConst;
                }
            }

            // Polynomial Degree
            if (tbPolyDegree.Enabled)
            {
                int polyDeg;
                if (!Int32.TryParse(tbPolyDegree.Text, out polyDeg))
                {
                    throw new ArgumentException("Invalid setting for [Polynomial Degree]: Must be a positive Integer i,\n with 0 < i < 10.");
                }
                else if (polyDeg <= 0 || polyDeg >= 10)
                {
                    throw new ArgumentException("Invalid setting for [Polynomial Degree]: Must be a positive Integer i,\n with 0 < i < 10.");
                }
                else
                {
                    _polyDegree = polyDeg;
                }
            }

            // Complexity
            if (tbComplexity.Enabled)
            {
                double complexity = (double)tbComplexity.Value;
                if (!Double.TryParse(tbComplexity.Text, out complexity))
                {
                    throw new ArgumentException("Invalid setting for [Complexity]: Must be a positive Number.");
                }
                else
                {
                     complexity = (double)tbComplexity.Value;
                }


                if (complexity <= 0)
                {
                    throw new ArgumentException("Invalid setting for [Complexity]: Must be a positive Number.");
                }
                else
                {
                    this._complexity = complexity;
                }
            }

            // Tolerance
            if (tbTolerance.Enabled)
            {
                double tolerance = (double)tbTolerance.Value;
                if (!Double.TryParse(tbTolerance.Text, out tolerance))
                {
                    throw new ArgumentException("Invalid setting for [Tolerance]: Must be a positive Number.");
                }
                else
                {
                    tolerance = (double)tbTolerance.Value;
                }


                if (tolerance < 0)
                {
                    throw new ArgumentException("Invalid setting for [Tolerance]: Must be a positive Number.");
                }
                else
                {
                    this._tolerance = tolerance;
                }
            }

            // Weight Positive Class
            if (tbWeightsPositive.Enabled)
            {
                double weight = (double)tbWeightsPositive.Value;
                if (!Double.TryParse(tbWeightsPositive.Text, out weight))
                {
                    throw new ArgumentException("Invalid setting for [Positive Class Weight]: Must be a positive Number.");
                }
                else
                {
                    weight = (double)tbWeightsPositive.Value;
                }


                if (weight <= 0)
                {
                    throw new ArgumentException("Invalid setting for [Positive Class Weight]: Must be a positive Number.");
                }
                else
                {
                    this._weightPositiveClass = weight;
                }
            }

            // Weight Negative Class
            if (tbWeightsNegative.Enabled)
            {
                double weight = (double)tbWeightsNegative.Value;
                if (!Double.TryParse(tbWeightsNegative.Text, out weight))
                {
                    throw new ArgumentException("Invalid setting for [Negative Class Weight]: Must be a positive Number.");
                }
                else
                {
                    weight = (double)tbWeightsNegative.Value;
                }


                if (weight <= 0)
                {
                    throw new ArgumentException("Invalid setting for [Negative Class Weight]: Must be a positive Number.");
                }
                else
                {
                    this._weightNegativeClass = weight;
                }
            }

            // Computed Weights
            if (checkClassProportions.Checked)
                this._useComputedWeights = true;
            else
                this._useComputedWeights = false;

            // Heuristical Complexity
            if (checkUseHeuristics.Checked)
                this._useHeuristicalComplexity = true;
            else
                this._useHeuristicalComplexity = false;

            // CV Num Folds
            if (tbNumFolds.Enabled)
            {
                int folds;
                if (!Int32.TryParse(tbNumFolds.Text, out folds))
                {
                    throw new ArgumentException("Invalid setting for [Folds]: Must be an integer between 1 and 20.");
                }
                else if (folds < 1 || folds > 20)
                {
                    throw new ArgumentException("Invalid setting for [Folds]: Must be an integer between 1 and 20.");
                }
                {
                    _numFolds = folds;
                }
            }

            // CV Num Folds
            if (tbNumRuns.Enabled)
            {
                int runs;
                if (!Int32.TryParse(tbNumRuns.Text, out runs))
                {
                    throw new ArgumentException("Invalid setting for [Runs]: Must be an integer between 1 and 10.");
                }
                else if (runs < 1 || runs > 100)
                {
                    throw new ArgumentException("Invalid setting for [Runs]: Must be an integer between 1 and 20.");
                }
                {
                    _numRuns = runs;
                }
            }

            // Active Features
            if (_activeFeatures.Count < 1)
                throw new ArgumentException("Invalid setting for [active features]: You must select at least one active feature.");
        }

        private void updateConfiguration()
        {
            // kernel
            _configuration.Kernel = _kernel;
            _configuration.GaussianSigma = _gaussianSigma;
            _configuration.LaplacianSigma = _laplacianSigma;
            _configuration.PolynomialConstant = _polyConst;
            _configuration.PolynomialDegree = _polyDegree;
            _configuration.SigmoidAlpha = _sigmoidAlpha;
            _configuration.ActiveFeatures = _activeFeatures;
            _configuration.SigmoidConstant = _sigmoidConst;

            // optionals
            _configuration.OptDouble1 = _optDouble1;
            _configuration.OptDouble2 = _optDouble2;
            _configuration.OptInt1 = _optInt1;
            _configuration.OptInt2 = _optInt2;

            // smo
            _configuration.Complexity = _complexity;
            _configuration.Tolerance = _tolerance;
            _configuration.UseComputedWeights = _useComputedWeights;
            _configuration.UseHeuristicalComplexity = _useHeuristicalComplexity;
            _configuration.WeightPositiveClass = _weightPositiveClass;
            _configuration.WeightNegativeClass = _weightNegativeClass;

            // cv
            _configuration.CrossValidationNumFolds = _numFolds;
            _configuration.CrossValidationNumRuns = _numRuns;

            //_activeFeatures.ForEach(feature => Console.WriteLine(feature + ", "));
        }

        private List<String> matchInFeatures(String expression)
        {
            List<String> selection = new List<String>();
            foreach (object item in lbActiveFeatures.Items)
            {
                try
                {
                    String itemName = item.ToString();
                    if (System.Text.RegularExpressions.Regex.Match(itemName, expression).Success)
                    {
                        //Console.WriteLine(itemName);
                        selection.Add(itemName);
                    }
                }
                catch (Exception) { }
            }

            foreach (object item in lbIgnoredFeatures.Items)
            {
                try
                {
                    String itemName = item.ToString();
                    if (System.Text.RegularExpressions.Regex.Match(itemName, expression).Success)
                    {
                        //Console.WriteLine(itemName);
                        selection.Add(itemName);
                    }
                }
                catch (Exception) { }
            }

            return selection;
        }

        private List<String> searchInFeatures(String searchValue)
        {
            List<String> selection = new List<String>();
            foreach (object item in lbActiveFeatures.Items)
            {
                try
                {
                    String itemName = item.ToString();
                    if (itemName.ToLower().Contains(searchValue.ToLower()))
                    {
                        //Console.WriteLine(itemName);
                        selection.Add(itemName);
                    }
                }
                catch (Exception) { }
            }

            foreach (object item in lbIgnoredFeatures.Items)
            {
                try
                {
                    String itemName = item.ToString();
                    if (itemName.ToLower().Contains(searchValue.ToLower()))
                    {
                        //Console.WriteLine(itemName);
                        selection.Add(itemName);
                    }
                }
                catch (Exception) { }
            }

            return selection;
        }

        private void buttonFilterSelect_Click(object sender, EventArgs e)
        {
            selectFeaturesByName(matchInFeatures(tbFilterExpression.Text));
        }

        private void buttonSearchFilter_Click(object sender, EventArgs e)
        {
            selectFeaturesByName(searchInFeatures(tbFilterExpression.Text));
        }

        private void selectFeaturesByName(List<String> featureNames)
        {
            Utility.writeToConsole(featureNames.ToArray());

            lbActiveFeatures.SelectedItems.Clear();
            lbIgnoredFeatures.SelectedItems.Clear();

            featureNames.ForEach(name =>
                {
                    
                    if (lbActiveFeatures.Items.Contains(name))
                    {
                        if (!lbActiveFeatures.SelectedItems.Contains(name))
                            lbActiveFeatures.SelectedItems.Add(name);
                    }

                    if (lbIgnoredFeatures.Items.Contains(name))
                    {
                        if (!lbIgnoredFeatures.SelectedItems.Contains(name))
                            lbIgnoredFeatures.SelectedItems.Add(name);
                    }
                });
        }

        private void checkUseHeuristics_CheckedChanged(object sender, EventArgs e)
        {
            if (checkUseHeuristics.Checked)
            {
                setUseHeuristics(true);
            }
            else
            {
                setUseHeuristics(false);
            }
        }

        private void checkClassProportions_CheckedChanged(object sender, EventArgs e)
        {
            if (this.checkClassProportions.Checked)
            {
                setUseComputedWeights(true);
            }
            else
            {
                setUseComputedWeights(false);
            }
        }

        private void setUseHeuristics(bool value)
        {
            if (value)
            {
                this.checkUseHeuristics.Checked = true;
                this.tbComplexity.Enabled = false;
            }
            else
            {
                this.checkUseHeuristics.Checked = false;
                this.tbComplexity.Enabled = true;
            }
        }

        private void setUseComputedWeights(bool value)
        {
            if (value)
            {
                this.checkClassProportions.Checked = true;
                this.tbWeightsNegative.Enabled = false;
                this.tbWeightsPositive.Enabled = false;
            }
            else
            {
                this.checkClassProportions.Checked = false;
                this.tbWeightsNegative.Enabled = true;
                this.tbWeightsPositive.Enabled = true;
            }
        }
    }
}

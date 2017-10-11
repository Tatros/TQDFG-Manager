using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StatsAnalyzer.MachineLearning
{
    internal enum Kernel { GAUSSIAN, POLYNOMIAL, LAPLACIAN, SIGMOID, Additive, Anova, Cauchy, Dirichlet, Bessel, ChiSquare, TimeWarping, Linear, Log, Triangular, SymmetricTriangular, Wave };

    internal class KernelSVMConfiguration
    {
        // kernel properties
        private Kernel _kernel;
        private double _gaussianSigma = 0.1;
        private double _polyConst = 1.0;
        private int _polyDegree = 2;
        private double _laplacianSigma = 0.1;
        private double _sigmoidAlpha = 0.1;
        private double _sigmoidConst = 1.0;
        private int _optInt1 = 1;
        private int _optInt2 = 1;
        private double _optDouble1 = 0.1;
        private double _optDouble2 = 0.1;

        // smo properties
        private double _weightPositiveClass = 1.0;
        private double _weightNegativeClass = 1.0;
        private double _complexity = 100.0;
        private double _tolerance = 0.4;
        private bool _useHeuristicalComplexity = false;
        private bool _useComputedWeights = false;

        // cross validation
        private int _numFolds = 5;
        private int _numRuns = 3;

        // features
        private List<String> _activeFeatures;

        internal KernelSVMConfiguration(List<String> activeFeatures) 
        {
            if (activeFeatures != null)
                _activeFeatures = activeFeatures;
            else
                _activeFeatures = new List<string>();
        }

        internal List<String> getAvailableKernels()
        {
            return Enum.GetNames(typeof(Kernel)).ToList();
        }

        internal double GaussianSigma
        {
            get { return _gaussianSigma; }
            set { this._gaussianSigma = value; }
        }

        internal double PolynomialConstant
        {
            get { return _polyConst; }
            set { this._polyConst = value; }
        }

        internal int PolynomialDegree
        {
            get { return _polyDegree; }
            set { this._polyDegree = value; }
        }

        internal double LaplacianSigma
        {
            get { return _laplacianSigma; }
            set { this._laplacianSigma = value; }
        }

        internal double SigmoidAlpha
        {
            get { return _sigmoidAlpha; }
            set { this._sigmoidAlpha = value; }
        }

        internal double SigmoidConstant
        {
            get { return _sigmoidConst; }
            set { this._sigmoidConst = value; }
        }

        internal double OptDouble1
        {
            get { return _optDouble1; }
            set { this._optDouble1 = value; }
        }

        internal double OptDouble2
        {
            get { return _optDouble2; }
            set { this._optDouble2 = value; }
        }

        internal int OptInt1
        {
            get { return _optInt1; }
            set { this._optInt1 = value; }
        }

        internal int OptInt2
        {
            get { return _optInt2; }
            set { this._optInt2 = value; }
        }

        internal Kernel Kernel
        {
            get { return this._kernel; }
            set { this._kernel = value; }
        }

        internal bool UseComputedWeights
        {
            get { return this._useComputedWeights; }
            set { this._useComputedWeights = value; }
        }

        internal bool UseHeuristicalComplexity
        {
            get { return this._useHeuristicalComplexity; }
            set { this._useHeuristicalComplexity = value; }
        }

        internal double WeightPositiveClass
        {
            get { return _weightPositiveClass; }
            set { this._weightPositiveClass = value; }
        }

        internal double WeightNegativeClass
        {
            get { return _weightNegativeClass; }
            set { this._weightNegativeClass = value; }
        }

        internal double Complexity
        {
            get { return _complexity; }
            set { this._complexity = value; }
        }

        internal double Tolerance
        {
            get { return _tolerance; }
            set { this._tolerance = value; }
        }

        internal int CrossValidationNumFolds
        {
            get { return _numFolds; }
            set { this._numFolds = value; }
        }

        internal int CrossValidationNumRuns
        {
            get { return _numRuns; }
            set { this._numRuns = value; }
        }

        internal List<String> ActiveFeatures
        {
            get
            {
                if (_activeFeatures == null)
                    _activeFeatures = new List<string>();

                return _activeFeatures;
            }
            set { this._activeFeatures = value; }
        }
    }
}

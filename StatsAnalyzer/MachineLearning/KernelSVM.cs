using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using StatsAnalyzer.Model;
using Accord;
using Accord.Math;
using Accord.MachineLearning;
using Accord.MachineLearning.VectorMachines;
using Accord.MachineLearning.VectorMachines.Learning;
using Accord.Statistics.Kernels;
using Accord.Statistics.Analysis;

namespace StatsAnalyzer.MachineLearning
{
    internal class KernelSVM
    {
        double[][] _inputs;
        int[] _outputs;
        IKernel _kernel;
        private KernelSupportVectorMachine _svm = null;
        private SequentialMinimalOptimization _smo = null;
        private IModel _model;
        private KernelSVMConfiguration _configuration;

        List<String> _featureColumns;
        List<String> _classifiedFeatureColumns;
        double[,] _classifiedData;
        double[,] _data;
        String[] _names;


        internal KernelSVM(IModel model, KernelSVMConfiguration configuration, List<String> nodeNames = null)
        {
            this._model = model;
            this._configuration = configuration;

            Console.WriteLine("New Kernel SVM (" + _configuration.Kernel.ToString() + "), Active Features: ");
            Utility.writeToConsole(_configuration.ActiveFeatures.ToArray());
            _featureColumns = new List<String>(_configuration.ActiveFeatures);
            _classifiedFeatureColumns = new List<String>(_configuration.ActiveFeatures); _classifiedFeatureColumns.Add("class");

            _classifiedData = _model.ClassifiedFeatureTable.ToMatrix<double>(_classifiedFeatureColumns.ToArray());
            _data = _model.ClassifiedFeatureTable.ToMatrix<double>(_featureColumns.ToArray());
            _names = _model.DataTable.ToMatrix<String>("Name").GetColumn(0);
            

            /*
            Utility.writeToConsole(_featureColumns.ToArray());
            Console.WriteLine("===============================================================================================");
            Utility.writeToConsole(_data); */

            /*
            Utility.writeToConsole(_classifiedFeatureColumns.ToArray());
            Console.WriteLine("===============================================================================================");
            Utility.writeToConsole(_classifiedData); */

            loadData(nodeNames);
            createSVM();
        }

        private void loadData(List<String> nodeNames)
        {
            _inputs = _data.ToArray();

            // transforms Inputs
            for (int j = 0; j < _inputs.Length; j++)
            {
                for (int i = 0; i < _inputs[j].Length; i++)
                {
                    //_inputs[j][i] = Math.Round(_inputs[j][i], _configuration.OptInt2);
                    _inputs[j][i] = Math.Round(_inputs[j][i], 2);
                    if (_configuration.UseHeuristicalComplexity)
                    {
                        if (_inputs[j][i] < 0)
                            _inputs[j][i] = 0;
                    }
                    //Console.Write(_inputs[j][i].ToString() + "    ");
                }
                //Console.WriteLine();
            }

            //Utility.writeToConsole(_inputs);
            List<int> classes = new List<int>();
            //Utility.writeToConsole<double>(_inputs);
            //Console.WriteLine(_inputs[0].Length);

            // for each value in the last column (class column)
            _classifiedData.GetColumn(_classifiedFeatureColumns.Count - 1).ToList().ForEach(val =>
            {
                classes.Add(Convert.ToInt32(val));
            });

            _outputs = classes.ToArray();

            // Utility.writeToConsole<int>(_outputs);
        }

        private void createSVM()
        {
            createKernel();

            // Create SVM for n input variables, where n is the number of features (columns)
            _svm = new KernelSupportVectorMachine(_kernel, inputs: _inputs[0].Length);

            // Create an instance of the SMO learning algorithm
            _smo = new SequentialMinimalOptimization(_svm, _inputs, _outputs)
            {
                // Set learning parameters
                
                Tolerance = _configuration.Tolerance,
                PositiveWeight = _configuration.WeightPositiveClass,
                NegativeWeight = _configuration.WeightNegativeClass,
                UseClassProportions = _configuration.UseComputedWeights,
                UseComplexityHeuristic = true
            };

            //if (!_configuration.UseHeuristicalComplexity)
                _smo.Complexity = _configuration.Complexity;

            Console.WriteLine("SVM> C=" + _smo.Complexity + ", Tolerance=" + _smo.Tolerance + ", PosW=" + _smo.PositiveWeight + ", NegW: " + _smo.NegativeWeight);
        }

        public void trainSVM()
        {
            try
            {
                // Run
                double error = _smo.Run();

                Console.WriteLine("Training complete!");
            }
            catch (ConvergenceException)
            {
                Console.WriteLine("Convergence could not be attained. The learned machine might still be usable.");
            }
        }

        public void testSVM()
        {
            if (_svm == null)
                return;

            int[] output = new int[_outputs.Length];

            // Compute the machine outputs
            for (int i = 0; i < _outputs.Length; i++)
            {
                double actual = _outputs[i];
                double predicted = _svm.Compute(_inputs[i]);
                // System.Console.WriteLine(Math.Sign(actual) + "   " + _names[i] + "   =>   " + predicted + "   =>   " + Math.Sign(predicted));
                output[i] = System.Math.Sign(predicted);
            }

            // Use confusion matrix to compute some performance metrics
            ConfusionMatrix confusionMatrix = new ConfusionMatrix(output, _outputs, 1, -1);
            FormDataView<double> f = new FormDataView<double>(new[] { confusionMatrix });
            f.Show();
            //dgvPerformance.DataSource = new[] { confusionMatrix };
        }

        internal List<CrossValidationResult> performCrossValidation()
        {
            Console.WriteLine("Starting Cross Validation - Number of Runs: " + _configuration.CrossValidationNumRuns);
            ConcurrentBag<CrossValidationResult> cvResults = new ConcurrentBag<CrossValidationResult>();
            
            for (int i = 1; i <= _configuration.CrossValidationNumRuns; i++)
            {
                Console.WriteLine("Performing Run " + i + " of " + _configuration.CrossValidationNumRuns);
                CrossValidationResult result = _performCrossValidation();
                cvResults.Add(result);
            }

            Console.WriteLine("Done. CV Results: " + cvResults.Count);
            return cvResults.ToList<CrossValidationResult>();
        }

        private CrossValidationResult _performCrossValidation()
        {
            // Create a new Cross-validation algorithm passing the data set size and the number of folds
            var crossvalidation = new CrossValidation(size: _inputs.Length, folds: _configuration.CrossValidationNumFolds);
            Console.WriteLine("Num Samples: " + crossvalidation.Samples);

             
            // ConcurrentDictionary<string, NodeClassification> nodeClassifications = new ConcurrentDictionary<string, NodeClassification>();
            CrossValidationResult cvResult = new CrossValidationResult();

            // Define a fitting function using Support Vector Machines. The objective of this
            // function is to learn a SVM in the subset of the data indicated by cross-validation.
            crossvalidation.Fitting = delegate(int k, int[] indicesTrain, int[] indicesValidation)
            {
                // The fitting function is passing the indices of the original set which
                // should be considered training data and the indices of the original set
                // which should be considered validation data.

                // Lets now grab the training data:
                var trainingInputs = _inputs.Submatrix(indicesTrain);
                var trainingOutputs = _outputs.Submatrix(indicesTrain);
                var trainingNames = _names.Submatrix(indicesTrain);

                // And now the validation data:
                var validationInputs = _inputs.Submatrix(indicesValidation);
                var validationOutputs = _outputs.Submatrix(indicesValidation);
                var validationNames = _names.Submatrix(indicesValidation);

                if (validationNames.Intersect<String>(trainingNames).Count() > 0)
                {
                    Console.WriteLine("Warning, Training and Validation Set not disjunct.");
                    Utility.writeToConsole<String>(validationNames.Intersect(trainingNames).ToArray());
                }

                //Console.WriteLine("=== TRAINING ===");
                //Utility.writeToConsole<String>(trainingNames);
                //Utility.writeToConsole<int>(trainingInputs);
                //Utility.writeToConsole<int>(trainingOutputs);

                //Console.WriteLine("=== VALIDATION ===");
                //Utility.writeToConsole<String>(validationNames);
                //Utility.writeToConsole<double>(validationInputs);
                //Utility.writeToConsole<int>(validationOutputs);

                //Console.WriteLine("=== INTERSECTION ===");
                //String[] intersection = Utility.Intersection(validationNames, trainingNames);
                //Utility.writeToConsole<String>(intersection);

                // Create a Kernel Support Vector Machine to operate on the set
                var svm = new KernelSupportVectorMachine(_kernel, _inputs[0].Length);
                //Accord.MachineLearning.Boosting.Boost<KernelSupportVectorMachine> b;
                // Create a training algorithm and learn the training data
                var smo = new SequentialMinimalOptimization(svm, trainingInputs, trainingOutputs)
                {
                    // Set learning parameters
                    Tolerance = _configuration.Tolerance,
                    PositiveWeight = _configuration.WeightPositiveClass,
                    NegativeWeight = _configuration.WeightNegativeClass,
                    UseClassProportions = _configuration.UseComputedWeights,
                    UseComplexityHeuristic = true
                };

                //if (!_configuration.UseHeuristicalComplexity)
                    _smo.Complexity = _configuration.Complexity;

                double trainingError = smo.Run();

                // Now we can compute the validation error on the validation data:
                double validationError = smo.ComputeError(validationInputs, validationOutputs);

                // Predictions & Confusion Matrix
                List<int> predictions = new List<int>();
                List<double> rawPredictions = new List<double>();

                int index = 0;
                foreach (double[] inputVector in validationInputs)
                {
                    // Compute the decision output for vector
                    // Console.WriteLine(validationNames[index]);
                    // Utility.writeToConsole<double>(inputVector);
                    double rawPrediction = svm.Compute(inputVector);
                    rawPredictions.Add(rawPrediction);

                    int prediction = rawPrediction > 0.0d ? +1 : -1;
                    predictions.Add(prediction);

                    try
                    {
                        // Update Node Classifications
                        cvResult.AddOrUpdateClassification(validationNames[index], rawPrediction, validationOutputs[index]);
                    }
                    catch (IndexOutOfRangeException)
                    {
                        Console.WriteLine("Failed to update CV Result: Input Vector larger than Name Vector.");
                    }

                    index++;
                }

                ConfusionMatrix confusionMatrix = new ConfusionMatrix(predictions.ToArray(), validationOutputs, 1, -1);
                cvResult.ConfusionMatrices.Add(confusionMatrix);

                // Return a new information structure containing the model and the errors achieved.
                return new CrossValidationValues(svm, trainingError, validationError);
            };


            // Compute the cross-validation
            var result = crossvalidation.Compute();

            // Finally, access the measured performance.
            double trainingErrors = result.Training.Mean;
            double validationErrors = result.Validation.Mean;

            Console.WriteLine("Training Errors: " + result.Training.Mean);
            Console.WriteLine("Validation Errors: " + result.Validation.Mean);

            ConfusionMatrix aggregatedConfusionMatrix = ConfusionMatrix.Combine(cvResult.ConfusionMatrices.ToArray());

            return cvResult;
        }

        private void createKernel()
        {
            Console.WriteLine("Creating Kernel with (" + _configuration.OptInt1 + ", " + _configuration.OptInt2 + ", " + _configuration.OptDouble1 + ", " + _configuration.OptDouble2 + ").");
            switch (_configuration.Kernel)
            {
                case Kernel.GAUSSIAN:
                    {
                        this._kernel = new Gaussian(_configuration.GaussianSigma);
                        break;
                    }
                case Kernel.LAPLACIAN:
                    {
                        this._kernel = new Laplacian(_configuration.LaplacianSigma);
                        break;
                    }
                case Kernel.POLYNOMIAL:
                    {
                        this._kernel = new Polynomial(_configuration.PolynomialDegree, _configuration.PolynomialConstant);
                        break;
                    }
                case Kernel.SIGMOID:
                    {
                        this._kernel = new Sigmoid(_configuration.SigmoidAlpha, _configuration.SigmoidConstant);
                        break;
                    }
                case Kernel.Additive:
                    {
                        this._kernel =  new Additive(new IKernel[] {new Gaussian(_configuration.OptDouble1), new Laplacian(_configuration.OptDouble2)});
                        break;
                    }
                case Kernel.Anova:
                    {
                        // vectorLength, sequenceLength
                        this._kernel = new Anova(_configuration.OptInt1, _configuration.OptInt2);
                        break;
                    }
                case Kernel.Bessel:
                    {
                        // order, sigma
                        this._kernel = new Accord.Statistics.Kernels.Bessel(_configuration.OptInt1, _configuration.OptDouble1);
                        break;
                    }
                case Kernel.Cauchy:
                    {
                        // sigma
                        this._kernel = new Accord.Statistics.Kernels.Cauchy(_configuration.OptDouble1);
                        break;
                    }
                case Kernel.ChiSquare:
                    {
                        this._kernel = new Accord.Statistics.Kernels.ChiSquare();
                        break;
                    }
                case Kernel.Linear:
                    {
                        this._kernel = new Accord.Statistics.Kernels.Linear();
                        break;
                    }
                case Kernel.Log:
                    {
                        // degree
                        this._kernel = new Accord.Statistics.Kernels.Log(_configuration.OptDouble1);
                        break;
                    }
                case Kernel.SymmetricTriangular:
                    {
                        // gamma
                        this._kernel = new Accord.Statistics.Kernels.SymmetricTriangle(_configuration.OptDouble1);
                        break;
                    }
                case Kernel.TimeWarping:
                    {
                        // length of feature vector in each sequence
                        this._kernel = new Accord.Statistics.Kernels.DynamicTimeWarping(_configuration.OptInt1);
                        break;
                    }
                case Kernel.Dirichlet:
                    {
                        // dimension
                        this._kernel = new Accord.Statistics.Kernels.Dirichlet(_configuration.OptInt1);
                        break;
                    }
                case Kernel.Triangular:
                    {
                        // degree
                        this._kernel = new Accord.Statistics.Kernels.Power(_configuration.OptDouble1);
                        break;
                    }
                case Kernel.Wave:
                    {
                        // sigma
                        this._kernel = new Accord.Statistics.Kernels.Wave(_configuration.OptDouble1);
                        break;
                    }
            }
            
        }
    }
}

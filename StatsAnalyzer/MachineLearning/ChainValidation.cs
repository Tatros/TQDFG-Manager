using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StatsAnalyzer;
using StatsAnalyzer.Model;
using System.Windows.Forms;

namespace StatsAnalyzer.MachineLearning
{
    // Static_Statistical_GLF_EXP, Static_Statistical_GLF, Static_Statistical_GLF3, Static_Statistical_GLF4, Static_Statistical_GLF2, Static_Statistical_Gaussian_1, Experimental, Static_Statistical_Gaussian_Laplacian, Static_Statistical_Gaussian_Features, Static_Statistical_Gaussian_Laplacian_Features, All_Permutations
    internal enum MODE { 
        // CMB Custom
        CMB_A_DS_LAP,

        // static
        STATIC_LINEAR,
        STATIC_GAU_005, // sigma
        STATIC_GAU_010,
        STATIC_GAU_030,
        STATIC_GAU_060,
        STATIC_LAP_005, // sigma
        STATIC_LAP_010,
        STATIC_LAP_030,
        STATIC_LAP_060,
        STATIC_SIG_005, // alpha
        STATIC_SIG_010,
        STATIC_SIG_030,
        STATIC_SIG_060,
        STATIC_CAU_005, // sigma
        STATIC_CAU_010,
        STATIC_CAU_030,
        STATIC_CAU_060,
        STATIC_ANOVA_16_2, //vecLen=16, seqLen=4
        STATIC_ANOVA_16_4, //vecLen=16, seqLen=4
        STATIC_ANOVA_16_8,
        STATIC_ANOVA_16_12,
        STATIC_ANOVA_16_14,
        STATIC_TW_005, // alpha
        STATIC_TW_010,
        STATIC_TW_030,
        STATIC_TW_060,
        STATIC_TRI_005, // gamma
        STATIC_TRI_010,
        STATIC_TRI_030,
        STATIC_TRI_060,
        STATIC_TRI_070,
        STATIC_TRI_080,

        // statistical
        STATISTICAL_LINEAR,
        STATISTICAL_GAU_005, // sigma
        STATISTICAL_GAU_010,
        STATISTICAL_GAU_030,
        STATISTICAL_GAU_060,
        STATISTICAL_LAP_005, // sigma
        STATISTICAL_LAP_010,
        STATISTICAL_LAP_030,
        STATISTICAL_LAP_060,
        STATISTICAL_SIG_005, // alpha
        STATISTICAL_SIG_010,
        STATISTICAL_SIG_030,
        STATISTICAL_SIG_060,
        STATISTICAL_CAU_005, // sigma
        STATISTICAL_CAU_010,
        STATISTICAL_CAU_030,
        STATISTICAL_CAU_060,
        STATISTICAL_ANOVA_16_2, //vecLen=16, seqLen=4
        STATISTICAL_ANOVA_16_4, //vecLen=16, seqLen=4
        STATISTICAL_ANOVA_16_8,
        STATISTICAL_ANOVA_16_12,
        STATISTICAL_ANOVA_16_14,
        STATISTICAL_TW_005, // alpha
        STATISTICAL_TW_010,
        STATISTICAL_TW_030,
        STATISTICAL_TW_060,
        STATISTICAL_TRI_005, // gamma
        STATISTICAL_TRI_010,
        STATISTICAL_TRI_030,
        STATISTICAL_TRI_060,
        STATISTICAL_TRI_070,
        STATISTICAL_TRI_080,

        // sampled
        SAMPLED_LINEAR,
        SAMPLED_GAU_005, // sigma
        SAMPLED_GAU_010,
        SAMPLED_GAU_030,
        SAMPLED_GAU_060,
        SAMPLED_LAP_005, // sigma
        SAMPLED_LAP_010,
        SAMPLED_LAP_030,
        SAMPLED_LAP_060,
        SAMPLED_SIG_005, // alpha
        SAMPLED_SIG_010,
        SAMPLED_SIG_030,
        SAMPLED_SIG_060,
        SAMPLED_CAU_005, // sigma
        SAMPLED_CAU_010,
        SAMPLED_CAU_030,
        SAMPLED_CAU_060,
        SAMPLED_ANOVA_16_2, //vecLen=16, seqLen=4
        SAMPLED_ANOVA_16_4, //vecLen=16, seqLen=4
        SAMPLED_ANOVA_16_8,
        SAMPLED_ANOVA_16_12,
        SAMPLED_ANOVA_16_14,
        SAMPLED_TW_005, // alpha
        SAMPLED_TW_010,
        SAMPLED_TW_030,
        SAMPLED_TW_060,
        SAMPLED_TRI_005, // gamma
        SAMPLED_TRI_010,
        SAMPLED_TRI_030,
        SAMPLED_TRI_060,
        SAMPLED_TRI_070,
        SAMPLED_TRI_080,
        
        // combined
        COMB_MIN_MAX_GAU_TRI, COMB_MIN_MAX_GAU_CAU, COMB_MIN_MAX_LAP_CAU, COMB_MIN_MAX_LAP_TRI, COMB_MIN_MAX_GAU_LAP, COMB_MIN_MAX_GAU_LAP_TRI, COMB_MIN_MAX_GAU_LAP_TRI_CAU, 
        STAT_MIN_MAX_GAU_TRI, STAT_MIN_MAX_GAU_CAU, STAT_MIN_MAX_LAP_CAU, STAT_MIN_MAX_LAP_TRI, STAT_MIN_MAX_GAU_LAP, STAT_MIN_MAX_GAU_LAP_TRI, STAT_MIN_MAX_GAU_LAP_TRI_CAU,
        SAMPLED_GAU_TRI, SAMPLED_GAU_CAU, SAMPLED_LAP_CAU, SAMPLED_LAP_TRI, SAMPLED_GAU_LAP, SAMPLED_GAU_LAP_TRI, SAMPLED_GAU_LAP_TRI_CAU,
        STATIC_GAU_TRI, STATIC_GAU_CAU, STATIC_LAP_CAU, STATIC_LAP_TRI, STATIC_GAU_LAP, STATIC_GAU_LAP_TRI, STATIC_GAU_LAP_TRI_CAU
    } // Static_Statistical_Laplacian

    internal class ChainValidation
    {
        ConcurrentDictionary<string, List<CrossValidationResult>> _results;
        ChainValidationConfiguration _configuration;
        List<String> _activeFeatures;
        Analyzer _analyzer;
        MODE _mode;
        int _numRuns = 2;
        int _numFolds = 5;
        double _tolerance = 0.5;
        long _timeElapsedMS = 0;
        long _memoryStart = 0;
        long _memoryMax = 0;
        System.Diagnostics.Process _currentProcess = null;
        System.Diagnostics.Stopwatch _stopwatch = null;

        internal ChainValidation(Analyzer analyzer, ChainValidationConfiguration configuration, KernelSVMConfiguration svmConfig)
        {
            this._results = new ConcurrentDictionary<string, List<CrossValidationResult>>();
            this._configuration = configuration;
            this._analyzer = analyzer;
            this._activeFeatures = svmConfig.ActiveFeatures;
            this._numFolds = svmConfig.CrossValidationNumFolds;
            this._numRuns = svmConfig.CrossValidationNumRuns;
            this._tolerance = svmConfig.Tolerance;
            loadConfiguration();
        }

        private void initPerformanceStats()
        {
            if (_stopwatch != null)
                _stopwatch.Stop();

            
            this._stopwatch = new System.Diagnostics.Stopwatch();
            this._currentProcess = System.Diagnostics.Process.GetCurrentProcess();
            this._currentProcess.Refresh();
            this._memoryStart = _currentProcess.WorkingSet64;
            this._memoryMax = _memoryStart;
            _stopwatch.Start();
        }
        private void updatePerformanceStats()
        {
            if (this._currentProcess != null && this._stopwatch != null)
            {
                this._currentProcess.Refresh();

                long memory = _currentProcess.WorkingSet64;
                this._timeElapsedMS = this._stopwatch.ElapsedMilliseconds;

                Console.WriteLine("STATS (memStart = " + this._memoryStart + ", memoryMax: " + this._memoryMax + ", memoryNow: " + memory + ", time: " + _timeElapsedMS + ")");
                if (memory > this._memoryMax)
                    this._memoryMax = memory;
            }
        }

        private void updateTasks(List<Task> tasks)
        {
            foreach (Task t in tasks)
            {
                updatePerformanceStats();
                Console.WriteLine("Task {0} Status: {1}", t.Id, t.Status);
            }

        }

        private void loadConfiguration()
        {
            this._mode = _configuration.ChainValidationMode;
        }

        internal bool createInstance(String name, MODEL modelType, double complexity, int numFolds, int numCVs, double gaussianSigma, double laplacianSigma, Kernel kernel, List<string> activeFeatures = null, double optDouble1 = 1.0, double optDouble2 = 1.0, int optInt1 = 1, int optInt2 = 1)
        {
            Console.WriteLine("STARTING CROSS VALIDATION INSTANCE: " + name);
            IModel model = _analyzer.getModel(modelType);
            KernelSVMConfiguration kernelSVMconfig = new KernelSVMConfiguration(model.getFeatureNames());
            kernelSVMconfig.Complexity = complexity;
            kernelSVMconfig.Tolerance = this._tolerance;
            kernelSVMconfig.CrossValidationNumFolds = numFolds;
            kernelSVMconfig.CrossValidationNumRuns = numCVs;
            kernelSVMconfig.GaussianSigma = gaussianSigma;
            kernelSVMconfig.LaplacianSigma = laplacianSigma;
            kernelSVMconfig.SigmoidAlpha = gaussianSigma;
            kernelSVMconfig.Kernel = kernel;

            kernelSVMconfig.OptDouble1 = optDouble1;
            kernelSVMconfig.OptDouble2 = optDouble2;
            kernelSVMconfig.OptInt1 = optInt1;
            kernelSVMconfig.OptInt2 = optInt2;

            if (activeFeatures != null)
                kernelSVMconfig.ActiveFeatures = activeFeatures;

            // create & run Instance for given model & config
            ChainValidationInstance instance = new ChainValidationInstance(model, kernelSVMconfig);
            instance.run();
            // obtain results
            return _results.TryAdd(name, instance.CVResults);
        }

        internal void run()
        {
            initPerformanceStats();

            _results.Clear();

            switch (_mode)
            {
                /*
                case MODE.All_Permutations:
                    {
                        List<double> gaussianSigmas = new List<double>() { 0.1, 0.05, 0.01 };
                        List<double> laplacianSigmas = new List<double>() { 0.5, 0.6, 0.7 };
                        

                        break;
                    }
                case MODE.Static_Statistical_Gaussian_1:
                    {
                        List<Task> tasks = new List<Task>();

                        // compute results for static model and standard gaussian ksvm
                        Task t1 = Task.Run(() => createInstance("static", MODEL.STATIC, 100.0, 5, 1, 0.1, 0.1, Kernel.GAUSSIAN));
                        Task t2 = Task.Run(() => createInstance("statistical", MODEL.STATISTICAL, 100.0, 5, 1, 0.1, 0.1, Kernel.GAUSSIAN));
                        tasks.Add(t1);
                        tasks.Add(t2);
                        Task.WaitAll(tasks.ToArray());
                        updateTasks(tasks);
                            
                        break;
                    }

                case MODE.Experimental:
                    {
                        List<string> featuresStatistical = new List<string>();
                        IModel modelStatistical = _analyzer.getModel(MODEL.STATISTICAL);
                        foreach (var feature in modelStatistical.getFeatureNames())
                        {
                            if (feature.Contains("_Max") || feature.Contains("_Min"))
                                featuresStatistical.Add(feature);
                        }

                        int numIterations = 3;
                        double stepLaplacian = 0.03;
                        double stepGaussian = 0.01;
                        double laplacianSigma = 0.4;
                        double gaussianSigma = 0.01;
                        double complexity = 100.0;
                        int folds = 5;
                        int runs = 1;

                        List<Task> tasks = new List<Task>();
                        for (int i = 0; i < numIterations; i++)
                        {
                            int iteration = i;
                            // compute results for static model and standard gaussian ksvm
                            Task t = Task.Run(() => createInstance("statisticalGaussian" + iteration, MODEL.STATISTICAL, complexity, folds, runs, gaussianSigma, laplacianSigma, Kernel.GAUSSIAN, featuresStatistical));
                            tasks.Add(t);

                            gaussianSigma += stepGaussian;
                            laplacianSigma += stepLaplacian;
                        }

                        Task.WaitAll(tasks.ToArray());
                        updateTasks(tasks);
                            

                        Console.WriteLine("NUM RESULTS: " + _results.Count);
                        break;
                    }

                case MODE.Static_Statistical_Gaussian_Features:
                    {
                        List<string> featuresStatistical = new List<string>();
                        IModel modelStatistical = _analyzer.getModel(MODEL.STATISTICAL);
                        foreach (var feature in modelStatistical.getFeatureNames())
                        {
                            if (feature.Contains("_Max") || feature.Contains("_Min"))
                                featuresStatistical.Add(feature);
                        }

                        Console.WriteLine("Features should be: ");
                        Utility.writeToConsole<string>(featuresStatistical.ToArray());

                        List<Task> tasks = new List<Task>();

                        // compute results for static model and standard gaussian ksvm
                        Task t1 = Task.Run(() => createInstance("static1", MODEL.STATIC, 100.0, 5, 1, 0.08, 0.08, Kernel.GAUSSIAN));
                        Task t2 = Task.Run(() => createInstance("statistical1", MODEL.STATISTICAL, 100.0, 5, 1, 0.08, 0.08, Kernel.GAUSSIAN, featuresStatistical));
                        tasks.Add(t1);
                        tasks.Add(t2);
                        Task.WaitAll(tasks.ToArray());
                        updateTasks(tasks);
                            

                        break;
                    }*/

                /* ------------------ BEGIN STATIC KERNEL MODES ------------------ */
                case MODE.STATIC_LINEAR:
                    {
                        List<Task> tasks = new List<Task>();
                        for (int i = 0; i < _numRuns; i++)
                        {
                            int c = i;
                            // compute results for static model and standard gaussian ksvm
                            Task t1 = Task.Run(() => createInstance("STATIC_LINEAR" + c, MODEL.STATIC, 100.0, this._numFolds, 1, 0.1, 0.6, Kernel.Linear, activeFeatures: this._activeFeatures));
                            tasks.Add(t1);
                        }
                        Task.WaitAll(tasks.ToArray());
                        updateTasks(tasks);

                        break;
                    }

                    /* GAUSSIAN */
                case MODE.STATIC_GAU_005:
                    {
                        List<Task> tasks = new List<Task>();
                        for (int i = 0; i < _numRuns; i++)
                        {
                            int c = i;
                            // compute results for static model and standard gaussian ksvm
                            Task t1 = Task.Run(() => createInstance("STATIC_GAU_005" + c, MODEL.STATIC, 100.0, this._numFolds, 1, 0.05, 0.05, Kernel.GAUSSIAN, activeFeatures: this._activeFeatures));
                            tasks.Add(t1);
                        }
                        Task.WaitAll(tasks.ToArray());
                        updateTasks(tasks);

                        break;
                    }
                case MODE.STATIC_GAU_010:
                    {
                        List<Task> tasks = new List<Task>();
                        for (int i = 0; i < _numRuns; i++)
                        {
                            int c = i;
                            // compute results for static model and standard gaussian ksvm
                            Task t1 = Task.Run(() => createInstance("STATIC_GAU_010" + c, MODEL.STATIC, 100.0, this._numFolds, 1, 0.1, 0.1, Kernel.GAUSSIAN, activeFeatures: this._activeFeatures));
                            tasks.Add(t1);
                        }
                        Task.WaitAll(tasks.ToArray());
                        updateTasks(tasks);

                        break;
                    }
                case MODE.STATIC_GAU_030:
                    {
                        List<Task> tasks = new List<Task>();
                        for (int i = 0; i < _numRuns; i++)
                        {
                            int c = i;
                            // compute results for static model and standard gaussian ksvm
                            Task t1 = Task.Run(() => createInstance("STATIC_GAU_030" + c, MODEL.STATIC, 100.0, this._numFolds, 1, 0.3, 0.3, Kernel.GAUSSIAN, activeFeatures: this._activeFeatures));
                            tasks.Add(t1);
                        }
                        Task.WaitAll(tasks.ToArray());
                        updateTasks(tasks);

                        break;
                    }
                case MODE.STATIC_GAU_060:
                    {
                        List<Task> tasks = new List<Task>();
                        for (int i = 0; i < _numRuns; i++)
                        {
                            int c = i;
                            // compute results for static model and standard gaussian ksvm
                            Task t1 = Task.Run(() => createInstance("STATIC_GAU_060" + c, MODEL.STATIC, 100.0, this._numFolds, 1, 0.6, 0.6, Kernel.GAUSSIAN, activeFeatures: this._activeFeatures));
                            tasks.Add(t1);
                        }
                        Task.WaitAll(tasks.ToArray());
                        updateTasks(tasks);

                        break;
                    }

                    /* LAPLACIAN */
                case MODE.STATIC_LAP_005:
                    {
                        List<Task> tasks = new List<Task>();
                        for (int i = 0; i < _numRuns; i++)
                        {
                            int c = i;
                            // compute results for static model and standard gaussian ksvm
                            Task t1 = Task.Run(() => createInstance("STATIC_LAP_005" + c, MODEL.STATIC, 100.0, this._numFolds, 1, 0.05, 0.05, Kernel.LAPLACIAN, activeFeatures: this._activeFeatures));
                            tasks.Add(t1);
                        }
                        Task.WaitAll(tasks.ToArray());
                        updateTasks(tasks);

                        break;
                    }
                case MODE.STATIC_LAP_010:
                    {
                        List<Task> tasks = new List<Task>();
                        for (int i = 0; i < _numRuns; i++)
                        {
                            int c = i;
                            // compute results for static model and standard gaussian ksvm
                            Task t1 = Task.Run(() => createInstance("STATIC_LAP_010" + c, MODEL.STATIC, 100.0, this._numFolds, 1, 0.1, 0.1, Kernel.LAPLACIAN, activeFeatures: this._activeFeatures));
                            tasks.Add(t1);
                        }
                        Task.WaitAll(tasks.ToArray());
                        updateTasks(tasks);

                        break;
                    }
                case MODE.STATIC_LAP_030:
                    {
                        List<Task> tasks = new List<Task>();
                        for (int i = 0; i < _numRuns; i++)
                        {
                            int c = i;
                            // compute results for static model and standard gaussian ksvm
                            Task t1 = Task.Run(() => createInstance("STATIC_LAP_030" + c, MODEL.STATIC, 100.0, this._numFolds, 1, 0.3, 0.3, Kernel.LAPLACIAN, activeFeatures: this._activeFeatures));
                            tasks.Add(t1);
                        }
                        Task.WaitAll(tasks.ToArray());
                        updateTasks(tasks);

                        break;
                    }
                case MODE.STATIC_LAP_060:
                    {
                        List<Task> tasks = new List<Task>();
                        for (int i = 0; i < _numRuns; i++)
                        {
                            int c = i;
                            // compute results for static model and standard gaussian ksvm
                            Task t1 = Task.Run(() => createInstance("STATIC_LAP_060" + c, MODEL.STATIC, 100.0, this._numFolds, 1, 0.6, 0.6, Kernel.LAPLACIAN, activeFeatures: this._activeFeatures));
                            tasks.Add(t1);
                        }
                        Task.WaitAll(tasks.ToArray());
                        updateTasks(tasks);

                        break;
                    }

                    /* SIGMOID */
                case MODE.STATIC_SIG_005:
                    {
                        List<Task> tasks = new List<Task>();
                        for (int i = 0; i < _numRuns; i++)
                        {
                            int c = i;
                            // compute results for static model and standard gaussian ksvm
                            Task t1 = Task.Run(() => createInstance("STATIC_SIG_005" + c, MODEL.STATIC, 100.0, this._numFolds, 1, 0.05, 0.05, Kernel.SIGMOID, activeFeatures: this._activeFeatures));
                            tasks.Add(t1);
                        }
                        Task.WaitAll(tasks.ToArray());
                        updateTasks(tasks);

                        break;
                    }
                case MODE.STATIC_SIG_010:
                    {
                        List<Task> tasks = new List<Task>();
                        for (int i = 0; i < _numRuns; i++)
                        {
                            int c = i;
                            // compute results for static model and standard gaussian ksvm
                            Task t1 = Task.Run(() => createInstance("STATIC_SIG_010" + c, MODEL.STATIC, 100.0, this._numFolds, 1, 0.1, 0.1, Kernel.SIGMOID, activeFeatures: this._activeFeatures));
                            tasks.Add(t1);
                        }
                        Task.WaitAll(tasks.ToArray());
                        updateTasks(tasks);

                        break;
                    }
                case MODE.STATIC_SIG_030:
                    {
                        List<Task> tasks = new List<Task>();
                        for (int i = 0; i < _numRuns; i++)
                        {
                            int c = i;
                            // compute results for static model and standard gaussian ksvm
                            Task t1 = Task.Run(() => createInstance("STATIC_SIG_030" + c, MODEL.STATIC, 100.0, this._numFolds, 1, 0.3, 0.3, Kernel.SIGMOID, activeFeatures: this._activeFeatures));
                            tasks.Add(t1);
                        }
                        Task.WaitAll(tasks.ToArray());
                        updateTasks(tasks);

                        break;
                    }
                case MODE.STATIC_SIG_060:
                    {
                        List<Task> tasks = new List<Task>();
                        for (int i = 0; i < _numRuns; i++)
                        {
                            int c = i;
                            // compute results for static model and standard gaussian ksvm
                            Task t1 = Task.Run(() => createInstance("STATIC_SIG_060" + c, MODEL.STATIC, 100.0, this._numFolds, 1, 0.6, 0.6, Kernel.SIGMOID, activeFeatures: this._activeFeatures));
                            tasks.Add(t1);
                        }
                        Task.WaitAll(tasks.ToArray());
                        updateTasks(tasks);

                        break;
                    }

                    /* CAUCHY */
                case MODE.STATIC_CAU_005:
                    {
                        List<Task> tasks = new List<Task>();
                        for (int i = 0; i < _numRuns; i++)
                        {
                            int c = i;
                            // compute results for static model and standard gaussian ksvm
                            Task t1 = Task.Run(() => createInstance("STATIC_CAU_005" + c, MODEL.STATIC, 100.0, this._numFolds, 1, 0.05, 0.05, Kernel.Cauchy, optDouble1: 0.05, activeFeatures: this._activeFeatures));
                            tasks.Add(t1);
                        }
                        Task.WaitAll(tasks.ToArray());
                        updateTasks(tasks);

                        break;
                    }
                case MODE.STATIC_CAU_010:
                    {
                        List<Task> tasks = new List<Task>();
                        for (int i = 0; i < _numRuns; i++)
                        {
                            int c = i;
                            // compute results for static model and standard gaussian ksvm
                            Task t1 = Task.Run(() => createInstance("STATIC_CAU_010" + c, MODEL.STATIC, 100.0, this._numFolds, 1, 0.1, 0.1, Kernel.Cauchy, optDouble1: 0.1, activeFeatures: this._activeFeatures));
                            tasks.Add(t1);
                        }
                        Task.WaitAll(tasks.ToArray());
                        updateTasks(tasks);

                        break;
                    }
                case MODE.STATIC_CAU_030:
                    {
                        List<Task> tasks = new List<Task>();
                        for (int i = 0; i < _numRuns; i++)
                        {
                            int c = i;
                            // compute results for static model and standard gaussian ksvm
                            Task t1 = Task.Run(() => createInstance("STATIC_CAU_030" + c, MODEL.STATIC, 100.0, this._numFolds, 1, 0.3, 0.3, Kernel.Cauchy, optDouble1: 0.3, activeFeatures: this._activeFeatures));
                            tasks.Add(t1);
                        }
                        Task.WaitAll(tasks.ToArray());
                        updateTasks(tasks);

                        break;
                    }
                case MODE.STATIC_CAU_060:
                    {
                        List<Task> tasks = new List<Task>();
                        for (int i = 0; i < _numRuns; i++)
                        {
                            int c = i;
                            // compute results for static model and standard gaussian ksvm
                            Task t1 = Task.Run(() => createInstance("STATIC_CAU_060" + c, MODEL.STATIC, 100.0, this._numFolds, 1, 0.6, 0.6, Kernel.Cauchy, optDouble1: 0.6, activeFeatures: this._activeFeatures));
                            tasks.Add(t1);
                        }
                        Task.WaitAll(tasks.ToArray());
                        updateTasks(tasks);

                        break;
                    }

                    /* Time Warp */
                case MODE.STATIC_TW_005:
                    {
                        List<Task> tasks = new List<Task>();
                        for (int i = 0; i < _numRuns; i++)
                        {
                            int c = i;
                            // compute results for static model and standard gaussian ksvm
                            Task t1 = Task.Run(() => createInstance("STATIC_TW_005" + c, MODEL.STATIC, 100.0, this._numFolds, 1, 0.05, 0.05, Kernel.TimeWarping, optDouble1: 0.05, activeFeatures: this._activeFeatures));
                            tasks.Add(t1);
                        }
                        Task.WaitAll(tasks.ToArray());
                        updateTasks(tasks);

                        break;
                    }
                case MODE.STATIC_TW_010:
                    {
                        List<Task> tasks = new List<Task>();
                        for (int i = 0; i < _numRuns; i++)
                        {
                            int c = i;
                            // compute results for static model and standard gaussian ksvm
                            Task t1 = Task.Run(() => createInstance("STATIC_TW_010" + c, MODEL.STATIC, 100.0, this._numFolds, 1, 0.1, 0.1, Kernel.TimeWarping, optDouble1: 0.1, activeFeatures: this._activeFeatures));
                            tasks.Add(t1);
                        }
                        Task.WaitAll(tasks.ToArray());
                        updateTasks(tasks);

                        break;
                    }
                case MODE.STATIC_TW_030:
                    {
                        List<Task> tasks = new List<Task>();
                        for (int i = 0; i < _numRuns; i++)
                        {
                            int c = i;
                            // compute results for static model and standard gaussian ksvm
                            Task t1 = Task.Run(() => createInstance("STATIC_TW_030" + c, MODEL.STATIC, 100.0, this._numFolds, 1, 0.3, 0.3, Kernel.TimeWarping, optDouble1: 0.3, activeFeatures: this._activeFeatures));
                            tasks.Add(t1);
                        }
                        Task.WaitAll(tasks.ToArray());
                        updateTasks(tasks);

                        break;
                    }
                case MODE.STATIC_TW_060:
                    {
                        List<Task> tasks = new List<Task>();
                        for (int i = 0; i < _numRuns; i++)
                        {
                            int c = i;
                            // compute results for static model and standard gaussian ksvm
                            Task t1 = Task.Run(() => createInstance("STATIC_TW_060" + c, MODEL.STATIC, 100.0, this._numFolds, 1, 0.6, 0.6, Kernel.TimeWarping, optDouble1: 0.6, activeFeatures: this._activeFeatures));
                            tasks.Add(t1);
                        }
                        Task.WaitAll(tasks.ToArray());
                        updateTasks(tasks);

                        break;
                    }

                    /* Symmetric Triangular */
                case MODE.STATIC_TRI_005:
                    {
                        List<Task> tasks = new List<Task>();
                        for (int i = 0; i < _numRuns; i++)
                        {
                            int c = i;
                            // compute results for static model and standard gaussian ksvm
                            Task t1 = Task.Run(() => createInstance("STATIC_TRI_005" + c, MODEL.STATIC, 100.0, this._numFolds, 1, 0.05, 0.05, Kernel.SymmetricTriangular, optDouble1: 0.05, activeFeatures: this._activeFeatures));
                            tasks.Add(t1);
                        }
                        Task.WaitAll(tasks.ToArray());
                        updateTasks(tasks);

                        break;
                    }
                case MODE.STATIC_TRI_010:
                    {
                        List<Task> tasks = new List<Task>();
                        for (int i = 0; i < _numRuns; i++)
                        {
                            int c = i;
                            // compute results for static model and standard gaussian ksvm
                            Task t1 = Task.Run(() => createInstance("STATIC_TRI_010" + c, MODEL.STATIC, 100.0, this._numFolds, 1, 0.1, 0.1, Kernel.SymmetricTriangular, optDouble1: 0.1, activeFeatures: this._activeFeatures));
                            tasks.Add(t1);
                        }
                        Task.WaitAll(tasks.ToArray());
                        updateTasks(tasks);

                        break;
                    }
                case MODE.STATIC_TRI_030:
                    {
                        List<Task> tasks = new List<Task>();
                        for (int i = 0; i < _numRuns; i++)
                        {
                            int c = i;
                            // compute results for static model and standard gaussian ksvm
                            Task t1 = Task.Run(() => createInstance("STATIC_TRI_030" + c, MODEL.STATIC, 100.0, this._numFolds, 1, 0.3, 0.3, Kernel.SymmetricTriangular, optDouble1: 0.3, activeFeatures: this._activeFeatures));
                            tasks.Add(t1);
                        }
                        Task.WaitAll(tasks.ToArray());
                        updateTasks(tasks);

                        break;
                    }
                case MODE.STATIC_TRI_060:
                    {
                        List<Task> tasks = new List<Task>();
                        for (int i = 0; i < _numRuns; i++)
                        {
                            int c = i;
                            // compute results for static model and standard gaussian ksvm
                            Task t1 = Task.Run(() => createInstance("STATIC_TRI_060" + c, MODEL.STATIC, 100.0, this._numFolds, 1, 0.6, 0.6, Kernel.SymmetricTriangular, optDouble1: 5.0, activeFeatures: this._activeFeatures));
                            tasks.Add(t1);
                        }
                        Task.WaitAll(tasks.ToArray());
                        updateTasks(tasks);

                        break;
                    }
                case MODE.STATIC_TRI_070:
                    {
                        List<Task> tasks = new List<Task>();
                        for (int i = 0; i < _numRuns; i++)
                        {
                            int c = i;
                            // compute results for static model and standard gaussian ksvm
                            Task t1 = Task.Run(() => createInstance("STATIC_TRI_070" + c, MODEL.STATIC, 100.0, this._numFolds, 1, 0.6, 0.6, Kernel.SymmetricTriangular, optDouble1: 0.7, activeFeatures: this._activeFeatures));
                            tasks.Add(t1);
                        }
                        Task.WaitAll(tasks.ToArray());
                        updateTasks(tasks);

                        break;
                    }
                case MODE.STATIC_TRI_080:
                    {
                        List<Task> tasks = new List<Task>();
                        for (int i = 0; i < _numRuns; i++)
                        {
                            int c = i;
                            // compute results for static model and standard gaussian ksvm
                            Task t1 = Task.Run(() => createInstance("STATIC_TRI_080" + c, MODEL.STATIC, 100.0, this._numFolds, 1, 0.6, 0.6, Kernel.SymmetricTriangular, optDouble1: 0.8, activeFeatures: this._activeFeatures));
                            tasks.Add(t1);
                        }
                        Task.WaitAll(tasks.ToArray());
                        updateTasks(tasks);

                        break;
                    }

                    /* ANOVA */
                case MODE.STATIC_ANOVA_16_2:
                    {
                        List<Task> tasks = new List<Task>();
                        for (int i = 0; i < _numRuns; i++)
                        {
                            int c = i;
                            // compute results for static model and standard gaussian ksvm
                            Task t1 = Task.Run(() => createInstance("STATIC_ANOVA_16_2" + c, MODEL.STATIC, 100.0, this._numFolds, 1, 0.05, 0.05, Kernel.Anova, optInt1: 16, optInt2: 2, activeFeatures: this._activeFeatures));
                            tasks.Add(t1);
                        }
                        Task.WaitAll(tasks.ToArray());
                        updateTasks(tasks);

                        break;
                    }
                case MODE.STATIC_ANOVA_16_4:
                    {
                        List<Task> tasks = new List<Task>();
                        for (int i = 0; i < _numRuns; i++)
                        {
                            int c = i;
                            // compute results for static model and standard gaussian ksvm
                            Task t1 = Task.Run(() => createInstance("STATIC_ANOVA_16_4" + c, MODEL.STATIC, 100.0, this._numFolds, 1, 0.05, 0.05, Kernel.Anova, optInt1: 16, optInt2: 4, activeFeatures: this._activeFeatures));
                            tasks.Add(t1);
                        }
                        Task.WaitAll(tasks.ToArray());
                        updateTasks(tasks);

                        break;
                    }
                case MODE.STATIC_ANOVA_16_8:
                    {
                        List<Task> tasks = new List<Task>();
                        for (int i = 0; i < _numRuns; i++)
                        {
                            int c = i;
                            // compute results for static model and standard gaussian ksvm
                            Task t1 = Task.Run(() => createInstance("STATIC_ANOVA_16_8" + c, MODEL.STATIC, 100.0, this._numFolds, 1, 0.05, 0.05, Kernel.Anova, optInt1: 16, optInt2: 8, activeFeatures: this._activeFeatures));
                            tasks.Add(t1);
                        }
                        Task.WaitAll(tasks.ToArray());
                        updateTasks(tasks);

                        break;
                    }
                case MODE.STATIC_ANOVA_16_12:
                    {
                        List<Task> tasks = new List<Task>();
                        for (int i = 0; i < _numRuns; i++)
                        {
                            int c = i;
                            // compute results for static model and standard gaussian ksvm
                            Task t1 = Task.Run(() => createInstance("STATIC_ANOVA_16_12" + c, MODEL.STATIC, 100.0, this._numFolds, 1, 0.05, 0.05, Kernel.Anova, optInt1: 16, optInt2: 12, activeFeatures: this._activeFeatures));
                            tasks.Add(t1);
                        }
                        Task.WaitAll(tasks.ToArray());
                        updateTasks(tasks);

                        break;
                    }
                case MODE.STATIC_ANOVA_16_14:
                    {
                        List<Task> tasks = new List<Task>();
                        for (int i = 0; i < _numRuns; i++)
                        {
                            int c = i;
                            // compute results for static model and standard gaussian ksvm
                            Task t1 = Task.Run(() => createInstance("STATIC_ANOVA_16_14" + c, MODEL.STATIC, 100.0, this._numFolds, 1, 0.05, 0.05, Kernel.Anova, optInt1: 16, optInt2: 14, activeFeatures: this._activeFeatures));
                            tasks.Add(t1);
                        }
                        Task.WaitAll(tasks.ToArray());
                        updateTasks(tasks);

                        break;
                    }
                /* ------------------ END OF STATIC KERNEL MODES ------------------ */

                /* ------------------ BEGIN STATISTICAL KERNEL MODES ------------------ */
                case MODE.STATISTICAL_LINEAR:
                    {
                        List<Task> tasks = new List<Task>();
                        for (int i = 0; i < _numRuns; i++)
                        {
                            int c = i;
                            // compute results for STATISTICAL model and standard gaussian ksvm
                            Task t1 = Task.Run(() => createInstance("STATISTICAL_LINEAR" + c, MODEL.STATISTICAL, 100.0, this._numFolds, 1, 0.1, 0.6, Kernel.Linear, activeFeatures: this._activeFeatures));
                            tasks.Add(t1);
                        }
                        Task.WaitAll(tasks.ToArray());
                        updateTasks(tasks);

                        break;
                    }

                /* GAUSSIAN */
                case MODE.STATISTICAL_GAU_005:
                    {
                        List<Task> tasks = new List<Task>();
                        for (int i = 0; i < _numRuns; i++)
                        {
                            int c = i;
                            // compute results for STATISTICAL model and standard gaussian ksvm
                            Task t1 = Task.Run(() => createInstance("STATISTICAL_GAU_005" + c, MODEL.STATISTICAL, 100.0, this._numFolds, 1, 0.05, 0.05, Kernel.GAUSSIAN, activeFeatures: this._activeFeatures));
                            tasks.Add(t1);
                        }
                        Task.WaitAll(tasks.ToArray());
                        updateTasks(tasks);

                        break;
                    }
                case MODE.STATISTICAL_GAU_010:
                    {
                        List<Task> tasks = new List<Task>();
                        for (int i = 0; i < _numRuns; i++)
                        {
                            int c = i;
                            // compute results for STATISTICAL model and standard gaussian ksvm
                            Task t1 = Task.Run(() => createInstance("STATISTICAL_GAU_010" + c, MODEL.STATISTICAL, 100.0, this._numFolds, 1, 0.1, 0.1, Kernel.GAUSSIAN, activeFeatures: this._activeFeatures));
                            tasks.Add(t1);
                        }
                        Task.WaitAll(tasks.ToArray());
                        updateTasks(tasks);

                        break;
                    }
                case MODE.STATISTICAL_GAU_030:
                    {
                        List<Task> tasks = new List<Task>();
                        for (int i = 0; i < _numRuns; i++)
                        {
                            int c = i;
                            // compute results for STATISTICAL model and standard gaussian ksvm
                            Task t1 = Task.Run(() => createInstance("STATISTICAL_GAU_030" + c, MODEL.STATISTICAL, 100.0, this._numFolds, 1, 0.3, 0.3, Kernel.GAUSSIAN, activeFeatures: this._activeFeatures));
                            tasks.Add(t1);
                        }
                        Task.WaitAll(tasks.ToArray());
                        updateTasks(tasks);

                        break;
                    }
                case MODE.STATISTICAL_GAU_060:
                    {
                        List<Task> tasks = new List<Task>();
                        for (int i = 0; i < _numRuns; i++)
                        {
                            int c = i;
                            // compute results for STATISTICAL model and standard gaussian ksvm
                            Task t1 = Task.Run(() => createInstance("STATISTICAL_GAU_060" + c, MODEL.STATISTICAL, 100.0, this._numFolds, 1, 0.6, 0.6, Kernel.GAUSSIAN, activeFeatures: this._activeFeatures));
                            tasks.Add(t1);
                        }
                        Task.WaitAll(tasks.ToArray());
                        updateTasks(tasks);

                        break;
                    }

                /* LAPLACIAN */
                case MODE.STATISTICAL_LAP_005:
                    {
                        List<Task> tasks = new List<Task>();
                        for (int i = 0; i < _numRuns; i++)
                        {
                            int c = i;
                            // compute results for STATISTICAL model and standard gaussian ksvm
                            Task t1 = Task.Run(() => createInstance("STATISTICAL_LAP_005" + c, MODEL.STATISTICAL, 100.0, this._numFolds, 1, 0.05, 0.05, Kernel.LAPLACIAN, activeFeatures: this._activeFeatures));
                            tasks.Add(t1);
                        }
                        Task.WaitAll(tasks.ToArray());
                        updateTasks(tasks);

                        break;
                    }
                case MODE.STATISTICAL_LAP_010:
                    {
                        List<Task> tasks = new List<Task>();
                        for (int i = 0; i < _numRuns; i++)
                        {
                            int c = i;
                            // compute results for STATISTICAL model and standard gaussian ksvm
                            Task t1 = Task.Run(() => createInstance("STATISTICAL_LAP_010" + c, MODEL.STATISTICAL, 100.0, this._numFolds, 1, 0.1, 0.1, Kernel.LAPLACIAN, activeFeatures: this._activeFeatures));
                            tasks.Add(t1);
                        }
                        Task.WaitAll(tasks.ToArray());
                        updateTasks(tasks);

                        break;
                    }
                case MODE.STATISTICAL_LAP_030:
                    {
                        List<Task> tasks = new List<Task>();
                        for (int i = 0; i < _numRuns; i++)
                        {
                            int c = i;
                            // compute results for STATISTICAL model and standard gaussian ksvm
                            Task t1 = Task.Run(() => createInstance("STATISTICAL_LAP_030" + c, MODEL.STATISTICAL, 100.0, this._numFolds, 1, 0.3, 0.3, Kernel.LAPLACIAN, activeFeatures: this._activeFeatures));
                            tasks.Add(t1);
                        }
                        Task.WaitAll(tasks.ToArray());
                        updateTasks(tasks);

                        break;
                    }
                case MODE.STATISTICAL_LAP_060:
                    {
                        List<Task> tasks = new List<Task>();
                        for (int i = 0; i < _numRuns; i++)
                        {
                            int c = i;
                            // compute results for STATISTICAL model and standard gaussian ksvm
                            Task t1 = Task.Run(() => createInstance("STATISTICAL_LAP_060" + c, MODEL.STATISTICAL, 100.0, this._numFolds, 1, 0.6, 0.6, Kernel.LAPLACIAN, activeFeatures: this._activeFeatures));
                            tasks.Add(t1);
                        }
                        Task.WaitAll(tasks.ToArray());
                        updateTasks(tasks);

                        break;
                    }

                /* SIGMOID */
                case MODE.STATISTICAL_SIG_005:
                    {
                        List<Task> tasks = new List<Task>();
                        for (int i = 0; i < _numRuns; i++)
                        {
                            int c = i;
                            // compute results for STATISTICAL model and standard gaussian ksvm
                            Task t1 = Task.Run(() => createInstance("STATISTICAL_SIG_005" + c, MODEL.STATISTICAL, 100.0, this._numFolds, 1, 0.05, 0.05, Kernel.SIGMOID, activeFeatures: this._activeFeatures));
                            tasks.Add(t1);
                        }
                        Task.WaitAll(tasks.ToArray());
                        updateTasks(tasks);

                        break;
                    }
                case MODE.STATISTICAL_SIG_010:
                    {
                        List<Task> tasks = new List<Task>();
                        for (int i = 0; i < _numRuns; i++)
                        {
                            int c = i;
                            // compute results for STATISTICAL model and standard gaussian ksvm
                            Task t1 = Task.Run(() => createInstance("STATISTICAL_SIG_010" + c, MODEL.STATISTICAL, 100.0, this._numFolds, 1, 0.1, 0.1, Kernel.SIGMOID, activeFeatures: this._activeFeatures));
                            tasks.Add(t1);
                        }
                        Task.WaitAll(tasks.ToArray());
                        updateTasks(tasks);

                        break;
                    }
                case MODE.STATISTICAL_SIG_030:
                    {
                        List<Task> tasks = new List<Task>();
                        for (int i = 0; i < _numRuns; i++)
                        {
                            int c = i;
                            // compute results for STATISTICAL model and standard gaussian ksvm
                            Task t1 = Task.Run(() => createInstance("STATISTICAL_SIG_030" + c, MODEL.STATISTICAL, 100.0, this._numFolds, 1, 0.3, 0.3, Kernel.SIGMOID, activeFeatures: this._activeFeatures));
                            tasks.Add(t1);
                        }
                        Task.WaitAll(tasks.ToArray());
                        updateTasks(tasks);

                        break;
                    }
                case MODE.STATISTICAL_SIG_060:
                    {
                        List<Task> tasks = new List<Task>();
                        for (int i = 0; i < _numRuns; i++)
                        {
                            int c = i;
                            // compute results for STATISTICAL model and standard gaussian ksvm
                            Task t1 = Task.Run(() => createInstance("STATISTICAL_SIG_060" + c, MODEL.STATISTICAL, 100.0, this._numFolds, 1, 0.6, 0.6, Kernel.SIGMOID, activeFeatures: this._activeFeatures));
                            tasks.Add(t1);
                        }
                        Task.WaitAll(tasks.ToArray());
                        updateTasks(tasks);

                        break;
                    }

                /* CAUCHY */
                case MODE.STATISTICAL_CAU_005:
                    {
                        List<Task> tasks = new List<Task>();
                        for (int i = 0; i < _numRuns; i++)
                        {
                            int c = i;
                            // compute results for STATISTICAL model and standard gaussian ksvm
                            Task t1 = Task.Run(() => createInstance("STATISTICAL_CAU_005" + c, MODEL.STATISTICAL, 100.0, this._numFolds, 1, 0.05, 0.05, Kernel.Cauchy, optDouble1: 0.05, activeFeatures: this._activeFeatures));
                            tasks.Add(t1);
                        }
                        Task.WaitAll(tasks.ToArray());
                        updateTasks(tasks);

                        break;
                    }
                case MODE.STATISTICAL_CAU_010:
                    {
                        List<Task> tasks = new List<Task>();
                        for (int i = 0; i < _numRuns; i++)
                        {
                            int c = i;
                            // compute results for STATISTICAL model and standard gaussian ksvm
                            Task t1 = Task.Run(() => createInstance("STATISTICAL_CAU_010" + c, MODEL.STATISTICAL, 100.0, this._numFolds, 1, 0.1, 0.1, Kernel.Cauchy, optDouble1: 0.1, activeFeatures: this._activeFeatures));
                            tasks.Add(t1);
                        }
                        Task.WaitAll(tasks.ToArray());
                        updateTasks(tasks);

                        break;
                    }
                case MODE.STATISTICAL_CAU_030:
                    {
                        List<Task> tasks = new List<Task>();
                        for (int i = 0; i < _numRuns; i++)
                        {
                            int c = i;
                            // compute results for STATISTICAL model and standard gaussian ksvm
                            Task t1 = Task.Run(() => createInstance("STATISTICAL_CAU_030" + c, MODEL.STATISTICAL, 100.0, this._numFolds, 1, 0.3, 0.3, Kernel.Cauchy, optDouble1: 0.3, activeFeatures: this._activeFeatures));
                            tasks.Add(t1);
                        }
                        Task.WaitAll(tasks.ToArray());
                        updateTasks(tasks);

                        break;
                    }
                case MODE.STATISTICAL_CAU_060:
                    {
                        List<Task> tasks = new List<Task>();
                        for (int i = 0; i < _numRuns; i++)
                        {
                            int c = i;
                            // compute results for STATISTICAL model and standard gaussian ksvm
                            Task t1 = Task.Run(() => createInstance("STATISTICAL_CAU_060" + c, MODEL.STATISTICAL, 100.0, this._numFolds, 1, 0.6, 0.6, Kernel.Cauchy, optDouble1: 0.6, activeFeatures: this._activeFeatures));
                            tasks.Add(t1);
                        }
                        Task.WaitAll(tasks.ToArray());
                        updateTasks(tasks);

                        break;
                    }

                /* Time Warp */
                case MODE.STATISTICAL_TW_005:
                    {
                        List<Task> tasks = new List<Task>();
                        for (int i = 0; i < _numRuns; i++)
                        {
                            int c = i;
                            // compute results for STATISTICAL model and standard gaussian ksvm
                            Task t1 = Task.Run(() => createInstance("STATISTICAL_TW_005" + c, MODEL.STATISTICAL, 100.0, this._numFolds, 1, 0.05, 0.05, Kernel.TimeWarping, optDouble1: 0.05, activeFeatures: this._activeFeatures));
                            tasks.Add(t1);
                        }
                        Task.WaitAll(tasks.ToArray());
                        updateTasks(tasks);

                        break;
                    }
                case MODE.STATISTICAL_TW_010:
                    {
                        List<Task> tasks = new List<Task>();
                        for (int i = 0; i < _numRuns; i++)
                        {
                            int c = i;
                            // compute results for STATISTICAL model and standard gaussian ksvm
                            Task t1 = Task.Run(() => createInstance("STATISTICAL_TW_010" + c, MODEL.STATISTICAL, 100.0, this._numFolds, 1, 0.1, 0.1, Kernel.TimeWarping, optDouble1: 0.1, activeFeatures: this._activeFeatures));
                            tasks.Add(t1);
                        }
                        Task.WaitAll(tasks.ToArray());
                        updateTasks(tasks);

                        break;
                    }
                case MODE.STATISTICAL_TW_030:
                    {
                        List<Task> tasks = new List<Task>();
                        for (int i = 0; i < _numRuns; i++)
                        {
                            int c = i;
                            // compute results for STATISTICAL model and standard gaussian ksvm
                            Task t1 = Task.Run(() => createInstance("STATISTICAL_TW_030" + c, MODEL.STATISTICAL, 100.0, this._numFolds, 1, 0.3, 0.3, Kernel.TimeWarping, optDouble1: 0.3, activeFeatures: this._activeFeatures));
                            tasks.Add(t1);
                        }
                        Task.WaitAll(tasks.ToArray());
                        updateTasks(tasks);

                        break;
                    }
                case MODE.STATISTICAL_TW_060:
                    {
                        List<Task> tasks = new List<Task>();
                        for (int i = 0; i < _numRuns; i++)
                        {
                            int c = i;
                            // compute results for STATISTICAL model and standard gaussian ksvm
                            Task t1 = Task.Run(() => createInstance("STATISTICAL_TW_060" + c, MODEL.STATISTICAL, 100.0, this._numFolds, 1, 0.6, 0.6, Kernel.TimeWarping, optDouble1: 0.6, activeFeatures: this._activeFeatures));
                            tasks.Add(t1);
                        }
                        Task.WaitAll(tasks.ToArray());
                        updateTasks(tasks);

                        break;
                    }

                /* Symmetric Triangular */
                case MODE.STATISTICAL_TRI_005:
                    {
                        List<Task> tasks = new List<Task>();
                        for (int i = 0; i < _numRuns; i++)
                        {
                            int c = i;
                            // compute results for STATISTICAL model and standard gaussian ksvm
                            Task t1 = Task.Run(() => createInstance("STATISTICAL_TRI_005" + c, MODEL.STATISTICAL, 100.0, this._numFolds, 1, 0.05, 0.05, Kernel.SymmetricTriangular, optDouble1: 0.05, activeFeatures: this._activeFeatures));
                            tasks.Add(t1);
                        }
                        Task.WaitAll(tasks.ToArray());
                        updateTasks(tasks);

                        break;
                    }
                case MODE.STATISTICAL_TRI_010:
                    {
                        List<Task> tasks = new List<Task>();
                        for (int i = 0; i < _numRuns; i++)
                        {
                            int c = i;
                            // compute results for STATISTICAL model and standard gaussian ksvm
                            Task t1 = Task.Run(() => createInstance("STATISTICAL_TRI_010" + c, MODEL.STATISTICAL, 100.0, this._numFolds, 1, 0.1, 0.1, Kernel.SymmetricTriangular, optDouble1: 0.1, activeFeatures: this._activeFeatures));
                            tasks.Add(t1);
                        }
                        Task.WaitAll(tasks.ToArray());
                        updateTasks(tasks);

                        break;
                    }
                case MODE.STATISTICAL_TRI_030:
                    {
                        List<Task> tasks = new List<Task>();
                        for (int i = 0; i < _numRuns; i++)
                        {
                            int c = i;
                            // compute results for STATISTICAL model and standard gaussian ksvm
                            Task t1 = Task.Run(() => createInstance("STATISTICAL_TRI_030" + c, MODEL.STATISTICAL, 100.0, this._numFolds, 1, 0.3, 0.3, Kernel.SymmetricTriangular, optDouble1: 0.3, activeFeatures: this._activeFeatures));
                            tasks.Add(t1);
                        }
                        Task.WaitAll(tasks.ToArray());
                        updateTasks(tasks);

                        break;
                    }
                case MODE.STATISTICAL_TRI_060:
                    {
                        List<Task> tasks = new List<Task>();
                        for (int i = 0; i < _numRuns; i++)
                        {
                            int c = i;
                            // compute results for STATISTICAL model and standard gaussian ksvm
                            Task t1 = Task.Run(() => createInstance("STATISTICAL_TRI_060" + c, MODEL.STATISTICAL, 100.0, this._numFolds, 1, 0.6, 0.6, Kernel.SymmetricTriangular, optDouble1: 5.0, activeFeatures: this._activeFeatures));
                            tasks.Add(t1);
                        }
                        Task.WaitAll(tasks.ToArray());
                        updateTasks(tasks);

                        break;
                    }
                case MODE.STATISTICAL_TRI_070:
                    {
                        List<Task> tasks = new List<Task>();
                        for (int i = 0; i < _numRuns; i++)
                        {
                            int c = i;
                            // compute results for STATISTICAL model and standard gaussian ksvm
                            Task t1 = Task.Run(() => createInstance("STATISTICAL_TRI_070" + c, MODEL.STATISTICAL, 100.0, this._numFolds, 1, 0.6, 0.6, Kernel.SymmetricTriangular, optDouble1: 0.7, activeFeatures: this._activeFeatures));
                            tasks.Add(t1);
                        }
                        Task.WaitAll(tasks.ToArray());
                        updateTasks(tasks);

                        break;
                    }
                case MODE.STATISTICAL_TRI_080:
                    {
                        List<Task> tasks = new List<Task>();
                        for (int i = 0; i < _numRuns; i++)
                        {
                            int c = i;
                            // compute results for STATISTICAL model and standard gaussian ksvm
                            Task t1 = Task.Run(() => createInstance("STATISTICAL_TRI_080" + c, MODEL.STATISTICAL, 100.0, this._numFolds, 1, 0.6, 0.6, Kernel.SymmetricTriangular, optDouble1: 0.8, activeFeatures: this._activeFeatures));
                            tasks.Add(t1);
                        }
                        Task.WaitAll(tasks.ToArray());
                        updateTasks(tasks);

                        break;
                    }

                /* ANOVA */
                case MODE.STATISTICAL_ANOVA_16_2:
                    {
                        List<Task> tasks = new List<Task>();
                        for (int i = 0; i < _numRuns; i++)
                        {
                            int c = i;
                            // compute results for STATISTICAL model and standard gaussian ksvm
                            Task t1 = Task.Run(() => createInstance("STATISTICAL_ANOVA_16_2" + c, MODEL.STATISTICAL, 100.0, this._numFolds, 1, 0.05, 0.05, Kernel.Anova, optInt1: 16, optInt2: 2, activeFeatures: this._activeFeatures));
                            tasks.Add(t1);
                        }
                        Task.WaitAll(tasks.ToArray());
                        updateTasks(tasks);

                        break;
                    }
                case MODE.STATISTICAL_ANOVA_16_4:
                    {
                        List<Task> tasks = new List<Task>();
                        for (int i = 0; i < _numRuns; i++)
                        {
                            int c = i;
                            // compute results for STATISTICAL model and standard gaussian ksvm
                            Task t1 = Task.Run(() => createInstance("STATISTICAL_ANOVA_16_4" + c, MODEL.STATISTICAL, 100.0, this._numFolds, 1, 0.05, 0.05, Kernel.Anova, optInt1: 16, optInt2: 4, activeFeatures: this._activeFeatures));
                            tasks.Add(t1);
                        }
                        Task.WaitAll(tasks.ToArray());
                        updateTasks(tasks);

                        break;
                    }
                case MODE.STATISTICAL_ANOVA_16_8:
                    {
                        List<Task> tasks = new List<Task>();
                        for (int i = 0; i < _numRuns; i++)
                        {
                            int c = i;
                            // compute results for STATISTICAL model and standard gaussian ksvm
                            Task t1 = Task.Run(() => createInstance("STATISTICAL_ANOVA_16_8" + c, MODEL.STATISTICAL, 100.0, this._numFolds, 1, 0.05, 0.05, Kernel.Anova, optInt1: 16, optInt2: 8, activeFeatures: this._activeFeatures));
                            tasks.Add(t1);
                        }
                        Task.WaitAll(tasks.ToArray());
                        updateTasks(tasks);

                        break;
                    }
                case MODE.STATISTICAL_ANOVA_16_12:
                    {
                        List<Task> tasks = new List<Task>();
                        for (int i = 0; i < _numRuns; i++)
                        {
                            int c = i;
                            // compute results for STATISTICAL model and standard gaussian ksvm
                            Task t1 = Task.Run(() => createInstance("STATISTICAL_ANOVA_16_12" + c, MODEL.STATISTICAL, 100.0, this._numFolds, 1, 0.05, 0.05, Kernel.Anova, optInt1: 16, optInt2: 12, activeFeatures: this._activeFeatures));
                            tasks.Add(t1);
                        }
                        Task.WaitAll(tasks.ToArray());
                        updateTasks(tasks);

                        break;
                    }
                case MODE.STATISTICAL_ANOVA_16_14:
                    {
                        List<Task> tasks = new List<Task>();
                        for (int i = 0; i < _numRuns; i++)
                        {
                            int c = i;
                            // compute results for STATISTICAL model and standard gaussian ksvm
                            Task t1 = Task.Run(() => createInstance("STATISTICAL_ANOVA_16_14" + c, MODEL.STATISTICAL, 100.0, this._numFolds, 1, 0.05, 0.05, Kernel.Anova, optInt1: 16, optInt2: 14, activeFeatures: this._activeFeatures));
                            tasks.Add(t1);
                        }
                        Task.WaitAll(tasks.ToArray());
                        updateTasks(tasks);

                        break;
                    }
                /* ------------------ END OF STATISTICAL KERNEL MODES ------------------ */

                /* ------------------ BEGIN SAMPLED KERNEL MODES ------------------ */
                case MODE.SAMPLED_LINEAR:
                    {
                        List<Task> tasks = new List<Task>();
                        for (int i = 0; i < _numRuns; i++)
                        {
                            int c = i;
                            // compute results for static model and standard gaussian ksvm
                            Task t1 = Task.Run(() => createInstance("SAMPLED_LINEAR" + c, MODEL.SAMPLED, 100.0, this._numFolds, 1, 0.1, 0.6, Kernel.Linear, activeFeatures: this._activeFeatures));
                            tasks.Add(t1);
                        }
                        Task.WaitAll(tasks.ToArray());
                        updateTasks(tasks);

                        break;
                    }

                /* GAUSSIAN */
                case MODE.SAMPLED_GAU_005:
                    {
                        List<Task> tasks = new List<Task>();
                        for (int i = 0; i < _numRuns; i++)
                        {
                            int c = i;
                            // compute results for static model and standard gaussian ksvm
                            Task t1 = Task.Run(() => createInstance("SAMPLED_GAU_005" + c, MODEL.SAMPLED, 100.0, this._numFolds, 1, 0.05, 0.05, Kernel.GAUSSIAN, activeFeatures: this._activeFeatures));
                            tasks.Add(t1);
                        }
                        Task.WaitAll(tasks.ToArray());
                        updateTasks(tasks);

                        break;
                    }
                case MODE.SAMPLED_GAU_010:
                    {
                        List<Task> tasks = new List<Task>();
                        for (int i = 0; i < _numRuns; i++)
                        {
                            int c = i;
                            // compute results for static model and standard gaussian ksvm
                            Task t1 = Task.Run(() => createInstance("SAMPLED_GAU_010" + c, MODEL.SAMPLED, 100.0, this._numFolds, 1, 0.1, 0.1, Kernel.GAUSSIAN, activeFeatures: this._activeFeatures));
                            tasks.Add(t1);
                        }
                        Task.WaitAll(tasks.ToArray());
                        updateTasks(tasks);

                        break;
                    }
                case MODE.SAMPLED_GAU_030:
                    {
                        List<Task> tasks = new List<Task>();
                        for (int i = 0; i < _numRuns; i++)
                        {
                            int c = i;
                            // compute results for static model and standard gaussian ksvm
                            Task t1 = Task.Run(() => createInstance("SAMPLED_GAU_030" + c, MODEL.SAMPLED, 100.0, this._numFolds, 1, 0.3, 0.3, Kernel.GAUSSIAN, activeFeatures: this._activeFeatures));
                            tasks.Add(t1);
                        }
                        Task.WaitAll(tasks.ToArray());
                        updateTasks(tasks);

                        break;
                    }
                case MODE.SAMPLED_GAU_060:
                    {
                        List<Task> tasks = new List<Task>();
                        for (int i = 0; i < _numRuns; i++)
                        {
                            int c = i;
                            // compute results for static model and standard gaussian ksvm
                            Task t1 = Task.Run(() => createInstance("SAMPLED_GAU_060" + c, MODEL.SAMPLED, 100.0, this._numFolds, 1, 0.6, 0.6, Kernel.GAUSSIAN, activeFeatures: this._activeFeatures));
                            tasks.Add(t1);
                        }
                        Task.WaitAll(tasks.ToArray());
                        updateTasks(tasks);

                        break;
                    }

                /* LAPLACIAN */
                case MODE.SAMPLED_LAP_005:
                    {
                        List<Task> tasks = new List<Task>();
                        for (int i = 0; i < _numRuns; i++)
                        {
                            int c = i;
                            // compute results for static model and standard gaussian ksvm
                            Task t1 = Task.Run(() => createInstance("SAMPLED_LAP_005" + c, MODEL.SAMPLED, 100.0, this._numFolds, 1, 0.05, 0.05, Kernel.LAPLACIAN, activeFeatures: this._activeFeatures));
                            tasks.Add(t1);
                        }
                        Task.WaitAll(tasks.ToArray());
                        updateTasks(tasks);

                        break;
                    }
                case MODE.SAMPLED_LAP_010:
                    {
                        List<Task> tasks = new List<Task>();
                        for (int i = 0; i < _numRuns; i++)
                        {
                            int c = i;
                            // compute results for static model and standard gaussian ksvm
                            Task t1 = Task.Run(() => createInstance("SAMPLED_LAP_010" + c, MODEL.SAMPLED, 100.0, this._numFolds, 1, 0.1, 0.1, Kernel.LAPLACIAN, activeFeatures: this._activeFeatures));
                            tasks.Add(t1);
                        }
                        Task.WaitAll(tasks.ToArray());
                        updateTasks(tasks);

                        break;
                    }
                case MODE.SAMPLED_LAP_030:
                    {
                        List<Task> tasks = new List<Task>();
                        for (int i = 0; i < _numRuns; i++)
                        {
                            int c = i;
                            // compute results for static model and standard gaussian ksvm
                            Task t1 = Task.Run(() => createInstance("SAMPLED_LAP_030" + c, MODEL.SAMPLED, 100.0, this._numFolds, 1, 0.3, 0.3, Kernel.LAPLACIAN, activeFeatures: this._activeFeatures));
                            tasks.Add(t1);
                        }
                        Task.WaitAll(tasks.ToArray());
                        updateTasks(tasks);

                        break;
                    }
                case MODE.SAMPLED_LAP_060:
                    {
                        List<Task> tasks = new List<Task>();
                        for (int i = 0; i < _numRuns; i++)
                        {
                            int c = i;
                            // compute results for static model and standard gaussian ksvm
                            Task t1 = Task.Run(() => createInstance("SAMPLED_LAP_060" + c, MODEL.SAMPLED, 100.0, this._numFolds, 1, 0.6, 0.6, Kernel.LAPLACIAN, activeFeatures: this._activeFeatures));
                            tasks.Add(t1);
                        }
                        Task.WaitAll(tasks.ToArray());
                        updateTasks(tasks);

                        break;
                    }

                /* SIGMOID */
                case MODE.SAMPLED_SIG_005:
                    {
                        List<Task> tasks = new List<Task>();
                        for (int i = 0; i < _numRuns; i++)
                        {
                            int c = i;
                            // compute results for static model and standard gaussian ksvm
                            Task t1 = Task.Run(() => createInstance("SAMPLED_SIG_005" + c, MODEL.SAMPLED, 100.0, this._numFolds, 1, 0.05, 0.05, Kernel.SIGMOID, activeFeatures: this._activeFeatures));
                            tasks.Add(t1);
                        }
                        Task.WaitAll(tasks.ToArray());
                        updateTasks(tasks);

                        break;
                    }
                case MODE.SAMPLED_SIG_010:
                    {
                        List<Task> tasks = new List<Task>();
                        for (int i = 0; i < _numRuns; i++)
                        {
                            int c = i;
                            // compute results for static model and standard gaussian ksvm
                            Task t1 = Task.Run(() => createInstance("SAMPLED_SIG_010" + c, MODEL.SAMPLED, 100.0, this._numFolds, 1, 0.1, 0.1, Kernel.SIGMOID, activeFeatures: this._activeFeatures));
                            tasks.Add(t1);
                        }
                        Task.WaitAll(tasks.ToArray());
                        updateTasks(tasks);

                        break;
                    }
                case MODE.SAMPLED_SIG_030:
                    {
                        List<Task> tasks = new List<Task>();
                        for (int i = 0; i < _numRuns; i++)
                        {
                            int c = i;
                            // compute results for static model and standard gaussian ksvm
                            Task t1 = Task.Run(() => createInstance("SAMPLED_SIG_030" + c, MODEL.SAMPLED, 100.0, this._numFolds, 1, 0.3, 0.3, Kernel.SIGMOID, activeFeatures: this._activeFeatures));
                            tasks.Add(t1);
                        }
                        Task.WaitAll(tasks.ToArray());
                        updateTasks(tasks);

                        break;
                    }
                case MODE.SAMPLED_SIG_060:
                    {
                        List<Task> tasks = new List<Task>();
                        for (int i = 0; i < _numRuns; i++)
                        {
                            int c = i;
                            // compute results for static model and standard gaussian ksvm
                            Task t1 = Task.Run(() => createInstance("SAMPLED_SIG_060" + c, MODEL.SAMPLED, 100.0, this._numFolds, 1, 0.6, 0.6, Kernel.SIGMOID, activeFeatures: this._activeFeatures));
                            tasks.Add(t1);
                        }
                        Task.WaitAll(tasks.ToArray());
                        updateTasks(tasks);

                        break;
                    }

                /* CAUCHY */
                case MODE.SAMPLED_CAU_005:
                    {
                        List<Task> tasks = new List<Task>();
                        for (int i = 0; i < _numRuns; i++)
                        {
                            int c = i;
                            // compute results for static model and standard gaussian ksvm
                            Task t1 = Task.Run(() => createInstance("SAMPLED_CAU_005" + c, MODEL.SAMPLED, 100.0, this._numFolds, 1, 0.05, 0.05, Kernel.Cauchy, optDouble1: 0.05, activeFeatures: this._activeFeatures));
                            tasks.Add(t1);
                        }
                        Task.WaitAll(tasks.ToArray());
                        updateTasks(tasks);

                        break;
                    }
                case MODE.SAMPLED_CAU_010:
                    {
                        List<Task> tasks = new List<Task>();
                        for (int i = 0; i < _numRuns; i++)
                        {
                            int c = i;
                            // compute results for static model and standard gaussian ksvm
                            Task t1 = Task.Run(() => createInstance("SAMPLED_CAU_010" + c, MODEL.SAMPLED, 100.0, this._numFolds, 1, 0.1, 0.1, Kernel.Cauchy, optDouble1: 0.1, activeFeatures: this._activeFeatures));
                            tasks.Add(t1);
                        }
                        Task.WaitAll(tasks.ToArray());
                        updateTasks(tasks);

                        break;
                    }
                case MODE.SAMPLED_CAU_030:
                    {
                        List<Task> tasks = new List<Task>();
                        for (int i = 0; i < _numRuns; i++)
                        {
                            int c = i;
                            // compute results for static model and standard gaussian ksvm
                            Task t1 = Task.Run(() => createInstance("SAMPLED_CAU_030" + c, MODEL.SAMPLED, 100.0, this._numFolds, 1, 0.3, 0.3, Kernel.Cauchy, optDouble1: 0.3, activeFeatures: this._activeFeatures));
                            tasks.Add(t1);
                        }
                        Task.WaitAll(tasks.ToArray());
                        updateTasks(tasks);

                        break;
                    }
                case MODE.SAMPLED_CAU_060:
                    {
                        List<Task> tasks = new List<Task>();
                        for (int i = 0; i < _numRuns; i++)
                        {
                            int c = i;
                            // compute results for static model and standard gaussian ksvm
                            Task t1 = Task.Run(() => createInstance("SAMPLED_CAU_060" + c, MODEL.SAMPLED, 100.0, this._numFolds, 1, 0.6, 0.6, Kernel.Cauchy, optDouble1: 0.6, activeFeatures: this._activeFeatures));
                            tasks.Add(t1);
                        }
                        Task.WaitAll(tasks.ToArray());
                        updateTasks(tasks);

                        break;
                    }

                /* Time Warp */
                case MODE.SAMPLED_TW_005:
                    {
                        List<Task> tasks = new List<Task>();
                        for (int i = 0; i < _numRuns; i++)
                        {
                            int c = i;
                            // compute results for static model and standard gaussian ksvm
                            Task t1 = Task.Run(() => createInstance("SAMPLED_TW_005" + c, MODEL.SAMPLED, 100.0, this._numFolds, 1, 0.05, 0.05, Kernel.TimeWarping, optDouble1: 0.05, activeFeatures: this._activeFeatures));
                            tasks.Add(t1);
                        }
                        Task.WaitAll(tasks.ToArray());
                        updateTasks(tasks);

                        break;
                    }
                case MODE.SAMPLED_TW_010:
                    {
                        List<Task> tasks = new List<Task>();
                        for (int i = 0; i < _numRuns; i++)
                        {
                            int c = i;
                            // compute results for static model and standard gaussian ksvm
                            Task t1 = Task.Run(() => createInstance("SAMPLED_TW_010" + c, MODEL.SAMPLED, 100.0, this._numFolds, 1, 0.1, 0.1, Kernel.TimeWarping, optDouble1: 0.1, activeFeatures: this._activeFeatures));
                            tasks.Add(t1);
                        }
                        Task.WaitAll(tasks.ToArray());
                        updateTasks(tasks);

                        break;
                    }
                case MODE.SAMPLED_TW_030:
                    {
                        List<Task> tasks = new List<Task>();
                        for (int i = 0; i < _numRuns; i++)
                        {
                            int c = i;
                            // compute results for static model and standard gaussian ksvm
                            Task t1 = Task.Run(() => createInstance("SAMPLED_TW_030" + c, MODEL.SAMPLED, 100.0, this._numFolds, 1, 0.3, 0.3, Kernel.TimeWarping, optDouble1: 0.3, activeFeatures: this._activeFeatures));
                            tasks.Add(t1);
                        }
                        Task.WaitAll(tasks.ToArray());
                        updateTasks(tasks);

                        break;
                    }
                case MODE.SAMPLED_TW_060:
                    {
                        List<Task> tasks = new List<Task>();
                        for (int i = 0; i < _numRuns; i++)
                        {
                            int c = i;
                            // compute results for static model and standard gaussian ksvm
                            Task t1 = Task.Run(() => createInstance("SAMPLED_TW_060" + c, MODEL.SAMPLED, 100.0, this._numFolds, 1, 0.6, 0.6, Kernel.TimeWarping, optDouble1: 0.6, activeFeatures: this._activeFeatures));
                            tasks.Add(t1);
                        }
                        Task.WaitAll(tasks.ToArray());
                        updateTasks(tasks);

                        break;
                    }

                /* Symmetric Triangular */
                case MODE.SAMPLED_TRI_005:
                    {
                        List<Task> tasks = new List<Task>();
                        for (int i = 0; i < _numRuns; i++)
                        {
                            int c = i;
                            // compute results for static model and standard gaussian ksvm
                            Task t1 = Task.Run(() => createInstance("SAMPLED_TRI_005" + c, MODEL.SAMPLED, 100.0, this._numFolds, 1, 0.05, 0.05, Kernel.SymmetricTriangular, optDouble1: 0.05, activeFeatures: this._activeFeatures));
                            tasks.Add(t1);
                        }
                        Task.WaitAll(tasks.ToArray());
                        updateTasks(tasks);

                        break;
                    }
                case MODE.SAMPLED_TRI_010:
                    {
                        List<Task> tasks = new List<Task>();
                        for (int i = 0; i < _numRuns; i++)
                        {
                            int c = i;
                            // compute results for static model and standard gaussian ksvm
                            Task t1 = Task.Run(() => createInstance("SAMPLED_TRI_010" + c, MODEL.SAMPLED, 100.0, this._numFolds, 1, 0.1, 0.1, Kernel.SymmetricTriangular, optDouble1: 0.1, activeFeatures: this._activeFeatures));
                            tasks.Add(t1);
                        }
                        Task.WaitAll(tasks.ToArray());
                        updateTasks(tasks);

                        break;
                    }
                case MODE.SAMPLED_TRI_030:
                    {
                        List<Task> tasks = new List<Task>();
                        for (int i = 0; i < _numRuns; i++)
                        {
                            int c = i;
                            // compute results for static model and standard gaussian ksvm
                            Task t1 = Task.Run(() => createInstance("SAMPLED_TRI_030" + c, MODEL.SAMPLED, 100.0, this._numFolds, 1, 0.3, 0.3, Kernel.SymmetricTriangular, optDouble1: 0.3, activeFeatures: this._activeFeatures));
                            tasks.Add(t1);
                        }
                        Task.WaitAll(tasks.ToArray());
                        updateTasks(tasks);

                        break;
                    }
                case MODE.SAMPLED_TRI_060:
                    {
                        List<Task> tasks = new List<Task>();
                        for (int i = 0; i < _numRuns; i++)
                        {
                            int c = i;
                            // compute results for static model and standard gaussian ksvm
                            Task t1 = Task.Run(() => createInstance("SAMPLED_TRI_060" + c, MODEL.SAMPLED, 100.0, this._numFolds, 1, 0.6, 0.6, Kernel.SymmetricTriangular, optDouble1: 5.0, activeFeatures: this._activeFeatures));
                            tasks.Add(t1);
                        }
                        Task.WaitAll(tasks.ToArray());
                        updateTasks(tasks);

                        break;
                    }
                case MODE.SAMPLED_TRI_070:
                    {
                        List<Task> tasks = new List<Task>();
                        for (int i = 0; i < _numRuns; i++)
                        {
                            int c = i;
                            // compute results for static model and standard gaussian ksvm
                            Task t1 = Task.Run(() => createInstance("SAMPLED_TRI_070" + c, MODEL.SAMPLED, 100.0, this._numFolds, 1, 0.6, 0.6, Kernel.SymmetricTriangular, optDouble1: 0.7, activeFeatures: this._activeFeatures));
                            tasks.Add(t1);
                        }
                        Task.WaitAll(tasks.ToArray());
                        updateTasks(tasks);

                        break;
                    }
                case MODE.SAMPLED_TRI_080:
                    {
                        List<Task> tasks = new List<Task>();
                        for (int i = 0; i < _numRuns; i++)
                        {
                            int c = i;
                            // compute results for static model and standard gaussian ksvm
                            Task t1 = Task.Run(() => createInstance("SAMPLED_TRI_080" + c, MODEL.SAMPLED, 100.0, this._numFolds, 1, 0.6, 0.6, Kernel.SymmetricTriangular, optDouble1: 0.8, activeFeatures: this._activeFeatures));
                            tasks.Add(t1);
                        }
                        Task.WaitAll(tasks.ToArray());
                        updateTasks(tasks);

                        break;
                    }

                /* ANOVA */
                case MODE.SAMPLED_ANOVA_16_2:
                    {
                        List<Task> tasks = new List<Task>();
                        for (int i = 0; i < _numRuns; i++)
                        {
                            int c = i;
                            // compute results for static model and standard gaussian ksvm
                            Task t1 = Task.Run(() => createInstance("SAMPLED_ANOVA_16_2" + c, MODEL.SAMPLED, 100.0, this._numFolds, 1, 0.05, 0.05, Kernel.Anova, optInt1: 16, optInt2: 2, activeFeatures: this._activeFeatures));
                            tasks.Add(t1);
                        }
                        Task.WaitAll(tasks.ToArray());
                        updateTasks(tasks);

                        break;
                    }
                case MODE.SAMPLED_ANOVA_16_4:
                    {
                        List<Task> tasks = new List<Task>();
                        for (int i = 0; i < _numRuns; i++)
                        {
                            int c = i;
                            // compute results for static model and standard gaussian ksvm
                            Task t1 = Task.Run(() => createInstance("SAMPLED_ANOVA_16_4" + c, MODEL.SAMPLED, 100.0, this._numFolds, 1, 0.05, 0.05, Kernel.Anova, optInt1: 16, optInt2: 4, activeFeatures: this._activeFeatures));
                            tasks.Add(t1);
                        }
                        Task.WaitAll(tasks.ToArray());
                        updateTasks(tasks);

                        break;
                    }
                case MODE.SAMPLED_ANOVA_16_8:
                    {
                        List<Task> tasks = new List<Task>();
                        for (int i = 0; i < _numRuns; i++)
                        {
                            int c = i;
                            // compute results for static model and standard gaussian ksvm
                            Task t1 = Task.Run(() => createInstance("SAMPLED_ANOVA_16_8" + c, MODEL.SAMPLED, 100.0, this._numFolds, 1, 0.05, 0.05, Kernel.Anova, optInt1: 16, optInt2: 8, activeFeatures: this._activeFeatures));
                            tasks.Add(t1);
                        }
                        Task.WaitAll(tasks.ToArray());
                        updateTasks(tasks);

                        break;
                    }
                case MODE.SAMPLED_ANOVA_16_12:
                    {
                        List<Task> tasks = new List<Task>();
                        for (int i = 0; i < _numRuns; i++)
                        {
                            int c = i;
                            // compute results for static model and standard gaussian ksvm
                            Task t1 = Task.Run(() => createInstance("SAMPLED_ANOVA_16_12" + c, MODEL.SAMPLED, 100.0, this._numFolds, 1, 0.05, 0.05, Kernel.Anova, optInt1: 16, optInt2: 12, activeFeatures: this._activeFeatures));
                            tasks.Add(t1);
                        }
                        Task.WaitAll(tasks.ToArray());
                        updateTasks(tasks);

                        break;
                    }
                case MODE.SAMPLED_ANOVA_16_14:
                    {
                        List<Task> tasks = new List<Task>();
                        for (int i = 0; i < _numRuns; i++)
                        {
                            int c = i;
                            // compute results for static model and standard gaussian ksvm
                            Task t1 = Task.Run(() => createInstance("SAMPLED_ANOVA_16_14" + c, MODEL.SAMPLED, 100.0, this._numFolds, 1, 0.05, 0.05, Kernel.Anova, optInt1: 16, optInt2: 14, activeFeatures: this._activeFeatures));
                            tasks.Add(t1);
                        }
                        Task.WaitAll(tasks.ToArray());
                        updateTasks(tasks);

                        break;
                    }
                /* ------------------ END OF SAMPLED KERNEL MODES ------------------ */

            /** STATIC **/

                case MODE.STATIC_GAU_CAU:
                    {
                        /*
                        List<string> featuresStatistical = new List<string>();
                        IModel modelStatistical = _analyzer.getModel(MODEL.STATISTICAL);
                        foreach (var feature in modelStatistical.getFeatureNames())
                        {
                            if (feature.Contains("_Max") || feature.Contains("_Min"))
                                featuresStatistical.Add(feature);
                        }
                        */
                        List<Task> tasks = new List<Task>();
                        for (int i = 0; i < _numRuns; i++)
                        {
                            int c = i;
                            // compute results for static model and standard gaussian ksvm
                            Task t1 = Task.Run(() => createInstance("staticGAU" + c, MODEL.STATIC, 100.0, this._numFolds, 1, 0.1, 0.6, Kernel.GAUSSIAN, activeFeatures: this._activeFeatures));
                            Task t4 = Task.Run(() => createInstance("staticCAU" + c, MODEL.STATIC, 100.0, this._numFolds, 1, 0.1, 0.5, Kernel.Cauchy, optDouble1: 0.1, activeFeatures: this._activeFeatures));
                            tasks.Add(t1);
                            tasks.Add(t4);
                        }
                        Task.WaitAll(tasks.ToArray());
                        updateTasks(tasks);
                            
                        
                        break;
                    }
                case MODE.STATIC_GAU_TRI:
                    {
                        /*
                        List<string> featuresStatistical = new List<string>();
                        IModel modelStatistical = _analyzer.getModel(MODEL.STATISTICAL);
                        foreach (var feature in modelStatistical.getFeatureNames())
                        {
                            if (feature.Contains("_Max") || feature.Contains("_Min"))
                                featuresStatistical.Add(feature);
                        }
                        */
                        List<Task> tasks = new List<Task>();

                        for (int i = 0; i < _numRuns; i++)
                        {
                            int c = i;
                            // compute results for static model and standard gaussian ksvm
                            Task t1 = Task.Run(() => createInstance("staticGAU" + c, MODEL.STATIC, 100.0, this._numFolds, 1, 0.1, 0.6, Kernel.GAUSSIAN, activeFeatures: this._activeFeatures));
                            //Task t2 = Task.Run(() => createInstance("static2", MODEL.STATIC, 100.0, this._numFolds, 1, 0.1, 0.6, Kernel.LAPLACIAN));
                            Task t3 = Task.Run(() => createInstance("staticTRI" + c, MODEL.STATIC, 100.0, this._numFolds, 1, 0.1, 0.6, Kernel.SymmetricTriangular, optDouble1: 5.0, activeFeatures: this._activeFeatures));
                            //Task t4 = Task.Run(() => createInstance("static4", MODEL.STATIC, 100.0, this._numFolds, 1, 0.1, 0.5, Kernel.Cauchy, optDouble1: 0.1));
                            tasks.Add(t1);
                            tasks.Add(t3);
                            //tasks.Add(t3);
                            //tasks.Add(t4);
                        }

                        Task.WaitAll(tasks.ToArray());
                        updateTasks(tasks);
                            

                        break;
                    }
                case MODE.STATIC_LAP_CAU:
                    {
                        /*
                        List<string> featuresStatistical = new List<string>();
                        IModel modelStatistical = _analyzer.getModel(MODEL.STATISTICAL);
                        foreach (var feature in modelStatistical.getFeatureNames())
                        {
                            if (feature.Contains("_Max") || feature.Contains("_Min"))
                                featuresStatistical.Add(feature);
                        }
                        */
                        List<Task> tasks = new List<Task>();
                        for (int i = 0; i < _numRuns; i++)
                        {
                            int c = i;
                            // compute results for static model and standard gaussian ksvm
                            //Task t1 = Task.Run(() => createInstance("static1", MODEL.STATIC, 100.0, this._numFolds, 1, 0.1, 0.6, Kernel.GAUSSIAN));
                            Task t2 = Task.Run(() => createInstance("staticLAP" + c, MODEL.STATIC, 100.0, this._numFolds, 1, 0.1, 0.6, Kernel.LAPLACIAN, activeFeatures: this._activeFeatures));
                            //Task t3 = Task.Run(() => createInstance("static3", MODEL.STATIC, 100.0, this._numFolds, 1, 0.1, 0.6, Kernel.SymmetricTriangular, optDouble1: 5.0));
                            Task t4 = Task.Run(() => createInstance("staticCAU" + c, MODEL.STATIC, 100.0, this._numFolds, 1, 0.1, 0.5, Kernel.Cauchy, optDouble1: 0.1, activeFeatures: this._activeFeatures));
                            tasks.Add(t2);
                            tasks.Add(t4);
                            //tasks.Add(t3);
                            //tasks.Add(t4);
                        }
                        
                        Task.WaitAll(tasks.ToArray());
                        updateTasks(tasks);
                            

                        break;
                    }
                case MODE.STATIC_LAP_TRI:
                    {
                        /*
                        List<string> featuresStatistical = new List<string>();
                        IModel modelStatistical = _analyzer.getModel(MODEL.STATISTICAL);
                        foreach (var feature in modelStatistical.getFeatureNames())
                        {
                            if (feature.Contains("_Max") || feature.Contains("_Min"))
                                featuresStatistical.Add(feature);
                        }
                        */
                        List<Task> tasks = new List<Task>();
                        for (int i = 0; i < _numRuns; i++)
                        {
                            int c = i;
                            // compute results for static model and standard gaussian ksvm
                            //Task t1 = Task.Run(() => createInstance("static1", MODEL.STATIC, 100.0, this._numFolds, 1, 0.1, 0.6, Kernel.GAUSSIAN));
                            Task t2 = Task.Run(() => createInstance("staticLAP" + c, MODEL.STATIC, 100.0, this._numFolds, 1, 0.1, 0.6, Kernel.LAPLACIAN, activeFeatures: this._activeFeatures));
                            Task t3 = Task.Run(() => createInstance("staticTRI" + c, MODEL.STATIC, 100.0, this._numFolds, 1, 0.1, 0.6, Kernel.SymmetricTriangular, optDouble1: 5.0, activeFeatures: this._activeFeatures));
                            //Task t4 = Task.Run(() => createInstance("static4", MODEL.STATIC, 100.0, this._numFolds, 1, 0.1, 0.5, Kernel.Cauchy, optDouble1: 0.1));
                            tasks.Add(t2);
                            tasks.Add(t3);
                            //tasks.Add(t3);
                            //tasks.Add(t4);
                        }
                        
                        Task.WaitAll(tasks.ToArray());
                        updateTasks(tasks);
                            

                        break;
                    }
                case MODE.STATIC_GAU_LAP:
                    {
                        /*
                        List<string> featuresStatistical = new List<string>();
                        IModel modelStatistical = _analyzer.getModel(MODEL.STATISTICAL);
                        foreach (var feature in modelStatistical.getFeatureNames())
                        {
                            if (feature.Contains("_Max") || feature.Contains("_Min"))
                                featuresStatistical.Add(feature);
                        }
                        */
                        List<Task> tasks = new List<Task>();
                        for (int i = 0; i < _numRuns; i++)
                        {
                            int c = i;
                            // compute results for static model and standard gaussian ksvm
                            Task t1 = Task.Run(() => createInstance("staticGAU" + c, MODEL.STATIC, 100.0, this._numFolds, 1, 0.1, 0.6, Kernel.GAUSSIAN, activeFeatures: this._activeFeatures));
                            Task t2 = Task.Run(() => createInstance("staticLAP" + c, MODEL.STATIC, 100.0, this._numFolds, 1, 0.1, 0.6, Kernel.LAPLACIAN, activeFeatures: this._activeFeatures));
                            //Task t3 = Task.Run(() => createInstance("static3", MODEL.STATIC, 100.0, this._numFolds, 1, 0.1, 0.6, Kernel.SymmetricTriangular, optDouble1: 5.0));
                            //Task t4 = Task.Run(() => createInstance("static4", MODEL.STATIC, 100.0, this._numFolds, 1, 0.1, 0.5, Kernel.Cauchy, optDouble1: 0.1));
                            tasks.Add(t1);
                            tasks.Add(t2);
                            //tasks.Add(t3);
                            //tasks.Add(t4);
                        }
                        
                        Task.WaitAll(tasks.ToArray());
                        updateTasks(tasks);
                            

                        break;
                    }

                case MODE.STATIC_GAU_LAP_TRI:
                    {
                        /*
                        List<string> featuresStatistical = new List<string>();
                        IModel modelStatistical = _analyzer.getModel(MODEL.STATISTICAL);
                        foreach (var feature in modelStatistical.getFeatureNames())
                        {
                            if (feature.Contains("_Max") || feature.Contains("_Min"))
                                featuresStatistical.Add(feature);
                        }
                        */
                        List<Task> tasks = new List<Task>();
                        for (int i = 0; i < _numRuns; i++)
                        {
                            int c = i;
                            // compute results for static model and standard gaussian ksvm
                            Task t1 = Task.Run(() => createInstance("staticGAU" + c, MODEL.STATIC, 100.0, this._numFolds, 1, 0.1, 0.6, Kernel.GAUSSIAN, activeFeatures: this._activeFeatures));
                            Task t2 = Task.Run(() => createInstance("staticLAP" + c, MODEL.STATIC, 100.0, this._numFolds, 1, 0.1, 0.6, Kernel.LAPLACIAN, activeFeatures: this._activeFeatures));
                            Task t3 = Task.Run(() => createInstance("staticTRI" + c, MODEL.STATIC, 100.0, this._numFolds, 1, 0.1, 0.6, Kernel.SymmetricTriangular, optDouble1: 5.0, activeFeatures: this._activeFeatures));
                            //Task t4 = Task.Run(() => createInstance("static4", MODEL.STATIC, 100.0, this._numFolds, 1, 0.1, 0.5, Kernel.Cauchy, optDouble1: 0.1));
                            tasks.Add(t1);
                            tasks.Add(t2);
                            tasks.Add(t3);
                            //tasks.Add(t4);
                        }
                       
                        Task.WaitAll(tasks.ToArray());
                        updateTasks(tasks);
                            

                        break;
                    }

                case MODE.STATIC_GAU_LAP_TRI_CAU:
                    {
                        /*
                        List<string> featuresStatistical = new List<string>();
                        IModel modelStatistical = _analyzer.getModel(MODEL.STATISTICAL);
                        foreach (var feature in modelStatistical.getFeatureNames())
                        {
                            if (feature.Contains("_Max") || feature.Contains("_Min"))
                                featuresStatistical.Add(feature);
                        }
                        */
                        List<Task> tasks = new List<Task>();
                        for (int i = 0; i < _numRuns; i++)
                        {
                            int c = i;
                            // compute results for static model and standard gaussian ksvm
                            Task t1 = Task.Run(() => createInstance("staticGAU" + c, MODEL.STATIC, 100.0, this._numFolds, 1, 0.1, 0.6, Kernel.GAUSSIAN, activeFeatures: this._activeFeatures));
                            Task t2 = Task.Run(() => createInstance("staticLAP" + c, MODEL.STATIC, 100.0, this._numFolds, 1, 0.1, 0.6, Kernel.LAPLACIAN, activeFeatures: this._activeFeatures));
                            Task t3 = Task.Run(() => createInstance("staticTRI" + c, MODEL.STATIC, 100.0, this._numFolds, 1, 0.1, 0.6, Kernel.SymmetricTriangular, optDouble1: 5.0, activeFeatures: this._activeFeatures));
                            Task t4 = Task.Run(() => createInstance("staticCAU" + c, MODEL.STATIC, 100.0, this._numFolds, 1, 0.1, 0.5, Kernel.Cauchy, optDouble1: 0.1, activeFeatures: this._activeFeatures));
                            tasks.Add(t1);
                            tasks.Add(t2);
                            tasks.Add(t3);
                            tasks.Add(t4);
                        }
                        
                        Task.WaitAll(tasks.ToArray());
                        updateTasks(tasks);
                            

                        break;
                    }

                /* COMBINED */

                case MODE.COMB_MIN_MAX_GAU_CAU:
                    {
                        List<string> featuresStatistical = new List<string>();
                        IModel modelStatistical = _analyzer.getModel(MODEL.STATISTICAL);
                        foreach (var feature in modelStatistical.getFeatureNames())
                        {
                            if (feature.Contains("_Max") || feature.Contains("_Min"))
                                featuresStatistical.Add(feature);
                        }
                        List<Task> tasks = new List<Task>();
                        for (int i = 0; i < _numRuns; i++)
                        {
                            int c = i;
                            // compute results for static model and standard gaussian ksvm
                            Task t1 = Task.Run(() => createInstance("statGAU" + c, MODEL.STATISTICAL, 100.0, this._numFolds, 1, 0.1, 0.6, Kernel.GAUSSIAN, activeFeatures: this._activeFeatures));
                            //Task t2 = Task.Run(() => createInstance("statLAP" + c, MODEL.STATISTICAL, 100.0, this._numFolds, 1, 0.1, 0.6, Kernel.LAPLACIAN, activeFeatures: featuresStatistical));
                            //Task t3 = Task.Run(() => createInstance("statTRI" + c, MODEL.STATISTICAL, 100.0, this._numFolds, 1, 0.1, 0.6, Kernel.SymmetricTriangular, optDouble1: 5.0, activeFeatures: featuresStatistical));
                            Task t4 = Task.Run(() => createInstance("statCAU" + c, MODEL.STATISTICAL, 100.0, this._numFolds, 1, 0.1, 0.5, Kernel.Cauchy, optDouble1: 0.1, activeFeatures: this._activeFeatures));
                            Task t2 = Task.Run(() => createInstance("staticGAU" + c, MODEL.STATIC, 100.0, this._numFolds, 1, 0.1, 0.6, Kernel.GAUSSIAN));
                            Task t3 = Task.Run(() => createInstance("staticCAU" + c, MODEL.STATIC, 100.0, this._numFolds, 1, 0.1, 0.5, Kernel.Cauchy, optDouble1: 0.1));
                            tasks.Add(t1);
                            tasks.Add(t2);
                            tasks.Add(t3);
                            tasks.Add(t4);
                        }
                        Task.WaitAll(tasks.ToArray());
                        updateTasks(tasks);
                            

                        break;
                    }
                case MODE.COMB_MIN_MAX_GAU_TRI:
                    {
                        List<string> featuresStatistical = new List<string>();
                        IModel modelStatistical = _analyzer.getModel(MODEL.STATISTICAL);
                        foreach (var feature in modelStatistical.getFeatureNames())
                        {
                            if (feature.Contains("_Max") || feature.Contains("_Min"))
                                featuresStatistical.Add(feature);
                        }
                        List<Task> tasks = new List<Task>();
                        for (int i = 0; i < _numRuns; i++)
                        {
                            int c = i;
                            // compute results for static model and standard gaussian ksvm
                            Task t1 = Task.Run(() => createInstance("statGAU" + c, MODEL.STATISTICAL, 100.0, this._numFolds, 1, 0.1, 0.6, Kernel.GAUSSIAN, activeFeatures: this._activeFeatures));
                            Task t2 = Task.Run(() => createInstance("staticGAU" + c, MODEL.STATIC, 100.0, this._numFolds, 1, 0.1, 0.6, Kernel.GAUSSIAN));
                            Task t3 = Task.Run(() => createInstance("statTRI" + c, MODEL.STATISTICAL, 100.0, this._numFolds, 1, 0.1, 0.6, Kernel.SymmetricTriangular, optDouble1: 5.0, activeFeatures: this._activeFeatures));
                            Task t4 = Task.Run(() => createInstance("staticTRI" + c, MODEL.STATIC, 100.0, this._numFolds, 1, 0.1, 0.6, Kernel.SymmetricTriangular, optDouble1: 5.0));
                            
                            tasks.Add(t1);
                            tasks.Add(t2);
                            tasks.Add(t3);
                            tasks.Add(t4);
                        }

                        Task.WaitAll(tasks.ToArray());
                        updateTasks(tasks);
                            

                        break;
                    }
                case MODE.COMB_MIN_MAX_LAP_CAU:
                    {
                        List<string> featuresStatistical = new List<string>();
                        IModel modelStatistical = _analyzer.getModel(MODEL.STATISTICAL);
                        foreach (var feature in modelStatistical.getFeatureNames())
                        {
                            if (feature.Contains("_Max") || feature.Contains("_Min"))
                                featuresStatistical.Add(feature);
                        }
                        List<Task> tasks = new List<Task>();
                        for (int i = 0; i < _numRuns; i++)
                        {
                            int c = i;
                            // compute results for static model and standard gaussian ksvm
                            Task t2 = Task.Run(() => createInstance("statLAP" + c, MODEL.STATISTICAL, 100.0, this._numFolds, 1, 0.1, 0.6, Kernel.LAPLACIAN, activeFeatures: this._activeFeatures));
                            Task t4 = Task.Run(() => createInstance("statCAU" + c, MODEL.STATISTICAL, 100.0, this._numFolds, 1, 0.1, 0.5, Kernel.Cauchy, optDouble1: 0.1, activeFeatures: this._activeFeatures));
                            Task t1 = Task.Run(() => createInstance("staticLAP" + c, MODEL.STATIC, 100.0, this._numFolds, 1, 0.1, 0.6, Kernel.LAPLACIAN));
                            Task t3 = Task.Run(() => createInstance("staticCAU" + c, MODEL.STATIC, 100.0, this._numFolds, 1, 0.1, 0.5, Kernel.Cauchy, optDouble1: 0.1));
                            tasks.Add(t1);
                            tasks.Add(t2);
                            tasks.Add(t3);
                            tasks.Add(t4);
                        }

                        Task.WaitAll(tasks.ToArray());
                        updateTasks(tasks);
                            

                        break;
                    }
                case MODE.COMB_MIN_MAX_LAP_TRI:
                    {
                        List<string> featuresStatistical = new List<string>();
                        IModel modelStatistical = _analyzer.getModel(MODEL.STATISTICAL);
                        foreach (var feature in modelStatistical.getFeatureNames())
                        {
                            if (feature.Contains("_Max") || feature.Contains("_Min"))
                                featuresStatistical.Add(feature);
                        }
                        List<Task> tasks = new List<Task>();
                        for (int i = 0; i < _numRuns; i++)
                        {
                            int c = i;
                            // compute results for static model and standard gaussian ksvm
                            Task t2 = Task.Run(() => createInstance("statLAP" + c, MODEL.STATISTICAL, 100.0, this._numFolds, 1, 0.1, 0.6, Kernel.LAPLACIAN, activeFeatures: this._activeFeatures));
                            Task t3 = Task.Run(() => createInstance("statTRI" + c, MODEL.STATISTICAL, 100.0, this._numFolds, 1, 0.1, 0.6, Kernel.SymmetricTriangular, optDouble1: 5.0, activeFeatures: this._activeFeatures));
                            Task t4 = Task.Run(() => createInstance("staticLAP" + c, MODEL.STATIC, 100.0, this._numFolds, 1, 0.1, 0.6, Kernel.LAPLACIAN));
                            Task t1 = Task.Run(() => createInstance("staticTRI" + c, MODEL.STATIC, 100.0, this._numFolds, 1, 0.1, 0.6, Kernel.SymmetricTriangular, optDouble1: 5.0));
                            
                            tasks.Add(t1);
                            tasks.Add(t2);
                            tasks.Add(t3);
                            tasks.Add(t4);
                        }

                        Task.WaitAll(tasks.ToArray());
                        updateTasks(tasks);
                            

                        break;
                    }
                case MODE.COMB_MIN_MAX_GAU_LAP:
                    {
                        List<string> featuresStatistical = new List<string>();
                        IModel modelStatistical = _analyzer.getModel(MODEL.STATISTICAL);
                        foreach (var feature in modelStatistical.getFeatureNames())
                        {
                            if (feature.Contains("_Max") || feature.Contains("_Min"))
                                featuresStatistical.Add(feature);
                        }
                        List<Task> tasks = new List<Task>();
                        for (int i = 0; i < _numRuns; i++)
                        {
                            int c = i;
                            // compute results for static model and standard gaussian ksvm
                            Task t1 = Task.Run(() => createInstance("statGAU" + c, MODEL.STATISTICAL, 100.0, this._numFolds, 1, 0.1, 0.6, Kernel.GAUSSIAN, activeFeatures: this._activeFeatures));
                            Task t2 = Task.Run(() => createInstance("statLAP" + c, MODEL.STATISTICAL, 100.0, this._numFolds, 1, 0.1, 0.6, Kernel.LAPLACIAN, activeFeatures: this._activeFeatures));
                            Task t3 = Task.Run(() => createInstance("staticGAU" + c, MODEL.STATIC, 100.0, this._numFolds, 1, 0.1, 0.6, Kernel.GAUSSIAN));
                            Task t4 = Task.Run(() => createInstance("staticLAP" + c, MODEL.STATIC, 100.0, this._numFolds, 1, 0.1, 0.6, Kernel.LAPLACIAN));
                            tasks.Add(t1);
                            tasks.Add(t2);
                            tasks.Add(t3);
                            tasks.Add(t4);
                        }

                        Task.WaitAll(tasks.ToArray());
                        updateTasks(tasks);
                            

                        break;
                    }

                case MODE.COMB_MIN_MAX_GAU_LAP_TRI:
                    {
                        List<string> featuresStatistical = new List<string>();
                        IModel modelStatistical = _analyzer.getModel(MODEL.STATISTICAL);
                        foreach (var feature in modelStatistical.getFeatureNames())
                        {
                            if (feature.Contains("_Max") || feature.Contains("_Min"))
                                featuresStatistical.Add(feature);
                        }
                        List<Task> tasks = new List<Task>();
                        for (int i = 0; i < _numRuns; i++)
                        {
                            int c = i;
                            // compute results for static model and standard gaussian ksvm
                            Task t1 = Task.Run(() => createInstance("statGAU" + c, MODEL.STATISTICAL, 100.0, this._numFolds, 1, 0.1, 0.6, Kernel.GAUSSIAN, activeFeatures: this._activeFeatures));
                            Task t2 = Task.Run(() => createInstance("statLAP" + c, MODEL.STATISTICAL, 100.0, this._numFolds, 1, 0.1, 0.6, Kernel.LAPLACIAN, activeFeatures: this._activeFeatures));
                            Task t3 = Task.Run(() => createInstance("statTRI" + c, MODEL.STATISTICAL, 100.0, this._numFolds, 1, 0.1, 0.6, Kernel.SymmetricTriangular, optDouble1: 5.0, activeFeatures: this._activeFeatures));
                            Task t4 = Task.Run(() => createInstance("staticGAU" + c, MODEL.STATIC, 100.0, this._numFolds, 1, 0.1, 0.6, Kernel.GAUSSIAN));
                            Task t5 = Task.Run(() => createInstance("staticLAP" + c, MODEL.STATIC, 100.0, this._numFolds, 1, 0.1, 0.6, Kernel.LAPLACIAN));
                            Task t6 = Task.Run(() => createInstance("staticTRI" + c, MODEL.STATIC, 100.0, this._numFolds, 1, 0.1, 0.6, Kernel.SymmetricTriangular, optDouble1: 5.0));
                            tasks.Add(t1);
                            tasks.Add(t2);
                            tasks.Add(t3);
                            tasks.Add(t4);
                            tasks.Add(t5);
                            tasks.Add(t6);
                        }

                        Task.WaitAll(tasks.ToArray());
                        updateTasks(tasks);
                            

                        break;
                    }

                case MODE.COMB_MIN_MAX_GAU_LAP_TRI_CAU:
                    {
                        List<string> featuresStatistical = new List<string>();
                        IModel modelStatistical = _analyzer.getModel(MODEL.STATISTICAL);
                        foreach (var feature in modelStatistical.getFeatureNames())
                        {
                            if (feature.Contains("_Max") || feature.Contains("_Min"))
                                featuresStatistical.Add(feature);
                        }
                        List<Task> tasks = new List<Task>();
                        for (int i = 0; i < _numRuns; i++)
                        {
                            int c = i;
                            // compute results for static model and standard gaussian ksvm
                            Task t1 = Task.Run(() => createInstance("statGAU" + c, MODEL.STATISTICAL, 100.0, this._numFolds, 1, 0.1, 0.6, Kernel.GAUSSIAN, activeFeatures: this._activeFeatures));
                            Task t2 = Task.Run(() => createInstance("statLAP" + c, MODEL.STATISTICAL, 100.0, this._numFolds, 1, 0.1, 0.6, Kernel.LAPLACIAN, activeFeatures: this._activeFeatures));
                            Task t3 = Task.Run(() => createInstance("statTRI" + c, MODEL.STATISTICAL, 100.0, this._numFolds, 1, 0.1, 0.6, Kernel.SymmetricTriangular, optDouble1: 5.0, activeFeatures: this._activeFeatures));
                            Task t4 = Task.Run(() => createInstance("statCAU" + c, MODEL.STATISTICAL, 100.0, this._numFolds, 1, 0.1, 0.5, Kernel.Cauchy, optDouble1: 0.1, activeFeatures: this._activeFeatures));
                            Task t5 = Task.Run(() => createInstance("staticGAU" + c, MODEL.STATIC, 100.0, this._numFolds, 1, 0.1, 0.6, Kernel.GAUSSIAN));
                            Task t6 = Task.Run(() => createInstance("staticLAP" + c, MODEL.STATIC, 100.0, this._numFolds, 1, 0.1, 0.6, Kernel.LAPLACIAN));
                            Task t7 = Task.Run(() => createInstance("staticTRI" + c, MODEL.STATIC, 100.0, this._numFolds, 1, 0.1, 0.6, Kernel.SymmetricTriangular, optDouble1: 5.0));
                            Task t8 = Task.Run(() => createInstance("staticCAU" + c, MODEL.STATIC, 100.0, this._numFolds, 1, 0.1, 0.5, Kernel.Cauchy, optDouble1: 0.1));
                            tasks.Add(t1);
                            tasks.Add(t2);
                            tasks.Add(t3);
                            tasks.Add(t4);
                            tasks.Add(t5);
                            tasks.Add(t6);
                            tasks.Add(t7);
                            tasks.Add(t8);
                        }

                        Task.WaitAll(tasks.ToArray());
                        updateTasks(tasks);
                            

                        break;
                    }

                /* SAMPLED */

                case MODE.SAMPLED_GAU_CAU:
                    {
                        List<string> featuresStatistical = new List<string>();
                        IModel modelStatistical = _analyzer.getModel(MODEL.SAMPLED);
                        foreach (var feature in modelStatistical.getFeatureNames())
                        {
                            if (feature.Contains("_Max") || feature.Contains("_Min"))
                                featuresStatistical.Add(feature);
                        }
                        List<Task> tasks = new List<Task>();
                        for (int i = 0; i < _numRuns; i++)
                        {
                            int c = i;
                            // compute results for static model and standard gaussian ksvm
                            Task t1 = Task.Run(() => createInstance("sampledGAU" + c, MODEL.SAMPLED, 100.0, this._numFolds, 1, 0.1, 0.6, Kernel.GAUSSIAN, activeFeatures: this._activeFeatures));
                            //Task t2 = Task.Run(() => createInstance("sampledLAP" + c, MODEL.SAMPLED, 100.0, this._numFolds, 1, 0.1, 0.6, Kernel.LAPLACIAN, activeFeatures: featuresStatistical));
                            //Task t3 = Task.Run(() => createInstance("sampledTRI" + c, MODEL.SAMPLED, 100.0, this._numFolds, 1, 0.1, 0.6, Kernel.SymmetricTriangular, optDouble1: 5.0, activeFeatures: featuresStatistical));
                            Task t4 = Task.Run(() => createInstance("sampledCAU" + c, MODEL.SAMPLED, 100.0, this._numFolds, 1, 0.1, 0.5, Kernel.Cauchy, optDouble1: 0.1, activeFeatures: this._activeFeatures));
                            tasks.Add(t1);
                            //tasks.Add(t2);
                            //tasks.Add(t3);
                            tasks.Add(t4);
                        }
                        Task.WaitAll(tasks.ToArray());
                        updateTasks(tasks);


                        break;
                    }
                case MODE.SAMPLED_GAU_TRI:
                    {
                        List<string> featuresStatistical = new List<string>();
                        IModel modelStatistical = _analyzer.getModel(MODEL.SAMPLED);
                        foreach (var feature in modelStatistical.getFeatureNames())
                        {
                            if (feature.Contains("_Max") || feature.Contains("_Min"))
                                featuresStatistical.Add(feature);
                        }
                        List<Task> tasks = new List<Task>();
                        for (int i = 0; i < _numRuns; i++)
                        {
                            int c = i;
                            // compute results for static model and standard gaussian ksvm
                            Task t1 = Task.Run(() => createInstance("sampledGAU" + c, MODEL.SAMPLED, 100.0, this._numFolds, 1, 0.1, 0.6, Kernel.GAUSSIAN, activeFeatures: this._activeFeatures));
                            //Task t2 = Task.Run(() => createInstance("sampledLAP" + c, MODEL.SAMPLED, 100.0, this._numFolds, 1, 0.1, 0.6, Kernel.LAPLACIAN, activeFeatures: featuresStatistical));
                            Task t3 = Task.Run(() => createInstance("sampledTRI" + c, MODEL.SAMPLED, 100.0, this._numFolds, 1, 0.1, 0.6, Kernel.SymmetricTriangular, optDouble1: 5.0, activeFeatures: this._activeFeatures));
                            //Task t4 = Task.Run(() => createInstance("sampledCAU" + c, MODEL.SAMPLED, 100.0, this._numFolds, 1, 0.1, 0.5, Kernel.Cauchy, optDouble1: 0.1, activeFeatures: featuresStatistical));
                            tasks.Add(t1);
                            //tasks.Add(t2);
                            tasks.Add(t3);
                            //tasks.Add(t4);
                        }

                        Task.WaitAll(tasks.ToArray());
                        updateTasks(tasks);


                        break;
                    }
                case MODE.SAMPLED_LAP_CAU:
                    {
                        List<string> featuresStatistical = new List<string>();
                        IModel modelStatistical = _analyzer.getModel(MODEL.SAMPLED);
                        foreach (var feature in modelStatistical.getFeatureNames())
                        {
                            if (feature.Contains("_Max") || feature.Contains("_Min"))
                                featuresStatistical.Add(feature);
                        }
                        List<Task> tasks = new List<Task>();
                        for (int i = 0; i < _numRuns; i++)
                        {
                            int c = i;
                            // compute results for static model and standard gaussian ksvm
                            //Task t1 = Task.Run(() => createInstance("sampledGAU" + c, MODEL.SAMPLED, 100.0, this._numFolds, 1, 0.1, 0.6, Kernel.GAUSSIAN, activeFeatures: featuresStatistical));
                            Task t2 = Task.Run(() => createInstance("sampledLAP" + c, MODEL.SAMPLED, 100.0, this._numFolds, 1, 0.1, 0.6, Kernel.LAPLACIAN, activeFeatures: this._activeFeatures));
                            //Task t3 = Task.Run(() => createInstance("sampledTRI" + c, MODEL.SAMPLED, 100.0, this._numFolds, 1, 0.1, 0.6, Kernel.SymmetricTriangular, optDouble1: 5.0, activeFeatures: featuresStatistical));
                            Task t4 = Task.Run(() => createInstance("sampledCAU" + c, MODEL.SAMPLED, 100.0, this._numFolds, 1, 0.1, 0.5, Kernel.Cauchy, optDouble1: 0.1, activeFeatures: this._activeFeatures));
                            //tasks.Add(t1);
                            tasks.Add(t2);
                            //tasks.Add(t3);
                            tasks.Add(t4);
                        }

                        Task.WaitAll(tasks.ToArray());
                        updateTasks(tasks);


                        break;
                    }
                case MODE.SAMPLED_LAP_TRI:
                    {
                        List<string> featuresStatistical = new List<string>();
                        IModel modelStatistical = _analyzer.getModel(MODEL.SAMPLED);
                        foreach (var feature in modelStatistical.getFeatureNames())
                        {
                            if (feature.Contains("_Max") || feature.Contains("_Min"))
                                featuresStatistical.Add(feature);
                        }
                        List<Task> tasks = new List<Task>();
                        for (int i = 0; i < _numRuns; i++)
                        {
                            int c = i;
                            // compute results for static model and standard gaussian ksvm
                            //Task t1 = Task.Run(() => createInstance("sampledGAU" + c, MODEL.SAMPLED, 100.0, this._numFolds, 1, 0.1, 0.6, Kernel.GAUSSIAN, activeFeatures: featuresStatistical));
                            Task t2 = Task.Run(() => createInstance("sampledLAP" + c, MODEL.SAMPLED, 100.0, this._numFolds, 1, 0.1, 0.6, Kernel.LAPLACIAN, activeFeatures: this._activeFeatures));
                            Task t3 = Task.Run(() => createInstance("sampledTRI" + c, MODEL.SAMPLED, 100.0, this._numFolds, 1, 0.1, 0.6, Kernel.SymmetricTriangular, optDouble1: 5.0, activeFeatures: this._activeFeatures));
                            //Task t4 = Task.Run(() => createInstance("sampledCAU" + c, MODEL.SAMPLED, 100.0, this._numFolds, 1, 0.1, 0.5, Kernel.Cauchy, optDouble1: 0.1, activeFeatures: featuresStatistical));
                            //tasks.Add(t1);
                            tasks.Add(t2);
                            tasks.Add(t3);
                            //tasks.Add(t4);
                        }

                        Task.WaitAll(tasks.ToArray());
                        updateTasks(tasks);


                        break;
                    }
                case MODE.SAMPLED_GAU_LAP:
                    {
                        List<string> featuresStatistical = new List<string>();
                        IModel modelStatistical = _analyzer.getModel(MODEL.SAMPLED);
                        foreach (var feature in modelStatistical.getFeatureNames())
                        {
                            if (feature.Contains("_Max") || feature.Contains("_Min"))
                                featuresStatistical.Add(feature);
                        }
                        List<Task> tasks = new List<Task>();
                        for (int i = 0; i < _numRuns; i++)
                        {
                            int c = i;
                            // compute results for static model and standard gaussian ksvm
                            Task t1 = Task.Run(() => createInstance("sampledGAU" + c, MODEL.SAMPLED, 100.0, this._numFolds, 1, 0.1, 0.6, Kernel.GAUSSIAN, activeFeatures: this._activeFeatures));
                            Task t2 = Task.Run(() => createInstance("sampledLAP" + c, MODEL.SAMPLED, 100.0, this._numFolds, 1, 0.1, 0.6, Kernel.LAPLACIAN, activeFeatures: this._activeFeatures));
                            //Task t3 = Task.Run(() => createInstance("sampledTRI" + c, MODEL.SAMPLED, 100.0, this._numFolds, 1, 0.1, 0.6, Kernel.SymmetricTriangular, optDouble1: 5.0, activeFeatures: featuresStatistical));
                            //Task t4 = Task.Run(() => createInstance("sampledCAU" + c, MODEL.SAMPLED, 100.0, this._numFolds, 1, 0.1, 0.5, Kernel.Cauchy, optDouble1: 0.1, activeFeatures: featuresStatistical));
                            tasks.Add(t1);
                            tasks.Add(t2);
                            //tasks.Add(t3);
                            //tasks.Add(t4);
                        }

                        Task.WaitAll(tasks.ToArray());
                        updateTasks(tasks);


                        break;
                    }

                case MODE.SAMPLED_GAU_LAP_TRI:
                    {
                        List<string> featuresStatistical = new List<string>();
                        IModel modelStatistical = _analyzer.getModel(MODEL.SAMPLED);
                        foreach (var feature in modelStatistical.getFeatureNames())
                        {
                            if (feature.Contains("_Max") || feature.Contains("_Min"))
                                featuresStatistical.Add(feature);
                        }
                        List<Task> tasks = new List<Task>();
                        for (int i = 0; i < _numRuns; i++)
                        {
                            int c = i;
                            // compute results for static model and standard gaussian ksvm
                            Task t1 = Task.Run(() => createInstance("sampledGAU" + c, MODEL.SAMPLED, 100.0, this._numFolds, 1, 0.1, 0.6, Kernel.GAUSSIAN, activeFeatures: this._activeFeatures));
                            Task t2 = Task.Run(() => createInstance("sampledLAP" + c, MODEL.SAMPLED, 100.0, this._numFolds, 1, 0.1, 0.6, Kernel.LAPLACIAN, activeFeatures: this._activeFeatures));
                            Task t3 = Task.Run(() => createInstance("sampledTRI" + c, MODEL.SAMPLED, 100.0, this._numFolds, 1, 0.1, 0.6, Kernel.SymmetricTriangular, optDouble1: 5.0, activeFeatures: this._activeFeatures));
                            //Task t4 = Task.Run(() => createInstance("sampledCAU" + c, MODEL.SAMPLED, 100.0, this._numFolds, 1, 0.1, 0.5, Kernel.Cauchy, optDouble1: 0.1, activeFeatures: this._activeFeatures));
                            tasks.Add(t1);
                            tasks.Add(t2);
                            tasks.Add(t3);
                            //tasks.Add(t4);
                        }

                        Task.WaitAll(tasks.ToArray());
                        updateTasks(tasks);


                        break;
                    }

                case MODE.SAMPLED_GAU_LAP_TRI_CAU:
                    {
                        List<string> featuresStatistical = new List<string>();
                        IModel modelStatistical = _analyzer.getModel(MODEL.SAMPLED);
                        foreach (var feature in modelStatistical.getFeatureNames())
                        {
                            if (feature.Contains("_Max") || feature.Contains("_Min"))
                                featuresStatistical.Add(feature);
                        }
                        List<Task> tasks = new List<Task>();
                        for (int i = 0; i < _numRuns; i++)
                        {
                            int c = i;
                            // compute results for static model and standard gaussian ksvm
                            Task t1 = Task.Run(() => createInstance("sampledGAU" + c, MODEL.SAMPLED, 100.0, this._numFolds, 1, 0.1, 0.6, Kernel.GAUSSIAN, activeFeatures: this._activeFeatures));
                            Task t2 = Task.Run(() => createInstance("sampledLAP" + c, MODEL.SAMPLED, 100.0, this._numFolds, 1, 0.1, 0.6, Kernel.LAPLACIAN, activeFeatures: this._activeFeatures));
                            Task t3 = Task.Run(() => createInstance("sampledTRI" + c, MODEL.SAMPLED, 100.0, this._numFolds, 1, 0.1, 0.6, Kernel.SymmetricTriangular, optDouble1: 5.0, activeFeatures: this._activeFeatures));
                            Task t4 = Task.Run(() => createInstance("sampledCAU" + c, MODEL.SAMPLED, 100.0, this._numFolds, 1, 0.1, 0.5, Kernel.Cauchy, optDouble1: 0.1, activeFeatures: this._activeFeatures));
                            tasks.Add(t1);
                            tasks.Add(t2);
                            tasks.Add(t3);
                            tasks.Add(t4);
                        }

                        Task.WaitAll(tasks.ToArray());
                        updateTasks(tasks);


                        break;
                    }

                /* STATISTICAL */

                case MODE.STAT_MIN_MAX_GAU_CAU:
                    {
                        List<string> featuresStatistical = new List<string>();
                        IModel modelStatistical = _analyzer.getModel(MODEL.STATISTICAL);
                        foreach (var feature in modelStatistical.getFeatureNames())
                        {
                            if (feature.Contains("_Max") || feature.Contains("_Min"))
                                featuresStatistical.Add(feature);
                        }
                        List<Task> tasks = new List<Task>();
                        for (int i = 0; i < _numRuns; i++)
                        {
                            int c = i;
                            // compute results for static model and standard gaussian ksvm
                            Task t1 = Task.Run(() => createInstance("statGAU" + c, MODEL.STATISTICAL, 100.0, this._numFolds, 1, 0.1, 0.6, Kernel.GAUSSIAN, activeFeatures: this._activeFeatures));
                            //Task t2 = Task.Run(() => createInstance("statLAP" + c, MODEL.STATISTICAL, 100.0, this._numFolds, 1, 0.1, 0.6, Kernel.LAPLACIAN, activeFeatures: featuresStatistical));
                            //Task t3 = Task.Run(() => createInstance("statTRI" + c, MODEL.STATISTICAL, 100.0, this._numFolds, 1, 0.1, 0.6, Kernel.SymmetricTriangular, optDouble1: 5.0, activeFeatures: featuresStatistical));
                            Task t4 = Task.Run(() => createInstance("statCAU" + c, MODEL.STATISTICAL, 100.0, this._numFolds, 1, 0.1, 0.5, Kernel.Cauchy, optDouble1: 0.1, activeFeatures: this._activeFeatures));
                            tasks.Add(t1);
                            //tasks.Add(t2);
                            //tasks.Add(t3);
                            tasks.Add(t4);
                        }
                        Task.WaitAll(tasks.ToArray());
                        updateTasks(tasks);
                            

                        break;
                    }
                case MODE.STAT_MIN_MAX_GAU_TRI:
                    {
                        List<string> featuresStatistical = new List<string>();
                        IModel modelStatistical = _analyzer.getModel(MODEL.STATISTICAL);
                        foreach (var feature in modelStatistical.getFeatureNames())
                        {
                            if (feature.Contains("_Max") || feature.Contains("_Min"))
                                featuresStatistical.Add(feature);
                        }
                        List<Task> tasks = new List<Task>();
                        for (int i = 0; i < _numRuns; i++)
                        {
                            int c = i;
                            // compute results for static model and standard gaussian ksvm
                            Task t1 = Task.Run(() => createInstance("statGAU" + c, MODEL.STATISTICAL, 100.0, this._numFolds, 1, 0.1, 0.6, Kernel.GAUSSIAN, activeFeatures: this._activeFeatures));
                            //Task t2 = Task.Run(() => createInstance("statLAP" + c, MODEL.STATISTICAL, 100.0, this._numFolds, 1, 0.1, 0.6, Kernel.LAPLACIAN, activeFeatures: featuresStatistical));
                            Task t3 = Task.Run(() => createInstance("statTRI" + c, MODEL.STATISTICAL, 100.0, this._numFolds, 1, 0.1, 0.6, Kernel.SymmetricTriangular, optDouble1: 5.0, activeFeatures: this._activeFeatures));
                            //Task t4 = Task.Run(() => createInstance("statCAU" + c, MODEL.STATISTICAL, 100.0, this._numFolds, 1, 0.1, 0.5, Kernel.Cauchy, optDouble1: 0.1, activeFeatures: featuresStatistical));
                            tasks.Add(t1);
                            //tasks.Add(t2);
                            tasks.Add(t3);
                            //tasks.Add(t4);
                        }

                        Task.WaitAll(tasks.ToArray());
                        updateTasks(tasks);
                            

                        break;
                    }
                case MODE.STAT_MIN_MAX_LAP_CAU:
                    {
                        List<string> featuresStatistical = new List<string>();
                        IModel modelStatistical = _analyzer.getModel(MODEL.STATISTICAL);
                        foreach (var feature in modelStatistical.getFeatureNames())
                        {
                            if (feature.Contains("_Max") || feature.Contains("_Min"))
                                featuresStatistical.Add(feature);
                        }
                        List<Task> tasks = new List<Task>();
                        for (int i = 0; i < _numRuns; i++)
                        {
                            int c = i;
                            // compute results for static model and standard gaussian ksvm
                            //Task t1 = Task.Run(() => createInstance("statGAU" + c, MODEL.STATISTICAL, 100.0, this._numFolds, 1, 0.1, 0.6, Kernel.GAUSSIAN, activeFeatures: featuresStatistical));
                            Task t2 = Task.Run(() => createInstance("statLAP" + c, MODEL.STATISTICAL, 100.0, this._numFolds, 1, 0.1, 0.6, Kernel.LAPLACIAN, activeFeatures: this._activeFeatures));
                            //Task t3 = Task.Run(() => createInstance("statTRI" + c, MODEL.STATISTICAL, 100.0, this._numFolds, 1, 0.1, 0.6, Kernel.SymmetricTriangular, optDouble1: 5.0, activeFeatures: featuresStatistical));
                            Task t4 = Task.Run(() => createInstance("statCAU" + c, MODEL.STATISTICAL, 100.0, this._numFolds, 1, 0.1, 0.5, Kernel.Cauchy, optDouble1: 0.1, activeFeatures: this._activeFeatures));
                            //tasks.Add(t1);
                            tasks.Add(t2);
                            //tasks.Add(t3);
                            tasks.Add(t4);
                        }

                        Task.WaitAll(tasks.ToArray());
                        updateTasks(tasks);
                            

                        break;
                    }
                case MODE.STAT_MIN_MAX_LAP_TRI:
                    {
                        List<string> featuresStatistical = new List<string>();
                        IModel modelStatistical = _analyzer.getModel(MODEL.STATISTICAL);
                        foreach (var feature in modelStatistical.getFeatureNames())
                        {
                            if (feature.Contains("_Max") || feature.Contains("_Min"))
                                featuresStatistical.Add(feature);
                        }
                        List<Task> tasks = new List<Task>();
                        for (int i = 0; i < _numRuns; i++)
                        {
                            int c = i;
                            // compute results for static model and standard gaussian ksvm
                            //Task t1 = Task.Run(() => createInstance("statGAU" + c, MODEL.STATISTICAL, 100.0, this._numFolds, 1, 0.1, 0.6, Kernel.GAUSSIAN, activeFeatures: featuresStatistical));
                            Task t2 = Task.Run(() => createInstance("statLAP" + c, MODEL.STATISTICAL, 100.0, this._numFolds, 1, 0.1, 0.6, Kernel.LAPLACIAN, activeFeatures: this._activeFeatures));
                            Task t3 = Task.Run(() => createInstance("statTRI" + c, MODEL.STATISTICAL, 100.0, this._numFolds, 1, 0.1, 0.6, Kernel.SymmetricTriangular, optDouble1: 5.0, activeFeatures: this._activeFeatures));
                            //Task t4 = Task.Run(() => createInstance("statCAU" + c, MODEL.STATISTICAL, 100.0, this._numFolds, 1, 0.1, 0.5, Kernel.Cauchy, optDouble1: 0.1, activeFeatures: featuresStatistical));
                            //tasks.Add(t1);
                            tasks.Add(t2);
                            tasks.Add(t3);
                            //tasks.Add(t4);
                        }

                        Task.WaitAll(tasks.ToArray());
                        updateTasks(tasks);
                            

                        break;
                    }
                case MODE.STAT_MIN_MAX_GAU_LAP:
                    {
                        List<string> featuresStatistical = new List<string>();
                        IModel modelStatistical = _analyzer.getModel(MODEL.STATISTICAL);
                        foreach (var feature in modelStatistical.getFeatureNames())
                        {
                            if (feature.Contains("_Max") || feature.Contains("_Min"))
                                featuresStatistical.Add(feature);
                        }
                        List<Task> tasks = new List<Task>();
                        for (int i = 0; i < _numRuns; i++)
                        {
                            int c = i;
                            // compute results for static model and standard gaussian ksvm
                            Task t1 = Task.Run(() => createInstance("statGAU" + c, MODEL.STATISTICAL, 100.0, this._numFolds, 1, 0.1, 0.6, Kernel.GAUSSIAN, activeFeatures: this._activeFeatures));
                            Task t2 = Task.Run(() => createInstance("statLAP" + c, MODEL.STATISTICAL, 100.0, this._numFolds, 1, 0.1, 0.6, Kernel.LAPLACIAN, activeFeatures: this._activeFeatures));
                            //Task t3 = Task.Run(() => createInstance("statTRI" + c, MODEL.STATISTICAL, 100.0, this._numFolds, 1, 0.1, 0.6, Kernel.SymmetricTriangular, optDouble1: 5.0, activeFeatures: featuresStatistical));
                            //Task t4 = Task.Run(() => createInstance("statCAU" + c, MODEL.STATISTICAL, 100.0, this._numFolds, 1, 0.1, 0.5, Kernel.Cauchy, optDouble1: 0.1, activeFeatures: featuresStatistical));
                            tasks.Add(t1);
                            tasks.Add(t2);
                            //tasks.Add(t3);
                            //tasks.Add(t4);
                        }

                        Task.WaitAll(tasks.ToArray());
                        updateTasks(tasks);
                            

                        break;
                    }

                case MODE.STAT_MIN_MAX_GAU_LAP_TRI:
                    {
                        List<string> featuresStatistical = new List<string>();
                        IModel modelStatistical = _analyzer.getModel(MODEL.STATISTICAL);
                        foreach (var feature in modelStatistical.getFeatureNames())
                        {
                            if (feature.Contains("_Max") || feature.Contains("_Min"))
                                featuresStatistical.Add(feature);
                        }
                        List<Task> tasks = new List<Task>();
                        for (int i = 0; i < _numRuns; i++)
                        {
                            int c = i;
                            // compute results for static model and standard gaussian ksvm
                            Task t1 = Task.Run(() => createInstance("statGAU" + c, MODEL.STATISTICAL, 100.0, this._numFolds, 1, 0.1, 0.6, Kernel.GAUSSIAN, activeFeatures: this._activeFeatures));
                            Task t2 = Task.Run(() => createInstance("statLAP" + c, MODEL.STATISTICAL, 100.0, this._numFolds, 1, 0.1, 0.6, Kernel.LAPLACIAN, activeFeatures: this._activeFeatures));
                            Task t3 = Task.Run(() => createInstance("statTRI" + c, MODEL.STATISTICAL, 100.0, this._numFolds, 1, 0.1, 0.6, Kernel.SymmetricTriangular, optDouble1: 5.0, activeFeatures: this._activeFeatures));
                            //Task t4 = Task.Run(() => createInstance("statCAU" + c, MODEL.STATISTICAL, 100.0, this._numFolds, 1, 0.1, 0.5, Kernel.Cauchy, optDouble1: 0.1, activeFeatures: this._activeFeatures));
                            tasks.Add(t1);
                            tasks.Add(t2);
                            tasks.Add(t3);
                            //tasks.Add(t4);
                        }

                        Task.WaitAll(tasks.ToArray());
                        updateTasks(tasks);
                            

                        break;
                    }

                case MODE.STAT_MIN_MAX_GAU_LAP_TRI_CAU:
                    {
                        List<string> featuresStatistical = new List<string>();
                        IModel modelStatistical = _analyzer.getModel(MODEL.STATISTICAL);
                        foreach (var feature in modelStatistical.getFeatureNames())
                        {
                            if (feature.Contains("_Max") || feature.Contains("_Min"))
                                featuresStatistical.Add(feature);
                        }
                        List<Task> tasks = new List<Task>();
                        for (int i = 0; i < _numRuns; i++)
                        {
                            int c = i;
                            // compute results for static model and standard gaussian ksvm
                            Task t1 = Task.Run(() => createInstance("statGAU" + c, MODEL.STATISTICAL, 100.0, this._numFolds, 1, 0.1, 0.6, Kernel.GAUSSIAN, activeFeatures: this._activeFeatures));
                            Task t2 = Task.Run(() => createInstance("statLAP" + c, MODEL.STATISTICAL, 100.0, this._numFolds, 1, 0.1, 0.6, Kernel.LAPLACIAN, activeFeatures: this._activeFeatures));
                            Task t3 = Task.Run(() => createInstance("statTRI" + c, MODEL.STATISTICAL, 100.0, this._numFolds, 1, 0.1, 0.6, Kernel.SymmetricTriangular, optDouble1: 5.0, activeFeatures: this._activeFeatures));
                            Task t4 = Task.Run(() => createInstance("statCAU" + c, MODEL.STATISTICAL, 100.0, this._numFolds, 1, 0.1, 0.5, Kernel.Cauchy, optDouble1: 0.1, activeFeatures: this._activeFeatures));
                            tasks.Add(t1);
                            tasks.Add(t2);
                            tasks.Add(t3);
                            tasks.Add(t4);
                        }

                        Task.WaitAll(tasks.ToArray());
                        updateTasks(tasks);
                            

                        break;
                    }

                case MODE.CMB_A_DS_LAP:
                    {
                        List<string> featuresStatistical = new List<string>();
                        IModel modelStatistical = _analyzer.getModel(MODEL.STATISTICAL);
                        foreach (var feature in modelStatistical.getFeatureNames())
                        {
                            if (feature.Contains("_Max") || feature.Contains("_Min"))
                                featuresStatistical.Add(feature);
                        }
                        List<Task> tasks = new List<Task>();
                        for (int i = 0; i < _numRuns; i++)
                        {
                            int c = i;
                            // compute results for static model and standard gaussian ksvm
                            Task t1 = Task.Run(() => createInstance("ADS1" + c, MODEL.STATISTICAL, 100.0, this._numFolds, 1, 0.1, 0.3, Kernel.LAPLACIAN, activeFeatures: this._activeFeatures));
                            Task t2 = Task.Run(() => createInstance("ADS2" + c, MODEL.STATIC, 100.0, this._numFolds, 1, 0.1, 0.3, Kernel.LAPLACIAN));
                            //Task t3 = Task.Run(() => createInstance("statTRI" + c, MODEL.STATISTICAL, 100.0, this._numFolds, 1, 0.1, 0.6, Kernel.SymmetricTriangular, optDouble1: 5.0, activeFeatures: this._activeFeatures));
                            //Task t4 = Task.Run(() => createInstance("statCAU" + c, MODEL.STATISTICAL, 100.0, this._numFolds, 1, 0.1, 0.5, Kernel.Cauchy, optDouble1: 0.1, activeFeatures: this._activeFeatures));
                            tasks.Add(t1);
                            tasks.Add(t2);
                            //tasks.Add(t3);
                            //tasks.Add(t4);
                        }

                        Task.WaitAll(tasks.ToArray());
                        updateTasks(tasks);


                        break;
                    }

                /*
                case MODE.Static_Statistical_GLF:
                    {
                        List<string> featuresStatistical = new List<string>();
                        IModel modelStatistical = _analyzer.getModel(MODEL.STATISTICAL);
                        foreach (var feature in modelStatistical.getFeatureNames())
                        {
                            if (feature.Contains("_Max") || feature.Contains("_Min"))
                                featuresStatistical.Add(feature);
                        }

                        List<Task> tasks = new List<Task>();

                        // compute results for static model and standard gaussian ksvm
                        Task t1 = Task.Run(() => createInstance("static1", MODEL.STATIC, 100.0, 5, 1, 0.01, 0.01, Kernel.GAUSSIAN));
                        Task t2 = Task.Run(() => createInstance("statistical1", MODEL.STATISTICAL, 100.0, 5, 1, 0.01, 0.01, Kernel.GAUSSIAN, featuresStatistical));
                        Task t3 = Task.Run(() => createInstance("static2", MODEL.STATIC, 100.0, 5, 1, 0.1, 0.5, Kernel.LAPLACIAN));
                        Task t4 = Task.Run(() => createInstance("statistical2", MODEL.STATISTICAL, 100.0, 5, 1, 0.1, 0.5, Kernel.LAPLACIAN, featuresStatistical));
                        tasks.Add(t1);
                        tasks.Add(t2);
                        tasks.Add(t3);
                        tasks.Add(t4);
                        Task.WaitAll(tasks.ToArray());
                        updateTasks(tasks);
                            

                        break;
                    }

                case MODE.Static_Statistical_GLF2:
                    {
                        List<string> featuresStatistical1 = new List<string>();
                        List<string> featuresStatistical2 = new List<string>();
                        IModel modelStatistical = _analyzer.getModel(MODEL.STATISTICAL);
                        foreach (var feature in modelStatistical.getFeatureNames())
                        {
                            if (feature.Contains("_Max") || feature.Contains("_Min"))
                                featuresStatistical1.Add(feature);

                            if (feature.Contains("_Max") || feature.Contains("_LastValue") || feature.Contains("_Mean"))
                                featuresStatistical2.Add(feature);
                        }

                        List<Task> tasks = new List<Task>();

                        // compute results for static model and standard gaussian ksvm
                        Task t1 = Task.Run(() => createInstance("static1", MODEL.STATIC, 100.0, 5, 1, 0.01, 0.01, Kernel.GAUSSIAN));
                        Task t2 = Task.Run(() => createInstance("statistical1", MODEL.STATISTICAL, 100.0, 5, 1, 0.01, 0.01, Kernel.GAUSSIAN, featuresStatistical1));
                        Task t3 = Task.Run(() => createInstance("static2", MODEL.STATIC, 100.0, 5, 1, 0.1, 0.5, Kernel.LAPLACIAN));
                        Task t4 = Task.Run(() => createInstance("statistical2", MODEL.STATISTICAL, 100.0, 5, 1, 0.1, 0.5, Kernel.LAPLACIAN, featuresStatistical2));
                        tasks.Add(t1);
                        tasks.Add(t2);
                        tasks.Add(t3);
                        tasks.Add(t4);
                        Task.WaitAll(tasks.ToArray());
                        updateTasks(tasks);
                            

                        break;
                    }

                case MODE.Static_Statistical_GLF3:
                    {
                        List<string> featuresStatistical1 = new List<string>();
                        List<string> featuresStatistical2 = new List<string>();
                        IModel modelStatistical = _analyzer.getModel(MODEL.STATISTICAL);
                        foreach (var feature in modelStatistical.getFeatureNames())
                        {
                            if (feature.Contains("_Max") || feature.Contains("_Min"))
                                featuresStatistical1.Add(feature);

                            if (feature.Contains("_LastValue") || feature.Contains("_Mean"))
                                featuresStatistical2.Add(feature);
                        }

                        List<Task> tasks = new List<Task>();

                        // compute results for static model and standard gaussian ksvm
                        //Task t1 = Task.Run(() => createInstance("static1", MODEL.STATIC, 100.0, 5, 1, 0.01, 0.01, Kernel.GAUSSIAN));
                        Task t1 = Task.Run(() => createInstance("statistical1", MODEL.STATISTICAL, 100.0, 5, 1, 0.01, 0.01, Kernel.GAUSSIAN, featuresStatistical1));
                        Task t2 = Task.Run(() => createInstance("statistical1", MODEL.STATISTICAL, 100.0, 5, 1, 0.01, 0.01, Kernel.GAUSSIAN, featuresStatistical2));
                        Task t3 = Task.Run(() => createInstance("statistical2", MODEL.STATISTICAL, 100.0, 5, 1, 0.1, 0.5, Kernel.LAPLACIAN, featuresStatistical1));
                        Task t4 = Task.Run(() => createInstance("statistical2", MODEL.STATISTICAL, 100.0, 5, 1, 0.1, 0.5, Kernel.LAPLACIAN, featuresStatistical2));
                        tasks.Add(t1);
                        tasks.Add(t2);
                        tasks.Add(t3);
                        tasks.Add(t4);
                        Task.WaitAll(tasks.ToArray());
                        updateTasks(tasks);
                            

                        break;
                    }

                case MODE.Static_Statistical_GLF_EXP:
                    {
                        List<string> featuresStatistical1 = new List<string>();
                        List<string> featuresStatistical2 = new List<string>();
                        List<string> featuresStatistical3 = new List<string>();
                        IModel modelStatistical = _analyzer.getModel(MODEL.STATISTICAL);
                        foreach (var feature in modelStatistical.getFeatureNames())
                        {
                            if (feature.Contains("_Max") || feature.Contains("_Min"))
                                featuresStatistical1.Add(feature);

                            if (feature.Contains("_MedianIndex"))
                                featuresStatistical3.Add(feature);

                            if (feature.Contains("_LastValue"))
                                featuresStatistical2.Add(feature);
                        }

                        List<Task> tasks = new List<Task>();

                        // compute results for static model and standard gaussian ksvm
                        //Task t1 = Task.Run(() => createInstance("static1", MODEL.STATIC, 100.0, 5, 1, 0.01, 0.01, Kernel.GAUSSIAN));
                        //Task t1 = Task.Run(() => createInstance("statistical1", MODEL.STATISTICAL, 100.0, 5, 1, 0.01, 0.01, Kernel.GAUSSIAN, featuresStatistical1));
                        //Task t2 = Task.Run(() => createInstance("statistical1", MODEL.STATISTICAL, 100.0, 5, 1, 0.01, 0.01, Kernel.GAUSSIAN, featuresStatistical2));
                        Task t1 = Task.Run(() => createInstance("statistical2", MODEL.STATISTICAL, 100.0, 5, 1, 0.1, 0.55, Kernel.LAPLACIAN, featuresStatistical1));
                        Task t2 = Task.Run(() => createInstance("statistical2", MODEL.STATISTICAL, 100.0, 5, 1, 0.1, 0.55, Kernel.LAPLACIAN, featuresStatistical2));
                        Task t3 = Task.Run(() => createInstance("statistical2", MODEL.STATISTICAL, 100.0, 5, 1, 0.1, 0.55, Kernel.LAPLACIAN, featuresStatistical3));
                        tasks.Add(t1);
                        tasks.Add(t2);
                        tasks.Add(t3);
                        //tasks.Add(t4);
                        Task.WaitAll(tasks.ToArray());
                        updateTasks(tasks);
                            

                        break;
                    }

                case MODE.Static_Statistical_GLF4:
                    {
                        List<string> featuresStatistical1 = new List<string>();
                        List<string> featuresStatistical2 = new List<string>();
                        IModel modelStatistical = _analyzer.getModel(MODEL.STATISTICAL);
                        foreach (var feature in modelStatistical.getFeatureNames())
                        {
                            if (feature.Contains("_Max") || feature.Contains("_Min"))
                                featuresStatistical1.Add(feature);

                            if (feature.Contains("_LastValue") || feature.Contains("_Mean"))
                                featuresStatistical2.Add(feature);
                        }

                        List<Task> tasks = new List<Task>();

                        // compute results for static model and standard gaussian ksvm
                        //Task t1 = Task.Run(() => createInstance("static1", MODEL.STATIC, 100.0, 5, 1, 0.01, 0.01, Kernel.GAUSSIAN));
                        //Task t1 = Task.Run(() => createInstance("statistical1", MODEL.STATISTICAL, 100.0, 5, 1, 0.01, 0.01, Kernel.GAUSSIAN, featuresStatistical1));
                        //Task t2 = Task.Run(() => createInstance("statistical1", MODEL.STATISTICAL, 100.0, 5, 1, 0.01, 0.01, Kernel.GAUSSIAN, featuresStatistical2));
                        Task t3 = Task.Run(() => createInstance("statistical2", MODEL.STATISTICAL, 100.0, 5, 1, 0.1, 0.5, Kernel.LAPLACIAN, featuresStatistical1));
                        Task t4 = Task.Run(() => createInstance("statistical2", MODEL.STATISTICAL, 100.0, 5, 1, 0.1, 0.5, Kernel.LAPLACIAN, featuresStatistical2));
                        tasks.Add(t3);
                        tasks.Add(t4);
                        //tasks.Add(t3);
                        //tasks.Add(t4);
                        Task.WaitAll(tasks.ToArray());
                        updateTasks(tasks);
                            

                        break;
                    }

                case MODE.Static_Statistical_Gaussian_Laplacian:
                    {
                        List<Task> tasks = new List<Task>();

                        // compute results for static model and standard gaussian ksvm
                        Task t1 = Task.Run(() => createInstance("static1", MODEL.STATIC, 100.0, 5, 1, 0.01, 0.01, Kernel.GAUSSIAN));
                        Task t2 = Task.Run(() => createInstance("statistical1", MODEL.STATISTICAL, 100.0, 5, 1, 0.01, 0.01, Kernel.GAUSSIAN));
                        Task t3 = Task.Run(() => createInstance("static2", MODEL.STATIC, 100.0, 5, 1, 0.1, 0.5, Kernel.LAPLACIAN));
                        Task t4 = Task.Run(() => createInstance("statistical2", MODEL.STATISTICAL, 100.0, 5, 1, 0.1, 0.5, Kernel.LAPLACIAN));
                        tasks.Add(t1);
                        tasks.Add(t2);
                        tasks.Add(t3);
                        tasks.Add(t4);
                        Task.WaitAll(tasks.ToArray());
                        updateTasks(tasks);
                            

                        break;
                    }

                case MODE.Static_Statistical_Gaussian_Laplacian_Features:
                    {
                        List<string> featuresStatistical = new List<string>();
                        IModel modelStatistical = _analyzer.getModel(MODEL.STATISTICAL);
                        foreach (var feature in modelStatistical.getFeatureNames())
                        {
                            if (feature.Contains("_Max") || feature.Contains("_Min"))
                                featuresStatistical.Add(feature);
                        }

                        Console.WriteLine("Features should be: ");
                        Utility.writeToConsole<string>(featuresStatistical.ToArray());

                        List<Task> tasks = new List<Task>();

                        // compute results for static model and standard gaussian ksvm
                        Task t1 = Task.Run(() => createInstance("static1", MODEL.STATIC, 100.0, 5, 1, 0.1, 0.5, Kernel.GAUSSIAN));
                        Task t2 = Task.Run(() => createInstance("statistical1", MODEL.STATISTICAL, 100.0, 5, 1, 0.1, 0.5, Kernel.GAUSSIAN, featuresStatistical));
                        Task t3 = Task.Run(() => createInstance("static2", MODEL.STATIC, 100.0, 5, 1, 0.1, 0.5, Kernel.LAPLACIAN));
                        Task t4 = Task.Run(() => createInstance("statistical2", MODEL.STATISTICAL, 100.0, 5, 1, 0.1, 0.5, Kernel.LAPLACIAN, featuresStatistical));
                        tasks.Add(t1);
                        tasks.Add(t2);
                        tasks.Add(t3);
                        tasks.Add(t4);
                        Task.WaitAll(tasks.ToArray());
                        updateTasks(tasks);
                        {
                            
                            updatePerformanceStats();
                        }

                        break;
                    }*/
            }

            
        }

        internal ConcurrentDictionary<string, List<CrossValidationResult>> Results
        {
            get { return this._results; }
        }

        internal void showResults()
        {
            List<CrossValidationResult> mergedResults = new List<CrossValidationResult>();
            foreach (var kvp in _results)
            {
                foreach (var result in kvp.Value)
                    mergedResults.Add(result);
            }

            long memoryUsedBytes = this._memoryMax - this._memoryStart;
            MachineLearning.CVResultDialog resultDialog = new MachineLearning.CVResultDialog(mergedResults, null, cMode: this._mode, memoryUsedBytes: memoryUsedBytes, timeElapsedMS: this._timeElapsedMS, numRuns: this._numRuns, numFolds: this._numFolds);
            resultDialog.StartPosition = FormStartPosition.CenterParent;
            resultDialog.ShowDialog();
        }


    }
}

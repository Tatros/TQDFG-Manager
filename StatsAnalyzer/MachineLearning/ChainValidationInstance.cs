using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StatsAnalyzer.Model;

namespace StatsAnalyzer.MachineLearning
{
    internal class ChainValidationInstance
    {
        IModel _model;
        MachineLearning.KernelSVM _kernelSVM = null;
        MachineLearning.KernelSVMConfiguration _kernelSVMConfiguration;
        List<MachineLearning.CrossValidationResult> _cvResults = null;

        internal ChainValidationInstance(IModel model, KernelSVMConfiguration kernelSVMConfiguration)
        {
            this._model = model;
            this._kernelSVMConfiguration = kernelSVMConfiguration;
        }

        internal void run()
        {
            _kernelSVM = new StatsAnalyzer.MachineLearning.KernelSVM(_model, _kernelSVMConfiguration);
            _cvResults = _kernelSVM.performCrossValidation();
        }

        internal List<CrossValidationResult> CVResults
        {
            get { return this._cvResults; }
        }
    }
}

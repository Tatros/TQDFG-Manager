using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StatsAnalyzer.MachineLearning
{
    internal class ChainValidationConfiguration
    {
        MODE _chainValidationMode;

        internal ChainValidationConfiguration()
        {
            _chainValidationMode = MODE.STATIC_LINEAR;
        }

        internal MODE ChainValidationMode
        {
            get { return this._chainValidationMode; }
            set { this._chainValidationMode = value; }
        }
    }
}

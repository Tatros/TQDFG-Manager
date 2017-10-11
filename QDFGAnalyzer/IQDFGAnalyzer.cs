using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QDFGAnalyzer
{
    interface ILogAnalyzer
    {
        void analyzeLog(String logPath, String outputDirectory);
    }
}

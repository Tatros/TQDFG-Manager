using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StatsAnalyzer.ExecutionEngine
{
    public abstract class ApplicationProcess
    {
        public abstract void StartProcess(string path);
        public abstract void EndProcess();
        public abstract void Kill();
    }
}

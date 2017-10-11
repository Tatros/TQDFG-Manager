using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using NLog;

using QDFGGraphManager.QDFGProcessor;

namespace QDFGGraphManager
{
    public class QDFGManager
    {
        public enum Mode { STATIC, DYNAMIC };
        private static Logger logger = LogManager.GetCurrentClassLogger();
        public QDFGManager (bool computeStats, Mode mode)
        {
            init();

            logger.Debug("Event Logs present:\n");

            List<String> files = Utility.IO.getActiveEventLogFilePaths();
            if (files != null)
            {
                foreach (String s in files)
                {
                    logger.Debug("    " + s);
                }

                foreach (String s in files)
                {
                    if (mode == Mode.DYNAMIC)
                    {
                        DynamicQDFGProcessor p = new DynamicQDFGProcessor();
                        p.generateQDFG(s, computeStats);
                    }

                    else if (mode == Mode.STATIC)
                    {
                        StaticQDFGProcessor p = new StaticQDFGProcessor();
                        p.generateQDFG(s, computeStats);
                    }
                }
            }

            else
            {
                logger.Error("Event Log Directory \"" + Settings.activeEventLogDirectory + "\" not found.");
            }
        }

        public QDFGManager(String logPath, bool computeStats, Mode mode)
        {
            init();
            logger.Warn("QDFG Manager - Single Log Mode");

            if (File.Exists(logPath))
            {
                if (mode == Mode.DYNAMIC)
                {
                    DynamicQDFGProcessor p = new DynamicQDFGProcessor();
                    p.generateQDFG(logPath, computeStats);
                }

                else if (mode == Mode.STATIC)
                {
                    StaticQDFGProcessor p = new StaticQDFGProcessor();
                    p.generateQDFG(logPath, computeStats);
                }
            }
            else
            {
                logger.Fatal("Could not find file: " + logPath);
            }
        }

        public void init()
        {
            logger.Info("QDFG Graph Manager started.");
            logger.Info("active Event Log directory: " + Settings.activeEventLogDirectory);
            logger.Info("output directory: " + Settings.outputDirectory);
        }
    }
}

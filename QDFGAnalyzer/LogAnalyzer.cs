using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QDFGGraphManager;
using QDFGGraphManager.QDFGProcessor;

namespace QDFGAnalyzer
{
    public class LogAnalyzer : ILogAnalyzer
    {
        public void analyzeLog(String logPath, String outputDirectory)
        {
            Settings.outputDirectory = outputDirectory;
            DynamicQDFGProcessor p = new DynamicQDFGProcessor();
            p.generateQDFG(logPath, true);
        }


        public String TimeDataFile
        {
            get
            {
                return Settings.TimeDataFile;
            }
        }

        public String ModelDataFile
        {
            get
            {
                return Settings.ModelDataFile;
            }
        }

        public String OutputDirectory
        {
            get 
            {
                return Settings.outputDirectory;
            }
            set 
            {
                Settings.outputDirectory = value;
            }
        }

        public String NumberFormat
        {
            get 
            {
                return Settings.NUMBER_FORMAT;
            }
            set
            {
                Settings.NUMBER_FORMAT = value;
            }
        }

        public long MemoryMaxBytesAllowed
        {
            get
            {
                return Settings.MEM_MAX_BYTES_ALLOWED;
            }
            set
            {
                Settings.MEM_MAX_BYTES_ALLOWED = value;
            }
        }

        public int TimeStepMS
        {
            get
            {
                return Settings.timeStepMS;
            }
            set
            {
                Settings.timeStepMS = value;
            }
        }

        public int MinNumSamples
        {
            get
            {
                return Settings.MIN_SAMPLES;
            }
            set
            {
                Settings.MIN_SAMPLES = value;
            }
        }

        public int NumSamples
        {
            get
            {
                return Settings.NUM_SAMPLES;
            }
            set
            {
                Settings.NUM_SAMPLES = value;
            }
        }

        public Settings.ModelDataType ModelDataType
        {
            get
            {
                return Settings.MODEL_DATA_TYPE;
            }
            set
            {
                Settings.MODEL_DATA_TYPE = value;
            }
        }

        public Settings.TimeDataType TimeDataType
        {
            get
            {
                return Settings.TIME_DATA_TYPE;
            }
            set
            {
                Settings.TIME_DATA_TYPE = value;
            }
        }

        public TimeSpan TimeSyncDetectMinutes
        {
            get
            {
                return Settings.timeSyncDetect;
            }
            set
            {
                Settings.timeSyncDetect = value;
            }
        }

        public String MissingValueIdentifier
        {
            get
            {
                return Settings.MISSING_VALUE_IDENTIFIER;
            }
            set
            {
                Settings.MISSING_VALUE_IDENTIFIER = value;
            }
        }


    }
}

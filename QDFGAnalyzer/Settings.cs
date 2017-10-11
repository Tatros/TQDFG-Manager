using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QDFGGraphManager
{
    public sealed class Settings
    {
        public enum ModelDataType { 
            SAMPLE_BASED,
            AGGREGATED_STATISTICS 
        }

        public enum TimeDataType
        {
            TIME_STEPS
        }

        public static String activeEventLogDirectory = @"active";
        public static String outputDirectory = "";
        public static String ModelDataFile = "ModelData.csv";
        public static String TimeDataFile = "TimeData.csv";
        public static String NUMBER_FORMAT = "F3";
        public static String MISSING_VALUE_IDENTIFIER = "?";
        //public static String DOUBLE_FORMAT = "0.00000";
        public static long MEM_MAX_BYTES_ALLOWED = 3500000000; // 2 GB = 2*10^9 bytes
        public static int timeStepMS = 1000; // 500
        public static int MIN_SAMPLES = 1; // 40 // 5
        public static int NUM_SAMPLES = 300;
        public static bool USE_OWN_STATS = false;
        public static ModelDataType MODEL_DATA_TYPE = ModelDataType.SAMPLE_BASED;
        public static TimeDataType TIME_DATA_TYPE = TimeDataType.TIME_STEPS;
        public static readonly DateTime NullTime = DateTime.MinValue;
        public static TimeSpan timeSyncDetect = TimeSpan.FromMinutes(3);
        public static bool generateModel = true;
    }
}

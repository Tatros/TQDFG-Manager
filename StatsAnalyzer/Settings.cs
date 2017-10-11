using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StatsAnalyzer
{
    class Settings
    {
        public enum TIME_MODEL { CONTINUOUS, LAST_VALUE };
        public static String matlabExecutablePath = @"C:\Program Files\MATLAB\R2015b\bin\matlab.exe";
        public static String matlabExecutableFolder = @"C:\Program Files\MATLAB\R2015b\bin\";
        public static String outputDirectory = "out";
        public static String coefficientFile = "coeffs.txt";
        public static String MISSING_VALUE_IDENTIFIER = "-1,0";
        public static String DOUBLE_FORMAT = "0.0";
        public static int outputInterval = 10000;
        public static bool closeMatlabWindow = false;
        public static bool saveFigures = false;
        public static bool plotPointsOnly = false;
        public static bool useFitting = false;
        public static String defaultStatusMessage = "Ready.";
        public static int StatusDisplayTimeMS = 5000;
    }
}

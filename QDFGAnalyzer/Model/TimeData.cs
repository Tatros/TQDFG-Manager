using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NLog;

namespace QDFGGraphManager.Model
{
    internal static class TimeData
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        public static String getStringTimeData(Settings.TimeDataType timeDataType, ModelData m)
        {
            if (timeDataType == Settings.TimeDataType.TIME_STEPS)
            {
                return getStringTimeDataTimeSteps(m);
            }

            throw new ArgumentException("Unable to produce Time Data String for " + m.Name + ". Invalid Time Data Type: " + timeDataType.ToString());
        }

        private static String getStringTimeDataTimeSteps(ModelData m)
        {
            StringBuilder s = new StringBuilder();

            foreach (String featureName in m.TimedFeatureCollection.Keys)
            {        
                s.Append(m.Name + ";");
                s.Append(featureName + ";");
                s.Append(Settings.timeStepMS + ";");

                foreach (Double featureValue in m.TimedFeatureCollection[featureName])
                {
                    s.Append(featureValue.ToString(Settings.NUMBER_FORMAT, new System.Globalization.CultureInfo("de-DE")) + ";");
                    //s.Append(featureValue.ToString("0.0000", new System.Globalization.CultureInfo("en-US")) + ";");
                }
                s.Length--;
                s.Append("\n");
            }
            return s.ToString();
        }
    }
}

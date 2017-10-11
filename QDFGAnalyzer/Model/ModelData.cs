using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;

using MathNet.Numerics.Statistics;
using NLog;

namespace QDFGGraphManager.Model
{
    internal sealed class ModelData
    {
        private static Logger logger = LogManager.GetLogger("ModelData");

        private String name;
        
        public String Name
        {
            get { return name; }
            set { name = value; }
        }

        Dictionary<String, List<Double>> timedFeatureList;


        public ModelData (String name, Dictionary<String, List<Double>> timedFeatureList)
        {
            this.name = name;

            this.timedFeatureList = timedFeatureList;
        }

        public Dictionary<String, List<Double>> TimedFeatureCollection
        {
            get { return timedFeatureList; }
        }

        public int ColumnCount
        {
            get { return (TimedFeatureCollection.Keys.Count * Settings.NUM_SAMPLES); }
        }


        public String getHeaderString(Settings.ModelDataType modelDataType)
        {
            if (modelDataType == Settings.ModelDataType.AGGREGATED_STATISTICS)
            {
                return this.getStatisticsHeaderStr();
            }

            else if (modelDataType == Settings.ModelDataType.SAMPLE_BASED)
            {
                return this.getSampleBasedHeaderString();
            }

            throw new ArgumentException("Unable to produce Model Data String for " + this.Name + ". Invalid Model Data Type: " + modelDataType.ToString());
        }

        public String getModelDataString(Settings.ModelDataType modelDataType, String classifier)
        {
            if (modelDataType == Settings.ModelDataType.AGGREGATED_STATISTICS)
            {
                return this.getStatisticsCompleteModelString(classifier);
            }

            else if (modelDataType == Settings.ModelDataType.SAMPLE_BASED)
            {
                return this.getSampleBasedModelString(classifier);
            }

            throw new ArgumentException("Unable to produce Model Data String for " + this.Name + ". Invalid Model Data Type: " + modelDataType.ToString());
        }

        public String getSampleBasedHeaderString()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("Name;");
            foreach (String featureName in TimedFeatureCollection.Keys)
            {
                for (int i = 0; i < Settings.NUM_SAMPLES; i++)
                {
                    sb.Append(featureName + "_" + i + ";");
                }
                // DEBUG sb.Append("\r\n");
            }
            sb.Append("Class\r\n");

            return sb.ToString();
        }

        public String getSampleBasedModelString(String classifier)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append(this.Name + "; ");
            foreach (String featureName in timedFeatureList.Keys)
            {
                // DEBUG sb.AppendLine(getSampleBasedFeatureString(featureName));
                // DEBUG sb.Append(featureName + ", ");
                sb.Append(getSampleBasedFeatureString(featureName));
                
            }
            sb.Append(classifier);

            return sb.ToString();
        }

        private String getSampleBasedFeatureString(String featureName)
        {
            List<Double> featureSamples = TimedFeatureCollection[featureName];

            if (featureSamples.Count < Settings.MIN_SAMPLES)
            {
                logger.Fatal("Unable to produce sample based string for feature <" + featureName + ">, due to insufficient number of samples. Samples: " + featureSamples.Count + ", Minimum: " + Settings.MIN_SAMPLES);
                throw new ArgumentException(featureName + " has insufficient number of samples in " + this.Name);
            }

            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < Settings.NUM_SAMPLES; i++)
            {
                if (i < featureSamples.Count)
                {
                    if (featureSamples[i] >= 0) // filter negative values
                    {
                        // DEBUG sb.Append(featureSamples[i] + "(" + featureName + ", " + i + "), ");
                        // DEBUG sb.Append(featureSamples[i].ToString("0.0000", CultureInfo.InvariantCulture) + "(" + featureName + ", " + i + "), ");
                        // DEBUG sb.Append(featureSamples[i].ToString(Settings.NUMBER_FORMAT, CultureInfo.InvariantCulture) + "(" + featureName + " " + i + ")" + "; ");
                        sb.Append(featureSamples[i].ToString(Settings.NUMBER_FORMAT, CultureInfo.InvariantCulture) + ";");
                    }
                    else // value of sample i < 0
                    {
                        sb.Append(Settings.MISSING_VALUE_IDENTIFIER + ";");
                    }
                }
                else // no value for sample i
                {
                    // DEBUG sb.Append("?" + "(" + featureName + ", " + i + "), ");
                    sb.Append(Settings.MISSING_VALUE_IDENTIFIER + ";");
                }
            }

            return sb.ToString();
        }

        public String getStatisticsCompleteModelString(String classifier)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append(this.Name + ";");
            foreach (String featureName in timedFeatureList.Keys)
            {
                sb.Append(getStatisticsString(getStatisticsForFeature(featureName)) + ";");
            }
            sb.Append(classifier);

            return sb.ToString();
        }

        private DescriptiveStatistics getStatisticsForFeature(String featureName)
        {
            if (timedFeatureList.ContainsKey(featureName))
            {
                int sampleCount = timedFeatureList[featureName].Count;

                List<Double> filteredList = new List<Double>(timedFeatureList[featureName]);
                int numRemoved = filteredList.RemoveAll(x => x < 0); // remove all negative values

                logger.Fatal("orig list: " + sampleCount + ", filtered List: " + filteredList.Count + ", removed values: " + numRemoved);

                if (sampleCount - numRemoved < Settings.MIN_SAMPLES)
                {
                    return null;
                }
                return new DescriptiveStatistics(filteredList);
            }

            throw new ArgumentException("No Feature with Name <" + featureName + "> exists in timed feature collection for model data <" + this.name + ">.");
        }

        private String getStatisticsString(DescriptiveStatistics ds)
        {
            if (ds != null)
            {
                return (
                ds.Count.ToString(Settings.NUMBER_FORMAT, CultureInfo.InvariantCulture) + ";" +
                ds.Maximum.ToString(Settings.NUMBER_FORMAT, CultureInfo.InvariantCulture) + ";" +
                ds.Minimum.ToString(Settings.NUMBER_FORMAT, CultureInfo.InvariantCulture) + ";" +
                ds.Mean.ToString(Settings.NUMBER_FORMAT, CultureInfo.InvariantCulture) + ";" +
                ds.StandardDeviation.ToString(Settings.NUMBER_FORMAT, CultureInfo.InvariantCulture) + ";" +
                ds.Variance.ToString(Settings.NUMBER_FORMAT, CultureInfo.InvariantCulture));
            }
            else
            {
                return (Settings.MISSING_VALUE_IDENTIFIER + ";"
                    + Settings.MISSING_VALUE_IDENTIFIER + ";"
                    + Settings.MISSING_VALUE_IDENTIFIER + ";"
                    + Settings.MISSING_VALUE_IDENTIFIER + ";"
                    + Settings.MISSING_VALUE_IDENTIFIER + ";"
                    + Settings.MISSING_VALUE_IDENTIFIER + ";");
                //return ("\"?\";\"?\";\"?\";\"?\";\"?\";\"?\"");
            }
            
        }

        public String getStatisticsReadableString()
        {
            return "----- readable string -------";
        }

        public String getStatisticsHeaderStr()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("Name;");
            foreach (String featureName in timedFeatureList.Keys)
            {
                sb.Append(
                      featureName + "Count;"
                    + featureName + "Max;"
                    + featureName + "Min;"
                    + featureName + "Mean;"
                    + featureName + "StdDev;"
                    + featureName + "Variance;");
            }
            sb.Append("Class\r\n");

            return sb.ToString();
        }


        /* public String getReadableString()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("   MODEL DATA FOR NODE: " + Name);
            sb.AppendLine("====================================");

            sb.AppendLine("  Property: " + "COUNT" + ", " + "MAX" + ", " + "MIN" + ", " + "MEAN" + ", " + "STD-DEV" + ", " + "VARIANCE");
            sb.AppendLine("  centrality: " + CentralityModelStr);
            sb.AppendLine("  centralityRel: " + CentralityRelativeModelStr);
            sb.AppendLine("  InDeg: " + InDegModelStr);
            sb.AppendLine("  OutDeg: " + OutDegModelStr);
            sb.AppendLine("  InData: " + InDataModelStr);
            sb.AppendLine("  OutData: " + OutDataModelStr);
            sb.AppendLine("  BetweennessSizeCentrality: " + BetweennessSizeCentralityModelStr);
            sb.AppendLine("  BoundBetweennessSizeCentrality: " + BoundBetweennessSizeCentralityModelStr);
            sb.AppendLine("  ClosenessSizeCentralityNormalized: " + ClosenessSizeCentralityNormalizedModelStr);
            sb.AppendLine("  BoundClosenessSizeCentralityNormalized: " + BoundClosenessSizeCentralityNormalizedModelStr);
            sb.AppendLine("  PageRankNormalized: " + PageRankNormalizedModelStr);
            sb.AppendLine("  BoundedPageRankNormalized: " + BoundedPageRankNormalizedModelStr);
            sb.AppendLine("  InEdgeCompression: " + InEdgeCompressionModelStr);
            sb.AppendLine("  OutEdgeCompression: " + OutEdgeCompressionModelStr);
            sb.AppendLine("  SizeEnt: " + SizeEntModelStr);
            sb.AppendLine("  SizeStdDev: " + SizeStdDevModelStr);
            sb.AppendLine("  InSizeRatioFileNode: " + InSizeRatioFileNodeModelStr);
            sb.AppendLine("  InSizeRatioProcessNode: " + InSizeRatioProcessNodeModelStr);
            sb.AppendLine("  InSizeRatioSocketNode: " + InSizeRatioSocketNodeModelStr);
            sb.AppendLine("  OutSizeRatioFileNode: " + OutSizeRatioFileNodeModelStr);
            sb.AppendLine("  OutSizeRatioProcessNode: " + OutSizeRatioProcessNodeModelStr);
            sb.AppendLine("  OutSizeRatioSocketNode: " + OutSizeRatioSocketNodeModelStr);

            return sb.ToString();
        }*/
    }
}

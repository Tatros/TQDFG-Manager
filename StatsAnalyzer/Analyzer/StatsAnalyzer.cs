using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using MathNet.Numerics.Statistics;
using StatsAnalyzer.Model;

namespace StatsAnalyzer
{
    class Analyzer
    {
        // Dictionary<String, Dictionary<int,NodeStats>> nodeStats;
        public enum NodeType { MALWARE, GOODWARE, UNKNOWN };
        public enum DataStat { TOTAL, MISSING, ZERO, NEGATIVE, POSITIVE, MEAN, MAX, MIN, VARIANCE, STDDEV, KURTOSIS }
        List<String> saveModels = new List<String>() { "poly1", "poly2", "poly3", "poly4", "poly5", "poly6", "poly7", "poly8" };
        Dictionary<String, Dictionary<String, List<Double>>> nodeTimeData;
        int timeStep = 500;
        String sourcePath = "";
        Dictionary<String, int> fitModels = new Dictionary<string,int>
        {
            { "poly1", 2 },
            { "poly2", 3 },
            { "poly3", 4 },
            { "poly4", 5 },
            { "poly5", 6 },
            { "poly6", 7 },
            { "poly7", 8 },
            { "poly8", 9 },
        };

        public Analyzer(String pathToStatsFile)
        {
            nodeTimeData = new Dictionary<string, Dictionary<string, List<double>>>();

            this.sourcePath = pathToStatsFile;
            openStatsFile(pathToStatsFile);
            
            // Console.WriteLine(this.getStringRepresentation());
        }

        public int TimeStep
        {
            get { return this.timeStep; }
        }

        public String SourcePath
        {
            get { return this.sourcePath; }
        }

        public List<String> getNodeNames()
        {
            return this.nodeTimeData.Keys.ToList();
        }

        public Dictionary<String, List<Double>> getAllStatsForNode(String nodeName)
        {
            if (nodeTimeData.ContainsKey(nodeName))
            {
                return nodeTimeData[nodeName];
            }

            throw new KeyNotFoundException("No Stats found for Node " + nodeName);
        }

        public List<Double> getStatsForNode(String nodeName, String statName)
        {
            if (nodeTimeData.ContainsKey(nodeName))
            {
                if (nodeTimeData[nodeName].ContainsKey(statName))
                {
                    return nodeTimeData[nodeName][statName];
                }

                throw new KeyNotFoundException("Stats found for Node " + nodeName + ", but no entry for Stat: " + statName);
            }

            throw new KeyNotFoundException("No Stats found for Node " + nodeName);
        }

        private void openStatsFile(String pathToStatsFile)
        {
            try
            {
                string[] statsLines = File.ReadAllLines(pathToStatsFile);

                foreach (String line in statsLines)
                {
                    readStats(line);
                }
            }

            catch (FileNotFoundException)
            {
                Console.WriteLine("Stats File was not found: " + pathToStatsFile);
            }
        }

        private void readStats(String line)
        {
            string[] parts = line.Split(';');
            List<Double> featureValues = new List<Double>();

            try
            {
                if (parts.Length > 3)
                {
                    String nodeName = parts[0];
                    String statName = parts[1];
                    int _timeStep;
                    if (!Int32.TryParse(parts[2], out _timeStep))
                    {
                        Console.WriteLine(nodeName + ": Failed to parse TimeStep.");
                        return;
                    }

                    timeStep = _timeStep;

                    for (int i = 3; i < parts.Length; i++)
                    {
                        if (parts[i] != Settings.MISSING_VALUE_IDENTIFIER)
                        {
                            Double featureValue;
                            if (Double.TryParse(parts[i], out featureValue))
                            {
                                featureValues.Add(featureValue);
                            }

                            else
                            {
                                Console.WriteLine(nodeName + ": Failed to Parse Feature Value: " + parts[i]);
                            }
                        }

                        else
                        {
                            featureValues.Add(-1.0);
                        }
                    }

                    if (!nodeTimeData.ContainsKey(nodeName))
                    {
                        nodeTimeData.Add(nodeName, new Dictionary<String, List<Double>>() { { statName, featureValues } });
                    }
                    else
                    {
                        if (!nodeTimeData[nodeName].ContainsKey(statName))
                            nodeTimeData[nodeName].Add(statName, featureValues);
                        else
                            Console.WriteLine(nodeName + ": Stat [" + statName + "] already exists.");
                    }
                }
            }

            catch (FormatException e)
            {
                Console.WriteLine("Error parsing: " + e.Message);
                return;
            }



           /*
                if (nodeStats.ContainsKey(nodeName))
                {
                    nodeStats[nodeName].Add(time, new NodeStats(nodeID, nodeName, inDegree, outDegree, inData, outData, centralityCount, centralityRelative, totalNodes, time));
                }

                else
                {
                    nodeStats.Add(nodeName, new Dictionary<int, NodeStats>());
                    nodeStats[nodeName].Add(time, new NodeStats(nodeID, nodeName, inDegree, outDegree, inData, outData, centralityCount, centralityRelative, totalNodes, time));
                }
            }*/
        }

        public int getMaxItemCount(String nodeName)
        {
            int max = 0;
            if (nodeTimeData.ContainsKey(nodeName))
            {
                foreach (String featureName in nodeTimeData[nodeName].Keys)
                {
                    int time = nodeTimeData[nodeName][featureName].Count;
                    if (time > max)
                        max = time;
                }
            }
            return max;
        }

        public int getMaxOverallSamples()
        {
            int max = 0;
            int c = 0;

            foreach (String nodeName in nodeTimeData.Keys)
            {
                c = getMaxItemCount(nodeName);
                if (c > max)
                    max = c;
            }

            return max;
        }

        public int getMaxTime(String nodeName, int timeStep)
        {
            return getMaxItemCount(nodeName) * timeStep;
        }

        public void addStatForNode(String nodeName, String statName, List<Double> values)
        {
            if (nodeTimeData.Keys.Contains(nodeName))
            {
                if (!nodeTimeData[nodeName].ContainsKey(statName))
                {
                    nodeTimeData[nodeName].Add(statName, values);
                }

                else
                {
                    //nodeTimeData[nodeName][statName].Clear();
                    nodeTimeData[nodeName][statName] = values;
                }
            }
        }

        public String getStringRepresentationNodeTimeData()
        {
            StringBuilder sb = new StringBuilder();

            foreach (String nodeName in nodeTimeData.Keys)
            {
                sb.AppendLine("Node: " + nodeName);

                foreach (KeyValuePair<String, List<Double>> kp in nodeTimeData[nodeName])
                {
                    sb.AppendLine("   Stat: " + kp.Key);
                    sb.Append("      Values: ");
                    foreach (Double d in kp.Value)
                    {
                        sb.Append(d.ToString("0.00", new System.Globalization.CultureInfo("de-DE")) + ", ");
                    }
                    sb.Length--;
                    sb.AppendLine();
                }
            }
            return sb.ToString();
        }

        public String getStringRepresentationNodeTimeData(String nodeName)
        {
            StringBuilder sb = new StringBuilder();

            if (nodeTimeData.ContainsKey(nodeName))
            {
                foreach (KeyValuePair<String, List<Double>> kp in nodeTimeData[nodeName])
                {
                    if (!kp.Key.Contains("fitModel"))
                    {
                        sb.AppendLine("  Stat: " + kp.Key);
                        foreach (Double d in kp.Value)
                        {
                            sb.Append(d.ToString("0.00", new System.Globalization.CultureInfo("en-US")) + ", ");
                        }
                        sb.Length--;
                        sb.AppendLine();
                        sb.AppendLine();
                    }
                }
            }
            return sb.ToString();
        }

        public String getStringRepresentationCurveFit(String nodeName)
        {
            StringBuilder sb = new StringBuilder();

            if (nodeTimeData.ContainsKey(nodeName))
            {
                foreach (KeyValuePair<String, List<Double>> kp in nodeTimeData[nodeName])
                {
                    if (kp.Key.Contains("fitModel") && kp.Key.Contains("poly"))
                    {
                        sb.AppendLine("Model: " + kp.Key + "\r\n");
                        for (int i = 0; i < kp.Value.Count; i++)
                        {
                            sb.AppendLine(String.Format("  c{0}: {1}", i, kp.Value[i]));
                        }
                        sb.AppendLine("\r\n\r\n");
                    }
                }
            }
            return sb.ToString();
        }

        public NodeType getNodeType(String nodeName)
        {
            if (nodeTimeData.ContainsKey(nodeName))
            {
                if (nodeName.ToLower().Contains("malware"))
                    return NodeType.MALWARE;

                else if (nodeName.ToLower().Contains("goodware"))
                    return NodeType.GOODWARE;
            }

            return NodeType.UNKNOWN;
        }

        public List<String> getFitModels()
        {
            return this.fitModels.Keys.ToList();
        }

        public List<String> getSaveModels()
        {
            return this.saveModels;
        }

        public Dictionary<string, double> getNodeDataStats(String nodeName, DataStat type)
        {
            Dictionary<String, double> result = new Dictionary<string, double>();
            if (!nodeTimeData.ContainsKey(nodeName))
            {
                throw new ArgumentException("The node <" + nodeName + "> is not present in the Data Set.");
            }

            switch (type)
            {
                case DataStat.MISSING:
                {
                    int count = 0;
                    foreach (String statName in nodeTimeData[nodeName].Keys)
                    {
                        count = 0;
                        List<Double> values = nodeTimeData[nodeName][statName];

                        foreach (Double value in values)
                        {
                            if (value < 0 || Double.IsNaN(value) || Double.IsInfinity(value))
                            {
                                count++;
                            }
                        }

                        if (!result.ContainsKey(statName))
                        {
                            result.Add(statName, count);
                        }
                    }
                    return result;
                }
                case DataStat.NEGATIVE:
                {
                    int count = 0;
                    foreach (String statName in nodeTimeData[nodeName].Keys)
                    {
                        count = 0;
                        List<Double> values = nodeTimeData[nodeName][statName];

                        foreach (Double value in values)
                        {
                            if (value < 0)
                            {
                                count++;
                            }
                        }

                        if (!result.ContainsKey(statName))
                        {
                            result.Add(statName, count);
                        }
                    }
                    return result;
                }
                case DataStat.TOTAL:
                {
                    foreach (String statName in nodeTimeData[nodeName].Keys)
                    {
                        List<Double> values = nodeTimeData[nodeName][statName];

                        if (!result.ContainsKey(statName))
                        {
                            result.Add(statName, values.Count);
                        }
                    }
                    return result;
                }
                case DataStat.VARIANCE:
                {
                    foreach (String statName in nodeTimeData[nodeName].Keys)
                    {
                        List<Double> values = nodeTimeData[nodeName][statName];
                        DescriptiveStatistics s = new DescriptiveStatistics(values);

                        if (!result.ContainsKey(statName))
                        {
                            if (!Double.IsNaN(s.Variance))
                                result.Add(statName, s.Variance);
                        }
                    }
                    return result;
                }
                case DataStat.STDDEV:
                {
                    foreach (String statName in nodeTimeData[nodeName].Keys)
                    {
                        List<Double> values = nodeTimeData[nodeName][statName];
                        DescriptiveStatistics s = new DescriptiveStatistics(values);

                        if (!result.ContainsKey(statName))
                        {
                            if (!Double.IsNaN(s.StandardDeviation))
                                result.Add(statName, s.StandardDeviation);
                        }
                    }
                    return result;
                }
                case DataStat.MIN:
                {
                    foreach (String statName in nodeTimeData[nodeName].Keys)
                    {
                        List<Double> values = nodeTimeData[nodeName][statName];
                        DescriptiveStatistics s = new DescriptiveStatistics(values);

                        if (!result.ContainsKey(statName))
                        {
                            result.Add(statName, s.Minimum);
                        }
                    }
                    return result;
                }
                case DataStat.MAX:
                {
                    foreach (String statName in nodeTimeData[nodeName].Keys)
                    {
                        List<Double> values = nodeTimeData[nodeName][statName];
                        DescriptiveStatistics s = new DescriptiveStatistics(values);

                        if (!result.ContainsKey(statName))
                        {
                            result.Add(statName, s.Maximum);
                        }
                    }
                    return result;
                }
                case DataStat.MEAN:
                {
                    foreach (String statName in nodeTimeData[nodeName].Keys)
                    {
                        List<Double> values = nodeTimeData[nodeName][statName];
                        DescriptiveStatistics s = new DescriptiveStatistics(values);

                        if (!result.ContainsKey(statName))
                        {
                            result.Add(statName, s.Mean);
                        }
                    }
                    return result;
                }
                case DataStat.ZERO:
                {
                    int count = 0;
                    foreach (String statName in nodeTimeData[nodeName].Keys)
                    {
                        count = 0;
                        List<Double> values = nodeTimeData[nodeName][statName];

                        foreach (Double value in values)
                        {
                            if (value == 0)
                            {
                                count++;
                            }
                        }

                        if (!result.ContainsKey(statName))
                        {
                            result.Add(statName, count);
                        }
                    }
                    return result;
                }
                case DataStat.POSITIVE:
                {
                    int count = 0;
                    foreach (String statName in nodeTimeData[nodeName].Keys)
                    {
                        count = 0;
                        List<Double> values = nodeTimeData[nodeName][statName];

                        foreach (Double value in values)
                        {
                            if (!(Double.IsNaN(value) || Double.IsInfinity(value)))
                            {
                                if (value > 0)
                                {
                                    count++;
                                }
                            }
                        }

                        if (!result.ContainsKey(statName))
                        {
                            result.Add(statName, count);
                        }
                    }
                    return result;
                }
            }
            return null;
        }

        public List<String> getStatisticalFeatures(bool useCount)
        {
            List<String> statisticalFeatures = new List<String>();
            if (nodeTimeData.Count > 0)
            {
                foreach (String statName in nodeTimeData.First().Value.Keys)
                {
                    if (isBaseFeature(statName) && !statisticalFeatures.Contains(statName))
                    {
                        // Console.WriteLine(statName + " == Base Feature.");
                        if (useCount)
                        {
                            statisticalFeatures.Add(statName + "_COUNT");
                        }
                        
                        statisticalFeatures.Add(statName + "_MIN");
                        statisticalFeatures.Add(statName + "_MAX");
                        statisticalFeatures.Add(statName + "_MEAN");
                        statisticalFeatures.Add(statName + "_STDDEV");
                        statisticalFeatures.Add(statName + "_VARIANCE");
                        statisticalFeatures.Add(statName + "_SKEWNESS");
                        statisticalFeatures.Add(statName + "_KURTOSIS");
                    }
                    else
                    {
                        Console.WriteLine(statName + " != Base Feature.");
                    }
                }
            }

            return statisticalFeatures;
        }

        public List<String> getBaseFeatures(int extension = -1)
        {
            List<String> baseFeatures = new List<String>();
            if (nodeTimeData.Count > 0)
            {
                foreach (String statName in nodeTimeData.First().Value.Keys)
                {
                    if (isBaseFeature(statName) && !baseFeatures.Contains(statName))
                    {
                        // Console.WriteLine(statName + " == Base Feature.");
                        if (extension < 0)
                            baseFeatures.Add(statName);
                        else
                        {
                            for (int i = 0; i < extension; i++)
                            {
                                baseFeatures.Add(statName + "_" + i);
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine(statName + " != Base Feature.");
                    }
                }
            }

            return baseFeatures;
        }

        public List<String> getFitFeatures(String fitModel)
        {
            List<String> fitFeatures = new List<String>();
            List<String> baseFeatures = getBaseFeatures();

            if (fitModels.ContainsKey(fitModel))
            {
                baseFeatures.ForEach(baseFeature => fitFeatures.Add(String.Format("fitModel-{0}-{1}", fitModel, baseFeature)));                
            }

            return fitFeatures;
        }

        public List<String> getFitFeatureCoeffs(String fitModel)
        {
            List<String> fitFeatures = new List<String>();
            List<String> baseFeatures = getBaseFeatures();

            if (fitModels.ContainsKey(fitModel))
            {
                int numCoeffs = fitModels[fitModel];

                baseFeatures.ForEach(baseFeature => {
                    for (int i = 0; i < numCoeffs; i++)
                        fitFeatures.Add(String.Format("fitModel-{0}-{1}-c{2}", fitModel, baseFeature, i));
                });
            }

            return fitFeatures;
        }

        public bool isBaseFeature(String featureName)
        {
            foreach (String modelName in fitModels.Keys)
                if (featureName.Contains(modelName))
                    return false;

            return true;
        }

        public String getFitFeatureName(String coefficientFeatureName)
        {
            string[] parts = coefficientFeatureName.Split('-');
            if (parts.Length == 4)
            {
                String fitModel = parts[1];
                String baseFeature = parts[2];
                String coeff = parts[3];

                return "fitModel" + "-" + fitModel + "-" + baseFeature;
            }

            else 
            {
                Console.WriteLine("Failed to parse base name of coefficient feature: " + coefficientFeatureName);
            }

            return "";
        }

        /*
        public void saveModel(String filePath, MODEL model, int numSamples)
        {
            switch (model)
            {
                case Model.MODEL.DYNAMIC:
                {
                    this.saveModelToFile(filePath, Model.MODEL.DYNAMIC, "", numSamples);
                    break;
                }

                case Model.MODEL.STATIC:
                {
                    this.saveModelToFile(filePath, Model.MODEL.STATIC, "", 1);
                    break;
                }

                case Model.MODEL.FFT:
                {
                    throw new NotImplementedException("This Model is not yet implemented.");
                }

                case Model.MODEL.STATISTICAL:
                {
                    //this.saveModelToFile(filePath, Settings.MODEL.STATISTICAL, "", numSamples);
                    StatisticalModel m = new StatisticalModel(this);
                    m.saveToFile(filePath);
                    break;
                }

                case Model.MODEL.POLY_FIT_1:
                {
                    this.saveCoefficientsToFile(filePath, "poly1", Settings.TIME_MODEL.LAST_VALUE, "");
                    break;
                }

                case Model.MODEL.POLY_FIT_2:
                {
                    this.saveCoefficientsToFile(filePath, "poly2", Settings.TIME_MODEL.LAST_VALUE, "");
                    break;
                }

                case Model.MODEL.POLY_FIT_3:
                {
                    this.saveCoefficientsToFile(filePath, "poly3", Settings.TIME_MODEL.LAST_VALUE, "");
                    break;
                }

                case Model.MODEL.POLY_FIT_4:
                {
                    this.saveCoefficientsToFile(filePath, "poly4", Settings.TIME_MODEL.LAST_VALUE, "");
                    break;
                }

                case Model.MODEL.POLY_FIT_5:
                {
                    this.saveCoefficientsToFile(filePath, "poly5", Settings.TIME_MODEL.LAST_VALUE, "");
                    break;
                }

                case Model.MODEL.POLY_FIT_6:
                {
                    this.saveCoefficientsToFile(filePath, "poly6", Settings.TIME_MODEL.LAST_VALUE, "");
                    break;
                }

                case Model.MODEL.POLY_FIT_7:
                {
                    this.saveCoefficientsToFile(filePath, "poly7", Settings.TIME_MODEL.LAST_VALUE, "");
                    break;
                }

                case Model.MODEL.POLY_FIT_8:
                {
                    this.saveCoefficientsToFile(filePath, "poly8", Settings.TIME_MODEL.LAST_VALUE, "");
                    break;
                }
            }
        }*/

        public List<String> getAvailableModels()
        {
            return Enum.GetNames(typeof(Model.MODEL)).ToList();
        }

        public IModel getModel(Model.MODEL type)
        {
            switch (type)
            {
                case MODEL.STATISTICAL:
                    {
                        return new StatisticalModel(new Analyzer(this.sourcePath));
                    }
                case MODEL.SAMPLED:
                    {
                        return new SampledModel(new Analyzer(this.sourcePath));
                    }
                case MODEL.STATIC:
                    {
                        return new StaticModel(new Analyzer(this.sourcePath));
                    }
                default:
                    {
                        throw new NotImplementedException("The Model of Type " + type.ToString() + " is not yet implemented.");
                    }
            }
        }

        /*
        private void saveModelToFile(String filePath, MODEL model, String selectedNode, int samples)
        {
            // The basic set of features
            List<String> baseFeatureSet = getBaseFeatures();

            // Start with the base feature set
            List<String> featureSet = baseFeatureSet;

            // Obtain the list of node names
            List<String> nodeNames = this.getNodeNames();

            // Use StringBuilder to build output
            StringBuilder sb = new StringBuilder();

            List<String> headerValues;
            if (model == Model.MODEL.DYNAMIC)
            {
                headerValues = getBaseFeatures(samples);
            }
            else if (model == Model.MODEL.STATISTICAL || model == Model.MODEL.STATISTICAL_FILL)
            {
                headerValues = getStatisticalFeatures(false);
            }
            else if (model == Model.MODEL.STATISTICAL_COUNT || model == Model.MODEL.STATISTICAL_FILL_COUNT)
            {
                headerValues = getStatisticalFeatures(true);
            }
            else
            {
                headerValues = featureSet;
            }

            int numFeatureValues = headerValues.Count;
            // Append the header line (columns)
            sb.Append("Name"); // first column is the name
            headerValues.ForEach(feature => sb.Append(";" + feature)); // following columns are dynamic features
            sb.AppendLine(";Class"); // last column is the classifier



            // Holds stat names and their list of values
            Dictionary<string, List<Double>> nodeStats = new Dictionary<string, List<double>>();


            // For each node (line)
            foreach (String nodeName in nodeNames)
            {
                if (selectedNode == "" || nodeName == selectedNode)
                {

                    List<String> processedFeatures = new List<String>();

                    // First column is the node name
                    sb.Append(nodeName);

                    // Obtain node stats, store them in previously defined dictionary
                    nodeStats = this.getAllStatsForNode(nodeName);


                    // For each feature specified in our feature set
                    foreach (String feature in featureSet)
                    {
                        String featureBaseName = feature;

                        // If we have valid statistics of this feature for the current node
                        if (nodeStats.ContainsKey(featureBaseName))
                        {
                            if (!processedFeatures.Contains(featureBaseName)) // Feature was not processed previously
                            {
                                if (isBaseFeature(featureBaseName)) // Process base feature
                                {
                                    if (model == Model.MODEL.STATIC)
                                    {
                                        double featureValue = nodeStats[featureBaseName].Last();
                                        if (featureValue >= 0)
                                            sb.Append(";" + featureValue.ToString(Settings.DOUBLE_FORMAT));
                                        else
                                            sb.Append(";" + Settings.MISSING_VALUE_IDENTIFIER);
                                    }
                                    else if (model == Model.MODEL.DYNAMIC)
                                    {
                                        int count = 0;
                                        foreach (double featureValue in nodeStats[featureBaseName])
                                        {
                                            if (count < samples)
                                            {
                                                if (featureValue >= 0)
                                                    sb.Append(";" + featureValue.ToString(Settings.DOUBLE_FORMAT));
                                                else
                                                    sb.Append(";" + Settings.MISSING_VALUE_IDENTIFIER);
                                                
                                                count++;
                                            }
                                            else
                                            {
                                                break;
                                            }
                                        }

                                        while (count < samples)
                                        {
                                            sb.Append(";" + Settings.MISSING_VALUE_IDENTIFIER);
                                            count++;
                                        }
                                    }
                                    else if (model == Model.MODEL.STATISTICAL || model == Model.MODEL.STATISTICAL_FILL ||
                                        model == Model.MODEL.STATISTICAL_COUNT || model == Model.MODEL.STATISTICAL_FILL_COUNT)
                                    {
                                        List<Double> featureValuesForStatistics = new List<Double>();
                                        int count = 0;
                                        foreach (double featureValue in nodeStats[featureBaseName])
                                        {
                                            if (count < samples)
                                            {
                                                if (featureValue >= 0)
                                                {
                                                    featureValuesForStatistics.Add(featureValue);
                                                    count++;
                                                }
                                            }
                                            else
                                            {
                                                break;
                                            }
                                        }

                                        if (model == Model.MODEL.STATISTICAL_FILL || model == Model.MODEL.STATISTICAL_FILL_COUNT)
                                        {
                                            while (count < samples)
                                            {
                                                featureValuesForStatistics.Add(0);
                                                count++;
                                            }
                                        }

                                        // Build Statistic from model
                                        // Count, Min, Max, Mean, StdDev, Variance, Skewness, Kurtosis
                                        DescriptiveStatistics featureStatistics = new DescriptiveStatistics(featureValuesForStatistics);
                                        List<Double> statValues = new List<Double>();

                                        statValues.Add(featureStatistics.Minimum);
                                        statValues.Add(featureStatistics.Maximum);
                                        statValues.Add(featureStatistics.Mean);
                                        statValues.Add(featureStatistics.StandardDeviation);
                                        statValues.Add(featureStatistics.Variance);
                                        statValues.Add(featureStatistics.Skewness);
                                        statValues.Add(featureStatistics.Kurtosis);

                                        if (model == Model.MODEL.STATISTICAL_COUNT || model == Model.MODEL.STATISTICAL_FILL_COUNT)
                                        {
                                            sb.Append(";" + featureStatistics.Count);
                                        }

                                        foreach (Double val in statValues)
                                        {
                                            if (Double.IsInfinity(val))
                                                sb.Append(";" + Settings.MISSING_VALUE_IDENTIFIER);
                                            else if (Double.IsNaN(val))
                                                sb.Append(";" + Settings.MISSING_VALUE_IDENTIFIER);
                                            else
                                                sb.Append(";" + val.ToString(Settings.DOUBLE_FORMAT));
                                        }
                                    }
                                    else
                                    {
                                        throw new NotImplementedException("The chosen Model is not implemented.");
                                    }
                                }

                                processedFeatures.Add(featureBaseName);
                            }
                        }
                        else // append missing value
                        {
                            // Console.WriteLine("Node Stats for " + nodeName + ", does not contain stat: " + featureBaseName);
                            for (int i = 0; i < samples; i++)
                                sb.Append(";" + Settings.MISSING_VALUE_IDENTIFIER);
                        }
                    }

                    // Add the classifier
                    Analyzer.NodeType nodeType = getNodeType(nodeName);
                    if (nodeType == NodeType.GOODWARE)
                        sb.Append(";goodware");
                    else if (nodeType == NodeType.MALWARE)
                        sb.Append(";malware");
                    else
                        sb.Append(";unknown");

                    sb.Append("\n");
                }
            }
            File.WriteAllText(filePath, sb.ToString());
        }*/

        public void saveCoefficientsToFile(String filePath, String saveModel, Settings.TIME_MODEL timeModel, String selectedNode)
        {
            // The basic set of features
            List<String> baseFeatureSet = getBaseFeatures();
            // Set of Features for the fit model
            List<String> fitFeatureSet = getFitFeatures(saveModel);
            // Each Coefficient of each Feature of the fit model
            List<String> coeffFeatureSet = getFitFeatureCoeffs(saveModel);

            // Start with the base feature set
            List<String> featureSet = baseFeatureSet;
            // Add the coefficient feature set
            featureSet.AddRange(coeffFeatureSet);


            // Obtain the list of node names
            List<String> nodeNames = this.getNodeNames();

            // Use StringBuilder to build output
            StringBuilder sb = new StringBuilder();

            // Append the header line (columns)
            sb.Append("Name"); // first column is the name
            featureSet.ForEach(feature => sb.Append(";" + feature)); // following columns are dynamic features
            sb.AppendLine(";Class"); // last column is the classifier
            


            // Holds stat names and their list of values
            Dictionary<string, List<Double>> nodeStats = new Dictionary<string, List<double>>();


            // For each node (line)
            foreach (String nodeName in nodeNames)
            {
                if (selectedNode == "" || nodeName == selectedNode)
                {

                    List<String> processedFeatures = new List<String>();

                    // First column is the node name
                    sb.Append(nodeName);

                    // Obtain node stats, store them in previously defined dictionary
                    nodeStats = this.getAllStatsForNode(nodeName);


                    // For each feature specified in our feature set
                    foreach (String feature in featureSet)
                    {
                        String featureBaseName = feature;

                        if (!isBaseFeature(feature))
                        {
                            featureBaseName = getFitFeatureName(feature);
                        }

                        // If we have valid statistics of this feature for the current node
                        if (nodeStats.ContainsKey(featureBaseName)) // Append value of each coefficient
                        {
                            if (!processedFeatures.Contains(featureBaseName)) // Feature was not processed previously
                            {
                                if (isBaseFeature(featureBaseName)) // Process base feature
                                {
                                    if (timeModel == Settings.TIME_MODEL.LAST_VALUE)
                                    {
                                        sb.Append(";" + nodeStats[featureBaseName].Last().ToString(Settings.DOUBLE_FORMAT));
                                    }
                                    else
                                    {
                                        nodeStats[featureBaseName].ForEach(timeValue => sb.Append(";" + timeValue.ToString(Settings.DOUBLE_FORMAT)));
                                    }
                                }

                                else // Process fit feature
                                {
                                    nodeStats[featureBaseName].ForEach(coeffValue => sb.Append(";" + coeffValue.ToString(Settings.DOUBLE_FORMAT)));
                                }

                                processedFeatures.Add(featureBaseName);
                            }
                        }
                        else // append missing value
                        {
                            // Console.WriteLine("Node Stats for " + nodeName + ", does not contain stat: " + featureBaseName);
                            sb.Append(";" + Settings.MISSING_VALUE_IDENTIFIER);
                        }
                    }

                    // Add the classifier
                    Analyzer.NodeType nodeType = getNodeType(nodeName);
                    if (nodeType == NodeType.GOODWARE)
                        sb.Append(";goodware");
                    else if (nodeType == NodeType.MALWARE)
                        sb.Append(";malware");
                    else
                        sb.Append(";unknown");

                    sb.Append("\n");



                }
            }
            File.WriteAllText(filePath, sb.ToString());
        }

        public void loadCoefficientsFromFile(String filePath)
        {
            string[] coeffLines = File.ReadAllLines(filePath);

            foreach (String line in coeffLines)
            {
                string[] parts = line.Split(';');
                
                if (parts.Length >= 3)
                {
                    try
                    {
                        String nodeName = parts[0];
                        String statName = parts[1];
                        List<Double> values = new List<Double>();

                        // Read coeffs
                        double coeff;
                        for (int i = 2; i < parts.Length; i++)
                        {
                            if (Double.TryParse(parts[i], out coeff))
                            {
                                if (!Double.IsNegativeInfinity(coeff) && !Double.IsPositiveInfinity(coeff) && !Double.IsNaN(coeff))
                                    values.Add(coeff);
                            }                                
                        }

                        if (values.Count > 0)
                            this.addStatForNode(nodeName, statName, values);
                    }

                    catch (Exception e)
                    {
                        Console.WriteLine("Error while parsing coefficients: " + e.Message);
                    }
                }
            }
        }
    }
}

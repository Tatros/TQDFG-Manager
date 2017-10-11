using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using StatsAnalyzer.ExecutionEngine;
using System.Collections.Concurrent;
using QDFGAnalyzer;
using System.IO;
using StatsAnalyzer.Model;
using Accord;
using Accord.MachineLearning;
using Accord.Statistics.Analysis;
using Accord.Math;
using Accord.Controls;
using Accord.MachineLearning.VectorMachines;
using Accord.MachineLearning.VectorMachines.Learning;
using Accord.Statistics.Kernels;

namespace StatsAnalyzer
{
    public partial class Form1 : Form
    {
        Analyzer analyzer = null;
        List<String> startFilters = new List<String>();
        List<String> containFilters = new List<String>();
        List<String> nodes = new List<String>();
        List<String> nodesFiltered = new List<String>();
        List<String> _selectedNodeNames = new List<String>();
        //ConcurrentBag<Tuple<String, String>> fitQueue = new ConcurrentBag<Tuple<string, string>>();
        ConcurrentQueue<Tuple<String, String>> fitQueue = new ConcurrentQueue<Tuple<string, string>>();
        DataTable table = new DataTable();
        List<String> fitModels;
        List<String> saveModels;
        List<String> models;
        //List<Tuple<String, String>> fitQueue = new List<Tuple<String, String>>();
        BindingSource bs = new BindingSource();
        ExecutionEnvironment matlabEE = null;
        bool hasAllocatedProcess = false;
        int timeStep = 500;
        String programDir = "";
        String lastLogDir = "";
        bool cancelPlot = false;
        private bool isPlotting = false;
        String selectedFitModel = "poly1";
        String selectedSaveModel = "poly1";
        Timer timerIOTask;
        List<String> runs = new List<String>();
        List<String> timeModels = Enum.GetNames(typeof(QDFGGraphManager.Settings.TimeDataType)).ToList();
        List<String> dataModels = Enum.GetNames(typeof(QDFGGraphManager.Settings.ModelDataType)).ToList();
        Dictionary<String, String> analysisQueue = new Dictionary<string, string>();
        IModel currentModel = null;
        MachineLearning.PCA _currentPCA = null;
        MachineLearning.PCAConfiguration _currentPCAConfiguration;
        MachineLearning.KernelSVM _currentSVM = null;
        MachineLearning.KernelSVMConfiguration _currentSVMConfiguration;
        MachineLearning.ChainValidationConfiguration _currentChainValidationConfiguration;
        MachineLearning.ChainValidation _currentChainValidation;

        public Form1()
        {
            InitializeComponent();

            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("de-DE");
            System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("de-DE");
            programDir = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            Settings.outputDirectory = programDir + "\\out";

            // default controls
            long memAllowedMB = (long) Math.Round((double)QDFGGraphManager.Settings.MEM_MAX_BYTES_ALLOWED / (1024.0 * 1024.0), 0);
            tbSettingsMinSamples.Text = QDFGGraphManager.Settings.MIN_SAMPLES.ToString();
            tbSettingsNumSamples.Text = QDFGGraphManager.Settings.NUM_SAMPLES.ToString();
            tbSettingsTimeStep.Text = QDFGGraphManager.Settings.timeStepMS.ToString();
            tbSettingsMemoryLimit.Text = memAllowedMB.ToString();
            tbSettingsNumberFormat.Text = QDFGGraphManager.Settings.NUMBER_FORMAT;
            tbSettingsMissingValueIdentifier.Text = QDFGGraphManager.Settings.MISSING_VALUE_IDENTIFIER;

            foreach (String model in timeModels)
            {
                cbSettingsTimeModel.Items.Add(model);
            }

            foreach (String model in dataModels)
            {
                cbSettingsDataModel.Items.Add(model);
            }

            cbSettingsTimeModel.SelectedIndex = 0;
            cbSettingsDataModel.SelectedIndex = 0;

            // filters
            cBoxShowGood.Checked = true;
            cBoxShowMalicious.Checked = true;
            containFilters.Add("goodware");
            containFilters.Add("malware");
            cBoxShowProcesses.Checked = true;
            
            IsPlotting = false;
            progressBar.Value = 100;
            addFeatureMenuItems(plotFeatureAllNodeToolStripMenuItem, MenuItemClickHandlerPlotFeatureAllNodes);
            addFeatureMenuItems(plotFeatureSelectedItemToolStripMenuItem, MenuItemClickHandlerPlotFeatureSelectedNode);

            matlabEE = new MatlabExecutionEngine(Settings.matlabExecutableFolder);
            allocateProcess();
            timerIOTask = new Timer();
            timerIOTask.Interval = Settings.outputInterval;
            timerIOTask.Tick += timerIOTask_Tick;
            timerIOTask.Start();
        }

        private void timerIOTask_Tick(object sender, EventArgs e)
        {
            if (analyzer != null && fitQueue.Count > 0)
            {
                if (System.IO.Directory.Exists(Settings.outputDirectory))
                {
                    List<Double> coeffs = new List<Double>();
                    StringBuilder contents = new StringBuilder();
                    Tuple<String, String> statTuple;
                    //Console.WriteLine("Fit Queue Size: " + fitQueue.Count);
                    while (fitQueue.TryDequeue(out statTuple))
                    {
                        coeffs = analyzer.getStatsForNode(statTuple.Item1, statTuple.Item2);
                        contents.Append(statTuple.Item1 + ";" + statTuple.Item2);

                        for (int i = 0; i < coeffs.Count; i++)
                        {
                            contents.Append(";" + coeffs[i].ToString(Settings.DOUBLE_FORMAT));
                        }

                        Analyzer.NodeType nodeType = analyzer.getNodeType(statTuple.Item1);

                        if (nodeType == Analyzer.NodeType.GOODWARE)
                            contents.Append(";goodware");
                        else if (nodeType == Analyzer.NodeType.MALWARE)
                            contents.Append(";malware");
                        else
                            contents.Append(";unknown");

                        contents.Append("\n");
                        System.IO.File.AppendAllText(Settings.outputDirectory + "\\" + Settings.coefficientFile, contents.ToString());
                        contents.Clear();
                        coeffs.Clear();
                    }

                    
                }
            }
        }

        private void initModels()
        {
            if (analyzer != null)
            {
                this.models = analyzer.getAvailableModels();
                comboBoxModel.DataSource = models;

                this.fitModels = analyzer.getFitModels();
                this.saveModels = analyzer.getSaveModels();

                comboBoxFitModels.DataSource = fitModels;
                comboBoxSaveModels.DataSource = saveModels;
            }
        }

        private void addFeatureMenuItems(ToolStripMenuItem parent, Action<object, EventArgs> handler)
        {
            var features = Enum.GetValues(typeof(NodeStats.Property)).Cast<NodeStats.Property>();
            ToolStripMenuItem[] items = new ToolStripMenuItem[features.Count()];
            for (int i = 0; i < items.Length; i++)
            {
                items[i] = new ToolStripMenuItem();
                items[i].Name = features.ElementAt(i).ToString();
                items[i].Tag = features.ElementAt(i).ToString();
                items[i].Text = features.ElementAt(i).ToString();
                items[i].Click += new EventHandler(handler);
            }

            parent.DropDownItems.AddRange(items);
        }

        private void MenuItemClickHandlerPlotFeatureAllNodes(object sender, EventArgs e)
        {
            if (IsPlotting)
            {
                MessageBox.Show("Please wait for the ongoing plot to finish.");
                return;
            }

            ToolStripMenuItem clickedItem = (ToolStripMenuItem)sender;

            
            if (Enum.IsDefined(typeof(NodeStats.Property), clickedItem.Name))
            {
                try
                {
                    NodeStats.Property feature = (NodeStats.Property)Enum.Parse(typeof(NodeStats.Property), clickedItem.Name);
                    plotFeatureForAllNodes(feature);
                }

                catch (ArgumentException)
                {
                    Console.WriteLine("Item " + clickedItem.Name + " is not in NodeStats.Property Enum.");
                    MessageBox.Show("Unable to plot unknown Feature: " + clickedItem.Name);
                }

                catch (PlotException ex)
                {
                    MessageBox.Show(ex.Message);
                }
                
            }
        }

        private void MenuItemClickHandlerPlotAllFeaturesAllNodes(object sender, EventArgs e)
        {
            if (IsPlotting)
            {
                MessageBox.Show("Please wait for the ongoing plot to finish.");
                return;
            }

            try { plotAllFeaturesForAllNodes(); } 
            catch (PlotException ex) { MessageBox.Show(ex.Message); }
        }

        private void MenuItemClickHandlerPlotAllFeaturesSelectedNode(object sender, EventArgs e)
        {
            if (IsPlotting)
            {
                MessageBox.Show("Please wait for the ongoing plot to finish.");
                return;
            }

            try
            {
                if (nodeBox.SelectedItem != null)
                    plotAllFeaturesForNode(nodeBox.SelectedItem.ToString(), selectedFitModel);
                else
                    MessageBox.Show("Please select a node first.");
            }
            catch (PlotException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void MenuItemClickHandlerPlotFeatureSelectedNode(object sender, EventArgs e)
        {
            if (IsPlotting)
            {
                MessageBox.Show("Please wait for the ongoing plot to finish.");
                return;
            }

            ToolStripMenuItem clickedItem = (ToolStripMenuItem)sender;


            if (Enum.IsDefined(typeof(NodeStats.Property), clickedItem.Name))
            {
                try
                {
                    NodeStats.Property feature = (NodeStats.Property)Enum.Parse(typeof(NodeStats.Property), clickedItem.Name);

                    if (nodeBox.SelectedItem != null)
                    {
                        plotFeatureForNode(feature, nodeBox.SelectedItem.ToString(), selectedFitModel);
                        updateInfoTextUI();
                    }
                    else
                        MessageBox.Show("Please select a node first.");
                }

                catch (ArgumentException)
                {
                    Console.WriteLine("Item " + clickedItem.Name + " is not in NodeStats.Property Enum.");
                    MessageBox.Show("Unable to commence plot. Unknown Feature: " + clickedItem.Name);
                }

                catch (PlotException ex)
                {
                    MessageBox.Show(ex.Message);
                }

            }
        }

        private async void allocateProcess()
        {
            await Task.Run(() => matlabEE.AcquireProcess());
            this.hasAllocatedProcess = true;
            Console.WriteLine("Matlab Process Allocated");
        }

        public void resetFilters()
        {
            startFilters = new List<String>();
            containFilters = new List<String>();

            containFilters.Add("goodware");
            containFilters.Add("malware");
            startFilters.Add("P>");

            this.cBoxShowGood.Checked = true;
            this.cBoxShowMalicious.Checked = true;
            this.cBoxShowFiles.Checked = false;
            this.cBoxShowKeys.Checked = false;
            this.cBoxShowMisc.Checked = false;
            this.cBoxShowSockets.Checked = false;
            this.cBoxShowUnknown.Checked = false;
            this.cBoxShowProcesses.Checked = true;

            nodesFiltered = new List<String>();
            bs = new BindingSource();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog oFileDialog = new OpenFileDialog();
            oFileDialog.Multiselect = false;
            oFileDialog.Title = "Select a Stats file";
            oFileDialog.Filter = "QDFG Stats|*.csv";

            if (oFileDialog.ShowDialog() == DialogResult.OK)
            {
                FolderBrowserDialog sFileDialog = new FolderBrowserDialog();
                sFileDialog.Description = "Select a output Directory";

                if (sFileDialog.ShowDialog() == DialogResult.OK)
                {
                    //MessageBox.Show(sFileDialog.SelectedPath);
                    openModel(oFileDialog.FileName);
                }
            }
        }

        private void openModel(String path)
        {
            analyzer = new Analyzer(path);
            this.nodes = analyzer.getNodeNames();
            this.timeStep = analyzer.TimeStep;

            resetFilters();
            applyFilter();
            nodeBox.DataSource = bs;
            bs.DataSource = this.nodesFiltered;
            initModels();
            //textBoxNumSamplesSave.Text = this.numSamplesSave.ToString();
            updateLabels();
            IsPlotting = false;
            progressBar.Value = 100;
        }

        private void updateInfoTextUI()
        {
            if (nodeBox.SelectedItem != null)
            {
                this.nodeInfoBox.Text = String.Format("Node Info for: {0}\r\n\r\n", nodeBox.SelectedItem.ToString());
                this.textBoxCoefficients.Text = String.Format("Curve Fitting Results for: {0}\r\n\r\n", nodeBox.SelectedItem.ToString());
                if (analyzer != null)
                {
                    Dictionary<String, List<Double>> timeStats = analyzer.getAllStatsForNode(nodeBox.SelectedItem.ToString());

                    this.nodeInfoBox.Text += analyzer.getStringRepresentationNodeTimeData(nodeBox.SelectedItem.ToString());
                    this.textBoxCoefficients.Text += analyzer.getStringRepresentationCurveFit(nodeBox.SelectedItem.ToString());
                }
            }
        }

        private void updateLabels()
        {
            numSelectedLabel.Text = "Selected: " + nodeBox.SelectedItems.Count + " / " + nodesFiltered.Count;
        }

        private void model_ModelChanged(object sender, EventArgs e)
        {
            if (currentModel != null)
            {
                textBoxModel.Text = currentModel.ToString();
            }
        }

        private void nodeBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            updateLabels();
            table = new DataTable();
            if (nodeBox.SelectedItem != null)
            {
                //table.Rows.Clear();
                //table.Columns.Clear();
                

                updateInfoTextUI();

                if (analyzer != null)
                {
                    Dictionary<String, List<Double>> timeStats = analyzer.getAllStatsForNode(nodeBox.SelectedItem.ToString());

                    nodeInfoTrackbar.Minimum = 0;
                    nodeInfoTrackbar.Value = 0;
                    trackbarValueLabel.Text = "Step 0, (0ms) ";
                    nodeInfoTrackbar.Maximum = analyzer.getMaxItemCount(nodeBox.SelectedItem.ToString()) - 1;
                    LabelTrackbarEnd.Text = nodeInfoTrackbar.Maximum.ToString();
                    this.nodeInfoTrackbar.Enabled = true;
                    nodeInfoTrackbar.TickFrequency = timeStep;

                    // 
                    if (nodeBox.SelectedItems.Count > 0)
                    {
                        Dictionary<String, double> total = new Dictionary<string, double>();
                        Dictionary<String, double> positive = new Dictionary<string, double>();
                        Dictionary<String, double> missing = new Dictionary<string, double>();
                        Dictionary<String, double> zero = new Dictionary<string, double>();
                        Dictionary<String, double> negative = new Dictionary<string, double>();
                        Dictionary<String, double> min = new Dictionary<string,  double>();
                        Dictionary<String, double> max = new Dictionary<string, double>();
                        Dictionary<String, double> mean = new Dictionary<string, double>();
                        Dictionary<String, double> minValue = new Dictionary<string, double>();
                        Dictionary<String, double> maxValue = new Dictionary<string, double>();
                        Dictionary<String, double> variance = new Dictionary<string, double>();
                        Dictionary<String, double> stddev = new Dictionary<string, double>();

                        Dictionary<String, double> ctotal = new Dictionary<string, double>();
                        Dictionary<String, double> cpositive = new Dictionary<string, double>();
                        Dictionary<String, double> cmissing = new Dictionary<string, double>();
                        Dictionary<String, double> czero = new Dictionary<string, double>();
                        Dictionary<String, double> cnegative = new Dictionary<string, double>();
                        Dictionary<String, double> cmin = new Dictionary<string, double>();
                        Dictionary<String, double> cmax = new Dictionary<string, double>();
                        Dictionary<String, double> cmean = new Dictionary<string, double>();
                        Dictionary<String, double> cvariance = new Dictionary<string, double>();
                        Dictionary<String, double> cstddev = new Dictionary<string, double>();

                        this._selectedNodeNames.Clear();

                        foreach (var item in nodeBox.SelectedItems)
                        {
                            this._selectedNodeNames.Add(item.ToString());

                            ctotal = analyzer.getNodeDataStats(item.ToString(), Analyzer.DataStat.TOTAL);
                            cpositive = analyzer.getNodeDataStats(item.ToString(), Analyzer.DataStat.POSITIVE);
                            cmissing = analyzer.getNodeDataStats(item.ToString(), Analyzer.DataStat.MISSING);
                            czero = analyzer.getNodeDataStats(item.ToString(), Analyzer.DataStat.ZERO);
                            cnegative = analyzer.getNodeDataStats(item.ToString(), Analyzer.DataStat.NEGATIVE);
                            cmin = analyzer.getNodeDataStats(item.ToString(), Analyzer.DataStat.MIN);
                            cmax = analyzer.getNodeDataStats(item.ToString(), Analyzer.DataStat.MAX);
                            cmean = analyzer.getNodeDataStats(item.ToString(), Analyzer.DataStat.MEAN);
                            cvariance = analyzer.getNodeDataStats(item.ToString(), Analyzer.DataStat.VARIANCE);
                            cstddev = analyzer.getNodeDataStats(item.ToString(), Analyzer.DataStat.STDDEV);


                            // update min value
                            foreach (var pair in cmin)
                            {
                                if (minValue.ContainsKey(pair.Key))
                                {
                                    if (minValue[pair.Key] > pair.Value)
                                    {
                                        minValue[pair.Key] = pair.Value;
                                    }
                                }

                                else
                                {
                                    minValue.Add(pair.Key, pair.Value);
                                }
                            }

                            // update max value
                            foreach (var pair in cmax)
                            {
                                if (maxValue.ContainsKey(pair.Key))
                                {
                                    if (maxValue[pair.Key] < pair.Value)
                                    {
                                        maxValue[pair.Key] = pair.Value;
                                    }
                                }

                                else
                                {
                                    maxValue.Add(pair.Key, pair.Value);
                                }
                            }


                            addCurrentToTotal(ref total, ref ctotal);
                            addCurrentToTotal(ref positive, ref cpositive);
                            addCurrentToTotal(ref missing, ref cmissing);
                            addCurrentToTotal(ref zero, ref czero);
                            addCurrentToTotal(ref negative, ref cnegative);
                            buildAverage(ref min, ref cmin);
                            buildAverage(ref max, ref cmax);
                            buildAverage(ref mean, ref cmean);
                            buildAverage(ref variance, ref cvariance);
                            buildAverage(ref stddev, ref cstddev);
                        }
                        
                        foreach (var statName in total.Keys)
                        {
                            table.Rows.Add ();
                        }

                        table.Columns.Add("Name", typeof(String));
                        table.Columns.Add("Total", typeof(String));
                        table.Columns.Add("Positive", typeof(String));
                        table.Columns.Add("Missing", typeof(String));
                        table.Columns.Add("Zero", typeof(String));
                        table.Columns.Add("Negative", typeof(String));

                        table.Columns.Add("Pos %", typeof(String));
                        table.Columns.Add("Miss %", typeof(String));
                        table.Columns.Add("Zero %", typeof(String));
                        table.Columns.Add("Neg %", typeof(String));

                        table.Columns.Add("Min (avg)", typeof(String));
                        table.Columns.Add("Min", typeof(String));
                        table.Columns.Add("Mean (avg)", typeof(String));
                        table.Columns.Add("Max (avg)", typeof(String));
                        table.Columns.Add("Max", typeof(String));
                        table.Columns.Add("Variance (avg)", typeof(String));
                        table.Columns.Add("StdDev (avg)", typeof(String));


                        int rowCount = 0;
                        foreach (var statName in total.Keys)
                        {
                            double positivePercent = (100.0 / total[statName]) * (double)positive[statName];
                            double missingPercent = (100.0 / total[statName]) * (double)missing[statName];
                            double zeroPercent = (100.0 / total[statName]) * (double)zero[statName];
                            double negativePercent = (100.0 / total[statName]) * (double)negative[statName];
                            
                            table.Rows[rowCount][0] = statName;

                            if (total.ContainsKey(statName))
                                table.Rows[rowCount][1] = total[statName];
                            if (positive.ContainsKey(statName))
                                table.Rows[rowCount][2] = positive[statName];
                            if (missing.ContainsKey(statName))
                                table.Rows[rowCount][3] = missing[statName];
                            if (zero.ContainsKey(statName))
                                table.Rows[rowCount][4] = zero[statName];
                            if (negative.ContainsKey(statName))
                                table.Rows[rowCount][5] = negative[statName];

                            table.Rows[rowCount][6] = positivePercent.ToString("0");
                            table.Rows[rowCount][7] = missingPercent.ToString("0");
                            table.Rows[rowCount][8] = zeroPercent.ToString("0");
                            table.Rows[rowCount][9] = negativePercent.ToString("0");

                            if (min.ContainsKey(statName))
                                table.Rows[rowCount][10] = min[statName].ToString("0.000");
                            if (minValue.ContainsKey(statName))
                                table.Rows[rowCount][11] = minValue[statName].ToString("0.000");
                            if (mean.ContainsKey(statName))
                                table.Rows[rowCount][12] = mean[statName].ToString("0.000");
                            if (max.ContainsKey(statName))
                                table.Rows[rowCount][13] = max[statName].ToString("0.000");
                            if (maxValue.ContainsKey(statName))
                                table.Rows[rowCount][14] = maxValue[statName].ToString("0.000");
                            if (variance.ContainsKey(statName))
                                table.Rows[rowCount][15] = variance[statName].ToString("0.000");
                            if (stddev.ContainsKey(statName))
                                table.Rows[rowCount][16] = stddev[statName].ToString("0.000");

                            rowCount++;
                            
                        }

                        this.NodeInfoDataGridView.DataSource = table;
                        this.NodeInfoDataGridView.AutoResizeColumns();
                    }
                }
            }
        }

        private void buildAverage(ref Dictionary<String, double> runningAverage, ref Dictionary<String, double> current)
        {
            if (current != null && runningAverage != null)
            {
                foreach (String statName in current.Keys)
                {
                    if (runningAverage.ContainsKey(statName))
                    {
                        if (!Double.IsNaN(current[statName]))
                        {
                            double[] data = new double[] { runningAverage[statName], current[statName] };
                            runningAverage[statName] = MathNet.Numerics.Statistics.ArrayStatistics.Mean(data);
                        }
                    }
                    else
                    {
                        if (!Double.IsNaN(current[statName]))
                        {
                            runningAverage.Add(statName, current[statName]);
                        }
                    }
                }
            }
        }

        private void addCurrentToTotal(ref Dictionary<String, double> totals, ref Dictionary<String, double> current)
        {
            foreach (String statName in current.Keys)
            {
                if (totals.ContainsKey(statName))
                {
                    totals[statName] += current[statName];
                }

                else
                {
                    totals.Add(statName, current[statName]);
                }
            }
        }

        private void applyFilter()
        {
            nodesFiltered.Clear();

            if (containFilters.Count > 0)
            {
                foreach (String nodeName in nodes)
                {
                    foreach (String filter in containFilters)
                    {
                        if (nodeName.Contains(filter))
                        {
                            if (!nodesFiltered.Contains(nodeName))
                            {
                                nodesFiltered.Add(nodeName);
                            }
                        }

                        if (filter == "unknown")
                        {
                            if ((!nodeName.Contains("goodware") && !nodeName.Contains("malware")))
                            {
                                if (!nodesFiltered.Contains(nodeName))
                                {
                                    nodesFiltered.Add(nodeName);
                                }
                            }
                        }
                    }

                    bool accept = false;
                    foreach (String filter in startFilters)
                    {   
                        if (nodeName.StartsWith(filter))
                        {
                            accept = true;
                        }
                    }

                    if (!accept)
                        nodesFiltered.Remove(nodeName);
                }
            }

            textBoxStartFilter.Text = "";
            textBoxContainFilter.Text = "";
            foreach (String s in startFilters)
            {
                textBoxStartFilter.Text = textBoxStartFilter.Text + s + " ";
            }

            foreach (String s in containFilters)
            {
                textBoxContainFilter.Text = textBoxContainFilter.Text + s + " ";
            }

            bs.ResetBindings(false);
        }

        private void nodeInfoTrackbar_Scroll(object sender, EventArgs e)
        {
            nodeInfoBox.Text = "Node: " + nodeBox.SelectedItem.ToString() + "\r\n\r\n";
            int trackbarValue = nodeInfoTrackbar.Value;
            trackbarValueLabel.Text = String.Format("Step {0}, ({1} ms)", nodeInfoTrackbar.Value, (nodeInfoTrackbar.Value * timeStep));
            Dictionary<String, List<Double>> timeStats = analyzer.getAllStatsForNode(nodeBox.SelectedItem.ToString());

            
            foreach (String nodeFeature in timeStats.Keys)
            {
                if (timeStats[nodeFeature].Count > trackbarValue)
                    nodeInfoBox.Text += nodeFeature + ": " + timeStats[nodeFeature][trackbarValue] + "\r\n";
                else
                    nodeInfoBox.Text += nodeFeature + ": No Data" + "\r\n";
            }
        }

        private void cBoxShowProcesses_CheckedChanged(object sender, EventArgs e)
        {
            String processFilter = "P>";
            if (cBoxShowProcesses.Checked)
            {
                if (!startFilters.Contains(processFilter))
                {
                    startFilters.Add(processFilter);
                }
            }

            else
            {
                startFilters.Remove(processFilter);
            }

            applyFilter();
        }

        private void cBoxShowKeys_CheckedChanged(object sender, EventArgs e)
        {
            String keysFilter = "R>";
            if (cBoxShowKeys.Checked)
            {
                if (!startFilters.Contains(keysFilter))
                {
                    startFilters.Add(keysFilter);
                }
            }

            else
            {
                startFilters.Remove(keysFilter);
            }

            applyFilter();
        }

        private void cBoxShowFiles_CheckedChanged(object sender, EventArgs e)
        {
            String filesFilter = "F>";
            if (cBoxShowFiles.Checked)
            {
                if (!startFilters.Contains(filesFilter))
                {
                    startFilters.Add(filesFilter);
                }
            }

            else
            {
                startFilters.Remove(filesFilter);
            }

            applyFilter();
        }

        private void cBoxShowSockets_CheckedChanged(object sender, EventArgs e)
        {
            String socketFilter = "S>";
            if (cBoxShowSockets.Checked)
            {
                if (!startFilters.Contains(socketFilter))
                {
                    startFilters.Add(socketFilter);
                }
            }

            else
            {
                startFilters.Remove(socketFilter);
            }

            applyFilter();
        }

        private void cBoxShowGood_CheckedChanged(object sender, EventArgs e)
        {
            String goodwareFilter = "goodware";
            if (cBoxShowGood.Checked)
            {
                if (!containFilters.Contains(goodwareFilter))
                {
                    containFilters.Add(goodwareFilter);
                }
            }

            else
            {
                containFilters.Remove(goodwareFilter);
            }

            applyFilter();
        }

        private void cBoxShowMalicious_CheckedChanged(object sender, EventArgs e)
        {
            String malwareFilter = "malware";
            if (cBoxShowMalicious.Checked)
            {
                if (!containFilters.Contains(malwareFilter))
                {
                    containFilters.Add(malwareFilter);
                }
            }

            else
            {
                containFilters.Remove(malwareFilter);
            }

            applyFilter();
        }

        private void cBoxShowUnknown_CheckedChanged(object sender, EventArgs e)
        {
            String unknownFilter = "unknown";
            if (cBoxShowUnknown.Checked)
            {
                if (!containFilters.Contains(unknownFilter))
                {
                    containFilters.Add(unknownFilter);
                }
            }

            else
            {
                containFilters.Remove(unknownFilter);
            }

            applyFilter();
        }

        private void cBoxShowMisc_CheckedChanged(object sender, EventArgs e)
        {
            applyFilter();
        }

        private String plotData(NodeStats.Property p, String xlimits, String ylimits, String yLabel, String nodeName, String fitModel)
        {
            List<Double> Values = analyzer.getStatsForNode(nodeName, p.ToString());

            StringBuilder xValues = new StringBuilder();
            StringBuilder yValues = new StringBuilder();
            // StringBuilder yValues2 = new StringBuilder();

            for (int i = 0; i < Values.Count; i++)
            {
                xValues.Append(Math.Round((i * timeStep) / 1000.0).ToString() + ";"); // seconds
                yValues.Append(Values[i].ToString(Settings.DOUBLE_FORMAT, new System.Globalization.CultureInfo("en-US")) + ";");
            }

            // Console.WriteLine(xValues.ToString());
            // Console.WriteLine(yValues.ToString());
                
            /* SIMPLE PLOT */
            // String plot = "fig = figure(); x = [" + xValues.ToString() + "]; y = [" + yValues.ToString() + "]; plot(x,y); saveas(fig,'testCS.fig');";


            // Create Command Passed to Matlab via COM
            StringBuilder advPlot = new StringBuilder();

            advPlot.Append("h1=figure(1);");

            if (Settings.closeMatlabWindow)
                advPlot.Append("set(h1, 'visible','off');");

            advPlot.Append("x = [" + xValues.ToString() + "];");
            advPlot.Append("y = [" + yValues.ToString() + "];");

            if (Settings.useFitting && fitModel != "")
            {
                if (fitModels.Contains(fitModel))
                {
                    if (fitModel.Contains("poly"))
                    {
                        //Console.WriteLine("Poly Fit: " + fitModel[4]);
                        if (Settings.plotPointsOnly)
                            advPlot.Append("plot(x,y,'o');");
                        else
                            advPlot.Append("plot(x,y,'-');");


                        advPlot.Append("[p, ~, mu] = polyfit(x, y, " + fitModel[4] + ");");
                        advPlot.Append("f = polyval(p,x,[],mu);");
                        advPlot.Append("hold on;");
                        advPlot.Append("plot(x,f,'r-');");
                        advPlot.Append("hold off;");
                    }

                    else
                    {
                        advPlot.Append("f=fit(x,y,'" + fitModel + "');");
                        advPlot.Append("plot(f,x,y);");
                        //Console.WriteLine("General Fit: " + fitModel);
                    }
                }

                else
                {
                    Console.WriteLine("Unknown Fit Model: " + fitModel);
                    return "";
                }
            }
            else
            {
                if (Settings.plotPointsOnly)
                    advPlot.Append("plot(x,y,'-o');");
                else
                    advPlot.Append("plot(x,y);");
            }
            if (xlimits != "")
                advPlot.Append("xlim(" + xlimits + ");");
            if (ylimits != "")
                advPlot.Append("ylim(" + ylimits + ");");
            advPlot.Append("title('" + nodeName + "');");
            advPlot.Append("ylabel('" + yLabel + "');");
            String fileName = nodeName.Substring(2);
            String folderName = fileName.Substring(0, fileName.Length - 4);

            String outputDir = Settings.outputDirectory;
            String outPath = outputDir + "\\plots\\" + folderName;
            if (!System.IO.Directory.Exists(outPath))
                System.IO.Directory.CreateDirectory(outPath);

            // Console.WriteLine(outPath);

            if (Settings.useFitting)
                advPlot.Append("print('" + outPath + "\\" + p.ToString() + " - " + fitModel + " - " + fileName + ".png','-dpng');");
            else
                advPlot.Append("print('" + outPath + "\\" + p.ToString() + " - " + fileName + ".png','-dpng');");

            if (Settings.saveFigures)
            {
                advPlot.Append("saveas(gcf,'" + outPath + "\\" + p.ToString() + " - " + fileName + ".fig');");
            }

            if (Settings.closeMatlabWindow)
            {
                advPlot.Append("delete(gcf);");
            }

            return advPlot.ToString();


            //Console.WriteLine(advPlot.ToString());
            //Utility.runMatlab(advPlot.ToString(), Settings.closeMatlabWindow);
                
            // saveas(fig,'testCS.fig');
            /*
            if (p == NodeStats.Property.Centrality)
            {
                advPlot.Append("figure;");
                advPlot.Append("ax1 = subplot(2,1,1);");
                advPlot.Append("ax2 = subplot(2,1,2);");
                advPlot.Append("x = [" + xValues.ToString() + "];");
                advPlot.Append("y1 = [" + yValues.ToString() + "];");
                advPlot.Append("y2 = [" + yValues2.ToString() + "];");
                advPlot.Append("plot(ax1,x,y1);");
                if (xlimits != "")
                    advPlot.Append("xlim(ax1," + xlimits + ");");
                if (ylimits != "")
                    advPlot.Append("ylim(ax1," + ylimits + ");");
                advPlot.Append("title(ax1,'" + selectedNodeName + "');");
                advPlot.Append("ylabel(ax1,'" + yLabel + " (Total)');");
                advPlot.Append("plot(ax2,x,y2);");
                if (xlimits != "")
                    advPlot.Append("xlim(ax2," + xlimits + ");");
                advPlot.Append("ylim(ax2,[0 100]);");
                advPlot.Append("title(ax2,'" + selectedNodeName + "');");
                advPlot.Append("ylabel(ax2,'" + yLabel + " (Relative)');");
            }*/

            /* EXAMPLE 
            advPlot.Append("figure;");
            advPlot.Append("ax1 = subplot(2,1,1);");
            advPlot.Append("ax2 = subplot(2,1,2);");
            advPlot.Append("x = linspace(0,3);");
            advPlot.Append("y1 = sin(5*x);");
            advPlot.Append("y2 = sin(15*x);");
            advPlot.Append("plot(ax1,x,y1);");
            advPlot.Append("title(ax1,'Top Subplot');");
            advPlot.Append("ylabel(ax1,'sin(5x)');");
            advPlot.Append("plot(ax2,x,y2);");
            advPlot.Append("title(ax2,'Bottom Subplot');");
            advPlot.Append("ylabel(ax2,'sin(15x)');");*/
        }

        private void checkPlotConstraints(String nodeName)
        {
            if (nodeBox.Items.Count <= 0)
                throw new PlotException("Unable to commence plot. No nodes found.\nTry adjusting your filter.");

            if (!nodes.Contains(nodeName))
                throw new PlotException("Unable to commence plot. No node with name: " + nodeName);

            if (matlabEE == null)
                throw new PlotException("Unable to commence plot. Execution Engine was not initialized. Please restart the application.");

            if (!hasAllocatedProcess)
            {
                allocateProcess();
                throw new PlotException("Unable to commence plot. Matlab Process was not allocated.\nPlease try again in a few seconds.");
            }
        }

        // Request Plot for Specific Node
        private void requestPlot(NodeStats.Property featureName, String nodeName, bool ignoreOngoing, String fitModel)
        {
            try
            {
                checkPlotConstraints(nodeName);
                String plot = plotData(featureName, "", "", featureName.ToString(), nodeName, fitModel);
                String res = matlabEE.ExecuteCommand(plot);

                Console.WriteLine("res: " + res);
                if (Settings.useFitting)
                {
                    String statName = "fitModel-" + fitModel + "-" + featureName;
                    Tuple<String, String> statTuple = new Tuple<String, String>(nodeName, statName);

                    try
                    {
                        var coeff = matlabEE.GetVariable("p");

                        if (coeff != null)
                        {
                            double[,] c = (double[,])coeff;
                            for (int i = 0; i < c.GetLength(1); i++)
                            {
                                Console.WriteLine("c{0}: {1}", i, c[0, i]);       
                                analyzer.addStatForNode(nodeName, statName, c.Cast<Double>().ToList());
                            }

                            if (!fitQueue.Contains(statTuple))
                                fitQueue.Enqueue(statTuple);
                        }
                        else
                        {
                            Console.WriteLine("var was null.");
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Failed to obtain coefficients of polynomial fit: " + e.Message);
                    }
                }
            }

            catch (PlotException e)
            {
                MessageBox.Show(e.Message);
            }
        }

        private void plotFeatureForNode(NodeStats.Property feature, String nodeName)
        {
            requestPlot(feature, nodeName, false, "");
        }

        private void plotFeatureForNode(NodeStats.Property feature, String nodeName, String fitModel)
        {
            requestPlot(feature, nodeName, false, fitModel);
        }

        private async void plotAllFeaturesForNode(String nodeName, String fitModel)
        {
            IsPlotting = true;
            var features = Enum.GetValues(typeof(NodeStats.Property)).Cast<NodeStats.Property>();

            int numPlots = features.Count();
            int numFinished = 0;

            foreach (NodeStats.Property feature in features)
            {
                await Task.Run(() => plotFeatureForNode(feature, nodeName, fitModel));
                numFinished++;
                int progress = Convert.ToInt32((100 / (double)numPlots) * numFinished);
                labelPlotProgress.Text = numFinished + " / " + numPlots + "  ( " + progress + "% )";
                this.progressBar.Value = progress;
                updateInfoTextUI();

                if (cancelPlot)
                    break;
            }

            labelPlotProgress.Text = "Finished";
            IsPlotting = false;
            cancelPlot = false;
        }

        private async void plotFeatureForAllNodes(NodeStats.Property featureName)
        {
            IsPlotting = true;
            String fitModel = selectedFitModel;
            int numPlots = nodes.Count;
            int numFinished = 0;

            foreach (String nodeName in nodes)
            {
                await Task.Run(() => plotFeatureForNode(featureName, nodeName, fitModel));
                numFinished++;
                int progress = Convert.ToInt32((100 / (double)numPlots) * numFinished);
                labelPlotProgress.Text = numFinished + " / " + numPlots + "  ( " + progress + "% )";
                this.progressBar.Value = progress;
                updateInfoTextUI();

                if (cancelPlot)
                    break;
            }

            labelPlotProgress.Text = "Finished";
            IsPlotting = false;
            cancelPlot = false;
        }

        private async void plotAllFeaturesForAllNodes()
        {
            IsPlotting = true;
            var features = Enum.GetValues(typeof(NodeStats.Property)).Cast<NodeStats.Property>();

            String fitModel = selectedFitModel;
            int numPlots = nodes.Count * features.Count();
            int numFinished = 0;

            foreach (String nodeName in nodes)
            {
                foreach (NodeStats.Property feature in features)
                {
                    await Task.Run(() => plotFeatureForNode(feature, nodeName, fitModel));
                    numFinished++;
                    int progress = Convert.ToInt32((100 / (double)numPlots) * numFinished);
                    labelPlotProgress.Text = numFinished + " / " + numPlots + "  ( " + progress + "% )";
                    this.progressBar.Value = progress;
                    updateInfoTextUI();

                    if (cancelPlot)
                        break;
                }

                if (cancelPlot)
                    break;
            }

            labelPlotProgress.Text = "Finished";
            IsPlotting = false;
            cancelPlot = false;
        }

        public void Wait(int ms)
        {
            System.Threading.Thread.Sleep(ms);
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
                Settings.closeMatlabWindow = true;
            else
                Settings.closeMatlabWindow = false;
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox2.Checked)
                Settings.saveFigures = true;
            else
                Settings.saveFigures = false;
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox3.Checked)
                Settings.plotPointsOnly = true;
            else
                Settings.plotPointsOnly = false;
            
        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox4.Checked)
                Settings.useFitting = true;
            else
                Settings.useFitting = false;
        }

        private void buttonCancelPlot_Click(object sender, EventArgs e)
        {
            if (IsPlotting)
            {
                cancelPlot = true;
            }
        }

        bool IsPlotting
        {
            get { return this.isPlotting; }
            set
            {
                this.isPlotting = value;
                buttonCancelPlot.Enabled = value;
                plotToolStripMenuItem.Enabled = !value;
            }
        }

        private void comboBoxFitModels_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (fitModels.Contains(comboBoxFitModels.SelectedValue.ToString()))
                selectedFitModel = comboBoxFitModels.SelectedValue.ToString();
            else
                MessageBox.Show("Please select a valid model.");
        }

        private void buttonLoadCoeff_Click(object sender, EventArgs e)
        {
            if (analyzer != null)
            {
                OpenFileDialog d = new OpenFileDialog();
                DialogResult res = d.ShowDialog();

                if (res == DialogResult.OK)
                {
                    String file = d.FileName;
                    if (file != "" && System.IO.File.Exists(file))
                    {
                        analyzer.loadCoefficientsFromFile(file);
                    }
                }
            }

            else
            {
                MessageBox.Show("Please open a statistics file first.");
            }
        }

        private void buttonSaveCoeffs_Click(object sender, EventArgs e)
        {
            if (analyzer != null)
            {
                SaveFileDialog d = new SaveFileDialog();
                DialogResult res = d.ShowDialog();

                if (res == DialogResult.OK)
                {
                    String file = d.FileName;
                    if (file != "" && file.Length > 1)
                    {
                        analyzer.saveCoefficientsToFile(file, selectedSaveModel, Settings.TIME_MODEL.LAST_VALUE, "");
                    }
                }
            }

            else
            {
                MessageBox.Show("Please open a statistics file first.");
            }
        }

        private void comboBoxSaveModels_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (saveModels.Contains(comboBoxSaveModels.SelectedValue.ToString()))
                selectedSaveModel = comboBoxSaveModels.SelectedValue.ToString();
            else
                MessageBox.Show("Please select a valid output model.");
        }

        private void buttonSaveModel_Click(object sender, EventArgs e)
        {
            try
            {
                if (analyzer != null && currentModel != null)
                {
                    String selectedModel = comboBoxModel.SelectedValue.ToString();
                    if (analyzer.getAvailableModels().Contains(selectedModel))
                    {
                        Model.MODEL m = (Model.MODEL)Enum.Parse(typeof(Model.MODEL), selectedModel);

                        if (currentModel.Type != m)
                        {
                            MessageBox.Show("Current Model <" + currentModel.Type.ToString() + "> did not match selected Model <" + m.ToString() + ">.");
                            return;
                        }

                        SaveFileDialog d = new SaveFileDialog();
                        DialogResult res = d.ShowDialog();

                        if (res == DialogResult.OK)
                        {
                            String file = d.FileName;
                            if (file != "" && file.Length > 1)
                            {
                                try {
                                    currentModel.saveToFile(file);
                                    showStatusLabel("Model saved: " + file, Settings.StatusDisplayTimeMS);
                                    //analyzer.saveModel(file, m, Int32.Parse(textBoxNumSamplesSave.Text));
                                } catch (FormatException)
                                {
                                    MessageBox.Show("Please choose a valid number of samples.");
                                }
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("Please select a valid Model from the dropdown box.");
                    }
                }

                else
                {
                    MessageBox.Show("Please open a statistics file first.");
                }
            }

        
            catch (Exception ex)
            {
                MessageBox.Show("Unable to save Model: " + ex.Message);
            }
        }

        private async void showStatusLabel(String message, int timeMS)
        {
            await Task.Run(() => {
                toolStripStatusLabel.Text = message;
                Wait(timeMS);
                toolStripStatusLabel.Text = Settings.defaultStatusMessage;
            });
        }

        private void setCurrentModel(IModel model)
        {
            this.currentModel = model;
            model.ModelChanged += model_ModelChanged;
            this.textBoxModel.Text = currentModel.ToString();

            if (_currentPCAConfiguration != null)
            {
                _currentPCAConfiguration.ActiveFeatures = model.getFeatureNames();
            }

            if (_currentSVMConfiguration != null)
            {
                _currentSVMConfiguration.ActiveFeatures = model.getFeatureNames();
            }
        }

        private void comboBoxModel_SelectedIndexChanged(object sender, EventArgs e)
        {
            String selectedModel = comboBoxModel.SelectedValue.ToString();
            if (analyzer.getAvailableModels().Contains(selectedModel))
            {
                Model.MODEL m = (Model.MODEL)Enum.Parse(typeof(Model.MODEL), selectedModel);

                try
                {
                    setCurrentModel(analyzer.getModel(m));
                }
                catch (Exception ex)
                {
                    currentModel = null;
                    textBoxModel.Text = "";
                    MessageBox.Show("Failed to load Model: " + ex.Message);
                }
            }

            else
            {
                MessageBox.Show("Please choose a valid Model from the list.");
            }
        }

        private void singleLogToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                String logFilePath = ofd.FileName;
                LogAnalyzer logAnalyzer = new LogAnalyzer();
                logAnalyzer.analyzeLog(logFilePath, Utility.getRunDirectory("default"));

                if (!runs.Contains("default"))
                    runs.Add("default");
            }
        }

        internal long MemoryMaxBytesAllowed
        {
            get
            {
                return (Int64.Parse(tbSettingsMemoryLimit.Text) * 1024 * 1024);
            }
        }

        internal int MinNumSamples
        {
            get
            {
                return Int32.Parse(tbSettingsMinSamples.Text);
            }
        }

        internal String MissingValueIdentifier
        {
            get
            {
                return tbSettingsMissingValueIdentifier.Text;
            }
        }

        internal QDFGGraphManager.Settings.ModelDataType ModelDataType
        {
            get
            {
                return (QDFGGraphManager.Settings.ModelDataType)Enum.Parse(typeof(QDFGGraphManager.Settings.ModelDataType), cbSettingsDataModel.Text);
            }
        }

        internal QDFGGraphManager.Settings.TimeDataType TimeDataType
        {
            get
            {
                return (QDFGGraphManager.Settings.TimeDataType)Enum.Parse(typeof(QDFGGraphManager.Settings.TimeDataType), cbSettingsTimeModel.Text);
            }
        }

        internal String NumberFormat
        {
            get
            {
                return tbSettingsNumberFormat.Text;
            }
        }

        internal int NumSamples
        {
            get
            {
                return Int32.Parse(tbSettingsNumSamples.Text);
            }
        }

        internal int TimeStepMS
        {
            get
            {
                return Int32.Parse(tbSettingsTimeStep.Text);
            }
        }

        private void initAnalyzerSettings(LogAnalyzer logAnalyzer)
        {
            logAnalyzer.MemoryMaxBytesAllowed = (Int64.Parse(tbSettingsMemoryLimit.Text) * 1024 * 1024);
            logAnalyzer.MinNumSamples = Int32.Parse(tbSettingsMinSamples.Text);
            logAnalyzer.MissingValueIdentifier = tbSettingsMissingValueIdentifier.Text;
            logAnalyzer.ModelDataType = (QDFGGraphManager.Settings.ModelDataType) Enum.Parse(typeof(QDFGGraphManager.Settings.ModelDataType), cbSettingsDataModel.Text);
            logAnalyzer.TimeDataType = (QDFGGraphManager.Settings.TimeDataType)Enum.Parse(typeof(QDFGGraphManager.Settings.TimeDataType), cbSettingsTimeModel.Text);
            logAnalyzer.NumberFormat = tbSettingsNumberFormat.Text;
            logAnalyzer.NumSamples = Int32.Parse(tbSettingsNumSamples.Text);
            logAnalyzer.TimeStepMS = Int32.Parse(tbSettingsTimeStep.Text);
        }
           
 
        private void createAnalyzerInstance(String runName, String folderPath)
        {
            /*
            InstanceProgressForm f = new InstanceProgressForm(runName, folderPath,
                MemoryMaxBytesAllowed, MinNumSamples, MissingValueIdentifier,
                ModelDataType, TimeDataType, NumberFormat, NumSamples, TimeStepMS);

            f.Show();*/

            String outputFolder = Application.StartupPath + "\\out";

	        ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.FileName = "AnalysisInstance.exe";
	        startInfo.Arguments = "\"" + runName + "\" "
                + "\"" + folderPath + "\" "
                + (MemoryMaxBytesAllowed / 1024 / 1024) + " "
                + MinNumSamples + " "
                + NumSamples + " "
                + MissingValueIdentifier + " "
                + ModelDataType.ToString() + " "
                + TimeDataType.ToString() + " "
                + NumberFormat.ToString() + " " 
                + TimeStepMS + " "
                + "\"" + outputFolder + "\"";
	        Process process = Process.Start(startInfo);
            process.EnableRaisingEvents = true;
            process.Exited += (sender, eventArgs) =>
                {
                    if (analysisQueue.ContainsKey(runName))
                        analysisQueue.Remove(runName);

                    if (analysisQueue.Count > 0)
                    {
                        String nextInstance = analysisQueue.Keys.First();
                        Console.WriteLine("Finished Run [" + runName + "], Starting next Run: " + nextInstance);
                        createAnalyzerInstance(nextInstance, analysisQueue[nextInstance]);
                    }

                    Console.WriteLine("Finished Run [" + runName + "]. Queue is empty.");
                };

            Console.WriteLine("Process Start: " + startInfo.Arguments);
        }

        private void folderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            //fbd.RootFolder = Environment.SpecialFolder.MyComputer;
            if (this.lastLogDir != "" && Directory.Exists(this.lastLogDir))
                fbd.SelectedPath = this.lastLogDir;
            else if (Directory.Exists(Application.StartupPath + "\\EventLogs"))
                fbd.SelectedPath = Application.StartupPath + "\\EventLogs";
            else
                fbd.SelectedPath = Application.StartupPath;

            if (fbd.ShowDialog() == DialogResult.OK)
            {
                this.lastLogDir = fbd.SelectedPath;
                String runName = "";
                if (Utility.InputBox("Name this Run", "Run Name", ref runName) == DialogResult.OK)
                {
                    if (runName == "")
                        runName = "default";
                }

                createAnalyzerInstance(runName, fbd.SelectedPath);

                if (!runs.Contains(runName))
                    runs.Add(runName);

                /*
                if (MessageBox.Show(this,"Would you like to open the Model?", "Open Model", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
                {
                    openModel(Utility.getRunDirectory(runName) + "\\" + logAnalyzer.TimeDataFile);
                }
                */
            }
        }

        private void tbSettingsMemoryLimit_Click(object sender, EventArgs e)
        {
            int maxValue = 120000;
            String newValueStr = "";
            if (Utility.InputBox("Set Memory Limit", "New Memory Limit [500,30000] mb: ", ref newValueStr) == DialogResult.OK)
            {
                int newValue;
                if (Int32.TryParse(newValueStr, out newValue))
                {
                    if (newValue < 500 || newValue > maxValue)
                    {
                        MessageBox.Show("The Memory Limit must be inside the following interval [500," + maxValue + "]");
                    }
                    else
                    {
                        tbSettingsMemoryLimit.Text = newValue.ToString();
                    }
                }
            }
        }

        private void tbSettingsNumberFormat_Click(object sender, EventArgs e)
        {
            String newValueStr = "";
            if (Utility.InputBox("Set Number Format", "New Number Format [F0,F1,F2,F3]: ", ref newValueStr) == DialogResult.OK)
            {
                List<String> acceptableValues = new List<String> {"F0", "F1", "F2", "F3"};
                if (acceptableValues.Contains(newValueStr))
                {
                    tbSettingsNumberFormat.Text = newValueStr;
                }

                else
                {
                    MessageBox.Show("This format is unknown.");
                }
            }
        }

        private void tbSettingsMissingValueIdentifier_Click(object sender, EventArgs e)
        {
            String newValueStr = "";
            if (Utility.InputBox("Set Missing Value Identifier", "New Identifier: ", ref newValueStr) == DialogResult.OK)
            {
                if (newValueStr.Trim().Length == 1)
                {
                    tbSettingsMissingValueIdentifier.Text = newValueStr;
                }

                else
                {
                    MessageBox.Show("Identifier must be a single char.");
                }
            }
        }

        private void tbSettingsMinSamples_Click(object sender, EventArgs e)
        {
            String newValueStr = "";
            if (Utility.InputBox("Set Min Samples", "New Min Samples [1,1000]: ", ref newValueStr) == DialogResult.OK)
            {
                int newValue;
                if (Int32.TryParse(newValueStr, out newValue))
                {
                    if (newValue < 1 || newValue > 1000)
                    {
                        MessageBox.Show("The minimum number of samples must be inside the following interval [1,1000]");
                    }
                    else
                    {
                        tbSettingsMinSamples.Text = newValue.ToString();
                    }
                }
            }
        }

        private void tbSettingsNumSamples_Click(object sender, EventArgs e)
        {
            String newValueStr = "";
            if (Utility.InputBox("Set Num Samples", "New Num Samples [1,10000]: ", ref newValueStr) == DialogResult.OK)
            {
                int newValue;
                if (Int32.TryParse(newValueStr, out newValue))
                {
                    if (newValue < 1 || newValue > 10000)
                    {
                        MessageBox.Show("The number of samples must be inside the following interval [1,10000]");
                    }
                    else
                    {
                        tbSettingsNumSamples.Text = newValue.ToString();
                    }
                }
            }
        }

        private void tbSettingsTimeStep_Click(object sender, EventArgs e)
        {
            String newValueStr = "";
            if (Utility.InputBox("Set Time Step", "New Time Step [10,10000]: ", ref newValueStr) == DialogResult.OK)
            {
                int newValue;
                if (Int32.TryParse(newValueStr, out newValue))
                {
                    if (newValue < 10 || newValue > 10000)
                    {
                        MessageBox.Show("Time step must be inside the following interval [10,10000]");
                    }
                    else
                    {
                        tbSettingsTimeStep.Text = newValue.ToString();
                    }
                }
            }
        }

        private void cbSettingsTimeModel_TextChanged(object sender, EventArgs e)
        {
            if (!timeModels.Contains(cbSettingsTimeModel.Text))
            {
                cbSettingsTimeModel.Text = timeModels.First();
            }
        }

        private void cbSettingsDataModel_TextChanged(object sender, EventArgs e)
        {
            if (!dataModels.Contains(cbSettingsDataModel.Text))
            {
                cbSettingsDataModel.Text = dataModels.First();
            }
        }

        private void pCAScatterPlotToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (currentModel != null)
            {
                OptionsDialog oDiag = new OptionsDialog(currentModel);
                oDiag.StartPosition = FormStartPosition.CenterParent;
                oDiag.ShowDialog();
            }
        }

        private void runPCAonModel(IModel model)
        {
            if (model != null)
            {
                if (_currentPCAConfiguration == null)
                    _currentPCAConfiguration = new MachineLearning.PCAConfiguration(currentModel.getFeatureNames());

                _currentPCA = new MachineLearning.PCA(this.currentModel, SelectedNodeNames, _currentPCAConfiguration);
                _currentPCA.compute();
            }
            else
            {
                throw new ArgumentException("The provided model is not initialized.");
            }
        }

        private void runPCAonCurrentModel()
        {
            try
            {
                runPCAonModel(currentModel);
            }

            catch (Exception ex)
            {
                MessageBox.Show("Unable to run PCA: " + ex.Message);
            }
        }

        private void buttonPCA_Click(object sender, EventArgs e)
        {
            runPCAonCurrentModel();
        }

        private void buttonPCAViewData_Click(object sender, EventArgs e)
        {

            if (currentModel != null)
            {
                if (_currentPCA != null)
                {
                    FormDataView<double> f = new FormDataView<double>(_currentPCA.Data,
                        captions: _currentPCA.ColumnHeaders,
                        nodeNames: _currentPCA.Nodes.Select(node => node.ToString()).ToList<String>());
                    f.Show();
                }
                else
                {
                    if (MessageBox.Show(this, "No PCA was completed yet. Would you like to run a PCA now?", "Run PCA", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        runPCAonCurrentModel();
                    }
                }
            }
            else
            {
                MessageBox.Show("Please load some data and select a model first.");
            }
        }

        private void buttonPCAViewTransformed_Click(object sender, EventArgs e)
        {
            if (_currentPCA != null)
            {
                try
                {
                    FormDataView<double> f = new FormDataView<double>(_currentPCA.TransformedData,
                        captions: _currentPCA.ColumnHeaders,
                        nodeNames: _currentPCA.Nodes.Select(node => node.ToString()).ToList<String>()
                    );
                    f.Show();
                } 
                catch (ArgumentException)
                {
                    try
                    {
                        FormDataView<double> f = new FormDataView<double>(_currentPCA.TransformedData,
                            captions: null,
                            nodeNames: _currentPCA.Nodes.Select(node => node.ToString()).ToList<String>()
                        );
                        f.Show();
                    }
                    catch (ArgumentException ex)
                    {
                        MessageBox.Show("Unable to show Data: " + ex.Message);
                    }
                }
            }
            else
            {
                if (MessageBox.Show(this, "No PCA was completed yet. Would you like to run a PCA now?", "Run PCA", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    runPCAonCurrentModel();
                }
            }
        }

        private void buttonViewDataTable_Click(object sender, EventArgs e)
        {
            if (currentModel != null)
            {
                FormDataView<double> f = new FormDataView<double>(currentModel, SelectedNodeNames);
                f.Show();
            }
            else
            {
                MessageBox.Show("Please load some data and select a model first.");
            }
        }

        internal List<String> SelectedNodeNames
        {
            get
            {
                return _selectedNodeNames;
            }
        }

        private void buttonPCAViewComponents_Click(object sender, EventArgs e)
        {
            if (_currentPCA != null)
            {
                FormDataView<double> f = new FormDataView<double>(_currentPCA.PrincipalComponentAnalysis);
                f.Show();
            }
            else
            {
                if (MessageBox.Show(this, "No PCA was completed yet. Would you like to run a PCA now?", "Run PCA", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    runPCAonCurrentModel();
                }
            }
        }

        private void buttonConfigurePCA_Click(object sender, EventArgs e)
        {
            if (currentModel != null)
            {
                if (_currentPCAConfiguration == null)
                    _currentPCAConfiguration = new MachineLearning.PCAConfiguration(currentModel.getFeatureNames());

                OptionsPCADialog oDiag = new OptionsPCADialog(_currentPCAConfiguration, currentModel);
                oDiag.StartPosition = FormStartPosition.CenterParent;
                oDiag.ShowDialog();
            }
            else
            {
                MessageBox.Show("Please go to the \"Model\" Tab and select a model first.");
            }
        }

        private void buttonExportPCA_Click(object sender, EventArgs e)
        {
            if (currentModel != null)
            {
                if (_currentPCA != null)
                {
                    SaveFileDialog d = new SaveFileDialog();
                    DialogResult res = d.ShowDialog();

                    if (res == DialogResult.OK)
                    {
                        String file = d.FileName;
                        if (file != "" && file.Length > 1)
                        {
                            try
                            {
                                _currentPCA.export(file);
                                showStatusLabel("PCA exported to: " + file, Settings.StatusDisplayTimeMS);
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show("Export Failed: " + ex.Message);
                            }
                        }
                    }
                }
            }
        }

        private void initSVM()
        {
            if (currentModel != null)
            {
                if (_currentSVMConfiguration == null)
                    _currentSVMConfiguration = new MachineLearning.KernelSVMConfiguration(currentModel.getFeatureNames());

                _currentSVM = new StatsAnalyzer.MachineLearning.KernelSVM(currentModel, _currentSVMConfiguration);
            }
            else
            {
                MessageBox.Show("Please go to the \"Model\" Tab and select a model first.");
            }
        }

        private void runSVMonCurrentModel()
        {
            if (_currentSVM == null)
                initSVM();

            _currentSVM.trainSVM();
            _currentSVM.testSVM();
        }

        private void buttonKSVMTrain_Click(object sender, EventArgs e)
        {
            if (currentModel != null)
            {
                try
                {
                    runSVMonCurrentModel();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occured during training: " + ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Please go to the \"Model\" Tab and select a model first.");
            }
        }

        private void buttonSVMPerformCV_Click(object sender, EventArgs e)
        {
            try
            {
                initSVM();

                List<MachineLearning.CrossValidationResult> cvResults = _currentSVM.performCrossValidation();

                MachineLearning.CVResultDialog resultDialog = new MachineLearning.CVResultDialog(cvResults, _currentSVMConfiguration, featureModel: currentModel.Name);
                resultDialog.StartPosition = FormStartPosition.CenterParent;
                resultDialog.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Convergence could not be achieved. Try lowering Complexity C");
            }
            /*
            try
            {

            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occured during cross validation: " + ex.Message);
            }*/
        }

        private void buttonSVMConfigure_Click(object sender, EventArgs e)
        {
            if (currentModel != null)
            {
                if (_currentSVMConfiguration == null)
                    _currentSVMConfiguration = new MachineLearning.KernelSVMConfiguration(currentModel.getFeatureNames());

                OptionsSVMDialog oDiag = new OptionsSVMDialog(_currentSVMConfiguration, currentModel);
                oDiag.StartPosition = FormStartPosition.CenterParent;
                oDiag.ShowDialog();
            }
            else
            {
                MessageBox.Show("Please go to the \"Model\" Tab and select a model first.");
            }
        }

        private void buttonChainConfigure_Click(object sender, EventArgs e)
        {
            if (currentModel != null)
            {
                if (_currentChainValidationConfiguration == null)
                    _currentChainValidationConfiguration = new MachineLearning.ChainValidationConfiguration();

                MachineLearning.ChainValidationConfigurationForm oDiag = new MachineLearning.ChainValidationConfigurationForm(_currentChainValidationConfiguration);
                oDiag.StartPosition = FormStartPosition.CenterParent;
                oDiag.ShowDialog();
            }
            else
            {
                MessageBox.Show("Please go to the \"Model\" Tab and select a model first.");
            }
        }

        private void buttonChainValidation_Click(object sender, EventArgs e)
        {
            if (currentModel != null)
            {
                if (_currentChainValidationConfiguration == null)
                    _currentChainValidationConfiguration = new MachineLearning.ChainValidationConfiguration();

                if (_currentSVMConfiguration == null)
                {
                    buttonSVMConfigure_Click(sender, e);
                    //MessageBox.Show("Please configure the kSVM first. [Kernel Support Vector Machine -> Configure]");
                }

                //MessageBox.Show(_currentChainValidationConfiguration.ChainValidationMode.ToString());
                if (!_currentChainValidationConfiguration.ChainValidationMode.ToString().StartsWith("STATIC_") && !_currentChainValidationConfiguration.ChainValidationMode.ToString().StartsWith("SAMPLED_"))
                {
                    if (currentModel.Type != MODEL.STATISTICAL)
                    {
                        try
                        {
                            setCurrentModel(analyzer.getModel(MODEL.STATISTICAL));

                            if (MessageBox.Show(this, "Since you have selected a Statistical Validation Mode, we have switched the Temporal Feature Model to Statistical.\n Would you like to reconfigure the feature space?", "Configuration Required", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                            {
                                buttonSVMConfigure_Click(sender, e);
                            }
                        }
                        catch (Exception ex)
                        {
                            currentModel = null;
                            textBoxModel.Text = "";
                            MessageBox.Show("Failed to load Model: " + ex.Message);
                            return;
                        }
                    }

                    List<String> allowedFeatures = analyzer.getModel(MODEL.STATISTICAL).getFeatureNames();
                    bool valid = true;

                    foreach (String feature in _currentSVMConfiguration.ActiveFeatures)
                    {
                        if (!allowedFeatures.Contains(feature))
                        {
                            valid = false;
                            break;
                        }
                    }

                    if (!valid)
                    {
                        if (MessageBox.Show(this, "You have selected a Statistical Model but your feature space is not yet configured.\n Would you like to configure it now?", "Configuration Required", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        {
                            buttonSVMConfigure_Click(sender, e);
                        }
                        else
                        {
                            return;
                        }
                    }
                }

                if (_currentChainValidationConfiguration.ChainValidationMode.ToString().StartsWith("STATIC_"))
                {
                    if (currentModel.Type != MODEL.STATIC)
                    {
                        try
                        {
                            setCurrentModel(analyzer.getModel(MODEL.STATIC));

                            if (MessageBox.Show(this, "Since you have selected a Static Validation Mode, we have switched the Temporal Feature Model to Static.\n Would you like to reconfigure the feature space?", "Configuration Required", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                            {
                                buttonSVMConfigure_Click(sender, e);
                            }
                        }
                        catch (Exception ex)
                        {
                            currentModel = null;
                            textBoxModel.Text = "";
                            MessageBox.Show("Failed to load Model: " + ex.Message);
                            return;
                        }
                    }

                    List<String> allowedFeatures = analyzer.getModel(MODEL.STATIC).getFeatureNames();
                    bool valid = true;

                    foreach (String feature in _currentSVMConfiguration.ActiveFeatures)
                    {
                        if (!allowedFeatures.Contains(feature))
                        {
                            valid = false;
                            break;
                        }
                    }

                    if (!valid)
                    {
                        if (MessageBox.Show(this, "You have selected a Static Model but your feature space is not yet configured.\n Would you like to configure it now?", "Configuration Required", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        {
                            buttonSVMConfigure_Click(sender, e);
                        }
                        else
                        {
                            return;
                        }
                    }
                }

                /* SAMPLED */
                if (_currentChainValidationConfiguration.ChainValidationMode.ToString().StartsWith("SAMPLED_"))
                {
                    if (currentModel.Type != MODEL.SAMPLED)
                    {
                        try
                        {
                            setCurrentModel(analyzer.getModel(MODEL.SAMPLED));

                            if (MessageBox.Show(this, "Since you have selected a Sampled Validation Mode, we have switched the Temporal Feature Model to Sampled.\n Would you like to reconfigure the feature space?", "Configuration Required", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                            {
                                buttonSVMConfigure_Click(sender, e);
                            }
                        }
                        catch (Exception ex)
                        {
                            currentModel = null;
                            textBoxModel.Text = "";
                            MessageBox.Show("Failed to load Model: " + ex.Message);
                            return;
                        }
                    }

                    List<String> allowedFeatures = analyzer.getModel(MODEL.SAMPLED).getFeatureNames();
                    bool valid = true;

                    foreach (String feature in _currentSVMConfiguration.ActiveFeatures)
                    {
                        if (!allowedFeatures.Contains(feature))
                        {
                            valid = false;
                            break;
                        }
                    }

                    if (!valid)
                    {
                        if (MessageBox.Show(this, "You have selected a Sampled Model but your feature space is not yet configured.\n Would you like to configure it now?", "Configuration Required", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        {
                            buttonSVMConfigure_Click(sender, e);
                        }
                        else
                        {
                            return;
                        }
                    }
                }

                _currentChainValidation = new MachineLearning.ChainValidation(analyzer, _currentChainValidationConfiguration, _currentSVMConfiguration);
                _currentChainValidation.run();
                _currentChainValidation.showResults();
            }
            else
            {
                MessageBox.Show("Please go to the \"Model\" Tab and select a model first.");
            }
        }

        private void preprocessBatchifyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            //fbd.RootFolder = Environment.SpecialFolder.MyComputer;
            if (this.lastLogDir != "" && Directory.Exists(this.lastLogDir))
                fbd.SelectedPath = this.lastLogDir;
            else if (Directory.Exists(Application.StartupPath + "\\EventLogs"))
                fbd.SelectedPath = Application.StartupPath + "\\EventLogs";
            else
                fbd.SelectedPath = Application.StartupPath;

            if (fbd.ShowDialog() == DialogResult.OK)
            {
                this.lastLogDir = fbd.SelectedPath;

                int filesInBatch = -1;
                int maxValue = 200000;
                String newValueStr = "";
                if (Utility.InputBox("Number of Files in Batch", "Files per Batch: ", ref newValueStr) == DialogResult.OK)
                {
                    int newValue;
                    if (Int32.TryParse(newValueStr, out newValue))
                    {
                        if (newValue < 1 || newValue > maxValue)
                        {
                            MessageBox.Show("The Filer per Batch Count must be inside the following interval: [1," + maxValue + "]");
                            return;
                        }
                        else
                        {
                            filesInBatch = newValue;
                        }
                    }
                }

                if (filesInBatch > 0)
                {
                    string[] files = Directory.GetFiles(fbd.SelectedPath, "*.exe.csv", SearchOption.AllDirectories);

                    DirectoryInfo dirGoodware = Directory.CreateDirectory(fbd.SelectedPath + "\\Goodware");
                    DirectoryInfo dirMalware = Directory.CreateDirectory(fbd.SelectedPath + "\\Malware");

                    String batchFolder = "batch";
                    int currentGoodwareBatch = 0;
                    int currentMalwareBatch = 0;
                    int batchSize = filesInBatch;
                    int goodwareBatchCount = 0;
                    int malwareBatchCount = 0;
                    foreach (var file in files)
                    {
                        if (goodwareBatchCount >= batchSize)
                        {
                            currentGoodwareBatch++;
                            goodwareBatchCount = 0;
                        }

                        if (malwareBatchCount >= batchSize)
                        {
                            currentMalwareBatch++;
                            malwareBatchCount = 0;
                        }

                        String fileName = Path.GetFileName(file);
                        if (fileName.Contains("goodware"))
                        {
                            String destFolder = Path.Combine(dirGoodware.FullName, batchFolder + currentGoodwareBatch.ToString());

                            if (!Directory.Exists(destFolder))
                                Directory.CreateDirectory(destFolder);

                            File.Copy(file, Path.Combine(destFolder, fileName));
                            goodwareBatchCount++;
                        }
                        else if (fileName.Contains("malware"))
                        {
                            String destFolder = Path.Combine(dirMalware.FullName, batchFolder + currentMalwareBatch.ToString());

                            if (!Directory.Exists(destFolder))
                                Directory.CreateDirectory(destFolder);

                            File.Copy(file, Path.Combine(destFolder, fileName));
                            malwareBatchCount++;
                        }
                    }

                    MessageBox.Show("Created " + currentGoodwareBatch + " goodware, and " + currentMalwareBatch + " malware batches.");
                }
            }
        }

        private void processBatchToolStripMenuItem_Click(object sender, EventArgs e)
        {
            analysisQueue.Clear();
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            //fbd.RootFolder = Environment.SpecialFolder.MyComputer;
            if (this.lastLogDir != "" && Directory.Exists(this.lastLogDir))
                fbd.SelectedPath = this.lastLogDir;
            else if (Directory.Exists(Application.StartupPath + "\\EventLogs"))
                fbd.SelectedPath = Application.StartupPath + "\\EventLogs";
            else
                fbd.SelectedPath = Application.StartupPath;

            if (fbd.ShowDialog() == DialogResult.OK)
            {
                this.lastLogDir = fbd.SelectedPath;
                String runName = "";
                if (Utility.InputBox("Name this Run", "Run Name", ref runName) == DialogResult.OK)
                {
                    if (runName == "")
                        runName = "default";
                }

                string[] directories = Directory.GetDirectories(fbd.SelectedPath);

                foreach (var batchDir in directories)
                {
                    String batchRunName = runName + "-" + Path.GetFileName(batchDir);
                    analysisQueue.Add(batchRunName, batchDir);
                    if (!runs.Contains(batchRunName))
                        runs.Add(batchRunName);
                }

                // Start the first run
                if (analysisQueue.Count > 0)
                {
                    String firstRun = analysisQueue.Keys.First();
                    createAnalyzerInstance(firstRun, analysisQueue[firstRun]);
                    analysisQueue.Remove(firstRun);
                }
            }
        }

        private void consolidateGraphsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            //fbd.RootFolder = Environment.SpecialFolder.MyComputer;
            if (this.lastLogDir != "" && Directory.Exists(this.lastLogDir))
                fbd.SelectedPath = this.lastLogDir;
            else if (Directory.Exists(Application.StartupPath + "\\EventLogs"))
                fbd.SelectedPath = Application.StartupPath + "\\EventLogs";
            else
                fbd.SelectedPath = Application.StartupPath;

            if (fbd.ShowDialog() == DialogResult.OK)
            {
                this.lastLogDir = fbd.SelectedPath;
                string[] files = Directory.GetFiles(fbd.SelectedPath, "TimeData.csv", SearchOption.AllDirectories);
                DirectoryInfo dirResult = Directory.CreateDirectory(fbd.SelectedPath + "\\ConsolidatedResult");

                if (!Directory.Exists(dirResult.FullName))
                {
                    MessageBox.Show("Failed to create Output Directory:\n" + dirResult.FullName);
                    return;
                }

                String fileName = "";
                if (Utility.InputBox("Name of files to merge", "File Name", ref fileName) == DialogResult.OK)
                {
                    if (fileName == "")
                        fileName = "TimeData.csv";
                }

                String resultPath = dirResult.FullName + "\\" + fileName;
            
                byte[] endLine = Encoding.UTF8.GetBytes("\n\n\n");
                using (var output = File.Create(resultPath))
                {
                    foreach (var file in files)
                    {
                        using (var input = File.OpenRead(file))
                        {
                            input.CopyTo(output);
                            output.Write(endLine, 0, endLine.Length);
                        }
                    }
                }
                
                MessageBox.Show("Consolidated Results available at:\n" + resultPath);
            }
        }
    }
}

namespace StatsAnalyzer
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.plotToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.plotAllAllNodesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.plotFeatureAllNodeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.plotFeatureSelectedItemToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.plotAllFeaturesSelectedNodeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pCAScatterPlotToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.analyzeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.singleLogToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.folderToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.preprocessBatchifyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.processBatchToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.numSelectedLabel = new System.Windows.Forms.Label();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.buttonCancelPlot = new System.Windows.Forms.Button();
            this.labelPlotProgress = new System.Windows.Forms.Label();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.cBoxShowProcesses = new System.Windows.Forms.CheckBox();
            this.cBoxShowKeys = new System.Windows.Forms.CheckBox();
            this.textBoxContainFilter = new System.Windows.Forms.TextBox();
            this.cBoxShowFiles = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.cBoxShowSockets = new System.Windows.Forms.CheckBox();
            this.textBoxStartFilter = new System.Windows.Forms.TextBox();
            this.cBoxShowGood = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.cBoxShowMalicious = new System.Windows.Forms.CheckBox();
            this.cBoxShowMisc = new System.Windows.Forms.CheckBox();
            this.cBoxShowUnknown = new System.Windows.Forms.CheckBox();
            this.nodeBox = new System.Windows.Forms.ListBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.nodeInfoBox = new System.Windows.Forms.TextBox();
            this.trackbarValueLabel = new System.Windows.Forms.Label();
            this.nodeInfoTrackbar = new System.Windows.Forms.TrackBar();
            this.LabelTrackbarEnd = new System.Windows.Forms.Label();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.groupBox14 = new System.Windows.Forms.GroupBox();
            this.buttonLoadCoeff = new System.Windows.Forms.Button();
            this.comboBoxSaveModels = new System.Windows.Forms.ComboBox();
            this.buttonSaveCoeffs = new System.Windows.Forms.Button();
            this.textBoxCoefficients = new System.Windows.Forms.TextBox();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.buttonSaveModel = new System.Windows.Forms.Button();
            this.buttonViewDataTable = new System.Windows.Forms.Button();
            this.comboBoxModel = new System.Windows.Forms.ComboBox();
            this.button1 = new System.Windows.Forms.Button();
            this.textBoxModel = new System.Windows.Forms.TextBox();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.NodeInfoDataGridView = new System.Windows.Forms.DataGridView();
            this.tabPage6 = new System.Windows.Forms.TabPage();
            this.groupBox15 = new System.Windows.Forms.GroupBox();
            this.buttonChainConfigure = new System.Windows.Forms.Button();
            this.buttonChainValidation = new System.Windows.Forms.Button();
            this.groupBox13 = new System.Windows.Forms.GroupBox();
            this.buttonSVMPerformCV = new System.Windows.Forms.Button();
            this.buttonSVMConfigure = new System.Windows.Forms.Button();
            this.buttonKSVMTrain = new System.Windows.Forms.Button();
            this.groupBox12 = new System.Windows.Forms.GroupBox();
            this.buttonExportPCA = new System.Windows.Forms.Button();
            this.buttonPCAViewComponents = new System.Windows.Forms.Button();
            this.buttonPCAViewTransformed = new System.Windows.Forms.Button();
            this.buttonPCAViewData = new System.Windows.Forms.Button();
            this.buttonPCA = new System.Windows.Forms.Button();
            this.buttonConfigurePCA = new System.Windows.Forms.Button();
            this.tabPage5 = new System.Windows.Forms.TabPage();
            this.groupBox11 = new System.Windows.Forms.GroupBox();
            this.groupBox10 = new System.Windows.Forms.GroupBox();
            this.tbSettingsMemoryLimit = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.tbSettingsNumberFormat = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.tbSettingsMissingValueIdentifier = new System.Windows.Forms.TextBox();
            this.groupBox8 = new System.Windows.Forms.GroupBox();
            this.label5 = new System.Windows.Forms.Label();
            this.tbSettingsNumSamples = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.tbSettingsMinSamples = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.tbSettingsTimeStep = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.groupBox9 = new System.Windows.Forms.GroupBox();
            this.label7 = new System.Windows.Forms.Label();
            this.cbSettingsDataModel = new System.Windows.Forms.ComboBox();
            this.cbSettingsTimeModel = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.groupBox7 = new System.Windows.Forms.GroupBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.checkBox3 = new System.Windows.Forms.CheckBox();
            this.checkBox2 = new System.Windows.Forms.CheckBox();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.label3 = new System.Windows.Forms.Label();
            this.comboBoxFitModels = new System.Windows.Forms.ComboBox();
            this.checkBox4 = new System.Windows.Forms.CheckBox();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.consolidateGraphsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox6.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nodeInfoTrackbar)).BeginInit();
            this.tabPage2.SuspendLayout();
            this.groupBox14.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.tabPage4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.NodeInfoDataGridView)).BeginInit();
            this.tabPage6.SuspendLayout();
            this.groupBox15.SuspendLayout();
            this.groupBox13.SuspendLayout();
            this.groupBox12.SuspendLayout();
            this.tabPage5.SuspendLayout();
            this.groupBox11.SuspendLayout();
            this.groupBox10.SuspendLayout();
            this.groupBox8.SuspendLayout();
            this.groupBox9.SuspendLayout();
            this.groupBox7.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.plotToolStripMenuItem,
            this.analyzeToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(819, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Size = new System.Drawing.Size(103, 22);
            this.openToolStripMenuItem.Text = "Open";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // plotToolStripMenuItem
            // 
            this.plotToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.plotAllAllNodesToolStripMenuItem,
            this.plotFeatureAllNodeToolStripMenuItem,
            this.plotFeatureSelectedItemToolStripMenuItem,
            this.plotAllFeaturesSelectedNodeToolStripMenuItem,
            this.pCAScatterPlotToolStripMenuItem});
            this.plotToolStripMenuItem.Name = "plotToolStripMenuItem";
            this.plotToolStripMenuItem.Size = new System.Drawing.Size(40, 20);
            this.plotToolStripMenuItem.Text = "Plot";
            // 
            // plotAllAllNodesToolStripMenuItem
            // 
            this.plotAllAllNodesToolStripMenuItem.Name = "plotAllAllNodesToolStripMenuItem";
            this.plotAllAllNodesToolStripMenuItem.Size = new System.Drawing.Size(246, 22);
            this.plotAllAllNodesToolStripMenuItem.Text = "Plot All Features (All Nodes)";
            this.plotAllAllNodesToolStripMenuItem.Click += new System.EventHandler(this.MenuItemClickHandlerPlotAllFeaturesAllNodes);
            // 
            // plotFeatureAllNodeToolStripMenuItem
            // 
            this.plotFeatureAllNodeToolStripMenuItem.Name = "plotFeatureAllNodeToolStripMenuItem";
            this.plotFeatureAllNodeToolStripMenuItem.Size = new System.Drawing.Size(246, 22);
            this.plotFeatureAllNodeToolStripMenuItem.Text = "Plot Feature (All Nodes)";
            // 
            // plotFeatureSelectedItemToolStripMenuItem
            // 
            this.plotFeatureSelectedItemToolStripMenuItem.Name = "plotFeatureSelectedItemToolStripMenuItem";
            this.plotFeatureSelectedItemToolStripMenuItem.Size = new System.Drawing.Size(246, 22);
            this.plotFeatureSelectedItemToolStripMenuItem.Text = "Plot Feature (Selected Node)";
            // 
            // plotAllFeaturesSelectedNodeToolStripMenuItem
            // 
            this.plotAllFeaturesSelectedNodeToolStripMenuItem.Name = "plotAllFeaturesSelectedNodeToolStripMenuItem";
            this.plotAllFeaturesSelectedNodeToolStripMenuItem.Size = new System.Drawing.Size(246, 22);
            this.plotAllFeaturesSelectedNodeToolStripMenuItem.Text = "Plot All Features (Selected Node)";
            this.plotAllFeaturesSelectedNodeToolStripMenuItem.Click += new System.EventHandler(this.MenuItemClickHandlerPlotAllFeaturesSelectedNode);
            // 
            // pCAScatterPlotToolStripMenuItem
            // 
            this.pCAScatterPlotToolStripMenuItem.Name = "pCAScatterPlotToolStripMenuItem";
            this.pCAScatterPlotToolStripMenuItem.Size = new System.Drawing.Size(246, 22);
            this.pCAScatterPlotToolStripMenuItem.Text = "PCA Scatter Plot";
            this.pCAScatterPlotToolStripMenuItem.Click += new System.EventHandler(this.pCAScatterPlotToolStripMenuItem_Click);
            // 
            // analyzeToolStripMenuItem
            // 
            this.analyzeToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.singleLogToolStripMenuItem,
            this.folderToolStripMenuItem,
            this.preprocessBatchifyToolStripMenuItem,
            this.processBatchToolStripMenuItem,
            this.consolidateGraphsToolStripMenuItem});
            this.analyzeToolStripMenuItem.Name = "analyzeToolStripMenuItem";
            this.analyzeToolStripMenuItem.Size = new System.Drawing.Size(60, 20);
            this.analyzeToolStripMenuItem.Text = "Analyze";
            // 
            // singleLogToolStripMenuItem
            // 
            this.singleLogToolStripMenuItem.Name = "singleLogToolStripMenuItem";
            this.singleLogToolStripMenuItem.Size = new System.Drawing.Size(185, 22);
            this.singleLogToolStripMenuItem.Text = "Single Log";
            this.singleLogToolStripMenuItem.Click += new System.EventHandler(this.singleLogToolStripMenuItem_Click);
            // 
            // folderToolStripMenuItem
            // 
            this.folderToolStripMenuItem.Name = "folderToolStripMenuItem";
            this.folderToolStripMenuItem.Size = new System.Drawing.Size(185, 22);
            this.folderToolStripMenuItem.Text = "Folder";
            this.folderToolStripMenuItem.Click += new System.EventHandler(this.folderToolStripMenuItem_Click);
            // 
            // preprocessBatchifyToolStripMenuItem
            // 
            this.preprocessBatchifyToolStripMenuItem.Name = "preprocessBatchifyToolStripMenuItem";
            this.preprocessBatchifyToolStripMenuItem.Size = new System.Drawing.Size(185, 22);
            this.preprocessBatchifyToolStripMenuItem.Text = "Preprocess (Batchify)";
            this.preprocessBatchifyToolStripMenuItem.Click += new System.EventHandler(this.preprocessBatchifyToolStripMenuItem_Click);
            // 
            // processBatchToolStripMenuItem
            // 
            this.processBatchToolStripMenuItem.Name = "processBatchToolStripMenuItem";
            this.processBatchToolStripMenuItem.Size = new System.Drawing.Size(185, 22);
            this.processBatchToolStripMenuItem.Text = "Process Batch";
            this.processBatchToolStripMenuItem.Click += new System.EventHandler(this.processBatchToolStripMenuItem_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.numSelectedLabel);
            this.groupBox1.Controls.Add(this.groupBox6);
            this.groupBox1.Controls.Add(this.groupBox4);
            this.groupBox1.Controls.Add(this.nodeBox);
            this.groupBox1.Location = new System.Drawing.Point(12, 27);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(795, 342);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Nodes";
            // 
            // numSelectedLabel
            // 
            this.numSelectedLabel.AutoSize = true;
            this.numSelectedLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.numSelectedLabel.Location = new System.Drawing.Point(367, 211);
            this.numSelectedLabel.Name = "numSelectedLabel";
            this.numSelectedLabel.Size = new System.Drawing.Size(59, 12);
            this.numSelectedLabel.TabIndex = 18;
            this.numSelectedLabel.Text = "Selected: 0/0";
            // 
            // groupBox6
            // 
            this.groupBox6.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox6.Controls.Add(this.buttonCancelPlot);
            this.groupBox6.Controls.Add(this.labelPlotProgress);
            this.groupBox6.Controls.Add(this.progressBar);
            this.groupBox6.Location = new System.Drawing.Point(584, 224);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(200, 108);
            this.groupBox6.TabIndex = 16;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "Progress";
            // 
            // buttonCancelPlot
            // 
            this.buttonCancelPlot.Location = new System.Drawing.Point(6, 77);
            this.buttonCancelPlot.Name = "buttonCancelPlot";
            this.buttonCancelPlot.Size = new System.Drawing.Size(188, 23);
            this.buttonCancelPlot.TabIndex = 17;
            this.buttonCancelPlot.Text = "Cancel";
            this.buttonCancelPlot.UseVisualStyleBackColor = true;
            this.buttonCancelPlot.Click += new System.EventHandler(this.buttonCancelPlot_Click);
            // 
            // labelPlotProgress
            // 
            this.labelPlotProgress.AutoSize = true;
            this.labelPlotProgress.BackColor = System.Drawing.Color.Transparent;
            this.labelPlotProgress.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelPlotProgress.Location = new System.Drawing.Point(6, 62);
            this.labelPlotProgress.Name = "labelPlotProgress";
            this.labelPlotProgress.Size = new System.Drawing.Size(40, 12);
            this.labelPlotProgress.TabIndex = 16;
            this.labelPlotProgress.Text = "Finished";
            // 
            // progressBar
            // 
            this.progressBar.Location = new System.Drawing.Point(6, 19);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(188, 40);
            this.progressBar.TabIndex = 15;
            // 
            // groupBox4
            // 
            this.groupBox4.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox4.Controls.Add(this.cBoxShowProcesses);
            this.groupBox4.Controls.Add(this.cBoxShowKeys);
            this.groupBox4.Controls.Add(this.textBoxContainFilter);
            this.groupBox4.Controls.Add(this.cBoxShowFiles);
            this.groupBox4.Controls.Add(this.label2);
            this.groupBox4.Controls.Add(this.cBoxShowSockets);
            this.groupBox4.Controls.Add(this.textBoxStartFilter);
            this.groupBox4.Controls.Add(this.cBoxShowGood);
            this.groupBox4.Controls.Add(this.label1);
            this.groupBox4.Controls.Add(this.cBoxShowMalicious);
            this.groupBox4.Controls.Add(this.cBoxShowMisc);
            this.groupBox4.Controls.Add(this.cBoxShowUnknown);
            this.groupBox4.Location = new System.Drawing.Point(6, 224);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(572, 108);
            this.groupBox4.TabIndex = 13;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Filtering Options";
            // 
            // cBoxShowProcesses
            // 
            this.cBoxShowProcesses.AutoSize = true;
            this.cBoxShowProcesses.Location = new System.Drawing.Point(9, 19);
            this.cBoxShowProcesses.Name = "cBoxShowProcesses";
            this.cBoxShowProcesses.Size = new System.Drawing.Size(75, 17);
            this.cBoxShowProcesses.TabIndex = 1;
            this.cBoxShowProcesses.Text = "Processes";
            this.cBoxShowProcesses.UseVisualStyleBackColor = true;
            this.cBoxShowProcesses.CheckedChanged += new System.EventHandler(this.cBoxShowProcesses_CheckedChanged);
            // 
            // cBoxShowKeys
            // 
            this.cBoxShowKeys.AutoSize = true;
            this.cBoxShowKeys.Location = new System.Drawing.Point(92, 19);
            this.cBoxShowKeys.Name = "cBoxShowKeys";
            this.cBoxShowKeys.Size = new System.Drawing.Size(49, 17);
            this.cBoxShowKeys.TabIndex = 2;
            this.cBoxShowKeys.Text = "Keys";
            this.cBoxShowKeys.UseVisualStyleBackColor = true;
            this.cBoxShowKeys.CheckedChanged += new System.EventHandler(this.cBoxShowKeys_CheckedChanged);
            // 
            // textBoxContainFilter
            // 
            this.textBoxContainFilter.Enabled = false;
            this.textBoxContainFilter.Location = new System.Drawing.Point(76, 79);
            this.textBoxContainFilter.Name = "textBoxContainFilter";
            this.textBoxContainFilter.Size = new System.Drawing.Size(484, 20);
            this.textBoxContainFilter.TabIndex = 11;
            // 
            // cBoxShowFiles
            // 
            this.cBoxShowFiles.AutoSize = true;
            this.cBoxShowFiles.Location = new System.Drawing.Point(149, 19);
            this.cBoxShowFiles.Name = "cBoxShowFiles";
            this.cBoxShowFiles.Size = new System.Drawing.Size(47, 17);
            this.cBoxShowFiles.TabIndex = 3;
            this.cBoxShowFiles.Text = "Files";
            this.cBoxShowFiles.UseVisualStyleBackColor = true;
            this.cBoxShowFiles.CheckedChanged += new System.EventHandler(this.cBoxShowFiles_CheckedChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 82);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 13);
            this.label2.TabIndex = 10;
            this.label2.Text = "class filter";
            // 
            // cBoxShowSockets
            // 
            this.cBoxShowSockets.AutoSize = true;
            this.cBoxShowSockets.Location = new System.Drawing.Point(204, 19);
            this.cBoxShowSockets.Name = "cBoxShowSockets";
            this.cBoxShowSockets.Size = new System.Drawing.Size(65, 17);
            this.cBoxShowSockets.TabIndex = 4;
            this.cBoxShowSockets.Text = "Sockets";
            this.cBoxShowSockets.UseVisualStyleBackColor = true;
            this.cBoxShowSockets.CheckedChanged += new System.EventHandler(this.cBoxShowSockets_CheckedChanged);
            // 
            // textBoxStartFilter
            // 
            this.textBoxStartFilter.Enabled = false;
            this.textBoxStartFilter.Location = new System.Drawing.Point(76, 43);
            this.textBoxStartFilter.Name = "textBoxStartFilter";
            this.textBoxStartFilter.Size = new System.Drawing.Size(484, 20);
            this.textBoxStartFilter.TabIndex = 4;
            // 
            // cBoxShowGood
            // 
            this.cBoxShowGood.AutoSize = true;
            this.cBoxShowGood.Checked = true;
            this.cBoxShowGood.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cBoxShowGood.Location = new System.Drawing.Point(277, 19);
            this.cBoxShowGood.Name = "cBoxShowGood";
            this.cBoxShowGood.Size = new System.Drawing.Size(73, 17);
            this.cBoxShowGood.TabIndex = 5;
            this.cBoxShowGood.Text = "goodware";
            this.cBoxShowGood.UseVisualStyleBackColor = true;
            this.cBoxShowGood.CheckedChanged += new System.EventHandler(this.cBoxShowGood_CheckedChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(5, 46);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(49, 13);
            this.label1.TabIndex = 9;
            this.label1.Text = "type filter";
            // 
            // cBoxShowMalicious
            // 
            this.cBoxShowMalicious.AutoSize = true;
            this.cBoxShowMalicious.Checked = true;
            this.cBoxShowMalicious.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cBoxShowMalicious.Location = new System.Drawing.Point(358, 19);
            this.cBoxShowMalicious.Name = "cBoxShowMalicious";
            this.cBoxShowMalicious.Size = new System.Drawing.Size(65, 17);
            this.cBoxShowMalicious.TabIndex = 6;
            this.cBoxShowMalicious.Text = "malware";
            this.cBoxShowMalicious.UseVisualStyleBackColor = true;
            this.cBoxShowMalicious.CheckedChanged += new System.EventHandler(this.cBoxShowMalicious_CheckedChanged);
            // 
            // cBoxShowMisc
            // 
            this.cBoxShowMisc.AutoSize = true;
            this.cBoxShowMisc.Location = new System.Drawing.Point(509, 19);
            this.cBoxShowMisc.Name = "cBoxShowMisc";
            this.cBoxShowMisc.Size = new System.Drawing.Size(50, 17);
            this.cBoxShowMisc.TabIndex = 8;
            this.cBoxShowMisc.Text = "other";
            this.cBoxShowMisc.UseVisualStyleBackColor = true;
            this.cBoxShowMisc.CheckedChanged += new System.EventHandler(this.cBoxShowMisc_CheckedChanged);
            // 
            // cBoxShowUnknown
            // 
            this.cBoxShowUnknown.AutoSize = true;
            this.cBoxShowUnknown.Location = new System.Drawing.Point(431, 19);
            this.cBoxShowUnknown.Name = "cBoxShowUnknown";
            this.cBoxShowUnknown.Size = new System.Drawing.Size(70, 17);
            this.cBoxShowUnknown.TabIndex = 7;
            this.cBoxShowUnknown.Text = "unknown";
            this.cBoxShowUnknown.UseVisualStyleBackColor = true;
            this.cBoxShowUnknown.CheckedChanged += new System.EventHandler(this.cBoxShowUnknown_CheckedChanged);
            // 
            // nodeBox
            // 
            this.nodeBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.nodeBox.FormattingEnabled = true;
            this.nodeBox.HorizontalScrollbar = true;
            this.nodeBox.Location = new System.Drawing.Point(6, 19);
            this.nodeBox.Name = "nodeBox";
            this.nodeBox.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.nodeBox.Size = new System.Drawing.Size(779, 186);
            this.nodeBox.TabIndex = 0;
            this.nodeBox.SelectedIndexChanged += new System.EventHandler(this.nodeBox_SelectedIndexChanged);
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Controls.Add(this.tabPage4);
            this.tabControl1.Controls.Add(this.tabPage6);
            this.tabControl1.Controls.Add(this.tabPage5);
            this.tabControl1.ItemSize = new System.Drawing.Size(95, 20);
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Margin = new System.Windows.Forms.Padding(0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(795, 420);
            this.tabControl1.TabIndex = 7;
            // 
            // tabPage1
            // 
            this.tabPage1.BackColor = System.Drawing.Color.Transparent;
            this.tabPage1.Controls.Add(this.nodeInfoBox);
            this.tabPage1.Controls.Add(this.trackbarValueLabel);
            this.tabPage1.Controls.Add(this.nodeInfoTrackbar);
            this.tabPage1.Controls.Add(this.LabelTrackbarEnd);
            this.tabPage1.Location = new System.Drawing.Point(4, 24);
            this.tabPage1.Margin = new System.Windows.Forms.Padding(0);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Size = new System.Drawing.Size(787, 392);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Raw Data";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // nodeInfoBox
            // 
            this.nodeInfoBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.nodeInfoBox.Location = new System.Drawing.Point(5, 3);
            this.nodeInfoBox.Multiline = true;
            this.nodeInfoBox.Name = "nodeInfoBox";
            this.nodeInfoBox.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.nodeInfoBox.Size = new System.Drawing.Size(775, 319);
            this.nodeInfoBox.TabIndex = 2;
            this.nodeInfoBox.WordWrap = false;
            // 
            // trackbarValueLabel
            // 
            this.trackbarValueLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.trackbarValueLabel.AutoSize = true;
            this.trackbarValueLabel.Location = new System.Drawing.Point(11, 372);
            this.trackbarValueLabel.Name = "trackbarValueLabel";
            this.trackbarValueLabel.Size = new System.Drawing.Size(38, 13);
            this.trackbarValueLabel.TabIndex = 6;
            this.trackbarValueLabel.Text = "Step 0";
            // 
            // nodeInfoTrackbar
            // 
            this.nodeInfoTrackbar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.nodeInfoTrackbar.BackColor = System.Drawing.SystemColors.Window;
            this.nodeInfoTrackbar.Enabled = false;
            this.nodeInfoTrackbar.Location = new System.Drawing.Point(2, 328);
            this.nodeInfoTrackbar.Name = "nodeInfoTrackbar";
            this.nodeInfoTrackbar.Size = new System.Drawing.Size(777, 45);
            this.nodeInfoTrackbar.TabIndex = 3;
            this.nodeInfoTrackbar.Scroll += new System.EventHandler(this.nodeInfoTrackbar_Scroll);
            // 
            // LabelTrackbarEnd
            // 
            this.LabelTrackbarEnd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.LabelTrackbarEnd.AutoSize = true;
            this.LabelTrackbarEnd.Location = new System.Drawing.Point(760, 372);
            this.LabelTrackbarEnd.Name = "LabelTrackbarEnd";
            this.LabelTrackbarEnd.Size = new System.Drawing.Size(19, 13);
            this.LabelTrackbarEnd.TabIndex = 5;
            this.LabelTrackbarEnd.Text = "10";
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.groupBox14);
            this.tabPage2.Controls.Add(this.textBoxCoefficients);
            this.tabPage2.Location = new System.Drawing.Point(4, 24);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(787, 392);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Coefficients";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // groupBox14
            // 
            this.groupBox14.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox14.Controls.Add(this.buttonLoadCoeff);
            this.groupBox14.Controls.Add(this.comboBoxSaveModels);
            this.groupBox14.Controls.Add(this.buttonSaveCoeffs);
            this.groupBox14.Location = new System.Drawing.Point(5, 331);
            this.groupBox14.Name = "groupBox14";
            this.groupBox14.Size = new System.Drawing.Size(472, 48);
            this.groupBox14.TabIndex = 4;
            this.groupBox14.TabStop = false;
            this.groupBox14.Text = "Actions";
            // 
            // buttonLoadCoeff
            // 
            this.buttonLoadCoeff.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonLoadCoeff.Location = new System.Drawing.Point(6, 17);
            this.buttonLoadCoeff.Name = "buttonLoadCoeff";
            this.buttonLoadCoeff.Size = new System.Drawing.Size(167, 23);
            this.buttonLoadCoeff.TabIndex = 1;
            this.buttonLoadCoeff.Text = "Load Coefficients From File";
            this.buttonLoadCoeff.UseVisualStyleBackColor = true;
            this.buttonLoadCoeff.Click += new System.EventHandler(this.buttonLoadCoeff_Click);
            // 
            // comboBoxSaveModels
            // 
            this.comboBoxSaveModels.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.comboBoxSaveModels.FormattingEnabled = true;
            this.comboBoxSaveModels.Location = new System.Drawing.Point(342, 19);
            this.comboBoxSaveModels.Name = "comboBoxSaveModels";
            this.comboBoxSaveModels.Size = new System.Drawing.Size(121, 21);
            this.comboBoxSaveModels.TabIndex = 3;
            this.comboBoxSaveModels.SelectedIndexChanged += new System.EventHandler(this.comboBoxSaveModels_SelectedIndexChanged);
            // 
            // buttonSaveCoeffs
            // 
            this.buttonSaveCoeffs.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonSaveCoeffs.Location = new System.Drawing.Point(179, 17);
            this.buttonSaveCoeffs.Name = "buttonSaveCoeffs";
            this.buttonSaveCoeffs.Size = new System.Drawing.Size(157, 23);
            this.buttonSaveCoeffs.TabIndex = 2;
            this.buttonSaveCoeffs.Text = "Save Coefficients to File";
            this.buttonSaveCoeffs.UseVisualStyleBackColor = true;
            this.buttonSaveCoeffs.Click += new System.EventHandler(this.buttonSaveCoeffs_Click);
            // 
            // textBoxCoefficients
            // 
            this.textBoxCoefficients.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxCoefficients.Location = new System.Drawing.Point(5, 3);
            this.textBoxCoefficients.Multiline = true;
            this.textBoxCoefficients.Name = "textBoxCoefficients";
            this.textBoxCoefficients.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBoxCoefficients.Size = new System.Drawing.Size(775, 322);
            this.textBoxCoefficients.TabIndex = 0;
            this.textBoxCoefficients.WordWrap = false;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.groupBox2);
            this.tabPage3.Controls.Add(this.textBoxModel);
            this.tabPage3.Location = new System.Drawing.Point(4, 24);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Size = new System.Drawing.Size(787, 392);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Model";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.groupBox2.Controls.Add(this.buttonSaveModel);
            this.groupBox2.Controls.Add(this.buttonViewDataTable);
            this.groupBox2.Controls.Add(this.comboBoxModel);
            this.groupBox2.Controls.Add(this.button1);
            this.groupBox2.Location = new System.Drawing.Point(5, 331);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(417, 58);
            this.groupBox2.TabIndex = 5;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Actions";
            // 
            // buttonSaveModel
            // 
            this.buttonSaveModel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonSaveModel.Location = new System.Drawing.Point(6, 21);
            this.buttonSaveModel.Name = "buttonSaveModel";
            this.buttonSaveModel.Size = new System.Drawing.Size(108, 23);
            this.buttonSaveModel.TabIndex = 1;
            this.buttonSaveModel.Text = "Export Model";
            this.buttonSaveModel.UseVisualStyleBackColor = true;
            this.buttonSaveModel.Click += new System.EventHandler(this.buttonSaveModel_Click);
            // 
            // buttonViewDataTable
            // 
            this.buttonViewDataTable.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonViewDataTable.Location = new System.Drawing.Point(328, 21);
            this.buttonViewDataTable.Name = "buttonViewDataTable";
            this.buttonViewDataTable.Size = new System.Drawing.Size(75, 23);
            this.buttonViewDataTable.TabIndex = 4;
            this.buttonViewDataTable.Text = "View Table";
            this.buttonViewDataTable.UseVisualStyleBackColor = true;
            this.buttonViewDataTable.Click += new System.EventHandler(this.buttonViewDataTable_Click);
            // 
            // comboBoxModel
            // 
            this.comboBoxModel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.comboBoxModel.FormattingEnabled = true;
            this.comboBoxModel.Location = new System.Drawing.Point(120, 23);
            this.comboBoxModel.Name = "comboBoxModel";
            this.comboBoxModel.Size = new System.Drawing.Size(121, 21);
            this.comboBoxModel.TabIndex = 2;
            this.comboBoxModel.SelectedIndexChanged += new System.EventHandler(this.comboBoxModel_SelectedIndexChanged);
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.button1.Location = new System.Drawing.Point(247, 21);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 3;
            this.button1.Text = "Configure";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // textBoxModel
            // 
            this.textBoxModel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxModel.Location = new System.Drawing.Point(5, 3);
            this.textBoxModel.Multiline = true;
            this.textBoxModel.Name = "textBoxModel";
            this.textBoxModel.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBoxModel.Size = new System.Drawing.Size(775, 322);
            this.textBoxModel.TabIndex = 0;
            this.textBoxModel.WordWrap = false;
            // 
            // tabPage4
            // 
            this.tabPage4.Controls.Add(this.NodeInfoDataGridView);
            this.tabPage4.Location = new System.Drawing.Point(4, 24);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Size = new System.Drawing.Size(787, 392);
            this.tabPage4.TabIndex = 3;
            this.tabPage4.Text = "Info";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // NodeInfoDataGridView
            // 
            this.NodeInfoDataGridView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.NodeInfoDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.NodeInfoDataGridView.EnableHeadersVisualStyles = false;
            this.NodeInfoDataGridView.Location = new System.Drawing.Point(2, 3);
            this.NodeInfoDataGridView.Name = "NodeInfoDataGridView";
            this.NodeInfoDataGridView.Size = new System.Drawing.Size(785, 386);
            this.NodeInfoDataGridView.TabIndex = 0;
            // 
            // tabPage6
            // 
            this.tabPage6.BackColor = System.Drawing.Color.White;
            this.tabPage6.Controls.Add(this.groupBox15);
            this.tabPage6.Controls.Add(this.groupBox13);
            this.tabPage6.Controls.Add(this.groupBox12);
            this.tabPage6.Location = new System.Drawing.Point(4, 24);
            this.tabPage6.Name = "tabPage6";
            this.tabPage6.Size = new System.Drawing.Size(787, 392);
            this.tabPage6.TabIndex = 5;
            this.tabPage6.Text = "ML / Accord";
            // 
            // groupBox15
            // 
            this.groupBox15.Controls.Add(this.buttonChainConfigure);
            this.groupBox15.Controls.Add(this.buttonChainValidation);
            this.groupBox15.Location = new System.Drawing.Point(12, 128);
            this.groupBox15.Name = "groupBox15";
            this.groupBox15.Size = new System.Drawing.Size(753, 55);
            this.groupBox15.TabIndex = 4;
            this.groupBox15.TabStop = false;
            this.groupBox15.Text = "Model Chain Validator";
            // 
            // buttonChainConfigure
            // 
            this.buttonChainConfigure.Location = new System.Drawing.Point(632, 19);
            this.buttonChainConfigure.Name = "buttonChainConfigure";
            this.buttonChainConfigure.Size = new System.Drawing.Size(112, 23);
            this.buttonChainConfigure.TabIndex = 4;
            this.buttonChainConfigure.Text = "Configure";
            this.buttonChainConfigure.UseVisualStyleBackColor = true;
            this.buttonChainConfigure.Click += new System.EventHandler(this.buttonChainConfigure_Click);
            // 
            // buttonChainValidation
            // 
            this.buttonChainValidation.Location = new System.Drawing.Point(6, 19);
            this.buttonChainValidation.Name = "buttonChainValidation";
            this.buttonChainValidation.Size = new System.Drawing.Size(75, 23);
            this.buttonChainValidation.TabIndex = 3;
            this.buttonChainValidation.Text = "Run";
            this.buttonChainValidation.UseVisualStyleBackColor = true;
            this.buttonChainValidation.Click += new System.EventHandler(this.buttonChainValidation_Click);
            // 
            // groupBox13
            // 
            this.groupBox13.Controls.Add(this.buttonSVMPerformCV);
            this.groupBox13.Controls.Add(this.buttonSVMConfigure);
            this.groupBox13.Controls.Add(this.buttonKSVMTrain);
            this.groupBox13.Location = new System.Drawing.Point(12, 66);
            this.groupBox13.Name = "groupBox13";
            this.groupBox13.Size = new System.Drawing.Size(753, 55);
            this.groupBox13.TabIndex = 3;
            this.groupBox13.TabStop = false;
            this.groupBox13.Text = "Kernel Support Vector Machine";
            // 
            // buttonSVMPerformCV
            // 
            this.buttonSVMPerformCV.Location = new System.Drawing.Point(89, 19);
            this.buttonSVMPerformCV.Name = "buttonSVMPerformCV";
            this.buttonSVMPerformCV.Size = new System.Drawing.Size(105, 23);
            this.buttonSVMPerformCV.TabIndex = 2;
            this.buttonSVMPerformCV.Text = "Cross Validation";
            this.buttonSVMPerformCV.UseVisualStyleBackColor = true;
            this.buttonSVMPerformCV.Click += new System.EventHandler(this.buttonSVMPerformCV_Click);
            // 
            // buttonSVMConfigure
            // 
            this.buttonSVMConfigure.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonSVMConfigure.Location = new System.Drawing.Point(632, 19);
            this.buttonSVMConfigure.Name = "buttonSVMConfigure";
            this.buttonSVMConfigure.Size = new System.Drawing.Size(112, 23);
            this.buttonSVMConfigure.TabIndex = 1;
            this.buttonSVMConfigure.Text = "Configure";
            this.buttonSVMConfigure.UseVisualStyleBackColor = true;
            this.buttonSVMConfigure.Click += new System.EventHandler(this.buttonSVMConfigure_Click);
            // 
            // buttonKSVMTrain
            // 
            this.buttonKSVMTrain.Location = new System.Drawing.Point(6, 19);
            this.buttonKSVMTrain.Name = "buttonKSVMTrain";
            this.buttonKSVMTrain.Size = new System.Drawing.Size(75, 23);
            this.buttonKSVMTrain.TabIndex = 0;
            this.buttonKSVMTrain.Text = "Train/Test";
            this.buttonKSVMTrain.UseVisualStyleBackColor = true;
            this.buttonKSVMTrain.Click += new System.EventHandler(this.buttonKSVMTrain_Click);
            // 
            // groupBox12
            // 
            this.groupBox12.Controls.Add(this.buttonExportPCA);
            this.groupBox12.Controls.Add(this.buttonPCAViewComponents);
            this.groupBox12.Controls.Add(this.buttonPCAViewTransformed);
            this.groupBox12.Controls.Add(this.buttonPCAViewData);
            this.groupBox12.Controls.Add(this.buttonPCA);
            this.groupBox12.Controls.Add(this.buttonConfigurePCA);
            this.groupBox12.Location = new System.Drawing.Point(12, 3);
            this.groupBox12.Name = "groupBox12";
            this.groupBox12.Size = new System.Drawing.Size(753, 57);
            this.groupBox12.TabIndex = 2;
            this.groupBox12.TabStop = false;
            this.groupBox12.Text = "Principle Component Analysis";
            // 
            // buttonExportPCA
            // 
            this.buttonExportPCA.Location = new System.Drawing.Point(460, 19);
            this.buttonExportPCA.Name = "buttonExportPCA";
            this.buttonExportPCA.Size = new System.Drawing.Size(75, 23);
            this.buttonExportPCA.TabIndex = 5;
            this.buttonExportPCA.Text = "Export";
            this.buttonExportPCA.UseVisualStyleBackColor = true;
            this.buttonExportPCA.Click += new System.EventHandler(this.buttonExportPCA_Click);
            // 
            // buttonPCAViewComponents
            // 
            this.buttonPCAViewComponents.Location = new System.Drawing.Point(336, 19);
            this.buttonPCAViewComponents.Name = "buttonPCAViewComponents";
            this.buttonPCAViewComponents.Size = new System.Drawing.Size(116, 23);
            this.buttonPCAViewComponents.TabIndex = 4;
            this.buttonPCAViewComponents.Text = "View Components";
            this.buttonPCAViewComponents.UseVisualStyleBackColor = true;
            this.buttonPCAViewComponents.Click += new System.EventHandler(this.buttonPCAViewComponents_Click);
            // 
            // buttonPCAViewTransformed
            // 
            this.buttonPCAViewTransformed.Location = new System.Drawing.Point(202, 19);
            this.buttonPCAViewTransformed.Name = "buttonPCAViewTransformed";
            this.buttonPCAViewTransformed.Size = new System.Drawing.Size(126, 23);
            this.buttonPCAViewTransformed.TabIndex = 3;
            this.buttonPCAViewTransformed.Text = "View Transformed Data";
            this.buttonPCAViewTransformed.UseVisualStyleBackColor = true;
            this.buttonPCAViewTransformed.Click += new System.EventHandler(this.buttonPCAViewTransformed_Click);
            // 
            // buttonPCAViewData
            // 
            this.buttonPCAViewData.Location = new System.Drawing.Point(89, 19);
            this.buttonPCAViewData.Name = "buttonPCAViewData";
            this.buttonPCAViewData.Size = new System.Drawing.Size(105, 23);
            this.buttonPCAViewData.TabIndex = 2;
            this.buttonPCAViewData.Text = "View Base Data";
            this.buttonPCAViewData.UseVisualStyleBackColor = true;
            this.buttonPCAViewData.Click += new System.EventHandler(this.buttonPCAViewData_Click);
            // 
            // buttonPCA
            // 
            this.buttonPCA.Location = new System.Drawing.Point(6, 19);
            this.buttonPCA.Name = "buttonPCA";
            this.buttonPCA.Size = new System.Drawing.Size(75, 23);
            this.buttonPCA.TabIndex = 0;
            this.buttonPCA.Text = "Run";
            this.buttonPCA.UseVisualStyleBackColor = true;
            this.buttonPCA.Click += new System.EventHandler(this.buttonPCA_Click);
            // 
            // buttonConfigurePCA
            // 
            this.buttonConfigurePCA.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonConfigurePCA.Location = new System.Drawing.Point(632, 19);
            this.buttonConfigurePCA.Name = "buttonConfigurePCA";
            this.buttonConfigurePCA.Size = new System.Drawing.Size(112, 23);
            this.buttonConfigurePCA.TabIndex = 1;
            this.buttonConfigurePCA.Text = "Configure";
            this.buttonConfigurePCA.UseVisualStyleBackColor = true;
            this.buttonConfigurePCA.Click += new System.EventHandler(this.buttonConfigurePCA_Click);
            // 
            // tabPage5
            // 
            this.tabPage5.Controls.Add(this.groupBox11);
            this.tabPage5.Controls.Add(this.groupBox7);
            this.tabPage5.Location = new System.Drawing.Point(4, 24);
            this.tabPage5.Name = "tabPage5";
            this.tabPage5.Size = new System.Drawing.Size(787, 392);
            this.tabPage5.TabIndex = 4;
            this.tabPage5.Text = "Settings";
            this.tabPage5.UseVisualStyleBackColor = true;
            // 
            // groupBox11
            // 
            this.groupBox11.Controls.Add(this.groupBox10);
            this.groupBox11.Controls.Add(this.groupBox8);
            this.groupBox11.Controls.Add(this.groupBox9);
            this.groupBox11.ForeColor = System.Drawing.SystemColors.ControlText;
            this.groupBox11.Location = new System.Drawing.Point(3, 3);
            this.groupBox11.Name = "groupBox11";
            this.groupBox11.Size = new System.Drawing.Size(703, 144);
            this.groupBox11.TabIndex = 20;
            this.groupBox11.TabStop = false;
            this.groupBox11.Text = "QDFG Provider Settings";
            // 
            // groupBox10
            // 
            this.groupBox10.Controls.Add(this.tbSettingsMemoryLimit);
            this.groupBox10.Controls.Add(this.label11);
            this.groupBox10.Controls.Add(this.label13);
            this.groupBox10.Controls.Add(this.label12);
            this.groupBox10.Controls.Add(this.tbSettingsNumberFormat);
            this.groupBox10.Controls.Add(this.label8);
            this.groupBox10.Controls.Add(this.tbSettingsMissingValueIdentifier);
            this.groupBox10.ForeColor = System.Drawing.SystemColors.ControlText;
            this.groupBox10.Location = new System.Drawing.Point(6, 19);
            this.groupBox10.Name = "groupBox10";
            this.groupBox10.Size = new System.Drawing.Size(215, 115);
            this.groupBox10.TabIndex = 19;
            this.groupBox10.TabStop = false;
            this.groupBox10.Text = "General";
            // 
            // tbSettingsMemoryLimit
            // 
            this.tbSettingsMemoryLimit.Location = new System.Drawing.Point(106, 19);
            this.tbSettingsMemoryLimit.Name = "tbSettingsMemoryLimit";
            this.tbSettingsMemoryLimit.ReadOnly = true;
            this.tbSettingsMemoryLimit.Size = new System.Drawing.Size(74, 20);
            this.tbSettingsMemoryLimit.TabIndex = 14;
            this.tbSettingsMemoryLimit.Click += new System.EventHandler(this.tbSettingsMemoryLimit_Click);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.ForeColor = System.Drawing.SystemColors.Desktop;
            this.label11.Location = new System.Drawing.Point(12, 22);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(68, 13);
            this.label11.TabIndex = 13;
            this.label11.Text = "Memory Limit";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.ForeColor = System.Drawing.SystemColors.Desktop;
            this.label13.Location = new System.Drawing.Point(12, 48);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(67, 13);
            this.label13.TabIndex = 17;
            this.label13.Text = "Num. Format";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(183, 22);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(21, 13);
            this.label12.TabIndex = 15;
            this.label12.Text = "mb";
            // 
            // tbSettingsNumberFormat
            // 
            this.tbSettingsNumberFormat.Location = new System.Drawing.Point(106, 45);
            this.tbSettingsNumberFormat.Name = "tbSettingsNumberFormat";
            this.tbSettingsNumberFormat.ReadOnly = true;
            this.tbSettingsNumberFormat.Size = new System.Drawing.Size(94, 20);
            this.tbSettingsNumberFormat.TabIndex = 16;
            this.tbSettingsNumberFormat.Click += new System.EventHandler(this.tbSettingsNumberFormat_Click);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.ForeColor = System.Drawing.SystemColors.Desktop;
            this.label8.Location = new System.Drawing.Point(13, 74);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(72, 26);
            this.label8.TabIndex = 9;
            this.label8.Text = "Missing Value\r\nIdentifier";
            // 
            // tbSettingsMissingValueIdentifier
            // 
            this.tbSettingsMissingValueIdentifier.Location = new System.Drawing.Point(107, 71);
            this.tbSettingsMissingValueIdentifier.Name = "tbSettingsMissingValueIdentifier";
            this.tbSettingsMissingValueIdentifier.ReadOnly = true;
            this.tbSettingsMissingValueIdentifier.Size = new System.Drawing.Size(93, 20);
            this.tbSettingsMissingValueIdentifier.TabIndex = 8;
            this.tbSettingsMissingValueIdentifier.Click += new System.EventHandler(this.tbSettingsMissingValueIdentifier_Click);
            // 
            // groupBox8
            // 
            this.groupBox8.Controls.Add(this.label5);
            this.groupBox8.Controls.Add(this.tbSettingsNumSamples);
            this.groupBox8.Controls.Add(this.label4);
            this.groupBox8.Controls.Add(this.tbSettingsMinSamples);
            this.groupBox8.Controls.Add(this.label9);
            this.groupBox8.Controls.Add(this.tbSettingsTimeStep);
            this.groupBox8.Controls.Add(this.label10);
            this.groupBox8.ForeColor = System.Drawing.SystemColors.ControlText;
            this.groupBox8.Location = new System.Drawing.Point(227, 19);
            this.groupBox8.Name = "groupBox8";
            this.groupBox8.Size = new System.Drawing.Size(214, 115);
            this.groupBox8.TabIndex = 16;
            this.groupBox8.TabStop = false;
            this.groupBox8.Text = "Sampling";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.ForeColor = System.Drawing.SystemColors.Desktop;
            this.label5.Location = new System.Drawing.Point(6, 51);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(57, 13);
            this.label5.TabIndex = 3;
            this.label5.Text = "# Samples";
            // 
            // tbSettingsNumSamples
            // 
            this.tbSettingsNumSamples.Location = new System.Drawing.Point(100, 48);
            this.tbSettingsNumSamples.Name = "tbSettingsNumSamples";
            this.tbSettingsNumSamples.ReadOnly = true;
            this.tbSettingsNumSamples.Size = new System.Drawing.Size(102, 20);
            this.tbSettingsNumSamples.TabIndex = 2;
            this.tbSettingsNumSamples.Click += new System.EventHandler(this.tbSettingsNumSamples_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.ForeColor = System.Drawing.SystemColors.Desktop;
            this.label4.Location = new System.Drawing.Point(6, 25);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(67, 13);
            this.label4.TabIndex = 1;
            this.label4.Text = "Min Samples";
            // 
            // tbSettingsMinSamples
            // 
            this.tbSettingsMinSamples.Location = new System.Drawing.Point(100, 22);
            this.tbSettingsMinSamples.Name = "tbSettingsMinSamples";
            this.tbSettingsMinSamples.ReadOnly = true;
            this.tbSettingsMinSamples.Size = new System.Drawing.Size(102, 20);
            this.tbSettingsMinSamples.TabIndex = 0;
            this.tbSettingsMinSamples.Click += new System.EventHandler(this.tbSettingsMinSamples_Click);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.ForeColor = System.Drawing.SystemColors.Desktop;
            this.label9.Location = new System.Drawing.Point(6, 77);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(55, 13);
            this.label9.TabIndex = 11;
            this.label9.Text = "Time Step";
            // 
            // tbSettingsTimeStep
            // 
            this.tbSettingsTimeStep.Location = new System.Drawing.Point(100, 74);
            this.tbSettingsTimeStep.Name = "tbSettingsTimeStep";
            this.tbSettingsTimeStep.ReadOnly = true;
            this.tbSettingsTimeStep.Size = new System.Drawing.Size(74, 20);
            this.tbSettingsTimeStep.TabIndex = 10;
            this.tbSettingsTimeStep.Click += new System.EventHandler(this.tbSettingsTimeStep_Click);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(179, 77);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(20, 13);
            this.label10.TabIndex = 12;
            this.label10.Text = "ms";
            // 
            // groupBox9
            // 
            this.groupBox9.Controls.Add(this.label7);
            this.groupBox9.Controls.Add(this.cbSettingsDataModel);
            this.groupBox9.Controls.Add(this.cbSettingsTimeModel);
            this.groupBox9.Controls.Add(this.label6);
            this.groupBox9.ForeColor = System.Drawing.SystemColors.ControlText;
            this.groupBox9.Location = new System.Drawing.Point(447, 19);
            this.groupBox9.Name = "groupBox9";
            this.groupBox9.Size = new System.Drawing.Size(243, 76);
            this.groupBox9.TabIndex = 18;
            this.groupBox9.TabStop = false;
            this.groupBox9.Text = "Model";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.ForeColor = System.Drawing.SystemColors.Desktop;
            this.label7.Location = new System.Drawing.Point(10, 49);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(62, 13);
            this.label7.TabIndex = 7;
            this.label7.Text = "Data Model";
            // 
            // cbSettingsDataModel
            // 
            this.cbSettingsDataModel.FormattingEnabled = true;
            this.cbSettingsDataModel.Location = new System.Drawing.Point(104, 46);
            this.cbSettingsDataModel.Name = "cbSettingsDataModel";
            this.cbSettingsDataModel.Size = new System.Drawing.Size(130, 21);
            this.cbSettingsDataModel.TabIndex = 6;
            this.cbSettingsDataModel.TextChanged += new System.EventHandler(this.cbSettingsDataModel_TextChanged);
            // 
            // cbSettingsTimeModel
            // 
            this.cbSettingsTimeModel.FormattingEnabled = true;
            this.cbSettingsTimeModel.Location = new System.Drawing.Point(104, 19);
            this.cbSettingsTimeModel.Name = "cbSettingsTimeModel";
            this.cbSettingsTimeModel.Size = new System.Drawing.Size(130, 21);
            this.cbSettingsTimeModel.TabIndex = 4;
            this.cbSettingsTimeModel.TextChanged += new System.EventHandler(this.cbSettingsTimeModel_TextChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.ForeColor = System.Drawing.SystemColors.Desktop;
            this.label6.Location = new System.Drawing.Point(10, 22);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(62, 13);
            this.label6.TabIndex = 5;
            this.label6.Text = "Time Model";
            // 
            // groupBox7
            // 
            this.groupBox7.Controls.Add(this.groupBox3);
            this.groupBox7.Controls.Add(this.groupBox5);
            this.groupBox7.Location = new System.Drawing.Point(3, 153);
            this.groupBox7.Name = "groupBox7";
            this.groupBox7.Size = new System.Drawing.Size(451, 126);
            this.groupBox7.TabIndex = 15;
            this.groupBox7.TabStop = false;
            this.groupBox7.Text = "Plot Settings";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.checkBox3);
            this.groupBox3.Controls.Add(this.checkBox2);
            this.groupBox3.Controls.Add(this.checkBox1);
            this.groupBox3.ForeColor = System.Drawing.SystemColors.ControlText;
            this.groupBox3.Location = new System.Drawing.Point(6, 19);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(215, 95);
            this.groupBox3.TabIndex = 12;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "General Plot Options";
            // 
            // checkBox3
            // 
            this.checkBox3.AutoSize = true;
            this.checkBox3.ForeColor = System.Drawing.SystemColors.Desktop;
            this.checkBox3.Location = new System.Drawing.Point(7, 66);
            this.checkBox3.Name = "checkBox3";
            this.checkBox3.Size = new System.Drawing.Size(102, 17);
            this.checkBox3.TabIndex = 2;
            this.checkBox3.Text = "Plot Data Points";
            this.checkBox3.UseVisualStyleBackColor = true;
            this.checkBox3.CheckedChanged += new System.EventHandler(this.checkBox3_CheckedChanged);
            // 
            // checkBox2
            // 
            this.checkBox2.AutoSize = true;
            this.checkBox2.ForeColor = System.Drawing.SystemColors.Desktop;
            this.checkBox2.Location = new System.Drawing.Point(7, 43);
            this.checkBox2.Name = "checkBox2";
            this.checkBox2.Size = new System.Drawing.Size(88, 17);
            this.checkBox2.TabIndex = 1;
            this.checkBox2.Text = "Save Figures";
            this.checkBox2.UseVisualStyleBackColor = true;
            this.checkBox2.CheckedChanged += new System.EventHandler(this.checkBox2_CheckedChanged);
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.ForeColor = System.Drawing.SystemColors.Desktop;
            this.checkBox1.Location = new System.Drawing.Point(6, 19);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(111, 17);
            this.checkBox1.TabIndex = 0;
            this.checkBox1.Text = "Hide Plot Window";
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.label3);
            this.groupBox5.Controls.Add(this.comboBoxFitModels);
            this.groupBox5.Controls.Add(this.checkBox4);
            this.groupBox5.ForeColor = System.Drawing.SystemColors.ControlText;
            this.groupBox5.Location = new System.Drawing.Point(227, 19);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(214, 72);
            this.groupBox5.TabIndex = 14;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Curve Fitting Options";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.ForeColor = System.Drawing.SystemColors.Desktop;
            this.label3.Location = new System.Drawing.Point(133, 45);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(36, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Model";
            // 
            // comboBoxFitModels
            // 
            this.comboBoxFitModels.FormattingEnabled = true;
            this.comboBoxFitModels.Location = new System.Drawing.Point(6, 42);
            this.comboBoxFitModels.Name = "comboBoxFitModels";
            this.comboBoxFitModels.Size = new System.Drawing.Size(121, 21);
            this.comboBoxFitModels.TabIndex = 4;
            this.comboBoxFitModels.SelectedIndexChanged += new System.EventHandler(this.comboBoxFitModels_SelectedIndexChanged);
            // 
            // checkBox4
            // 
            this.checkBox4.AutoSize = true;
            this.checkBox4.ForeColor = System.Drawing.SystemColors.Desktop;
            this.checkBox4.Location = new System.Drawing.Point(6, 19);
            this.checkBox4.Name = "checkBox4";
            this.checkBox4.Size = new System.Drawing.Size(107, 17);
            this.checkBox4.TabIndex = 3;
            this.checkBox4.Text = "Use Curve Fitting";
            this.checkBox4.UseVisualStyleBackColor = true;
            this.checkBox4.CheckedChanged += new System.EventHandler(this.checkBox4_CheckedChanged);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel});
            this.statusStrip1.Location = new System.Drawing.Point(0, 807);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(819, 22);
            this.statusStrip1.TabIndex = 4;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel
            // 
            this.toolStripStatusLabel.Name = "toolStripStatusLabel";
            this.toolStripStatusLabel.Size = new System.Drawing.Size(42, 17);
            this.toolStripStatusLabel.Text = "Ready.";
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.Controls.Add(this.tabControl1);
            this.panel1.Location = new System.Drawing.Point(12, 375);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(795, 420);
            this.panel1.TabIndex = 5;
            // 
            // consolidateGraphsToolStripMenuItem
            // 
            this.consolidateGraphsToolStripMenuItem.Name = "consolidateGraphsToolStripMenuItem";
            this.consolidateGraphsToolStripMenuItem.Size = new System.Drawing.Size(185, 22);
            this.consolidateGraphsToolStripMenuItem.Text = "Consolidate Graphs";
            this.consolidateGraphsToolStripMenuItem.Click += new System.EventHandler(this.consolidateGraphsToolStripMenuItem_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(819, 829);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Form1";
            this.Text = "Dynamic QDFG Statistics";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox6.ResumeLayout(false);
            this.groupBox6.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nodeInfoTrackbar)).EndInit();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.groupBox14.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            this.tabPage3.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.tabPage4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.NodeInfoDataGridView)).EndInit();
            this.tabPage6.ResumeLayout(false);
            this.groupBox15.ResumeLayout(false);
            this.groupBox13.ResumeLayout(false);
            this.groupBox12.ResumeLayout(false);
            this.tabPage5.ResumeLayout(false);
            this.groupBox11.ResumeLayout(false);
            this.groupBox10.ResumeLayout(false);
            this.groupBox10.PerformLayout();
            this.groupBox8.ResumeLayout(false);
            this.groupBox8.PerformLayout();
            this.groupBox9.ResumeLayout(false);
            this.groupBox9.PerformLayout();
            this.groupBox7.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ListBox nodeBox;
        private System.Windows.Forms.CheckBox cBoxShowUnknown;
        private System.Windows.Forms.CheckBox cBoxShowMalicious;
        private System.Windows.Forms.CheckBox cBoxShowGood;
        private System.Windows.Forms.CheckBox cBoxShowSockets;
        private System.Windows.Forms.CheckBox cBoxShowFiles;
        private System.Windows.Forms.CheckBox cBoxShowKeys;
        private System.Windows.Forms.CheckBox cBoxShowProcesses;
        private System.Windows.Forms.CheckBox cBoxShowMisc;
        private System.Windows.Forms.TextBox textBoxContainFilter;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBoxStartFilter;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ToolStripMenuItem plotToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem plotFeatureSelectedItemToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem plotFeatureAllNodeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem plotAllAllNodesToolStripMenuItem;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.Label labelPlotProgress;
        private System.Windows.Forms.Button buttonCancelPlot;
        private System.Windows.Forms.ToolStripMenuItem plotAllFeaturesSelectedNodeToolStripMenuItem;
        private System.Windows.Forms.Label numSelectedLabel;
        private System.Windows.Forms.ToolStripMenuItem analyzeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem singleLogToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem folderToolStripMenuItem;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TextBox nodeInfoBox;
        private System.Windows.Forms.Label trackbarValueLabel;
        private System.Windows.Forms.TrackBar nodeInfoTrackbar;
        private System.Windows.Forms.Label LabelTrackbarEnd;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.ComboBox comboBoxSaveModels;
        private System.Windows.Forms.Button buttonSaveCoeffs;
        private System.Windows.Forms.Button buttonLoadCoeff;
        private System.Windows.Forms.TextBox textBoxCoefficients;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.ComboBox comboBoxModel;
        private System.Windows.Forms.Button buttonSaveModel;
        private System.Windows.Forms.TextBox textBoxModel;
        private System.Windows.Forms.TabPage tabPage4;
        private System.Windows.Forms.DataGridView NodeInfoDataGridView;
        private System.Windows.Forms.TabPage tabPage5;
        private System.Windows.Forms.GroupBox groupBox11;
        private System.Windows.Forms.GroupBox groupBox10;
        private System.Windows.Forms.TextBox tbSettingsMemoryLimit;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox tbSettingsNumberFormat;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox tbSettingsMissingValueIdentifier;
        private System.Windows.Forms.GroupBox groupBox8;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox tbSettingsNumSamples;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox tbSettingsMinSamples;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox tbSettingsTimeStep;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.GroupBox groupBox9;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox cbSettingsDataModel;
        private System.Windows.Forms.ComboBox cbSettingsTimeModel;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.GroupBox groupBox7;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.CheckBox checkBox3;
        private System.Windows.Forms.CheckBox checkBox2;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox comboBoxFitModels;
        private System.Windows.Forms.CheckBox checkBox4;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel;
        private System.Windows.Forms.TabPage tabPage6;
        private System.Windows.Forms.ToolStripMenuItem pCAScatterPlotToolStripMenuItem;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button buttonPCA;
        private System.Windows.Forms.Button buttonViewDataTable;
        private System.Windows.Forms.GroupBox groupBox13;
        private System.Windows.Forms.Button buttonSVMConfigure;
        private System.Windows.Forms.Button buttonKSVMTrain;
        private System.Windows.Forms.GroupBox groupBox12;
        private System.Windows.Forms.Button buttonConfigurePCA;
        private System.Windows.Forms.GroupBox groupBox14;
        private System.Windows.Forms.Button buttonPCAViewTransformed;
        private System.Windows.Forms.Button buttonPCAViewData;
        private System.Windows.Forms.Button buttonPCAViewComponents;
        private System.Windows.Forms.Button buttonExportPCA;
        private System.Windows.Forms.Button buttonSVMPerformCV;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox15;
        private System.Windows.Forms.Button buttonChainConfigure;
        private System.Windows.Forms.Button buttonChainValidation;
        private System.Windows.Forms.ToolStripMenuItem preprocessBatchifyToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem processBatchToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem consolidateGraphsToolStripMenuItem;
    }
}


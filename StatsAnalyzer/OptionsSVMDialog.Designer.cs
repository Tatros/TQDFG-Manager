namespace StatsAnalyzer
{
    partial class OptionsSVMDialog
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
            this.groupKernel = new System.Windows.Forms.GroupBox();
            this.groupSigmoidKernel = new System.Windows.Forms.GroupBox();
            this.label7 = new System.Windows.Forms.Label();
            this.tbSigmoidConst = new System.Windows.Forms.NumericUpDown();
            this.tbSigmoidAlpha = new System.Windows.Forms.NumericUpDown();
            this.label6 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.groupLaplacianKernel = new System.Windows.Forms.GroupBox();
            this.tbLaplacianSigma = new System.Windows.Forms.NumericUpDown();
            this.label5 = new System.Windows.Forms.Label();
            this.cbKernel = new System.Windows.Forms.ComboBox();
            this.groupGaussianKernel = new System.Windows.Forms.GroupBox();
            this.tbGaussianSigma = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.groupPolyKernel = new System.Windows.Forms.GroupBox();
            this.tbPolyConstant = new System.Windows.Forms.NumericUpDown();
            this.tbPolyDegree = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.lbActiveFeatures = new System.Windows.Forms.ListBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lbIgnoredFeatures = new System.Windows.Forms.ListBox();
            this.buttonMarkIgnored = new System.Windows.Forms.Button();
            this.buttonMarkActive = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.buttonEstimate = new System.Windows.Forms.Button();
            this.buttonSave = new System.Windows.Forms.Button();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.buttonSearchFilter = new System.Windows.Forms.Button();
            this.tbFilterExpression = new System.Windows.Forms.TextBox();
            this.buttonFilterSelect = new System.Windows.Forms.Button();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.groupBox7 = new System.Windows.Forms.GroupBox();
            this.checkClassProportions = new System.Windows.Forms.CheckBox();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.tbWeightsPositive = new System.Windows.Forms.NumericUpDown();
            this.tbWeightsNegative = new System.Windows.Forms.NumericUpDown();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.checkUseHeuristics = new System.Windows.Forms.CheckBox();
            this.label8 = new System.Windows.Forms.Label();
            this.tbComplexity = new System.Windows.Forms.NumericUpDown();
            this.tbTolerance = new System.Windows.Forms.NumericUpDown();
            this.label9 = new System.Windows.Forms.Label();
            this.groupBox8 = new System.Windows.Forms.GroupBox();
            this.labelRuns = new System.Windows.Forms.Label();
            this.tbNumRuns = new System.Windows.Forms.NumericUpDown();
            this.tbNumFolds = new System.Windows.Forms.NumericUpDown();
            this.label12 = new System.Windows.Forms.Label();
            this.groupBox9 = new System.Windows.Forms.GroupBox();
            this.optInt2 = new System.Windows.Forms.NumericUpDown();
            this.optInt1 = new System.Windows.Forms.NumericUpDown();
            this.optDouble2 = new System.Windows.Forms.NumericUpDown();
            this.optDouble1 = new System.Windows.Forms.NumericUpDown();
            this.tbExpression = new System.Windows.Forms.TextBox();
            this.groupKernel.SuspendLayout();
            this.groupSigmoidKernel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tbSigmoidConst)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbSigmoidAlpha)).BeginInit();
            this.groupLaplacianKernel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tbLaplacianSigma)).BeginInit();
            this.groupGaussianKernel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tbGaussianSigma)).BeginInit();
            this.groupPolyKernel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tbPolyConstant)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbPolyDegree)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.groupBox7.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tbWeightsPositive)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbWeightsNegative)).BeginInit();
            this.groupBox6.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tbComplexity)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbTolerance)).BeginInit();
            this.groupBox8.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tbNumRuns)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbNumFolds)).BeginInit();
            this.groupBox9.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.optInt2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.optInt1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.optDouble2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.optDouble1)).BeginInit();
            this.SuspendLayout();
            // 
            // groupKernel
            // 
            this.groupKernel.Controls.Add(this.groupSigmoidKernel);
            this.groupKernel.Controls.Add(this.label1);
            this.groupKernel.Controls.Add(this.groupLaplacianKernel);
            this.groupKernel.Controls.Add(this.cbKernel);
            this.groupKernel.Controls.Add(this.groupGaussianKernel);
            this.groupKernel.Controls.Add(this.groupPolyKernel);
            this.groupKernel.Location = new System.Drawing.Point(12, 13);
            this.groupKernel.Name = "groupKernel";
            this.groupKernel.Size = new System.Drawing.Size(245, 335);
            this.groupKernel.TabIndex = 0;
            this.groupKernel.TabStop = false;
            this.groupKernel.Text = "Kernel Configuration";
            // 
            // groupSigmoidKernel
            // 
            this.groupSigmoidKernel.Controls.Add(this.label7);
            this.groupSigmoidKernel.Controls.Add(this.tbSigmoidConst);
            this.groupSigmoidKernel.Controls.Add(this.tbSigmoidAlpha);
            this.groupSigmoidKernel.Controls.Add(this.label6);
            this.groupSigmoidKernel.Location = new System.Drawing.Point(13, 246);
            this.groupSigmoidKernel.Name = "groupSigmoidKernel";
            this.groupSigmoidKernel.Size = new System.Drawing.Size(222, 78);
            this.groupSigmoidKernel.TabIndex = 5;
            this.groupSigmoidKernel.TabStop = false;
            this.groupSigmoidKernel.Text = "Sigmoid Kernel";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(6, 49);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(49, 13);
            this.label7.TabIndex = 4;
            this.label7.Text = "Constant";
            // 
            // tbSigmoidConst
            // 
            this.tbSigmoidConst.DecimalPlaces = 7;
            this.tbSigmoidConst.Location = new System.Drawing.Point(102, 47);
            this.tbSigmoidConst.Minimum = new decimal(new int[] {
            100,
            0,
            0,
            -2147483648});
            this.tbSigmoidConst.Name = "tbSigmoidConst";
            this.tbSigmoidConst.Size = new System.Drawing.Size(105, 20);
            this.tbSigmoidConst.TabIndex = 3;
            // 
            // tbSigmoidAlpha
            // 
            this.tbSigmoidAlpha.DecimalPlaces = 7;
            this.tbSigmoidAlpha.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.tbSigmoidAlpha.Location = new System.Drawing.Point(102, 21);
            this.tbSigmoidAlpha.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.tbSigmoidAlpha.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            458752});
            this.tbSigmoidAlpha.Name = "tbSigmoidAlpha";
            this.tbSigmoidAlpha.Size = new System.Drawing.Size(105, 20);
            this.tbSigmoidAlpha.TabIndex = 2;
            this.tbSigmoidAlpha.Value = new decimal(new int[] {
            1,
            0,
            0,
            458752});
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(6, 23);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(34, 13);
            this.label6.TabIndex = 1;
            this.label6.Text = "Alpha";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(40, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(37, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Kernel";
            // 
            // groupLaplacianKernel
            // 
            this.groupLaplacianKernel.Controls.Add(this.tbLaplacianSigma);
            this.groupLaplacianKernel.Controls.Add(this.label5);
            this.groupLaplacianKernel.Location = new System.Drawing.Point(13, 191);
            this.groupLaplacianKernel.Name = "groupLaplacianKernel";
            this.groupLaplacianKernel.Size = new System.Drawing.Size(222, 49);
            this.groupLaplacianKernel.TabIndex = 4;
            this.groupLaplacianKernel.TabStop = false;
            this.groupLaplacianKernel.Text = "Laplacian Kernel";
            // 
            // tbLaplacianSigma
            // 
            this.tbLaplacianSigma.DecimalPlaces = 7;
            this.tbLaplacianSigma.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.tbLaplacianSigma.Location = new System.Drawing.Point(102, 20);
            this.tbLaplacianSigma.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.tbLaplacianSigma.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            458752});
            this.tbLaplacianSigma.Name = "tbLaplacianSigma";
            this.tbLaplacianSigma.Size = new System.Drawing.Size(105, 20);
            this.tbLaplacianSigma.TabIndex = 2;
            this.tbLaplacianSigma.Value = new decimal(new int[] {
            1,
            0,
            0,
            458752});
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 22);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(36, 13);
            this.label5.TabIndex = 1;
            this.label5.Text = "Sigma";
            // 
            // cbKernel
            // 
            this.cbKernel.FormattingEnabled = true;
            this.cbKernel.Location = new System.Drawing.Point(83, 22);
            this.cbKernel.Name = "cbKernel";
            this.cbKernel.Size = new System.Drawing.Size(137, 21);
            this.cbKernel.TabIndex = 0;
            this.cbKernel.SelectedIndexChanged += new System.EventHandler(this.cbKernel_SelectedIndexChanged);
            // 
            // groupGaussianKernel
            // 
            this.groupGaussianKernel.Controls.Add(this.tbGaussianSigma);
            this.groupGaussianKernel.Controls.Add(this.label2);
            this.groupGaussianKernel.Location = new System.Drawing.Point(13, 49);
            this.groupGaussianKernel.Name = "groupGaussianKernel";
            this.groupGaussianKernel.Size = new System.Drawing.Size(221, 51);
            this.groupGaussianKernel.TabIndex = 3;
            this.groupGaussianKernel.TabStop = false;
            this.groupGaussianKernel.Text = "Gaussian Kernel";
            // 
            // tbGaussianSigma
            // 
            this.tbGaussianSigma.DecimalPlaces = 7;
            this.tbGaussianSigma.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.tbGaussianSigma.Location = new System.Drawing.Point(102, 20);
            this.tbGaussianSigma.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.tbGaussianSigma.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            458752});
            this.tbGaussianSigma.Name = "tbGaussianSigma";
            this.tbGaussianSigma.Size = new System.Drawing.Size(105, 20);
            this.tbGaussianSigma.TabIndex = 4;
            this.tbGaussianSigma.Value = new decimal(new int[] {
            1,
            0,
            0,
            458752});
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 22);
            this.label2.Name = "label2";
            this.label2.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.label2.Size = new System.Drawing.Size(36, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Sigma";
            // 
            // groupPolyKernel
            // 
            this.groupPolyKernel.Controls.Add(this.tbPolyConstant);
            this.groupPolyKernel.Controls.Add(this.tbPolyDegree);
            this.groupPolyKernel.Controls.Add(this.label4);
            this.groupPolyKernel.Controls.Add(this.label3);
            this.groupPolyKernel.Location = new System.Drawing.Point(13, 106);
            this.groupPolyKernel.Name = "groupPolyKernel";
            this.groupPolyKernel.Size = new System.Drawing.Size(222, 79);
            this.groupPolyKernel.TabIndex = 2;
            this.groupPolyKernel.TabStop = false;
            this.groupPolyKernel.Text = "Polynomial Kernel";
            // 
            // tbPolyConstant
            // 
            this.tbPolyConstant.DecimalPlaces = 7;
            this.tbPolyConstant.Location = new System.Drawing.Point(102, 43);
            this.tbPolyConstant.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.tbPolyConstant.Name = "tbPolyConstant";
            this.tbPolyConstant.Size = new System.Drawing.Size(105, 20);
            this.tbPolyConstant.TabIndex = 9;
            // 
            // tbPolyDegree
            // 
            this.tbPolyDegree.Location = new System.Drawing.Point(102, 17);
            this.tbPolyDegree.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.tbPolyDegree.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.tbPolyDegree.Name = "tbPolyDegree";
            this.tbPolyDegree.Size = new System.Drawing.Size(105, 20);
            this.tbPolyDegree.TabIndex = 8;
            this.tbPolyDegree.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 45);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(49, 13);
            this.label4.TabIndex = 7;
            this.label4.Text = "Constant";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 19);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(42, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Degree";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.lbActiveFeatures);
            this.groupBox2.Location = new System.Drawing.Point(263, 13);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(335, 637);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Active Features";
            // 
            // lbActiveFeatures
            // 
            this.lbActiveFeatures.FormattingEnabled = true;
            this.lbActiveFeatures.HorizontalScrollbar = true;
            this.lbActiveFeatures.Location = new System.Drawing.Point(7, 20);
            this.lbActiveFeatures.Name = "lbActiveFeatures";
            this.lbActiveFeatures.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.lbActiveFeatures.Size = new System.Drawing.Size(322, 602);
            this.lbActiveFeatures.TabIndex = 0;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.lbIgnoredFeatures);
            this.groupBox1.Location = new System.Drawing.Point(666, 14);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(338, 637);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Ignored Features";
            // 
            // lbIgnoredFeatures
            // 
            this.lbIgnoredFeatures.FormattingEnabled = true;
            this.lbIgnoredFeatures.Location = new System.Drawing.Point(7, 20);
            this.lbIgnoredFeatures.Name = "lbIgnoredFeatures";
            this.lbIgnoredFeatures.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.lbIgnoredFeatures.Size = new System.Drawing.Size(322, 602);
            this.lbIgnoredFeatures.TabIndex = 0;
            // 
            // buttonMarkIgnored
            // 
            this.buttonMarkIgnored.Location = new System.Drawing.Point(604, 251);
            this.buttonMarkIgnored.Name = "buttonMarkIgnored";
            this.buttonMarkIgnored.Size = new System.Drawing.Size(56, 23);
            this.buttonMarkIgnored.TabIndex = 3;
            this.buttonMarkIgnored.Text = ">>";
            this.buttonMarkIgnored.UseVisualStyleBackColor = true;
            this.buttonMarkIgnored.Click += new System.EventHandler(this.buttonMarkIgnored_Click);
            // 
            // buttonMarkActive
            // 
            this.buttonMarkActive.Location = new System.Drawing.Point(604, 280);
            this.buttonMarkActive.Name = "buttonMarkActive";
            this.buttonMarkActive.Size = new System.Drawing.Size(56, 23);
            this.buttonMarkActive.TabIndex = 4;
            this.buttonMarkActive.Text = "<<";
            this.buttonMarkActive.UseVisualStyleBackColor = true;
            this.buttonMarkActive.Click += new System.EventHandler(this.buttonMarkActive_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.groupBox3.Controls.Add(this.buttonEstimate);
            this.groupBox3.Controls.Add(this.buttonSave);
            this.groupBox3.Location = new System.Drawing.Point(827, 710);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(177, 45);
            this.groupBox3.TabIndex = 5;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Actions";
            // 
            // buttonEstimate
            // 
            this.buttonEstimate.Location = new System.Drawing.Point(6, 13);
            this.buttonEstimate.Name = "buttonEstimate";
            this.buttonEstimate.Size = new System.Drawing.Size(81, 23);
            this.buttonEstimate.TabIndex = 5;
            this.buttonEstimate.Text = "Estimate";
            this.buttonEstimate.UseVisualStyleBackColor = true;
            this.buttonEstimate.Click += new System.EventHandler(this.buttonEstimate_Click);
            // 
            // buttonSave
            // 
            this.buttonSave.Location = new System.Drawing.Point(93, 13);
            this.buttonSave.Name = "buttonSave";
            this.buttonSave.Size = new System.Drawing.Size(75, 23);
            this.buttonSave.TabIndex = 0;
            this.buttonSave.Text = "Save";
            this.buttonSave.UseVisualStyleBackColor = true;
            this.buttonSave.Click += new System.EventHandler(this.buttonSave_Click);
            // 
            // groupBox4
            // 
            this.groupBox4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.groupBox4.Controls.Add(this.buttonSearchFilter);
            this.groupBox4.Controls.Add(this.tbFilterExpression);
            this.groupBox4.Controls.Add(this.buttonFilterSelect);
            this.groupBox4.Location = new System.Drawing.Point(263, 709);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(558, 45);
            this.groupBox4.TabIndex = 6;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Select by Filter Expression";
            // 
            // buttonSearchFilter
            // 
            this.buttonSearchFilter.Location = new System.Drawing.Point(477, 14);
            this.buttonSearchFilter.Name = "buttonSearchFilter";
            this.buttonSearchFilter.Size = new System.Drawing.Size(75, 23);
            this.buttonSearchFilter.TabIndex = 2;
            this.buttonSearchFilter.Text = "Search";
            this.buttonSearchFilter.UseVisualStyleBackColor = true;
            this.buttonSearchFilter.Click += new System.EventHandler(this.buttonSearchFilter_Click);
            // 
            // tbFilterExpression
            // 
            this.tbFilterExpression.Location = new System.Drawing.Point(6, 16);
            this.tbFilterExpression.Name = "tbFilterExpression";
            this.tbFilterExpression.Size = new System.Drawing.Size(384, 20);
            this.tbFilterExpression.TabIndex = 1;
            // 
            // buttonFilterSelect
            // 
            this.buttonFilterSelect.Location = new System.Drawing.Point(396, 14);
            this.buttonFilterSelect.Name = "buttonFilterSelect";
            this.buttonFilterSelect.Size = new System.Drawing.Size(75, 23);
            this.buttonFilterSelect.TabIndex = 0;
            this.buttonFilterSelect.Text = "Regex";
            this.buttonFilterSelect.UseVisualStyleBackColor = true;
            this.buttonFilterSelect.Click += new System.EventHandler(this.buttonFilterSelect_Click);
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.groupBox7);
            this.groupBox5.Controls.Add(this.groupBox6);
            this.groupBox5.Location = new System.Drawing.Point(12, 355);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(245, 227);
            this.groupBox5.TabIndex = 7;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "SMO Configuration";
            // 
            // groupBox7
            // 
            this.groupBox7.Controls.Add(this.checkClassProportions);
            this.groupBox7.Controls.Add(this.label10);
            this.groupBox7.Controls.Add(this.label11);
            this.groupBox7.Controls.Add(this.tbWeightsPositive);
            this.groupBox7.Controls.Add(this.tbWeightsNegative);
            this.groupBox7.Location = new System.Drawing.Point(13, 121);
            this.groupBox7.Name = "groupBox7";
            this.groupBox7.Size = new System.Drawing.Size(221, 97);
            this.groupBox7.TabIndex = 8;
            this.groupBox7.TabStop = false;
            this.groupBox7.Text = "Class Weights";
            // 
            // checkClassProportions
            // 
            this.checkClassProportions.AutoSize = true;
            this.checkClassProportions.Location = new System.Drawing.Point(102, 72);
            this.checkClassProportions.Name = "checkClassProportions";
            this.checkClassProportions.Size = new System.Drawing.Size(107, 17);
            this.checkClassProportions.TabIndex = 9;
            this.checkClassProportions.Text = "Class Proportions";
            this.checkClassProportions.UseVisualStyleBackColor = true;
            this.checkClassProportions.CheckedChanged += new System.EventHandler(this.checkClassProportions_CheckedChanged);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(6, 21);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(44, 13);
            this.label10.TabIndex = 7;
            this.label10.Text = "Positive";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(6, 48);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(53, 13);
            this.label11.TabIndex = 8;
            this.label11.Text = "Negative ";
            // 
            // tbWeightsPositive
            // 
            this.tbWeightsPositive.DecimalPlaces = 7;
            this.tbWeightsPositive.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.tbWeightsPositive.Location = new System.Drawing.Point(102, 19);
            this.tbWeightsPositive.Name = "tbWeightsPositive";
            this.tbWeightsPositive.Size = new System.Drawing.Size(105, 20);
            this.tbWeightsPositive.TabIndex = 2;
            // 
            // tbWeightsNegative
            // 
            this.tbWeightsNegative.DecimalPlaces = 7;
            this.tbWeightsNegative.Location = new System.Drawing.Point(102, 46);
            this.tbWeightsNegative.Name = "tbWeightsNegative";
            this.tbWeightsNegative.Size = new System.Drawing.Size(105, 20);
            this.tbWeightsNegative.TabIndex = 3;
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.checkUseHeuristics);
            this.groupBox6.Controls.Add(this.label8);
            this.groupBox6.Controls.Add(this.tbComplexity);
            this.groupBox6.Controls.Add(this.tbTolerance);
            this.groupBox6.Controls.Add(this.label9);
            this.groupBox6.Location = new System.Drawing.Point(13, 19);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(221, 96);
            this.groupBox6.TabIndex = 10;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "Algorithm";
            // 
            // checkUseHeuristics
            // 
            this.checkUseHeuristics.AutoSize = true;
            this.checkUseHeuristics.Location = new System.Drawing.Point(102, 72);
            this.checkUseHeuristics.Name = "checkUseHeuristics";
            this.checkUseHeuristics.Size = new System.Drawing.Size(94, 17);
            this.checkUseHeuristics.TabIndex = 7;
            this.checkUseHeuristics.Text = "Use Heuristics";
            this.checkUseHeuristics.UseVisualStyleBackColor = true;
            this.checkUseHeuristics.CheckedChanged += new System.EventHandler(this.checkUseHeuristics_CheckedChanged);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(6, 21);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(57, 13);
            this.label8.TabIndex = 5;
            this.label8.Text = "Complexity";
            // 
            // tbComplexity
            // 
            this.tbComplexity.DecimalPlaces = 7;
            this.tbComplexity.Increment = new decimal(new int[] {
            2,
            0,
            0,
            65536});
            this.tbComplexity.Location = new System.Drawing.Point(102, 19);
            this.tbComplexity.Name = "tbComplexity";
            this.tbComplexity.Size = new System.Drawing.Size(105, 20);
            this.tbComplexity.TabIndex = 0;
            this.tbComplexity.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            // 
            // tbTolerance
            // 
            this.tbTolerance.DecimalPlaces = 7;
            this.tbTolerance.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.tbTolerance.Location = new System.Drawing.Point(102, 46);
            this.tbTolerance.Maximum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.tbTolerance.Name = "tbTolerance";
            this.tbTolerance.Size = new System.Drawing.Size(105, 20);
            this.tbTolerance.TabIndex = 1;
            this.tbTolerance.Value = new decimal(new int[] {
            5,
            0,
            0,
            65536});
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(6, 48);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(55, 13);
            this.label9.TabIndex = 6;
            this.label9.Text = "Tolerance";
            // 
            // groupBox8
            // 
            this.groupBox8.Controls.Add(this.labelRuns);
            this.groupBox8.Controls.Add(this.tbNumRuns);
            this.groupBox8.Controls.Add(this.tbNumFolds);
            this.groupBox8.Controls.Add(this.label12);
            this.groupBox8.Location = new System.Drawing.Point(12, 588);
            this.groupBox8.Name = "groupBox8";
            this.groupBox8.Size = new System.Drawing.Size(245, 75);
            this.groupBox8.TabIndex = 8;
            this.groupBox8.TabStop = false;
            this.groupBox8.Text = "Cross Validation";
            // 
            // labelRuns
            // 
            this.labelRuns.AutoSize = true;
            this.labelRuns.Location = new System.Drawing.Point(6, 44);
            this.labelRuns.Name = "labelRuns";
            this.labelRuns.Size = new System.Drawing.Size(32, 13);
            this.labelRuns.TabIndex = 3;
            this.labelRuns.Text = "Runs";
            // 
            // tbNumRuns
            // 
            this.tbNumRuns.Location = new System.Drawing.Point(115, 42);
            this.tbNumRuns.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.tbNumRuns.Name = "tbNumRuns";
            this.tbNumRuns.Size = new System.Drawing.Size(105, 20);
            this.tbNumRuns.TabIndex = 2;
            this.tbNumRuns.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // tbNumFolds
            // 
            this.tbNumFolds.Location = new System.Drawing.Point(115, 16);
            this.tbNumFolds.Maximum = new decimal(new int[] {
            20,
            0,
            0,
            0});
            this.tbNumFolds.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.tbNumFolds.Name = "tbNumFolds";
            this.tbNumFolds.Size = new System.Drawing.Size(105, 20);
            this.tbNumFolds.TabIndex = 1;
            this.tbNumFolds.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(6, 18);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(32, 13);
            this.label12.TabIndex = 0;
            this.label12.Text = "Folds";
            // 
            // groupBox9
            // 
            this.groupBox9.Controls.Add(this.optInt2);
            this.groupBox9.Controls.Add(this.optInt1);
            this.groupBox9.Controls.Add(this.optDouble2);
            this.groupBox9.Controls.Add(this.optDouble1);
            this.groupBox9.Location = new System.Drawing.Point(12, 670);
            this.groupBox9.Name = "groupBox9";
            this.groupBox9.Size = new System.Drawing.Size(245, 85);
            this.groupBox9.TabIndex = 9;
            this.groupBox9.TabStop = false;
            this.groupBox9.Text = "Optional Arguments";
            // 
            // optInt2
            // 
            this.optInt2.Location = new System.Drawing.Point(13, 53);
            this.optInt2.Minimum = new decimal(new int[] {
            100,
            0,
            0,
            -2147483648});
            this.optInt2.Name = "optInt2";
            this.optInt2.Size = new System.Drawing.Size(92, 20);
            this.optInt2.TabIndex = 6;
            this.optInt2.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // optInt1
            // 
            this.optInt1.Location = new System.Drawing.Point(13, 19);
            this.optInt1.Minimum = new decimal(new int[] {
            100,
            0,
            0,
            -2147483648});
            this.optInt1.Name = "optInt1";
            this.optInt1.Size = new System.Drawing.Size(92, 20);
            this.optInt1.TabIndex = 5;
            this.optInt1.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // optDouble2
            // 
            this.optDouble2.DecimalPlaces = 7;
            this.optDouble2.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.optDouble2.Location = new System.Drawing.Point(115, 53);
            this.optDouble2.Minimum = new decimal(new int[] {
            100,
            0,
            0,
            -2147483648});
            this.optDouble2.Name = "optDouble2";
            this.optDouble2.Size = new System.Drawing.Size(105, 20);
            this.optDouble2.TabIndex = 4;
            this.optDouble2.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // optDouble1
            // 
            this.optDouble1.DecimalPlaces = 7;
            this.optDouble1.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.optDouble1.Location = new System.Drawing.Point(115, 19);
            this.optDouble1.Minimum = new decimal(new int[] {
            100,
            0,
            0,
            -2147483648});
            this.optDouble1.Name = "optDouble1";
            this.optDouble1.Size = new System.Drawing.Size(105, 20);
            this.optDouble1.TabIndex = 3;
            this.optDouble1.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // tbExpression
            // 
            this.tbExpression.Location = new System.Drawing.Point(269, 670);
            this.tbExpression.Name = "tbExpression";
            this.tbExpression.ReadOnly = true;
            this.tbExpression.Size = new System.Drawing.Size(735, 20);
            this.tbExpression.TabIndex = 10;
            // 
            // OptionsSVMDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1011, 766);
            this.Controls.Add(this.tbExpression);
            this.Controls.Add(this.groupBox9);
            this.Controls.Add(this.groupBox8);
            this.Controls.Add(this.groupBox5);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.buttonMarkActive);
            this.Controls.Add(this.buttonMarkIgnored);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupKernel);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "OptionsSVMDialog";
            this.Text = "SVM Configuration";
            this.groupKernel.ResumeLayout(false);
            this.groupKernel.PerformLayout();
            this.groupSigmoidKernel.ResumeLayout(false);
            this.groupSigmoidKernel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tbSigmoidConst)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbSigmoidAlpha)).EndInit();
            this.groupLaplacianKernel.ResumeLayout(false);
            this.groupLaplacianKernel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tbLaplacianSigma)).EndInit();
            this.groupGaussianKernel.ResumeLayout(false);
            this.groupGaussianKernel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tbGaussianSigma)).EndInit();
            this.groupPolyKernel.ResumeLayout(false);
            this.groupPolyKernel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tbPolyConstant)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbPolyDegree)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.groupBox7.ResumeLayout(false);
            this.groupBox7.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tbWeightsPositive)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbWeightsNegative)).EndInit();
            this.groupBox6.ResumeLayout(false);
            this.groupBox6.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tbComplexity)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbTolerance)).EndInit();
            this.groupBox8.ResumeLayout(false);
            this.groupBox8.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tbNumRuns)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbNumFolds)).EndInit();
            this.groupBox9.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.optInt2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.optInt1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.optDouble2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.optDouble1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupKernel;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupPolyKernel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cbKernel;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.GroupBox groupGaussianKernel;
        private System.Windows.Forms.GroupBox groupLaplacianKernel;
        private System.Windows.Forms.GroupBox groupSigmoidKernel;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ListBox lbActiveFeatures;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ListBox lbIgnoredFeatures;
        private System.Windows.Forms.Button buttonMarkIgnored;
        private System.Windows.Forms.Button buttonMarkActive;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button buttonSave;
        private System.Windows.Forms.NumericUpDown tbSigmoidAlpha;
        private System.Windows.Forms.NumericUpDown tbLaplacianSigma;
        private System.Windows.Forms.NumericUpDown tbGaussianSigma;
        private System.Windows.Forms.NumericUpDown tbPolyConstant;
        private System.Windows.Forms.NumericUpDown tbPolyDegree;
        private System.Windows.Forms.Button buttonEstimate;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.TextBox tbFilterExpression;
        private System.Windows.Forms.Button buttonFilterSelect;
        private System.Windows.Forms.Button buttonSearchFilter;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.NumericUpDown tbSigmoidConst;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.NumericUpDown tbWeightsNegative;
        private System.Windows.Forms.NumericUpDown tbWeightsPositive;
        private System.Windows.Forms.NumericUpDown tbTolerance;
        private System.Windows.Forms.NumericUpDown tbComplexity;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.GroupBox groupBox7;
        private System.Windows.Forms.CheckBox checkClassProportions;
        private System.Windows.Forms.CheckBox checkUseHeuristics;
        private System.Windows.Forms.GroupBox groupBox8;
        private System.Windows.Forms.NumericUpDown tbNumFolds;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label labelRuns;
        private System.Windows.Forms.NumericUpDown tbNumRuns;
        private System.Windows.Forms.GroupBox groupBox9;
        private System.Windows.Forms.NumericUpDown optInt2;
        private System.Windows.Forms.NumericUpDown optInt1;
        private System.Windows.Forms.NumericUpDown optDouble2;
        private System.Windows.Forms.NumericUpDown optDouble1;
        private System.Windows.Forms.TextBox tbExpression;
    }
}
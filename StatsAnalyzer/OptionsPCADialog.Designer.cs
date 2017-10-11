namespace StatsAnalyzer
{
    partial class OptionsPCADialog
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
            this.cbAnalysisMethod = new System.Windows.Forms.ComboBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label6 = new System.Windows.Forms.Label();
            this.tbNumberFormat = new System.Windows.Forms.TextBox();
            this.checkForcedComponents = new System.Windows.Forms.CheckBox();
            this.label5 = new System.Windows.Forms.Label();
            this.tbForcedComponents = new System.Windows.Forms.TextBox();
            this.checkForcedLabel = new System.Windows.Forms.CheckBox();
            this.tbForcedLabel = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.tbMissingValueIdentifier = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.tbSeparator = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.buttonSave = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.lbActiveFeatures = new System.Windows.Forms.ListBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.lbIgnoredFeatures = new System.Windows.Forms.ListBox();
            this.buttonMarkIgnored = new System.Windows.Forms.Button();
            this.buttonMarkActive = new System.Windows.Forms.Button();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.SuspendLayout();
            // 
            // cbAnalysisMethod
            // 
            this.cbAnalysisMethod.FormattingEnabled = true;
            this.cbAnalysisMethod.Location = new System.Drawing.Point(146, 19);
            this.cbAnalysisMethod.Name = "cbAnalysisMethod";
            this.cbAnalysisMethod.Size = new System.Drawing.Size(121, 21);
            this.cbAnalysisMethod.TabIndex = 0;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.cbAnalysisMethod);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(274, 53);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "PCA Configuration";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(84, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Analysis Method";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.tbNumberFormat);
            this.groupBox2.Controls.Add(this.checkForcedComponents);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.tbForcedComponents);
            this.groupBox2.Controls.Add(this.checkForcedLabel);
            this.groupBox2.Controls.Add(this.tbForcedLabel);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.tbMissingValueIdentifier);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.tbSeparator);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Location = new System.Drawing.Point(12, 71);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(274, 158);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "PCA Export Configuration";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(6, 75);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(79, 13);
            this.label6.TabIndex = 11;
            this.label6.Text = "Number Format";
            // 
            // tbNumberFormat
            // 
            this.tbNumberFormat.Location = new System.Drawing.Point(146, 72);
            this.tbNumberFormat.Name = "tbNumberFormat";
            this.tbNumberFormat.Size = new System.Drawing.Size(121, 20);
            this.tbNumberFormat.TabIndex = 10;
            // 
            // checkForcedComponents
            // 
            this.checkForcedComponents.AutoSize = true;
            this.checkForcedComponents.Location = new System.Drawing.Point(127, 128);
            this.checkForcedComponents.Name = "checkForcedComponents";
            this.checkForcedComponents.Size = new System.Drawing.Size(15, 14);
            this.checkForcedComponents.TabIndex = 9;
            this.checkForcedComponents.UseVisualStyleBackColor = true;
            this.checkForcedComponents.CheckedChanged += new System.EventHandler(this.checkForcedComponents_CheckedChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(7, 128);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(106, 13);
            this.label5.TabIndex = 8;
            this.label5.Text = "Force # Components";
            // 
            // tbForcedComponents
            // 
            this.tbForcedComponents.Location = new System.Drawing.Point(147, 125);
            this.tbForcedComponents.Name = "tbForcedComponents";
            this.tbForcedComponents.Size = new System.Drawing.Size(121, 20);
            this.tbForcedComponents.TabIndex = 7;
            // 
            // checkForcedLabel
            // 
            this.checkForcedLabel.AutoSize = true;
            this.checkForcedLabel.Location = new System.Drawing.Point(127, 101);
            this.checkForcedLabel.Name = "checkForcedLabel";
            this.checkForcedLabel.Size = new System.Drawing.Size(15, 14);
            this.checkForcedLabel.TabIndex = 6;
            this.checkForcedLabel.UseVisualStyleBackColor = true;
            this.checkForcedLabel.CheckedChanged += new System.EventHandler(this.checkForcedLabel_CheckedChanged);
            // 
            // tbForcedLabel
            // 
            this.tbForcedLabel.Location = new System.Drawing.Point(147, 98);
            this.tbForcedLabel.Name = "tbForcedLabel";
            this.tbForcedLabel.Size = new System.Drawing.Size(121, 20);
            this.tbForcedLabel.TabIndex = 5;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(7, 101);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(63, 13);
            this.label4.TabIndex = 4;
            this.label4.Text = "Force Label";
            // 
            // tbMissingValueIdentifier
            // 
            this.tbMissingValueIdentifier.Location = new System.Drawing.Point(146, 45);
            this.tbMissingValueIdentifier.Name = "tbMissingValueIdentifier";
            this.tbMissingValueIdentifier.Size = new System.Drawing.Size(121, 20);
            this.tbMissingValueIdentifier.TabIndex = 3;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 48);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(115, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Missing Value Identifier";
            // 
            // tbSeparator
            // 
            this.tbSeparator.Location = new System.Drawing.Point(146, 19);
            this.tbSeparator.Name = "tbSeparator";
            this.tbSeparator.Size = new System.Drawing.Size(121, 20);
            this.tbSeparator.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 22);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "Separator";
            // 
            // buttonSave
            // 
            this.buttonSave.Location = new System.Drawing.Point(9, 19);
            this.buttonSave.Name = "buttonSave";
            this.buttonSave.Size = new System.Drawing.Size(75, 23);
            this.buttonSave.TabIndex = 10;
            this.buttonSave.Text = "Save";
            this.buttonSave.UseVisualStyleBackColor = true;
            this.buttonSave.Click += new System.EventHandler(this.buttonSave_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.lbActiveFeatures);
            this.groupBox3.Location = new System.Drawing.Point(293, 13);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(200, 216);
            this.groupBox3.TabIndex = 13;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Active Features";
            // 
            // lbActiveFeatures
            // 
            this.lbActiveFeatures.FormattingEnabled = true;
            this.lbActiveFeatures.Location = new System.Drawing.Point(8, 18);
            this.lbActiveFeatures.Name = "lbActiveFeatures";
            this.lbActiveFeatures.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.lbActiveFeatures.Size = new System.Drawing.Size(186, 186);
            this.lbActiveFeatures.Sorted = true;
            this.lbActiveFeatures.TabIndex = 0;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.lbIgnoredFeatures);
            this.groupBox4.Location = new System.Drawing.Point(558, 13);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(200, 216);
            this.groupBox4.TabIndex = 14;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Ignored Features";
            // 
            // lbIgnoredFeatures
            // 
            this.lbIgnoredFeatures.FormattingEnabled = true;
            this.lbIgnoredFeatures.Location = new System.Drawing.Point(6, 18);
            this.lbIgnoredFeatures.Name = "lbIgnoredFeatures";
            this.lbIgnoredFeatures.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.lbIgnoredFeatures.Size = new System.Drawing.Size(188, 186);
            this.lbIgnoredFeatures.Sorted = true;
            this.lbIgnoredFeatures.TabIndex = 0;
            // 
            // buttonMarkIgnored
            // 
            this.buttonMarkIgnored.Location = new System.Drawing.Point(499, 88);
            this.buttonMarkIgnored.Name = "buttonMarkIgnored";
            this.buttonMarkIgnored.Size = new System.Drawing.Size(53, 23);
            this.buttonMarkIgnored.TabIndex = 15;
            this.buttonMarkIgnored.Text = ">>";
            this.buttonMarkIgnored.UseVisualStyleBackColor = true;
            this.buttonMarkIgnored.Click += new System.EventHandler(this.buttonMarkIgnored_Click);
            // 
            // buttonMarkActive
            // 
            this.buttonMarkActive.Location = new System.Drawing.Point(499, 120);
            this.buttonMarkActive.Name = "buttonMarkActive";
            this.buttonMarkActive.Size = new System.Drawing.Size(53, 23);
            this.buttonMarkActive.TabIndex = 16;
            this.buttonMarkActive.Text = "<<";
            this.buttonMarkActive.UseVisualStyleBackColor = true;
            this.buttonMarkActive.Click += new System.EventHandler(this.buttonMarkActive_Click);
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.buttonSave);
            this.groupBox5.Location = new System.Drawing.Point(12, 235);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(746, 50);
            this.groupBox5.TabIndex = 17;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Actions";
            // 
            // OptionsPCADialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(775, 293);
            this.Controls.Add(this.groupBox5);
            this.Controls.Add(this.buttonMarkActive);
            this.Controls.Add(this.buttonMarkIgnored);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "OptionsPCADialog";
            this.Text = "PCA Configuration";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.groupBox5.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ComboBox cbAnalysisMethod;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox tbMissingValueIdentifier;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox tbSeparator;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox checkForcedLabel;
        private System.Windows.Forms.TextBox tbForcedLabel;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.CheckBox checkForcedComponents;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox tbForcedComponents;
        private System.Windows.Forms.Button buttonSave;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox tbNumberFormat;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.ListBox lbActiveFeatures;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.ListBox lbIgnoredFeatures;
        private System.Windows.Forms.Button buttonMarkIgnored;
        private System.Windows.Forms.Button buttonMarkActive;
        private System.Windows.Forms.GroupBox groupBox5;
    }
}
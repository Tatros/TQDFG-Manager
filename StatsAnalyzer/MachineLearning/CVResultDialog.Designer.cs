namespace StatsAnalyzer.MachineLearning
{
    partial class CVResultDialog
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.buttonROC = new System.Windows.Forms.Button();
            this.buttonSummary = new System.Windows.Forms.Button();
            this.buttonDetails = new System.Windows.Forms.Button();
            this.buttonShowGeneric = new System.Windows.Forms.Button();
            this.cbVotingScheme = new System.Windows.Forms.ComboBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.buttonExport = new System.Windows.Forms.Button();
            this.buttonClose = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.buttonROC);
            this.groupBox1.Controls.Add(this.buttonSummary);
            this.groupBox1.Controls.Add(this.buttonDetails);
            this.groupBox1.Controls.Add(this.buttonShowGeneric);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(151, 158);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Views";
            // 
            // buttonROC
            // 
            this.buttonROC.Location = new System.Drawing.Point(6, 124);
            this.buttonROC.Name = "buttonROC";
            this.buttonROC.Size = new System.Drawing.Size(135, 23);
            this.buttonROC.TabIndex = 3;
            this.buttonROC.Text = "Plot ROC";
            this.buttonROC.UseVisualStyleBackColor = true;
            this.buttonROC.Click += new System.EventHandler(this.buttonROC_Click);
            // 
            // buttonSummary
            // 
            this.buttonSummary.Location = new System.Drawing.Point(6, 95);
            this.buttonSummary.Name = "buttonSummary";
            this.buttonSummary.Size = new System.Drawing.Size(136, 23);
            this.buttonSummary.TabIndex = 2;
            this.buttonSummary.Text = "Show Summary";
            this.buttonSummary.UseVisualStyleBackColor = true;
            this.buttonSummary.Click += new System.EventHandler(this.buttonSummary_Click);
            // 
            // buttonDetails
            // 
            this.buttonDetails.Location = new System.Drawing.Point(6, 66);
            this.buttonDetails.Name = "buttonDetails";
            this.buttonDetails.Size = new System.Drawing.Size(136, 23);
            this.buttonDetails.TabIndex = 1;
            this.buttonDetails.Text = "Show Details";
            this.buttonDetails.UseVisualStyleBackColor = true;
            this.buttonDetails.Click += new System.EventHandler(this.buttonDetails_Click);
            // 
            // buttonShowGeneric
            // 
            this.buttonShowGeneric.Location = new System.Drawing.Point(6, 19);
            this.buttonShowGeneric.Name = "buttonShowGeneric";
            this.buttonShowGeneric.Size = new System.Drawing.Size(135, 41);
            this.buttonShowGeneric.TabIndex = 0;
            this.buttonShowGeneric.Text = "Confusion Matrix Statistics";
            this.buttonShowGeneric.UseVisualStyleBackColor = true;
            this.buttonShowGeneric.Click += new System.EventHandler(this.buttonShowGeneric_Click);
            // 
            // cbVotingScheme
            // 
            this.cbVotingScheme.FormattingEnabled = true;
            this.cbVotingScheme.Location = new System.Drawing.Point(6, 19);
            this.cbVotingScheme.Name = "cbVotingScheme";
            this.cbVotingScheme.Size = new System.Drawing.Size(139, 21);
            this.cbVotingScheme.TabIndex = 3;
            this.cbVotingScheme.SelectedIndexChanged += new System.EventHandler(this.cbVotingScheme_SelectedIndexChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.cbVotingScheme);
            this.groupBox2.Location = new System.Drawing.Point(12, 176);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(151, 51);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Class Voter";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.buttonExport);
            this.groupBox3.Controls.Add(this.buttonClose);
            this.groupBox3.Location = new System.Drawing.Point(12, 233);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(151, 79);
            this.groupBox3.TabIndex = 2;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Actions";
            // 
            // buttonExport
            // 
            this.buttonExport.Location = new System.Drawing.Point(5, 19);
            this.buttonExport.Name = "buttonExport";
            this.buttonExport.Size = new System.Drawing.Size(136, 23);
            this.buttonExport.TabIndex = 1;
            this.buttonExport.Text = "Export";
            this.buttonExport.UseVisualStyleBackColor = true;
            this.buttonExport.Click += new System.EventHandler(this.buttonExport_Click);
            // 
            // buttonClose
            // 
            this.buttonClose.Location = new System.Drawing.Point(5, 48);
            this.buttonClose.Name = "buttonClose";
            this.buttonClose.Size = new System.Drawing.Size(135, 23);
            this.buttonClose.TabIndex = 0;
            this.buttonClose.Text = "Close";
            this.buttonClose.UseVisualStyleBackColor = true;
            this.buttonClose.Click += new System.EventHandler(this.buttonClose_Click);
            // 
            // CVResultDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(171, 317);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "CVResultDialog";
            this.ShowIcon = false;
            this.Text = "CV Results";
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button buttonSummary;
        private System.Windows.Forms.Button buttonDetails;
        private System.Windows.Forms.Button buttonShowGeneric;
        private System.Windows.Forms.ComboBox cbVotingScheme;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button buttonExport;
        private System.Windows.Forms.Button buttonClose;
        private System.Windows.Forms.Button buttonROC;

    }
}
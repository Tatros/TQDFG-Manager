namespace StatsAnalyzer
{
    partial class ScatterPlotForm
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
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.scatterplotView1 = new Accord.Controls.ScatterplotView();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel1.SuspendLayout();
            this.groupBox6.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
| System.Windows.Forms.AnchorStyles.Left)
| System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.Location = new System.Drawing.Point(13, 13);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(480, 350);
            this.panel1.TabIndex = 0;
            this.panel1.Controls.Add(groupBox6);
            // 
            // groupBox6
            // 
            this.groupBox6.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
| System.Windows.Forms.AnchorStyles.Left)
| System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox6.Controls.Add(this.scatterplotView1);
            this.groupBox6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox6.Location = new System.Drawing.Point(0, 0);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(470, 350);
            this.groupBox6.TabIndex = 6;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "Receiver Operating Characteristic Curve";
            // 
            // scatterplotView1
            // 
            this.scatterplotView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.scatterplotView1.LinesVisible = true;
            this.scatterplotView1.Location = new System.Drawing.Point(3, 16);
            this.scatterplotView1.Name = "scatterplotView1";
            this.scatterplotView1.ScaleTight = true;
            this.scatterplotView1.Size = new System.Drawing.Size(458, 309);
            this.scatterplotView1.SymbolSize = 0F;
            this.scatterplotView1.TabIndex = 7;
            // 
            // ScatterPlotForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(500,370);
            this.Controls.Add(this.panel1);

            this.DoubleBuffered = true;

            this.Name = "MainForm";
            this.Text = "Receiver Operating Characteristic Curve Demonstration";
            this.panel1.ResumeLayout(false);


            this.groupBox6.ResumeLayout(false);
            this.groupBox6.PerformLayout();


            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

    
        private System.Windows.Forms.GroupBox groupBox6;
        private Accord.Controls.ScatterplotView scatterplotView1;
        private System.Windows.Forms.Panel panel1;
    }
}
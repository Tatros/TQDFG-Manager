namespace StatsAnalyzer
{
    partial class VisualizationForm
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
            this.cumulativeView = new Accord.Controls.ComponentView();
            

            this.components = new System.ComponentModel.Container();
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Text = "VisualizationForm";

            // cumulativeView
            // 
            this.cumulativeView.Cumulative = true;
            this.cumulativeView.DataSource = null;
            this.cumulativeView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cumulativeView.Location = new System.Drawing.Point(0, 0);
            this.cumulativeView.Name = "cumulativeView";
            this.cumulativeView.Size = new System.Drawing.Size(572, 167);
            this.cumulativeView.TabIndex = 5;
        }

        private Accord.Controls.ComponentView cumulativeView;
        #endregion
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Accord.Statistics.Analysis;

namespace StatsAnalyzer
{
    public enum VisualizationType { NONE, COMPONENTS_CUMULATIVE, COMPONENTS_DISTRIBUTION, ROC_PLOT_POINTS };

    public partial class VisualizationForm : Form
    {
        private int _screenWidth;
        private int _screenHeight;    

        public VisualizationForm(PrincipalComponentCollection componentsToVisualize, bool cumulative, String windowTitle)
        {
            InitializeComponent();

            this.Text = windowTitle;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.cumulativeView.Cumulative = cumulative;
            this._screenWidth = Screen.PrimaryScreen.Bounds.Width;
            this._screenHeight = Screen.PrimaryScreen.Bounds.Height;
            this.Controls.Add(this.cumulativeView);
            cumulativeView.DataSource = componentsToVisualize;
            this.Width = _screenWidth / 2;
            this.Height = _screenHeight / 2;
        }

        public VisualizationForm(ReceiverOperatingCharacteristic roc, String windowTitle)
        {
            InitializeComponent();
            ScatterPlotForm sp = new ScatterPlotForm(roc.GetScatterplot(true));
            sp.Show();
        }
    }
}

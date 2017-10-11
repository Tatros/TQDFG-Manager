using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Accord.Statistics.Visualizations;
using Accord.Statistics.Analysis;
using Accord.Controls;
using Accord.IO;
using System.IO;

namespace StatsAnalyzer
{
    public partial class ScatterPlotForm : Form
    {
        private DataTable sourceTable;
        private ReceiverOperatingCharacteristic rocCurve;
        private Scatterplot plot;


        public ScatterPlotForm(Scatterplot p)
        {
            InitializeComponent();
            
            this.plot = p;
            scatterplotView1.Scatterplot = this.plot;
        }
    }
}

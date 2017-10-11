using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using StatsAnalyzer.Model;
using Accord;
using Accord.MachineLearning;
using Accord.Statistics.Analysis;

namespace StatsAnalyzer
{
    internal partial class FormDataView<T> : Form
    {
        private DataTable _table;
        private VisualizationType _visualizationType = VisualizationType.NONE;
        private List<VisualizationType> _availableVisualizations = null;
        private object _visualizationSource = null;

        internal FormDataView(DataTable table)
        {
            InitializeComponent();
            this._table = table;
            generateView();
        }

        internal FormDataView(object dataSource)
        {
            InitializeComponent();
            this._table = dataSource as DataTable;
            this.dataGridView1.DataSource = dataSource;
            this.dataGridView1.AutoResizeColumns();

            fitHeight();
        }

        private void checkColumnLength<V>(List<V> columns, int expectedLength)
        {
            if (columns.Count != expectedLength)
                throw new ArgumentException("Number of specified captions does not equal number of provided data columns.");
        }

        internal FormDataView(T[,] data, List<String> captions = null, List<String> nodeNames = null)
        {
            InitializeComponent();
            this._table = new DataTable();

            int arrayRows = data.GetLength(0);
            int arrayCols = data.GetLength(1);

            if (nodeNames.Count != arrayRows)
                throw new ArgumentException("Number of provided Node Names does not equal number of data rows.");

            if (nodeNames != null)
                _table.Columns.Add("Name", typeof(String));

            // Columns
            for (int i = 0; i < arrayCols; i++)
            {
                if (captions != null)
                {
                    checkColumnLength<String>(captions, arrayCols);
                    _table.Columns.Add(captions[i], typeof(T));
                }

                else
                {
                    _table.Columns.Add("Column " + i.ToString(), typeof(T));
                }
            }

            for (int i = 0; i < arrayRows; i++)
            {
                if (nodeNames != null)
                {
                    object[] row = new object[arrayCols + 1];
                    row[0] = nodeNames[i];
                    for (int j = 0; j < arrayCols; j++)
                    {
                        row[j+1] = data[i, j];
                    }

                    _table.LoadDataRow(row, true);
                }

                else
                {
                    object[] row = new object[arrayCols];
                    for (int j = 0; j < arrayCols; j++)
                    {
                        row[j] = data[i, j];
                    }

                    _table.LoadDataRow(row, true);
                }
            }

            generateView();
        }

        internal FormDataView(T[][] data, List<String> captions = null)
        {
            InitializeComponent();
            this._table = new DataTable();
            List<List<T>> rows = new List<List<T>>();
            List<String> columns;

            // Constraints
            int numCols = data[0].Length;
            if (captions != null)
                checkColumnLength<String>(captions, numCols);

            if (!Utility.isArrayUniform<T>(data))
                throw new ArgumentException("Data Array must be uniform, i.e. each row must be of identical length.");

            // Prepare Columns
            if (captions != null)
                columns = captions;
            else
            {
                columns = new List<string>();
                for (int i = 0; i < numCols; i++)
                    columns.Add("Column " + i);
            }
            
            // Prepare Rows
            for (int i = 0; i < data.Length; i++)
            {
                rows.Add(data[i].ToList());
            }

            populateTable(columns, rows);
            generateView();
        }

        internal FormDataView(IModel model, List<String> nodeNames)
        {
            InitializeComponent();
            this._table = new DataTable();

            List<INode> nodes = new List<INode>(model.getNodes());
            List<String> featureNames = new List<String>(nodes.First().getFeatureNames());

            List<List<Double>> rows = new List<List<Double>>();
            nodeNames.ForEach(nodeName => 
            {
                INode node = model.getNode(nodeName);

                List<Double> values = new List<Double>();
                featureNames.ForEach(featureName =>
                {
                    values.Add(node.getFeatureValue(featureName));
                });

                rows.Add(values);
            });

            populateTable(featureNames, rows);
            generateView();
        }

        internal FormDataView(List<String> colNames, List<List<Double>> rows)
        {
            InitializeComponent();

            this._table = new DataTable();

            populateTable(colNames, rows);
            generateView();
        }
        
        internal FormDataView(PrincipalComponentAnalysis pca)
        {
            InitializeComponent();
            dataGridView1.DataSource = pca.Components;
            this.dataGridView1.AutoResizeColumns();
            this._availableVisualizations = new List<VisualizationType> { VisualizationType.COMPONENTS_CUMULATIVE, VisualizationType.COMPONENTS_DISTRIBUTION };
            enableVisualization(pca);

            fitHeight();
        }

        internal FormDataView(DataTable table, MachineLearning.SchemeSumPredictions sumPredictionScheme)
        {
            InitializeComponent();
            this._table = table;
            // this._availableVisualizations = new List<VisualizationType> { Visuali}
            generateView();
        }

        internal FormDataView(ReceiverOperatingCharacteristic roc)
        {
            InitializeComponent();

            DataTable data = new DataTable();
            
            data.Columns.Add("Observations", typeof(String));
            data.Columns.Add("Negatives", typeof(String));
            data.Columns.Add("Positives", typeof(String));
            data.Columns.Add("Area", typeof(String));
            data.Columns.Add("Std Error", typeof(String));
            data.Columns.Add("Variance", typeof(String));
            
            data.Rows.Add(roc.Observations, roc.Negatives, roc.Positives, roc.Area, roc.StandardError, roc.Variance);

            dataGridView1.DataSource = data;
            this.dataGridView1.AutoResizeColumns();
            this._availableVisualizations = new List<VisualizationType> { VisualizationType.ROC_PLOT_POINTS };
            enableVisualization(roc);

            fitHeight();
        }

        private void enableVisualization(object visualizationSource)
        {
            if (_availableVisualizations != null && _availableVisualizations.Count > 0)
            {
                this._visualizationSource = visualizationSource;
                this.cbVisualizations.Enabled = true;
                _availableVisualizations.ForEach(visualization => cbVisualizations.Items.Add(visualization));
                this.cbVisualizations.SelectedIndex = 0;
                this._visualizationType = getSelectedVisualization();
                this.buttonVisualize.Enabled = true;
            }
        }

        private VisualizationType getSelectedVisualization()
        {
            if (_availableVisualizations != null && _availableVisualizations.Count > 0)
            {
                if (cbVisualizations.Enabled && cbVisualizations.Items.Count > 0)
                {
                    return (VisualizationType)cbVisualizations.SelectedItem;
                }
            }

            return VisualizationType.NONE;
        }

        private void populateTable<V>(List<String> colNames, List<List<V>> rows)
        {
            colNames.ForEach(colName => 
            {
                _table.Columns.Add(new DataColumn(colName, typeof(Double)));
            });
            
            rows.ForEach(row =>
            {
                if (colNames.Count != row.Count)
                    throw new ArgumentException("Length of row inconsistent with number of column names.");


                object[] rowArray = new object[colNames.Count];
                for (int i = 0; i < row.Count; i++)
                {
                    rowArray[i] = row[i];
                }

                _table.Rows.Add(rowArray);
            });
        }

        private void generateView()
        {
            this.dataGridView1.DataSource = _table;
            this.dataGridView1.AutoResizeColumns();

            fitHeight();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;
        }

        private int getFitHeight()
        {
            int totalHeight = dataGridView1.Rows.Count * this.dataGridView1.Rows[0].Height + 50;
            return (totalHeight + 150);
        }

        private void fitHeight()
        {
            if (getFitHeight() < Screen.PrimaryScreen.Bounds.Height)
                this.Height = getFitHeight();

            fillScreenHorizontal();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            fitHeight();
        }

        private void buttonSwitchView_Click(object sender, EventArgs e)
        {
            if (dataGridView1.AutoSizeColumnsMode == DataGridViewAutoSizeColumnsMode.Fill)
                dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            else if (dataGridView1.AutoSizeColumnsMode == DataGridViewAutoSizeColumnsMode.AllCells)
                dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells;
            else if (dataGridView1.AutoSizeColumnsMode == DataGridViewAutoSizeColumnsMode.None)
                dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            else
                dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
                
        }

        private void buttonVisualize_Click(object sender, EventArgs e)
        {
            visualize();
        }

        private void visualize()
        {
            switch (_visualizationType)
            {
                case VisualizationType.COMPONENTS_CUMULATIVE:
                    {
                        if (_visualizationSource != null)
                        {
                            if (_visualizationSource.GetType() == typeof(PrincipalComponentAnalysis))
                            {
                                PrincipalComponentAnalysis pca = (PrincipalComponentAnalysis)_visualizationSource;
                                VisualizationForm f = new VisualizationForm(pca.Components, true, "Cumulative Component Distribution");
                                f.Show();
                            }
                        }
                        
                        break;
                    }
                case VisualizationType.COMPONENTS_DISTRIBUTION:
                    {
                        if (_visualizationSource != null)
                        {
                            if (_visualizationSource.GetType() == typeof(PrincipalComponentAnalysis))
                            {
                                PrincipalComponentAnalysis pca = (PrincipalComponentAnalysis)_visualizationSource;
                                VisualizationForm f = new VisualizationForm(pca.Components, false, "Component Distribution");
                                f.Show();
                            }
                        }
                        break;
                    }
                case VisualizationType.ROC_PLOT_POINTS:
                    {
                        if (_visualizationSource != null)
                        {
                            if (_visualizationSource.GetType() == typeof(ReceiverOperatingCharacteristic))
                            {
                                ReceiverOperatingCharacteristic roc = (ReceiverOperatingCharacteristic)_visualizationSource;
                                ScatterPlotForm sp = new ScatterPlotForm(roc.GetScatterplot(true));
                                sp.Show();
                            }
                        }
                        break;
                    }
                default:
                    {
                        break;
                    }
            }
        }

        private void cbVisualizations_SelectedIndexChanged(object sender, EventArgs e)
        {
            _visualizationType = getSelectedVisualization();
        }

        internal void fillScreenHorizontal()
        {
            this.Width = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width - 20;
        }
    }
}

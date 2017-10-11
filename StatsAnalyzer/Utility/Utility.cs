using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Windows.Forms;
using System.Drawing;
using ZedGraph;

namespace StatsAnalyzer
{
    class Utility
    {
        public static void runMatlab(String command, bool closeWindow)
        {
            String commandEnd = "\"";
            if (closeWindow)
            {
                commandEnd = " exit;\"";
            }

            var process = new Process()
            {
                StartInfo = new ProcessStartInfo()
                {
                    FileName = Settings.matlabExecutablePath,
                    Arguments = "-nosplash -nodesktop -minimize" +
                       " -r \"" + command + commandEnd
                }
            };
            process.Start();
        }

        public static double BytesToGB(long bytes)
        {
            return  ((double)bytes / 1000000000.0);
        }
        

        public static String formatNumber(double d)
        {
            d = Math.Round(d, 3);
            return d.ToString("0.000", new System.Globalization.CultureInfo("en-US"));
        }

        internal void CreateScatterplot(ZedGraphControl zgc, double[,] graph, String xAxisTitle, String yAxisTitle)
        {
            GraphPane myPane = zgc.GraphPane;
            myPane.CurveList.Clear();

            // Set the titles
            myPane.Title.IsVisible = false;
            myPane.XAxis.Title.Text = xAxisTitle;
            myPane.YAxis.Title.Text = yAxisTitle;


            // Classification problem
            PointPairList list1 = new PointPairList(); // Z = -1
            PointPairList list2 = new PointPairList(); // Z = +1
            for (int i = 0; i < graph.GetLength(0); i++)
            {
                if (graph[i, 2] == -1)
                    list1.Add(graph[i, 0], graph[i, 1]);
                if (graph[i, 2] == 1)
                    list2.Add(graph[i, 0], graph[i, 1]);
            }

            // Add the curve
            LineItem myCurve = myPane.AddCurve("G1", list1, Color.Blue, SymbolType.Diamond);
            myCurve.Line.IsVisible = false;
            myCurve.Symbol.Border.IsVisible = false;
            myCurve.Symbol.Fill = new Fill(Color.Blue);

            myCurve = myPane.AddCurve("G2", list2, Color.Green, SymbolType.Diamond);
            myCurve.Line.IsVisible = false;
            myCurve.Symbol.Border.IsVisible = false;
            myCurve.Symbol.Fill = new Fill(Color.Green);


            // Fill the background of the chart rect and pane
            //myPane.Chart.Fill = new Fill(Color.White, Color.LightGoldenrodYellow, 45.0f);
            //myPane.Fill = new Fill(Color.White, Color.SlateGray, 45.0f);
            myPane.Fill = new Fill(Color.WhiteSmoke);

            zgc.AxisChange();
            zgc.Invalidate();
        }

        public static string getRunDirectory(String runName)
        {
            return Settings.outputDirectory + "\\Analyzer\\" + runName;
        }

        public static bool isArraySymmetric<T>(T[,] arr)
        {
            return (arr.GetLength(0) == arr.GetLength(1)) ? (true) : (false);
        }

        public static bool isArrayUniform<T>(T[][] arr)
        {
            int rowLength = arr[0].Length;
            for (int i = 0; i < arr.Length; i++)
                if (arr[i].Length != rowLength)
                    return false;

            return true;
        }

        public enum RandomData { Double, Int };

        public static void getRandomData(out object[,] data, int dim0, int dim1, RandomData type)
        {
            Random ran = new Random();
            data = new object[dim0, dim1];
            for (int i = 0; i < dim0; i++)
            {
                for (int j = 0; j < dim1; j++)
                {
                    switch (type)
                    {
                        case RandomData.Double:
                            {
                                data[i,j] = ran.NextDouble();
                                break;
                            }
                        case RandomData.Int:
                            {
                                data[i,j] = ran.Next();
                                break;
                            }
                        default:
                            throw new NotImplementedException("The chosen RandomData Type <" + type.ToString() + "> is not yet implemented.");
                    }
                }
            }
        }

        public static void getRandomData(out object[][] data, int dim0, int dim1, RandomData type)
        {
            Random ran = new Random();
            data = new object[dim0][];
            for (int i = 0; i < dim0; i++)
            {
                data[i] = new object[dim1];

                for (int j = 0; j < data[i].Length; j++)
                {
                    switch (type)
                    {
                        case RandomData.Double:
                            {
                                data[i][j] = ran.NextDouble();
                                break;
                            }
                        case RandomData.Int:
                            {
                                data[i][j] = ran.Next();
                                break;
                            }
                        default:
                            throw new NotImplementedException("The chosen RandomData Type <" + type.ToString() + "> is not yet implemented.");
                    }
                }
            }
        }

        public static String[] Intersection(String[] a, String[] b)
        {
            String[] intersection = a.Intersect(b).ToArray();
            return intersection;
        }


        public static void writeToConsole<T>(T[,] arr)
        {
            int dim0 = arr.GetLength(0);
            int dim1 = arr.GetLength(1);

            Console.WriteLine("Output for Array[" + dim0 + "," + dim1 + "]");

            for (int i = 0; i < dim0; i++)
            {
                for (int j = 0; j < dim1; j++)
                {
                    Console.Write(arr[i, j].ToString() + "    ");
                }
                Console.WriteLine();
            }
        }

        public static void writeToConsole<T>(T[] arr)
        {
            for (int j = 0; j < arr.Length; j++)
            {
                Console.Write(arr[j].ToString() + "    ");
            }
            Console.WriteLine();
        }

        public static void writeToConsole<T>(T[][] arr)
        {
            for (int j = 0; j < arr.Length; j++)
            {
                for (int i = 0; i < arr[j].Length; i++)
                {
                    Console.Write(arr[j][i].ToString() + "    ");
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }

        public static void swapItemBetweenListBox(ListBox sourceBox, ListBox destinationBox)
        {
            sourceBox.SelectedItems.OfType<String>().ToList().ForEach(feature => destinationBox.Items.Add(feature));
            destinationBox.Items.OfType<String>().ToList().ForEach(item => sourceBox.Items.Remove(item));
        }

        public static DialogResult InputBox(string title, string promptText, ref string value)
        {
            Form form = new Form();
            System.Windows.Forms.Label label = new System.Windows.Forms.Label();
            TextBox textBox = new TextBox();
            Button buttonOk = new Button();
            Button buttonCancel = new Button();

            form.Text = title;
            label.Text = promptText;
            textBox.Text = value;

            buttonOk.Text = "OK";
            buttonCancel.Text = "Cancel";
            buttonOk.DialogResult = DialogResult.OK;
            buttonCancel.DialogResult = DialogResult.Cancel;

            label.SetBounds(9, 20, 372, 13);
            textBox.SetBounds(12, 36, 372, 20);
            buttonOk.SetBounds(228, 72, 75, 23);
            buttonCancel.SetBounds(309, 72, 75, 23);

            label.AutoSize = true;
            textBox.Anchor = textBox.Anchor | AnchorStyles.Right;
            buttonOk.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            buttonCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;

            form.ClientSize = new Size(396, 107);
            form.Controls.AddRange(new Control[] { label, textBox, buttonOk, buttonCancel });
            form.ClientSize = new Size(Math.Max(300, label.Right + 10), form.ClientSize.Height);
            form.FormBorderStyle = FormBorderStyle.FixedDialog;
            form.StartPosition = FormStartPosition.CenterScreen;
            form.MinimizeBox = false;
            form.MaximizeBox = false;
            form.AcceptButton = buttonOk;
            form.CancelButton = buttonCancel;

            DialogResult dialogResult = form.ShowDialog();
            value = textBox.Text;
            return dialogResult;
        }
    }
}

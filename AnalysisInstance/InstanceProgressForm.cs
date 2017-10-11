using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using QDFGAnalyzer;
using System.IO;
using System.Diagnostics;
using System.Threading;

namespace AnalysisInstance
{
    public partial class InstanceProgressForm : Form
    {
        private String folderPath = "";
        private String outputDirectory = Application.StartupPath;
        private String runName = "";
        private bool cancel = false;

        // Analyzer Config
        long MemoryMaxBytesAllowed;
        int minNumSamples;
        String MissingValueIdentifier;
        QDFGGraphManager.Settings.ModelDataType dataModel;
        QDFGGraphManager.Settings.TimeDataType timeModel;
        String numberFormat;
        int numSamples;
        int timeStepMS;

        public InstanceProgressForm()
        {
            InitializeComponent();
            string[] args = Environment.GetCommandLineArgs();

            // init Analyzer
            this.init(args);

            // run Instance
            //this.run();
            //Task.Run(this.runTasksSync());
            //this.runTasksSync();
            this.runTasksBackgroundSequential();
        }

        private void init(string[] args)
        {
            /*
            if (args.Length != 10)
            {
                throw new ArgumentException("Some arguments were invalid.");
            }*/

            Console.WriteLine("Arguments: ");
            foreach (string arg in args)
            {
                Console.WriteLine(arg + ", ");
            }

            /* Parse Arguments */
            try
            {
                //1: Run Name
                //2: folderPath
                //3: maxMBytes
                //4: minSamples
                //5: numSamples
                //6: missingValue
                //7: dataModel
                //8: timeModel
                //9: numberFormat
                //10: timeStep

                this.runName = args[1];
                this.folderPath = args[2];
                this.MemoryMaxBytesAllowed = (Int64.Parse(args[3]) * 1024 * 1024);
                this.minNumSamples = Int32.Parse(args[4]);
                this.numSamples = Int32.Parse(args[5]);
                this.MissingValueIdentifier = args[6];

                if (args[7] == "SAMPLE_BASED")
                    dataModel = QDFGGraphManager.Settings.ModelDataType.SAMPLE_BASED;
                else if (args[7] == "AGGREGATED_STATISTICS")
                    dataModel = QDFGGraphManager.Settings.ModelDataType.AGGREGATED_STATISTICS;
                else
                    throw new ArgumentException("Invalid Data Model: " + args[7]);

                if (args[8] == "TIME_STEPS")
                    timeModel = QDFGGraphManager.Settings.TimeDataType.TIME_STEPS;
                else
                    throw new ArgumentException("Invalid Time Model: " + args[8]);

                this.numberFormat = args[9];
                this.timeStepMS = Int32.Parse(args[10]);
                this.outputDirectory = args[11];

                // Verify Args
                if (minNumSamples <= 0)
                    minNumSamples = 1;

                if (numSamples <= 0)
                    numSamples = 10;

                try
                {
                    if (!Directory.Exists(outputDirectory))
                        Directory.CreateDirectory(outputDirectory);
                }
                catch (Exception ex)
                {
                    throw new ArgumentException("Failed to create output Directory:\n" + outputDirectory);
                }
            }

            catch (Exception ex)
            {
                throw new ArgumentException("Failed to parse Arguments: " + ex.Message);
            }

            this.Text = "Analyzer Instance: " + runName;
            this.labelFolder.Text = folderPath;
            this.labelMinSamples.Text = minNumSamples.ToString();
            this.labelNumSamples.Text = numSamples.ToString();
            this.labelRunName.Text = runName;
            this.labelTimeStep.Text = timeStepMS + " ms";
        }

        private void initAnalyzerSettings(LogAnalyzer logAnalyzer)
        {
            logAnalyzer.MemoryMaxBytesAllowed = this.MemoryMaxBytesAllowed;
            logAnalyzer.MinNumSamples = this.minNumSamples;
            logAnalyzer.MissingValueIdentifier = this.MissingValueIdentifier;
            logAnalyzer.ModelDataType = this.dataModel;
            logAnalyzer.TimeDataType = this.timeModel;
            logAnalyzer.NumberFormat = this.numberFormat;
            logAnalyzer.NumSamples = this.numSamples;
            logAnalyzer.TimeStepMS = this.timeStepMS;
        }

        private void runAnalysisTask(IGrouping<int, String> partition)
        {

            foreach (var item in partition)
            {
                Console.WriteLine("processing: {0}", item);
                LogAnalyzer logAnalyzer = new LogAnalyzer();

                try
                {
                    initAnalyzerSettings(logAnalyzer);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Failed to initialize Analyzer: " + ex.Message);
                    return;
                }

                logAnalyzer.analyzeLog(item, getRunDirectory(runName));
            }

           /*
                this.labelCurrentFile.Text = files[i];

                numFinished++;
                int progress = Convert.ToInt32((100 / (double)numFiles) * numFinished);
                labelProgress.Text = numFinished + " / " + numFiles + "  ( " + progress + "% )";
                this.progressBar.Value = progress;
            */
        }

        private void runTasksSync()
        {
            Console.WriteLine("Searching Files in " + folderPath);

            string[] files = Directory.GetFiles(folderPath, "*.exe.csv", SearchOption.AllDirectories);
            int numFiles = files.Length;
            int numFinished = 0;

            foreach (var file in files)
            {
                Console.WriteLine("Processing: " + file);
                LogAnalyzer l = new LogAnalyzer();
                initAnalyzerSettings(l);


                l.analyzeLog(file, getRunDirectory(runName));
                

                numFinished++;
                int progress = Convert.ToInt32((100 / (double)numFiles) * numFinished);
                labelProgress.Text = numFinished + " / " + numFiles + "  ( " + progress + "% )";
                this.progressBar.Value = progress;
            }


            if (!Directory.Exists(Application.StartupPath + "\\Results"))
                Directory.CreateDirectory(Application.StartupPath + "\\Results");
        }

        private void runTasksBackgroundSequential()
        {
            Console.WriteLine("Searching Files in " + folderPath);
            string[] files = Directory.GetFiles(folderPath, "*.exe.csv", SearchOption.AllDirectories);
            int numFiles = files.Length;
            int numFinished = 0;

            BackgroundWorker bw = new BackgroundWorker();

            // this allows our worker to report progress during work
            bw.WorkerReportsProgress = true;

            // what to do in the background thread
            bw.DoWork += new DoWorkEventHandler(
            delegate(object o, DoWorkEventArgs args)
            {
                BackgroundWorker b = o as BackgroundWorker;

                foreach (var file in files)
                {
                    if (cancel)
                        break;

                    Console.WriteLine("Processing: " + file);
                    
                    LogAnalyzer l = new LogAnalyzer();
                    initAnalyzerSettings(l);

                    l.analyzeLog(file, getRunDirectory(runName));

                    numFinished++;
                    int progress = Convert.ToInt32((100 / (double)numFiles) * numFinished);
                    b.ReportProgress(progress, file);
                }
            });

            // what to do when progress changed (update the progress bar for example)
            bw.ProgressChanged += new ProgressChangedEventHandler(
            delegate(object o, ProgressChangedEventArgs args)
            {
                if (cancel)
                {
                    this.buttonCancel.Enabled = false;
                    this.Text = "Canceling...";
                }
                labelProgress.Text = numFinished + " / " + numFiles + "  ( " + args.ProgressPercentage + "% )";
                this.progressBar.Value = args.ProgressPercentage;
                this.labelCurrentFile.Text = args.UserState.ToString();
            });

            // what to do when worker completes its task (notify the user)
            bw.RunWorkerCompleted += new RunWorkerCompletedEventHandler(
            delegate(object o, RunWorkerCompletedEventArgs args)
            {
                this.buttonCancel.Enabled = false;
                this.buttonCancel.Text = "Canceled";
                this.labelCurrentFile.Text = "N/A";

                if (cancel)
                {
                    this.Text = "Instance Canceled: " + runName;
                    this.buttonCancel.Text = "Canceled";
                    File.WriteAllText(Application.StartupPath + "\\logs\\Instance-" + runName + ".txt", "Analysis Instance \"" + runName + "\" was canceled (" + numFinished + " files).");
                }
                else
                {
                    this.Text = "Instance Complete: " + runName;
                    this.buttonCancel.Text = "Complete";
                    File.WriteAllText(Application.StartupPath + "\\logs\\Instance-" + runName + ".txt", "Analysis Instance \"" + runName + "\" completed (" + numFinished + " files).");
                }

                this.Close();
                Application.Exit(); 
            });

            bw.RunWorkerAsync();
        }

        private void runTasksThreaded()
        {
            Console.WriteLine("Searching Files in " + folderPath);

            string[] files = Directory.GetFiles(folderPath, "*.exe.csv", SearchOption.AllDirectories);
            int numFiles = files.Length;
            int numFinished = 0;

            foreach (var file in files)
            {
                new Thread(() => 
                {
                    Thread.CurrentThread.IsBackground = true; 

                    Console.WriteLine("Processing: " + file);
                    LogAnalyzer l = new LogAnalyzer();
                    initAnalyzerSettings(l);


                    l.analyzeLog(file, getRunDirectory(runName));
                }).Start();

                numFinished++;
                int progress = Convert.ToInt32((100 / (double)numFiles) * numFinished);
                labelProgress.Text = numFinished + " / " + numFiles + "  ( " + progress + "% )";
                this.progressBar.Value = progress;
            }


            if (!Directory.Exists(Application.StartupPath + "\\Results"))
                Directory.CreateDirectory(Application.StartupPath + "\\Results");
        }

        private async void run()
        {
            Console.WriteLine("Searching Files in " + folderPath);
            
            string[] files = Directory.GetFiles(folderPath, "*.exe.csv", SearchOption.AllDirectories);
            int numFiles = files.Length;
            int numFinished = 0;

            // determine number of logical CPUs
            int numCPUs = Environment.ProcessorCount;

            // number of Tasks (1 or CPU/2)
            int numTasks = 1;
            if (numCPUs > 1)
                numTasks = numCPUs / 2;

            // number of partitions
            int partitionSize = numFiles / numTasks;
            partitionSize = (partitionSize <= 0) ? 1 : partitionSize;

            Console.WriteLine("NumCPUs:" + numCPUs + ", NumTasks: " + numTasks + ", NumFiles: " + numFiles + ", PartitionSize: " + partitionSize);

            // partition source
            var partitions = files.Select((i, index) => new
            {
                i,
                index
            }).GroupBy(group => group.index / partitionSize, element => element.i);

            List<Task> tasks = new List<Task>();

            foreach (var group in partitions)
            {
                Console.WriteLine("Group: {0}", group.Key);

                Task t = Task.Run(() =>
                {
                    runAnalysisTask(group);
                });
                tasks.Add(t);
            }

            Task.WaitAll(tasks.ToArray());
            foreach (Task t in tasks)
                Console.WriteLine("Task {0} Status: {1}", t.Id, t.Status);


            if (!Directory.Exists(Application.StartupPath + "\\Results"))
                Directory.CreateDirectory(Application.StartupPath + "\\Results");

            /* copy file
            String modelFile = Utility.getRunDirectory(runName) + "\\TimeData.csv";
            String modelFileCopy = (Application.StartupPath + "\\Results\\Model-" + timeStepMS + "ms-" + minNumSamples + "min-" + numSamples + "s-" + runName + ".csv");

            Console.WriteLine("Source Length:" + modelFile.Length + ", String: " + modelFile);
            Console.WriteLine("Dest Length:" + modelFileCopy.Length + ", String: " + modelFileCopy);

            try
            {
                File.Copy(modelFile, modelFileCopy);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed to Copy Model File.");
            }*/

            // run analysis
            /*
            for (int i = 0; i < numFiles; i++)
            {
                if (this.cancel)
                    break;

                await Task.Run(() => 
                    {
                        Console.WriteLine("log:  " + files[i]); 
                        logAnalyzer.analyzeLog(files[i], Utility.getRunDirectory(runName));
                    });
                
                Console.WriteLine("Processing " + files[i]);
                this.labelCurrentFile.Text = files[i];

                numFinished++;
                int progress = Convert.ToInt32((100 / (double)numFiles) * numFinished);
                labelProgress.Text = numFinished + " / " + numFiles + "  ( " + progress + "% )";
                this.progressBar.Value = progress;
            }

            this.buttonCancel.Enabled = false;

            if (!Directory.Exists(Application.StartupPath + "\\Results"))
                Directory.CreateDirectory(Application.StartupPath + "\\Results");

            String modelFile = Utility.getRunDirectory(runName) + "\\TimeData.csv";
            String modelFileCopy = (Application.StartupPath + "\\Results\\Model-" + timeStepMS + "ms-" + minNumSamples + "min-" + numSamples + "s-" + runName + ".csv");

            Console.WriteLine("Source Length:" + modelFile.Length + ", String: " + modelFile);
            Console.WriteLine("Dest Length:" + modelFileCopy.Length + ", String: " + modelFileCopy);

            try
            {
                File.Copy(modelFile, modelFileCopy);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed to Copy Model File.");
            }

            if (this.cancel)
            {
                Console.WriteLine("Run Canceled: " + runName);
                this.Text = "Instance Canceled: " + runName;
            }
            else
            {
                Console.WriteLine("Run Finished: " + runName);
                this.Text = "Instance Complete: " + runName;
            }*/
        }

        private string getRunDirectory(String runName)
        {
            return this.outputDirectory + "\\Analyzer\\" + runName;
        }

        private void buttonOpenFolder_Click(object sender, EventArgs e)
        {
            try
            {
                Process.Start(@getRunDirectory(runName));
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed to open Folder: " + ex.Message);
            }
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            this.Close();
            Application.Exit();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(this, "Are you sure?", "Cancel Run", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
            {
                this.cancel = true;
                this.buttonCancel.Text = "Canceling..";
                this.Text = "Canceling.." + runName;
                this.buttonCancel.Enabled = false;

            }
        }
    }
}

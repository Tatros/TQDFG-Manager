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

namespace StatsAnalyzer
{
    public partial class InstanceProgressForm : Form
    {
        private String folderPath = "";
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

        public InstanceProgressForm(String runName, String folderPath, long memoryMaxBytesAllowed, 
            int minNumSamples, 
            String missingValueIdentifier, 
            QDFGGraphManager.Settings.ModelDataType dataModel, 
            QDFGGraphManager.Settings.TimeDataType timeModel,
            String numberFormat,
            int numSamples,
            int timeStepMS
            )
        {
            InitializeComponent();

            // init variables
            this.runName = runName;
            this.folderPath = folderPath;
            this.MemoryMaxBytesAllowed = memoryMaxBytesAllowed;
            this.minNumSamples = minNumSamples;
            this.MissingValueIdentifier = missingValueIdentifier;
            this.dataModel = dataModel;
            this.timeModel = timeModel;
            this.numberFormat = numberFormat;
            this.numSamples = numSamples;
            this.timeStepMS = timeStepMS;

            // init Analyzer
            this.init();

            // run Instance
            //this.run();
            //Task.Run(this.runTasksSync());
            //this.runTasksSync();
            this.runTasksBackgroundSequential();
        }

        private void init()
        {
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

                logAnalyzer.analyzeLog(item, Utility.getRunDirectory(runName));
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


                l.analyzeLog(file, Utility.getRunDirectory(runName));
                

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
                    Console.WriteLine("Processing: " + file);
                    LogAnalyzer l = new LogAnalyzer();
                    initAnalyzerSettings(l);

                    l.analyzeLog(file, Utility.getRunDirectory(runName));

                    numFinished++;
                    int progress = Convert.ToInt32((100 / (double)numFiles) * numFinished);
                    b.ReportProgress(progress);
                }

            });

            // what to do when progress changed (update the progress bar for example)
            bw.ProgressChanged += new ProgressChangedEventHandler(
            delegate(object o, ProgressChangedEventArgs args)
            {
                labelProgress.Text = numFinished + " / " + numFiles + "  ( " + args.ProgressPercentage + "% )";
                this.progressBar.Value = args.ProgressPercentage;
            });

            // what to do when worker completes its task (notify the user)
            bw.RunWorkerCompleted += new RunWorkerCompletedEventHandler(
            delegate(object o, RunWorkerCompletedEventArgs args)
            {
                this.Text = "Instance Complete: " + runName;
                this.Close();
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


                    l.analyzeLog(file, Utility.getRunDirectory(runName));
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

        private void buttonOpenFolder_Click(object sender, EventArgs e)
        {
            try
            {
                Process.Start(@Utility.getRunDirectory(runName));
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed to open Folder: " + ex.Message);
            }
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(this, "Are you sure?", "Cancel Run", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
            {
                this.cancel = true;
            }
        }
    }
}

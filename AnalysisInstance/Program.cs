using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace AnalysisInstance
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            try
            {
                Application.Run(new InstanceProgressForm());
            }

            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
                string[] args = Environment.GetCommandLineArgs();
                
                if (args.Length >= 1)
                    File.WriteAllText(Application.StartupPath + "\\logs\\Instance-" + args[1] + ".txt", "Analysis Instance \"" + args[1] + "\" failed:\n" + ex.Message);
                else
                    File.WriteAllText(Application.StartupPath + "\\logs\\Instance-unk_" + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss") + ".txt", "Analysis Instance failed:\n" + ex.Message);

                Application.Exit();
            }
            // Application.Run(new InstanceProgressForm());
        }
    }
}

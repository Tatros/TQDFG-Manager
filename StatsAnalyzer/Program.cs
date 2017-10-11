using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StatsAnalyzer
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            try
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Settings.outputDirectory = Application.StartupPath + "\\GeneratorOutput";
                if (!System.IO.Directory.Exists(Settings.outputDirectory))
                {
                    System.IO.Directory.CreateDirectory(Settings.outputDirectory);
                }
                Application.Run(new Form1());
            }

            catch (NotImplementedException)
            {
                MessageBox.Show("This functionality is not yet implemented.");
            }
        }
    }
}

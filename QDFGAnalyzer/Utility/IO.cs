using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using de.tum.i22.monitor.malware.applications;
using de.tum.i22.monitor.structures;

using NLog;

namespace QDFGGraphManager.Utility
{
    internal static class IO
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        public static List<String> getActiveEventLogFilePaths()
        {
            // List of file paths of files present in active event log directory
            try
            {
                List<String> files = Directory.EnumerateFiles(Settings.activeEventLogDirectory).ToList();
                return files;
            }

            catch (DirectoryNotFoundException)
            {
                return null;
            }
        }

        public static DirectoryInfo createOutputDirectory(String name)
        {
            try
            {
                return Directory.CreateDirectory(Settings.outputDirectory + "\\" + name);
            }
            catch (Exception e)
            {
                logger.Error("Failed to create output directory: " + e.Message);
                return null;
            }
        }

        public static String getFileNameFromPath(String path)
        {
            int start = path.LastIndexOf('\\') + 1;
            String name = path.Substring(start);
            if (name.Length > 70)
                return path.Substring(start, 70);
            else
                return name;
        }
    }
}

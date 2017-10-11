using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NLog;

namespace QDFGGraphManager.EventLogProcessor
{
    internal sealed class EventLog
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private String path;

        List<Event> events;

        public EventLog(String pathToEventLogFile)
        {
            this.path = pathToEventLogFile;
            events = new List<Event>();
            collectEvents();
        }

        public EventLog(List<Event> events)
        {
            this.events = events;
        }

        public void appendEvents(List<Event> events)
        {
            this.events.AddRange(events);
        }

        private void collectEvents()
        {
            logger.Info("Parsing File for events: <" + this.path + ">");
            using (StreamReader r = new StreamReader(this.path))
            {
                if (r != null)
                {
                    String line;
                    while ((line = r.ReadLine()) != null)
                    {
                        if (line.StartsWith("event"))
                        {
                            events.Add(new Event(line));
                        }
                    }

                    logger.Info("Finished parsing file. Event Count: " + Events.Count);
                }

                else
                {
                    logger.Debug("No such file: <" + this.path + ">");
                }
            }
        }

        public List<Event> Events
        {
            get
            {
                return this.events;
            }

            set
            {
                this.events = value;
            }
        }

        public bool containsMalwareProcess()
        {
            foreach (Event e in events)
            {
                if (e.EventString.Contains("malware"))
                    return true;
            }

            return false;
        }

        public bool containsGoodwareProcess()
        {
            foreach (Event e in events)
            {
                if (e.EventString.Contains("goodware"))
                    return true;
            }

            return false;
        }

        public bool containsMonitoredProcess()
        {
            foreach (Event e in events)
            {
                if (e.EventString.Contains("malware") || e.EventString.Contains("goodware"))
                    return true;
            }

            return false;
        }

        public void writeToFile(String filename)
        {
            File.WriteAllText(Settings.outputDirectory + "\\" + filename, this.ToString());
        }

        public String Path
        {
            get { return this.path; }
        }

        public override String ToString()
        {
            StringBuilder logStr = new StringBuilder();
            foreach (Event e in Events)
            {
                logStr.Append(e.EventString + "\r\n");
            }

            return logStr.ToString();
        }
    }
}

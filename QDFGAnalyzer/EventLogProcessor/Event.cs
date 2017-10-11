using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NLog;

namespace QDFGGraphManager.EventLogProcessor
{
    internal sealed class Event
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        public static String[] knownProperties = { "time", "timeString", "ProcessName", "PID", "ProcessOwner", "TID", "Key", "SubKey", "SAM" };

        String eventName;
        String processName;
        String processOwner;
        String timeString;
        String eventString;

        Dictionary<String, String> properties;

        long time;

        int pid;
        int tid;
        TimeSpan globalPassed;
        TimeSpan idlePeriod;

        public Event(String eventString)
        {
            // logger.Debug("\tNew Event from String <" + eventString + ">");

            
            this.properties = new Dictionary<string, string>();
            this.eventString = eventString;
            this.eventName = "";
            this.processName = "";
            this.processOwner = "";
            this.timeString = "";
            this.time = -1;
            this.pid = -1;
            this.tid = -1;

            parseEventString();
            processProperties();
            verify();
        }

        private bool verify()
        {
            if (this.time > 0 && this.pid > 0 && this.tid > 0 && this.eventString != "" && this.processName != "" && this.processOwner != "" && this.timeString != "")
            {
                return true;
            }

            else
            {
                /*
                logger.Warn("Basic Event information missing from Event <" + eventName + ">." +
                    "\r\n  Basic Event information: " +
                    "\r\n   <<time, " + this.time + ">," +
                    "\r\n    <pid, " + this.pid + ">," +
                    "\r\n    <tid, " + this.tid + ">," +
                    "\r\n    <processName, " + this.processName + ">," +
                    "\r\n    <processOwner, " + this.processOwner + ">," +
                    "\r\n    <timeString, " + this.timeString + ">>" +
                    "\r\n  in eventString <" + eventString + ">");
                 * */
                return false;
            }
        }

        private String getPropertyValue(String propertyName)
        {
            if (properties != null)
            {
                if (properties.ContainsKey(propertyName))
                {
                    return properties[propertyName];
                }
                else
                {
                    logger.Warn("\t\tEvent <" + eventName + "> does not contain property <" + propertyName + "> in EventString <" + eventString + ">");
                    return "";
                }
            }

            else
            {
                logger.Warn("\t\tEvent <" + eventName + "> does not contain an initialized property list for EventString <" + eventString + ">");
                return "";
            }
        }

        private long parsePropertyLong(String propertyName)
        {
            try
            {
                long temp = long.Parse(getPropertyValue(propertyName));
                return temp;
            }
            catch (FormatException) 
            { 
                logger.Warn("\t\tUnable to parse property <" + propertyName + "> as type <long> from EventString <" + eventString + ">");
                return -1;
            }
        }

        private int parsePropertyInt(String propertyName)
        {
            try
            {
                int temp = int.Parse(getPropertyValue(propertyName));
                return temp;
            }
            catch (FormatException)
            {
                logger.Fatal("\t\tUnable to parse property <" + propertyName + "> as type <int> from EventString <" + eventString + ">");
                return -1;
            }
        }

        private void processProperties()
        {
            this.time = parsePropertyLong("time");
            this.pid = -1; // parsePropertyInt("PID");
            this.tid = -1; // parsePropertyInt("TID");
            this.processName = getPropertyValue("ProcessName");
            this.processOwner = getPropertyValue("ProcessOwner");
            this.timeString = getPropertyValue("timeString");
        }

        private void parseEventString()
        {
            if (eventString.StartsWith("event"))
            {
                String[] eventProperties = eventString.Split('|');

                if (eventProperties != null && eventProperties.Length >= 2)
                {
                    foreach (String property in eventProperties)
                    {
                        if (property != null && property != "") {

                            String[] propertyParts = property.Split('*');

                            if (propertyParts.Length == 2)
                            {
                                String propertyName = propertyParts[0];
                                String propertyValue = propertyParts[1];

                                if (knownProperties.Contains(propertyName))
                                {
                                    if (!properties.ContainsKey(propertyName))
                                    {
                                        properties.Add(propertyName, propertyValue);
                                        //logger.Debug("\t\tAdded Property <" + propertyName + "> with value <" + propertyValue + "> to Event <" + eventName + "> from EventString <" + eventString + ">");
                                    }

                                    else 
                                    {
                                        logger.Warn("\t\tSkipped Duplicate Property <" + propertyName + "> in Event <" + eventName + ">" + " in EventString <" + eventString + ">");
                                    }
                                }

                                else if (propertyName == "event")
                                {
                                    if (eventName == "")
                                    {
                                        eventName = propertyValue;
                                    }

                                    else
                                    {
                                        logger.Warn("Duplicate Event <" + propertyValue + "> in EventString <" + eventString + ">");
                                    }
                                }
                            }
                        }
                    }
                }
            }

            else
            {
                throw new InvalidEventStringException("Failed to parse Event String <" + eventString + ">");
            }
        }

        public TimeSpan GlobalPassed
        {
            get { return this.globalPassed; }
            set { this.globalPassed = value; }
        }

        public TimeSpan IdlePeriod
        {
            get { return this.idlePeriod; }
            set { this.idlePeriod = value; }
        }

        public long Time
        {
            get { return this.time; }
            set { 
                this.time = value;
                this.timeString = DateTime.FromFileTime(value).ToString("MM-dd-yy, H:mm:ss.ffff");
            }
        }

        public String EventString
        {
            get { return this.eventString; }
        }

        public String EventName
        {
            get { return this.eventName; }
        }

        public String ProcessName
        {
            get { return this.processName; }
        }

        public int PID
        {
            get { return this.pid; }
        }

        public String TimeString
        {
            get { return this.timeString; }
            set { this.timeString = value; }
        }
    }
}

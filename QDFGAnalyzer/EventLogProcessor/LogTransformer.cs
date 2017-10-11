using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using de.tum.i22.monitor.malware.applications;
using de.tum.i22.monitor.structures;

using NLog;

namespace QDFGGraphManager.EventLogProcessor
{
    internal sealed class EventLogTransformer
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private static Logger dateFixLogger = LogManager.GetLogger("DateFixer");
        private static Logger logTransformer = LogManager.GetLogger("LogTransformer");
        EventLog log;

        public EventLogTransformer(EventLog log)
        {
            this.log = log;
        }

        /* Nearly all log files exhibit incosistencies in their event time stamps
         * due to one or more time synchronizations taking place during the log.
         * 
         * We are not interested in the absolute time, but only in the 
         * time passed from the beginning of the log and the time differences
         * inbetween events. Therefore we create two properties for each event,
         * GlobalPassed and IdlePeriod. Global Passed is the Time Span from the
         * beginning of the log up to the event. Idle Period is the Time Span lying 
         * between the event and the previous event. For the first event GlobalPassed
         * and IdlePeriod are 0. For following events  we compute IdlePeriod by 
         * subtracting the time of the last event from the time of the current event.
         * 
         * If the absolute duration of this value (Idle Period) is abnormally high 
         * (i.e. larger than Settings.TimeSyncDetect) we can safely assume
         * that a time sync occured at this event. In this exception case,
         * we set the Idle Period to 0 and Global Passed is therefore
         * equal to Global Passed of the previous event.
         * */
        public void fixDates()
        {
            // EventLog origLog = new EventLog(this.log.Path);
            // origLog.writeToFile("origLog.txt");

            Event lastEvent = null;
            dateFixLogger.Debug("Processing: " + log.Path);
            dateFixLogger.Debug("Num Events: " + this.log.Events.Count);
            foreach (Event e in this.log.Events)
            {
                dateFixLogger.Debug("Working on Event:\n" + e.EventString);
                if (lastEvent == null)
                {
                    e.GlobalPassed = TimeSpan.FromMilliseconds(0);
                    e.IdlePeriod = TimeSpan.FromMilliseconds(0);
                    dateFixLogger.Debug("This is first Event. No <Last Event>: Setting GlobalPassed & IdlePeriod to 0");
                }

                else
                {
                    DateTime eventTime = DateTime.FromFileTime(e.Time);
                    DateTime lastEventTime = DateTime.FromFileTime(lastEvent.Time);
                    dateFixLogger.Debug("Event occured at time: " + eventTime.ToString());
                    dateFixLogger.Debug("Last Event recorded at time: " + lastEventTime.ToString());
                    e.IdlePeriod = (eventTime - lastEventTime).Duration();
                    e.GlobalPassed = lastEvent.GlobalPassed + e.IdlePeriod;

                    dateFixLogger.Debug("-- Idle Period: " + e.IdlePeriod);
                    dateFixLogger.Debug("-- Global Passed: " + e.GlobalPassed);

                    if (e.IdlePeriod > Settings.timeSyncDetect)
                    {
                        e.IdlePeriod = TimeSpan.FromMilliseconds(0);
                        e.GlobalPassed = lastEvent.GlobalPassed;
                        dateFixLogger.Debug("Idle Period too long --> setting idle period to 0 and global passed to last Event");
                        dateFixLogger.Debug("-- Idle Period: " + e.IdlePeriod);
                        dateFixLogger.Debug("-- Global Passed: " + e.GlobalPassed);
                    }
                }

                lastEvent = e;
            }

            /**** Debug Section ****/
            /*
            StringBuilder origEvents = new StringBuilder();
            foreach (Event e in origLog.Events)
            {
                DateTime eventTime = DateTime.FromFileTime(e.Time);
                // Console.WriteLine(eventTime.ToString("MM-dd-yy, H:mm:ss.ffff") + ", " + e.EventName + ", " + e.ProcessName);
                origEvents.AppendLine(eventTime.ToString("MM-dd-yy, H:mm:ss.ffff") + ", " + e.EventName + ", " + e.ProcessName);
            }
            File.WriteAllText(Settings.outputDirectory + "\\" + Utility.getFileNameFromPath(this.log.Path) + "\\" + "origEvents.txt", origEvents.ToString());
            */

            StringBuilder fixedEvents = new StringBuilder();
            foreach (Event e in log.Events)
            {
                DateTime eventTime = DateTime.FromFileTime(e.Time);
                // Console.WriteLine(eventTime.ToString("MM-dd-yy, H:mm:ss.ffff") + ", " + e.EventName + ", " + e.ProcessName);
                fixedEvents.AppendLine(e.GlobalPassed + ",\t\t\t" + e.IdlePeriod + ",\t\t\t" + e.EventName + ", " + e.ProcessName + ",\t(" + eventTime.ToString("MM-dd-yy, H:mm:ss.ffff") + ")");
            }
            File.WriteAllText(Settings.outputDirectory + "\\" + Utility.IO.getFileNameFromPath(this.log.Path) + "\\" + "fixedEvents.txt", fixedEvents.ToString());
        }

        public List<EventLog> splitLogByTimeAdditive(int timeStepMS)
        {
            List<EventLog> listOfEventLogs = new List<EventLog>();
            logger.Warn("Performing combined split & merge on log using " + timeStepMS + " ms.");

            // The time interval for logs is defined by the given time span in ms
            TimeSpan timeStep = TimeSpan.FromMilliseconds(timeStepMS);
            TimeSpan currentInterval = TimeSpan.FromMilliseconds(timeStepMS);
            TimeSpan maxInterval = log.Events.Last().GlobalPassed;

            logTransformer.Debug("Processing: " + this.log.Path);
            logTransformer.Debug("Selected Time Step: " + timeStep.ToString());
            logTransformer.Debug("Max Interval (Last Event Global Passed): " + maxInterval.TotalSeconds + "s, " + maxInterval);
            // Console.WriteLine("Event " + log.Events.Last().EventString);

            // Iterate over all Intervals
            while (currentInterval <= maxInterval)
            {
                logTransformer.Debug("Interval Iteration: " + currentInterval);
                List<Event> eventsCurrentInterval = new List<Event>();

                // Iterate over all Events, add relevant Events to Interval List
                int eventsRelCount = 0;
                foreach (Event e in this.log.Events)
                {
                    if (e.GlobalPassed <= currentInterval)
                    {
                        eventsCurrentInterval.Add(e);
                        if (e.EventString.Contains("malware") || e.EventString.Contains("goodware"))
                        {
                            eventsRelCount++;
                        }
                    }
                    else
                    {
                        logTransformer.Debug("Done. Events in Interval: " + eventsCurrentInterval.Count + "; Relevant: " + eventsRelCount);

                        break;
                    }
                }

                // Add Interval List to list of logs
                if (eventsRelCount > 0)
                    listOfEventLogs.Add(new EventLog(eventsCurrentInterval));

                // Increase current Interval by timeStep
                currentInterval += timeStep;
            }

            return listOfEventLogs;
        }

        public List<EventLog> splitAndMerge(int numEvents)
        {
            List<EventLog> listOfEventLogs = new List<EventLog>();
            logger.Warn("Performing combined split & merge on log using " + numEvents + " events.");
            // Todo: Sort out duplicate code, see above
            if (numEvents > 0)
            {
                for (int i = 0; i < log.Events.Count; i += numEvents)
                {
                    List<Event> eventPart = new List<Event>();
                    try
                    {
                        eventPart = this.log.Events.GetRange(i, numEvents);
                    }

                    catch (ArgumentException)
                    {
                        // logger.Debug("Could not get range: " + i + "-" + (i+numEvents));
                        eventPart = log.Events.GetRange(i, log.Events.Count - i);
                        // logger.Debug("trying: " + i + "-" + (log.Events.Count) + " yielded " + eventPart.Count + " Results");
                    }


                    if (eventPart.Count > 0)
                    {
                        if (listOfEventLogs.Count > 0)
                        {
                            List<Event> nextEvents = new List<Event>(listOfEventLogs.Last().Events);
                            nextEvents.AddRange(eventPart);
                            listOfEventLogs.Add(new EventLog(nextEvents));
                        }

                        else
                        {
                            listOfEventLogs.Add(new EventLog(eventPart));
                        }

                        logger.Debug("\tAdded " + eventPart.Count + " Events to SubLog (" + i  + " to " + (i+numEvents) + "). Number of SubLogs: " + listOfEventLogs.Count);
                    }
                }

                return listOfEventLogs;
            }

            else
            {
                logger.Error("\tTried to split log with parameter <numEvents> <= 0");
                return null;
            }
        }

        public DFTGraph generateGraphFromLog()
        {
            logger.Info("Generating QDFG for Log");
            try
            {
                DFTGraph g = Utility.Graph.generateGraphFromString(this.log.ToString());
                return g;
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                return null;
            }
        }
    }
}

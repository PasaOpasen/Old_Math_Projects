using System;

using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Threading;
using System.IO;

namespace Memory.Timers
{
    static class Timer
    {
        public static StringBuilder destination;

        static Timer()
        {
            destination = new StringBuilder();
        }

        public static string lastTimerName; 
        public static bool lastWasStart = false;
        public static bool firstRecord = true;
        public static DateTime lastReportTime;
        public static int count = 0;
        public static int sum = 0;

        public static void Write(string name, bool start, int level)
        {
            DateTime dt = DateTime.Now;
            bool p = false;
            if (count == 0)
            {
                lastTimerName = null;
                destination = new StringBuilder();
                sum = 0;
                firstRecord = true;
            }
            if (start) count++; else count--;

            if (lastTimerName != null)
            {
                string n = "";
                if (start && lastWasStart) { p = true; n = lastTimerName; }
                if (!start && lastWasStart && name.Equals(lastTimerName)) { p = true; n = name; }
                if (!start && !lastWasStart) { p = true; n = "Rest"; level++; }

                if (p)
                {
                    sum += (dt - lastReportTime).Milliseconds;
                    destination.Append((level > 0 ? "".PadRight(4 * level - 4) : "") + n.PadRight(20 - 4 * level + 4) + ": " + (firstRecord ? "&" : (dt - lastReportTime).Milliseconds.ToString()) + "\n");
                    firstRecord = false;
                }
            }

            lastWasStart = start;
            lastReportTime = dt;
            lastTimerName = name;
        }


        public static string Report=>destination.ToString().Replace("&", sum.ToString());


        public static RecordReport Start(string timerName = null)
        {
            if (timerName != null)
            {
                Write(timerName, true, count);
                return new RecordReport(timerName, count);
            }
            else
                destination.Append("*".PadRight(20) + ": " + (0).ToString() + "\n");
            return null;
        }

        public class RecordReport : IDisposable
        {
            readonly string name;
            readonly int level;
            public RecordReport(string name, int level)
            {
                this.name = name;
                this.level = level;
            }
            public void Dispose()
            {
                Write(name, false, level);
            }
        }
    }
}
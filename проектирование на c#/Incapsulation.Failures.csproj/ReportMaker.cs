using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Incapsulation.Failures
{
    public struct Failure
    {
        public enum FailureType { yes,no}
        int i;
        public bool IsFailureSerious => (i % 2 == 0);
        public Failure(int a) => i = a;
    }

    public struct Device
    {
        public int DeviceId;
        public string Name;
        public Device(int id, string name)
        {
            DeviceId = id;
            Name = name;
        }
    }

    public class Common
    {
        public static bool Earlier(DateTime v, DateTime date)
        {
            return v < date;
            int vYear = v.Year;
            int vMonth = v.Month;
            int vDay = v.Day;
            
            if (vYear < date.Year) return true;
            if (vYear > date.Year) return false;
            if (vMonth < date.Month) return true;
            if (vMonth > date.Month) return false;
            if (vDay < date.Day) return true;
            return false;
        }
    }

    public class ReportMaker
    {
        /// <summary>
        /// </summary>
        /// <param name="day"></param>
        /// <param name="failureTypes">
        /// 0 for unexpected shutdown, 
        /// 1 for short non-responding, 
        /// 2 for hardware failures, 
        /// 3 for connection problems
        /// </param>
        /// <param name="deviceId"></param>
        /// <param name="times"></param>
        /// <param name="devices"></param>
        /// <returns></returns>
        public static List<string> FindDevicesFailedBeforeDateObsolete(
            int day,
            int month,
            int year,
            int[] failureTypes,
            int[] deviceId,
            object[][] times,
            List<Dictionary<string, object>> devices)
        {
            Failure[] MasToType(int[] arr)
            {
                Failure[] m = new Failure[arr.Length];
                for (int i = 0; i < m.Length; i++)
                    m[i] = new Failure(failureTypes[i]);
                return m;
            }
            DateTime[] GetTimes()
            {
                DateTime[] mas = new DateTime[times.GetLength(0)];
                for (int i = 0; i < mas.Length; i++)
                    mas[i] = new DateTime((int)times[i][2], (int)times[i][1], (int)times[i][0]);
                return mas;
            }

            Device[] dev = new Device[devices.Count];
            for (int i = 0; i < dev.Length; i++)
                dev[i] = new Device((int)devices[i]["DeviceId"], (string)devices[i]["Name"]);

            return FindDevicesFailedBeforeDate(new DateTime(year, month, day), MasToType(failureTypes), GetTimes(), dev);
        }

        public static List<string> FindDevicesFailedBeforeDate(
    DateTime date,
    Failure[] failureTypes,
    DateTime[] times,
    Device[] devices)
        {

            var problematicDevices = new HashSet<int>();
            for (int i = 0; i < failureTypes.Length; i++)
                if (failureTypes[i].IsFailureSerious && Common.Earlier(times[i], date))
                    problematicDevices.Add(devices[i].DeviceId);

            var result = new List<string>();
            foreach (var device in devices)
                if (problematicDevices.Contains(device.DeviceId))
                    result.Add(device.Name);

            return result;
        }

    }
}

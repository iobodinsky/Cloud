using System;

namespace Cloud.Common.Managers
{
    public class IdGenerator
    {
        public string ForFile()
        {
            var dateTime = DateTime.Now;
            return string.Format("{0}{1}{2}{3}{4}{5}",
                dateTime.Year, dateTime.Month, dateTime.Day, dateTime.Hour, dateTime.Second, dateTime.Millisecond);
        }

        public string ForFolder()
        {
            var dateTime = DateTime.Now;
            return string.Format("{0}{1}{2}{3}{4}{5}",
                dateTime.Year, dateTime.Month, dateTime.Day, dateTime.Hour, dateTime.Second, dateTime.Millisecond);
        }
    }
}
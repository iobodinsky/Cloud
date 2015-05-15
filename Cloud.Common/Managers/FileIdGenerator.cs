using System;

namespace Cloud.Common.Managers
{
    public class FileIdGenerator
    {
        public string Get()
        {
            var dateTime = DateTime.Now;
            return string.Format("{0}{1}{2}{3}{4}{5}", 
                dateTime.Year, dateTime.Month, dateTime.Day, dateTime.Hour, dateTime.Second, dateTime.Millisecond);
        }
    }
}

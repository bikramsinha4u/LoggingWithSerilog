using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;

namespace LoggingWithSerilog
{
    public class PerfTracker
    {
        private readonly Stopwatch _sw;
        private readonly LogDetails _logDetails;

        public PerfTracker(string name, string userId, string userName, 
            string location, string product, string layer)
        {
            _sw = Stopwatch.StartNew();
            _logDetails = new LogDetails()
            {
                Message = name,
                UserId = userId,
                UserName = userName,
                Product = product,
                Layer = layer,
                Location = location,
                Hostname = Environment.MachineName
            };

            var beginTime = DateTime.Now;
            _logDetails.AdditionalInfo = new Dictionary<string, object>()
            {
                {"Started", beginTime.ToString(CultureInfo.InvariantCulture) }
            };
        }

        public PerfTracker(string name, string userId, string userName, 
            string location, string product, string layer, Dictionary<string, object> perfParams)
            :this(name, userId, userName, location, product, layer)
        {
            foreach (var item in perfParams)
            {
                _logDetails.AdditionalInfo.Add("input-" + item.Key, item.Value);
            }
            
        }

        public void Stop()
        {
            _sw.Stop();
            _logDetails.ElapsedMilliSeconds = _sw.ElapsedMilliseconds;
            SerilogLogger.WritePerfLog(_logDetails);
        }
    }
}

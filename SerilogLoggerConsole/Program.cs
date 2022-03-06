using LoggingWithSerilog;
using System;

namespace SerilogLoggerConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            var logDetails = GetLogDetails("Starting application", null);
            SerilogLogger.WriteDiagnosticLog(logDetails);

            var tracker = new PerfTracker("Logger console exceution", "", logDetails.UserName,
                logDetails.Location, logDetails.Product, logDetails.Layer);

            try
            {
                var ex = new Exception("Some exception occured");
                ex.Data.Add("input param", "details");

                throw ex;
            }
            catch (Exception ex)
            {
                logDetails = GetLogDetails("", ex);
                SerilogLogger.WriteErrorLog(logDetails);
            }

            logDetails = GetLogDetails("Used logging console", null);
            SerilogLogger.WriteUsageLog(logDetails);

            logDetails = GetLogDetails("Stopping console application", null);
            SerilogLogger.WriteDiagnosticLog(logDetails);

            tracker.Stop();

            Console.ReadLine(); // To keep the console open
        }

        private static LogDetails GetLogDetails(string message, Exception ex)
        {
            return new LogDetails
            {
                Product = "Product Log",
                Location = " Logger Console",
                Layer = "Job",
                UserName = Environment.UserName,
                Hostname = Environment.MachineName,
                Message = message,
                Exception = ex
            };
        }
    }
}

using System;
using System.Collections.Generic;

namespace LoggingWithSerilog
{
    public class LogDetails
    {
        public LogDetails()
        {
            Timestamp = DateTime.Now;
        }

        // What
        public DateTime Timestamp { get; set; }
        public string Message { get; set; }

        // Where
        public string Product { get; set; }
        public string Layer { get; set; }
        public string Location { get; set; }
        public string Hostname { get; set; }

        // Who
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string CustomerId { get; set; }
        public string CustomerName { get; set; }

        // Everthing else
        public long? ElapsedMilliSeconds { get; set; } // For performance entries
        public Exception Exception { get; set; } // Exception fro error logging
        public string CorrelationId { get; set; } // Exception shielding from server to client
        public IDictionary<string, object> AdditionalInfo { get; set; } // Catch all for anything else
    }
}

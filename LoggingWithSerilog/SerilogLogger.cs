using Serilog;
using Serilog.Events;
using Serilog.Formatting.Compact;
using System;
using System.Configuration;
using System.Reflection;

namespace LoggingWithSerilog
{
    public static class SerilogLogger
    {
        static private readonly string _perfLoggerSink = @"D:\Program\LoggingWithSerilog\LoggingWithSerilog\SerilogFileSinks\PerfLoggerSink.txt";
        static private readonly string _usageLoggerSink = @"D:\Program\LoggingWithSerilog\LoggingWithSerilog\SerilogFileSinks\UsageLoggerSink.txt";
        static private readonly string _errorLoggerSink = @"D:\Program\LoggingWithSerilog\LoggingWithSerilog\SerilogFileSinks\ErrorLoggerSink.txt";
        static private readonly string _diagnosticLoggerSink = @"D:\Program\LoggingWithSerilog\LoggingWithSerilog\SerilogFileSinks\DiagnosticLoggerSink.txt";

        private static readonly ILogger _perfLogger;
        private static readonly ILogger _usageLogger;
        private static readonly ILogger _errorLogger;
        private static readonly ILogger _diagnosticLogger;

        static SerilogLogger()
        {
            var executingAssembly = Assembly.GetExecutingAssembly().GetName();
            
            _perfLogger = new LoggerConfiguration()
                .WriteTo.File(new RenderedCompactJsonFormatter(), path: _perfLoggerSink)
                .CreateLogger();

            _usageLogger = new LoggerConfiguration()

                // Define debug level
                .MinimumLevel.Debug()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)

                .WriteTo.File(path: _usageLoggerSink)
                .CreateLogger();

            _errorLogger = new LoggerConfiguration()
                .WriteTo.File(path: _errorLoggerSink)
                .CreateLogger();

            _diagnosticLogger = new LoggerConfiguration()

                // Enrichers
                .Enrich.FromLogContext()
                .Enrich.WithMachineName()
                .Enrich.WithProperty("Assembly", $"{executingAssembly.Name}")
                .Enrich.WithProperty("Version", $"{executingAssembly.Version}")

                .WriteTo.File(path: _diagnosticLoggerSink)
                .CreateLogger();
        }

        public static void WritePerfLog(LogDetails logDetails)
        {
            _perfLogger.Write(LogEventLevel.Information, "{@LogDetails}", logDetails);
        }

        public static void WriteUsageLog(LogDetails logDetails)
        {
            _usageLogger.Write(LogEventLevel.Information, "{@LogDetails}", logDetails);
        }

        public static void WriteErrorLog(LogDetails logDetails)
        {
            _errorLogger.Write(LogEventLevel.Information, "{@LogDetails}", logDetails);
        }

        public static void WriteDiagnosticLog(LogDetails logDetails)
        {
            var writeDiagnostics = Convert.ToBoolean(ConfigurationManager.AppSettings["EnableDiagnostics"]);

            if (!writeDiagnostics)
            {
                return;
            }

            _diagnosticLogger.Write(LogEventLevel.Information, "{@LogDetails}", logDetails);
        }
    }
}

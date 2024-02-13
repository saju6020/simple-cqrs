namespace Platform.Infrastructure.LogConfiguration.SerilogConfiguration
{
    using System.IO;
    using Newtonsoft.Json;
    using Serilog.Events;
    using Serilog.Formatting;

    public class AwsCustomTextFormatter : ITextFormatter
    {
        public void Format(LogEvent logEvent, TextWriter output)
        {
            output.Write("Timestamp - {0} | Level - {1} | Message {2} {3} {4}", logEvent.Timestamp, logEvent.Level, logEvent.MessageTemplate, JsonConvert.SerializeObject(logEvent.Properties), output.NewLine);
            if (logEvent.Exception != null)
            {
                output.Write("Exception - {0}", logEvent.Exception);
            }
        }
    }
}

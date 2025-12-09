using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.Extensibility;
using System.Runtime.CompilerServices;
using vynce_api.Model;

namespace cp.common
{
    public static class AppTrace
    {
        private static readonly TelemetryClient _telemetryClient = new TelemetryClient(TelemetryConfiguration.CreateDefault());
        const int FLUSH_INTERVAL = 10000;
        private static int _currentAttemptFlushCount = 0;
        static AppTrace()
        {
            _telemetryClient.InstrumentationKey = ApplicationConfigurations.InstrumentationKey;
        }
        public static void Verbose(string message, int id = 16, [CallerMemberName] string memberName = "", [CallerFilePath] string filePath = "", [CallerLineNumber] int lineNumber = 0)
        {
            _telemetryClient.TrackTrace(message, SeverityLevel.Verbose, GetCustomData(memberName, null, null, $"{filePath} - Line {lineNumber}"));
            AttemptFlush();
        }

        public static void Error(Exception exception, string sourceMessage = null, string sourceDeviceId = null, string notes = null, [CallerFilePath] string filePath = "", [CallerLineNumber] int lineNumber = 0, string methodName = "", string channelId = null)
        {
            _telemetryClient.TrackException(exception, GetCustomData(sourceMessage, sourceDeviceId, notes, $"{filePath} - Line {lineNumber}", methodName, channelId: channelId), null);
            AttemptFlush();
        }

        public static void Information(string message, int id = 8, [CallerMemberName] string memberName = "", [CallerFilePath] string filePath = "", [CallerLineNumber] int lineNumber = 0)
        {
            _telemetryClient.TrackTrace(message, SeverityLevel.Information, GetCustomData(memberName, null, null, $"{filePath} - Line {lineNumber}"));
            AttemptFlush();
        }

        public static void Critical(Exception exception, string sourceMessage = null, string sourceDeviceId = null, string notes = null, [CallerFilePath] string filePath = "", [CallerLineNumber] int lineNumber = 0)
        {
            _telemetryClient.TrackException(exception, GetCustomData(sourceMessage, sourceDeviceId, notes, $"{filePath} - Line {lineNumber}"), null);
            AttemptFlush();
        }

        public static void Warning(string message, int id = 4, [CallerMemberName] string memberName = "", [CallerFilePath] string filePath = "", [CallerLineNumber] int lineNumber = 0)
        {
            _telemetryClient.TrackTrace(message, SeverityLevel.Warning, GetCustomData(memberName, null, null, $"{filePath} - Line {lineNumber}"));
            AttemptFlush();
        }

        public static void Start(string service, int id = 256, [CallerMemberName] string memberName = "", [CallerFilePath] string filePath = "", [CallerLineNumber] int lineNumber = 0)
        {
            _telemetryClient.TrackTrace("Starting - " + service, SeverityLevel.Information, GetCustomData(memberName, null, null, $"{filePath} - Line {lineNumber}"));
            AttemptFlush();
        }

        public static void Stop(string service, int id = 512, [CallerMemberName] string memberName = "", [CallerFilePath] string filePath = "", [CallerLineNumber] int lineNumber = 0)
        {
            _telemetryClient.TrackTrace("Stopped - " + service, SeverityLevel.Information, GetCustomData(memberName, null, null, $"{filePath} - Line {lineNumber}"));
            AttemptFlush();
        }

        public static void TrackRequest(string message, DateTimeOffset startTime, TimeSpan duration, bool status)
        {
            _telemetryClient.TrackRequest(message, startTime, duration, "200", status);
            AttemptFlush();
        }

        private static Dictionary<string, string> GetCustomData(string source = null, string deviceId = null, string notes = null, string logCodeLine = null, string methodName = null, string channelId = null)
        {
            var properties = new Dictionary<string, string>
            {
                { "App Name", "Windows" }
            };
            if (!String.IsNullOrWhiteSpace(deviceId))
                properties.Add("Entity Identifier", deviceId);
            if (!String.IsNullOrWhiteSpace(source))
                properties.Add("Source", source);
            if (!String.IsNullOrWhiteSpace(notes))
                properties.Add("Notes", notes);
            if (!String.IsNullOrWhiteSpace(logCodeLine))
                properties.Add("Log Code Line", logCodeLine);
            if (!String.IsNullOrWhiteSpace(methodName))
                properties.Add("Method Name", methodName);
            if (!String.IsNullOrWhiteSpace(channelId))
                properties.Add("Channel Id", channelId);

            return properties;
        }

        private static void AttemptFlush()
        {
            if (_currentAttemptFlushCount > FLUSH_INTERVAL)
            {
                _telemetryClient.Flush();
                _currentAttemptFlushCount = 0;
            }
            else
            {
                _currentAttemptFlushCount++;
            }
        }
    }
}

using System;
using System.Globalization;
using System.Security.Claims;
using System.Threading;
using NLog;

namespace HRTools.Crosscutting.Common.Logging
{
    public class MySqlNLogLogger : ILogger
    {
        private const string InvalidUserId = "0";

        private readonly NLog.ILogger _logger;

        public MySqlNLogLogger()
        {
            _logger = LogManager.GetCurrentClassLogger();
        }

        public void Debug(string message)
        {
            Guard.ArgumentIsNotNullOrEmpty(message, nameof(message));

            string typeName = GetType().FullName;

            Log(LogLevel.Debug, typeName, message);
        }

        public void Info(string message)
        {
            Guard.ArgumentIsNotNullOrEmpty(message, nameof(message));

            string typeName = GetType().FullName;

            Log(LogLevel.Info, typeName, message);
        }

        public void Trace(string message)
        {
            Guard.ArgumentIsNotNullOrEmpty(message, nameof(message));

            string typeName = GetType().FullName;

            Log(LogLevel.Trace, typeName, message);
        }

        public void Warning(string message)
        {
            Guard.ArgumentIsNotNullOrEmpty(message, nameof(message));

            string typeName = GetType().FullName;

            Log(LogLevel.Warn, typeName, message);
        }

        public void Error(Exception exception, string message = null)
        {
            string typeName = GetType().FullName;

            message = message == null
                ? $"Exception: {exception}"
                : string.Concat(message, $" Exception: {exception}");
           
            Log(LogLevel.Error, typeName, message);
        }

        private void Log(LogLevel info, string typeName, string message)
        {
            LogEventInfo eventInfo = new LogEventInfo(info, typeName, message)
            {
                TimeStamp = DateTime.UtcNow
            };

            SetEventInfoProperties(eventInfo);

            _logger.Log(eventInfo);
        }

        private static void SetEventInfoProperties(LogEventInfo logEventInfo)
        {
            logEventInfo.Properties["UserId"] = GetCurrentUserId();
            logEventInfo.Properties["ThreadId"] = Thread.CurrentThread.ManagedThreadId.ToString(CultureInfo.InvariantCulture);
        }

        private static string GetCurrentUserId()
        {
            if (!Thread.CurrentPrincipal.Identity.IsAuthenticated)
            {
                return InvalidUserId;
            }

            ClaimsIdentity userIdentity = Thread.CurrentPrincipal.Identity as ClaimsIdentity;

            if (userIdentity == null)
            {
                return InvalidUserId;
            }

            Claim userId = userIdentity.FindFirst(ClaimTypes.NameIdentifier);

            return userId != null ? userId.Value : InvalidUserId;
        }
    }
}

using System;

namespace HRTools.Crosscutting.Common.Logging
{
    public interface ILogger
    {
        void Debug(string message);

        void Info(string message);

        void Trace(string message);

        void Warning(string message);
        
        void Error(Exception exception, string message = null);
    }
}

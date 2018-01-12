using System;

namespace HRTools.Crosscutting.Common.Exceptions
{
    public class ValidationException : Exception
    {
        public string DisplayMessage { get; set; }
        
        public ValidationException(string displayMessage, string internalMessage = null)
            : base(internalMessage ?? displayMessage)
        {
            DisplayMessage = displayMessage;
        }

        public ValidationException(string message, Exception exception)
            :base(message, exception)
        {
            
        }
    }
}

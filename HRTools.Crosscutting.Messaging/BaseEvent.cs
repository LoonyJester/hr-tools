using System;

namespace HRTools.Crosscutting.Messaging
{
    public abstract class BaseEvent
    {
        public Guid? EventId { get; set; }
    }
}

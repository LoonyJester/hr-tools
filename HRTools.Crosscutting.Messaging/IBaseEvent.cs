using System;

namespace HRTools.Crosscutting.Messaging
{
    public interface IBaseEvent
    {
        Guid? EventId { get; set; }
    }
}

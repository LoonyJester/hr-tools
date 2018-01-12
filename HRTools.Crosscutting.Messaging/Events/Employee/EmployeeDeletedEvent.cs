using System;

namespace HRTools.Crosscutting.Messaging.Events.Employee
{
    public class EmployeeDeletedEvent: BaseEvent, IEmployeeDeletedEvent
    {
        public Guid EmployeeId { get; set; }
    }
}

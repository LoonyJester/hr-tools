using System;

namespace HRTools.Crosscutting.Messaging.Events.Employee
{
    public class EmployeeCreatedEvent: BaseEvent, IEmployeeCreatedEvent
    {
        public Guid EmployeeId { get; set; }

        public string FullName { get; set;  }

        public string JobTitle { get; set; }

        public string Technology { get; set; }

        public DateTime StartDate { get; set; }
    }
}

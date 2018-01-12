using System;

namespace HRTools.Crosscutting.Messaging.Events.Employee
{
    public interface IEmployeeUpdatedEvent : IBaseEvent
    {
        Guid EmployeeId { get; }

        string FullName { get; }

        string JobTitle { get; set; }

        string Technology { get; set; }

        DateTime StartDate { get; }
    }
}

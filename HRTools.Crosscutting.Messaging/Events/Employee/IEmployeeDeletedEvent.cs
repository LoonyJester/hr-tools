using System;

namespace HRTools.Crosscutting.Messaging.Events.Employee
{
    public interface IEmployeeDeletedEvent : IBaseEvent
    {
        Guid EmployeeId { get; }
    }
}

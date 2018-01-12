using System.Threading.Tasks;
using HRTools.Crosscutting.Common;
using HRTools.Crosscutting.Messaging.Events.Employee;
using HRTools.ProjectAssignment.Repositories.Employee;
using MassTransit;

namespace HRTools.ProjectAssignment.Handlers.Employee
{
    public class DeleteEmployeeConsumer: IConsumer<IEmployeeDeletedEvent>
    {
        private readonly IEmployeeRepositoryAdmin _employeeRepositoryAdmin;

        public DeleteEmployeeConsumer(
            IEmployeeRepositoryAdmin employeeRepositoryAdmin)
        {
            Guard.ConstructorArgumentIsNotNull(employeeRepositoryAdmin, nameof(employeeRepositoryAdmin));

            _employeeRepositoryAdmin = employeeRepositoryAdmin;
        }

        public Task Consume(ConsumeContext<IEmployeeDeletedEvent> context)
        {
            Guard.ArgumentIsNotNull(context, nameof(context));
            Guard.ArgumentIsNotNull(context.Message, nameof(context.Message));
            Guard.ArgumentIsNotNull(context.Message.EmployeeId, nameof(context.Message.EmployeeId));

            return _employeeRepositoryAdmin.DeleteAsync(context.Message.EmployeeId);
        }
    }
}

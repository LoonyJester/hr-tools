using System.Threading.Tasks;
using AutoMapper;
using HRTools.Crosscutting.Common;
using HRTools.Crosscutting.Messaging.Events.Employee;
using HRTools.ProjectAssignment.Repositories.Employee;
using MassTransit;

namespace HRTools.ProjectAssignment.Handlers.Employee
{
    public class CreateEmployeeConsumer: IConsumer<IEmployeeCreatedEvent>
    {
        private readonly IEmployeeRepositoryAdmin _employeeRepositoryAdmin;

        public CreateEmployeeConsumer(
            IEmployeeRepositoryAdmin employeeRepositoryAdmin,
            IMapper mapper)
        {
            Guard.ConstructorArgumentIsNotNull(employeeRepositoryAdmin, nameof(employeeRepositoryAdmin));

            _employeeRepositoryAdmin = employeeRepositoryAdmin;
        }

        public Task Consume(ConsumeContext<IEmployeeCreatedEvent> context)
        {
            Guard.ArgumentIsNotNull(context, nameof(context));
            Guard.ArgumentIsNotNull(context.Message, nameof(context.Message));

            //EmployeeDto dto = _mapper.Map<Common.Models.Core.Employee, EmployeeDto>(employee);

            IEmployeeCreatedEvent eventBody = context.Message;

            EmployeeDto dto = new EmployeeDto
            {
                EmployeeId = eventBody.EmployeeId,
                FullName = eventBody.FullName,
                JobTitle = eventBody.JobTitle,
                Technology = eventBody.Technology,
                StartDate = eventBody.StartDate,
                //AssignedForInPersentsSum = eventBody.AssignedForInPersentsSum
            };

            return _employeeRepositoryAdmin.CreateAsync(dto);
        }
    }
}

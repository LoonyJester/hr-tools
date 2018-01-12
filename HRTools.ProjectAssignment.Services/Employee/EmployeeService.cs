using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using HRTools.Crosscutting.Common;
using HRTools.ProjectAssignment.Repositories.Employee;

namespace HRTools.ProjectAssignment.Services.Employee
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IMapper _mapper;

        public EmployeeService(
            IEmployeeRepository employeeRepository,
            IMapper mapper)
        {
            Guard.ConstructorArgumentIsNotNull(employeeRepository, nameof(employeeRepository));
            Guard.ConstructorArgumentIsNotNull(mapper, nameof(mapper));

            _employeeRepository = employeeRepository;
            _mapper = mapper;
        }

        #region Public

        public async Task<IEnumerable<Common.Models.Core.Employee>> GetAllByNameAutocompleteAsync(string nameAutocomplete)
        {
            Guard.ArgumentIsNotNullOrWhiteSpace(nameAutocomplete, nameof(nameAutocomplete));

            IEnumerable<EmployeeDto> dtoList = await _employeeRepository.GetAllByNameAutocompleteAsync(nameAutocomplete);

            return _mapper.Map<IEnumerable<EmployeeDto>, IEnumerable<Common.Models.Core.Employee>>(dtoList);
        }

        #endregion
    }
}
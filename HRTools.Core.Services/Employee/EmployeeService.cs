using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using HRTools.Core.Common.Models.Employee;
using HRTools.Core.Repositories.Employee;
using HRTools.Crosscutting.Common;
using HRTools.Crosscutting.Common.Models;

namespace HRTools.Core.Services.Employee
{
    public class EmployeeService : IEmployeeService
    {
        private const string CdnUrl = @"d:\HR Tools\HR_Tools\CDN\";
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IMapper _mapper;

        public EmployeeService(IEmployeeRepository employeeRepository,
            IMapper mapper)
        {
            Guard.ConstructorArgumentIsNotNull(employeeRepository, nameof(employeeRepository));
            Guard.ConstructorArgumentIsNotNull(mapper, nameof(mapper));

            _employeeRepository = employeeRepository;
            _mapper = mapper;
        }

        #region Public

        async Task<GrigData<Common.Models.Employee.Employee>> IEmployeeService.GetAllAsync(EmployeeGridSettings settings)
        {
            Guard.ArgumentIsNotNull(settings, nameof(settings));

            IEnumerable<EmployeeDto> dtoList = await _employeeRepository.GetAllAsync(settings);
            int totalCount = await _employeeRepository.GetTotalCountAsync(settings);

            IEnumerable<Common.Models.Employee.Employee> result = _mapper.Map<IEnumerable<EmployeeDto>, IEnumerable<Common.Models.Employee.Employee>>(dtoList);

            return new GrigData<Common.Models.Employee.Employee>
            {
                Data = result,
                TotalCount = totalCount
            };
        }

        #endregion
    }
}
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using HRTools.Crosscutting.Common;
using HRTools.Crosscutting.Common.Exceptions;
using HRTools.ProjectAssignment.Repositories.Department;

namespace HRTools.ProjectAssignment.Services.Department
{
    public class DepartmentService : IDepartmentService, IDepartmentServiceAdmin
    {
        private readonly IDepartmentRepository _departmentRepository;
        private readonly IDepartmentRepositoryAdmin _departmentRepositoryAdmin;
        private readonly IMapper _mapper;

        public DepartmentService(
            IDepartmentRepository departmentRepository,
            IDepartmentRepositoryAdmin departmentRepositoryAdmin,
            IMapper mapper)
        {
            Guard.ConstructorArgumentIsNotNull(departmentRepository, nameof(departmentRepository));
            Guard.ConstructorArgumentIsNotNull(departmentRepositoryAdmin, nameof(departmentRepositoryAdmin));
            Guard.ConstructorArgumentIsNotNull(mapper, nameof(mapper));

            _departmentRepository = departmentRepository;
            _departmentRepositoryAdmin = departmentRepositoryAdmin;
            _mapper = mapper;
        }

        #region Public

        public async Task<IEnumerable<Common.Models.Department.Department>> GetAllAsync()
        {
            IEnumerable<DepartmentDto> dtoList = await _departmentRepository.GetAllAsync();

            return _mapper.Map<IEnumerable<DepartmentDto>, IEnumerable<Common.Models.Department.Department>>(dtoList);
        }

        #endregion

        #region Admin

        public async Task<IEnumerable<Common.Models.Department.Department>> GetAllAsync(string searchKeyword)
        {
            IEnumerable<DepartmentDto> dtoList = await _departmentRepositoryAdmin.GetAllAsync(searchKeyword);

            return _mapper.Map<IEnumerable<DepartmentDto>, IEnumerable<Common.Models.Department.Department>>(dtoList);
        }

        public async Task<int> CreateAsync(Common.Models.Department.Department department)
        {
            Guard.ArgumentIsNotNull(department, nameof(department));

            await ValdiateIfDepartmentWithSameNameExists(department);

            DepartmentDto dto = _mapper.Map<Common.Models.Department.Department, DepartmentDto>(department);

            return await _departmentRepositoryAdmin.CreateAsync(dto);
        }

        public async Task<int> UpdateAsync(Common.Models.Department.Department department)
        {
            Guard.ArgumentIsNotNull(department, nameof(department));

            await ValdiateIfDepartmentWithSameNameExists(department);

            DepartmentDto dto = _mapper.Map<Common.Models.Department.Department, DepartmentDto>(department);

            return await _departmentRepositoryAdmin.UpdateAsync(dto);
        }

        private async Task ValdiateIfDepartmentWithSameNameExists(Common.Models.Department.Department department)
        {
            DepartmentDto departmentWithSameName = await _departmentRepositoryAdmin.GetByNameAsync(department.Name);

            if (departmentWithSameName != null && departmentWithSameName.Id != department.Id)
            {
                throw new ValidationException("Department with same Name already exists.");
            }
        }

        public Task<int> DeleteAsync(int id)
        {
            Guard.ArgumentIsNotNull(id, nameof(id));

            return _departmentRepositoryAdmin.DeleteAsync(id);
        }

        #endregion
    }
}
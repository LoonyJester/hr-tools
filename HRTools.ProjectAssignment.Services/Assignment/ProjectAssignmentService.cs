using System.Collections.Generic;
using System.Threading.Tasks;
using System.Transactions;
using AutoMapper;
using HRTools.Crosscutting.Common;
using HRTools.Crosscutting.Common.Models;
using HRTools.ProjectAssignment.Common.Models.Assignment;
using HRTools.ProjectAssignment.Repositories.Assignment;

namespace HRTools.ProjectAssignment.Services.Assignment
{
    public class ProjectAssignmentService : IProjectAssignmentService
    {
        private readonly IMapper _mapper;
        private readonly IProjectAssignmentRepository _projectAssignmentRepository;

        public ProjectAssignmentService(IProjectAssignmentRepository projectAssignmentRepository,
            IMapper mapper)
        {
            Guard.ConstructorArgumentIsNotNull(projectAssignmentRepository, nameof(projectAssignmentRepository));
            Guard.ConstructorArgumentIsNotNull(mapper, nameof(mapper));

            _projectAssignmentRepository = projectAssignmentRepository;
            _mapper = mapper;
        }

        public async Task<GrigData<Common.Models.Assignment.ProjectAssignment>> GetAllAsync(ProjectAssignmentGridSettings settings)
        {
            Guard.ArgumentIsNotNull(settings, nameof(settings));

            IEnumerable<ProjectAssignmentDto> dtoList = await _projectAssignmentRepository.GetAllAsync(settings);
            int totalCount = await _projectAssignmentRepository.GetTotalCountAsync(settings);

            IEnumerable<Common.Models.Assignment.ProjectAssignment> result = _mapper.Map<IEnumerable<ProjectAssignmentDto>, IEnumerable<Common.Models.Assignment.ProjectAssignment>>(dtoList);

            return new GrigData<Common.Models.Assignment.ProjectAssignment>
            {
                Data = result,
                TotalCount = totalCount
            };
        }

        public Task<int> CreateAsync(Common.Models.Assignment.ProjectAssignment projectAssignment)
        {
            Guard.ArgumentIsNotNull(projectAssignment, nameof(projectAssignment));
            Guard.ArgumentIsNotNull(projectAssignment.Id, nameof(projectAssignment.Id));
            Guard.ArgumentIsNotNull(projectAssignment.Department.Id, nameof(projectAssignment.Department.Id));

            ProjectAssignmentDto dto = _mapper.Map<Common.Models.Assignment.ProjectAssignment, ProjectAssignmentDto>(projectAssignment);

            return _projectAssignmentRepository.CreateAsync(dto);
        }

        public Task<int> UpdateAsync(Common.Models.Assignment.ProjectAssignment projectAssignment)
        {
            Guard.ArgumentIsNotNull(projectAssignment, nameof(projectAssignment));
            Guard.ArgumentIsNotNull(projectAssignment.Id, nameof(projectAssignment.Id));
            Guard.ArgumentIsNotNull(projectAssignment.Department.Id, nameof(projectAssignment.Department.Id));

            ProjectAssignmentDto dto = _mapper.Map<Common.Models.Assignment.ProjectAssignment, ProjectAssignmentDto>(projectAssignment);

            return _projectAssignmentRepository.UpdateAsync(dto);
        }

        public Task<int> DeleteAsync(int id)
        {
            Guard.ArgumentIsNotNull(id, nameof(id));

            return _projectAssignmentRepository.DeleteAsync(id);
        }
    }
}
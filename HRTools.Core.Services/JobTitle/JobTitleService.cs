using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using HRTools.Core.Repositories.JobTitle;
using HRTools.Crosscutting.Common;
using HRTools.Crosscutting.Common.Exceptions;

namespace HRTools.Core.Services.JobTitle
{
    public class JobTitleService : IJobTitleService, IJobTitleServiceAdmin
    {
        private readonly IJobTitleRepository _jobTitleRepository;
        private readonly IJobTitleRepositoryAdmin _jobTitleRepositoryAdmin;
        private readonly IMapper _mapper;

        public JobTitleService(
            IJobTitleRepository jobTitleRepository,
            IJobTitleRepositoryAdmin jobTitleRepositoryAdmin,
            IMapper mapper)
        {
            Guard.ConstructorArgumentIsNotNull(jobTitleRepository, nameof(jobTitleRepository));
            Guard.ConstructorArgumentIsNotNull(jobTitleRepositoryAdmin, nameof(jobTitleRepositoryAdmin));
            Guard.ConstructorArgumentIsNotNull(mapper, nameof(mapper));

            _jobTitleRepository = jobTitleRepository;
            _jobTitleRepositoryAdmin = jobTitleRepositoryAdmin;
            _mapper = mapper;
        }

        #region Public

        async Task<IEnumerable<Common.Models.JobTitle>> IJobTitleService.GetAllAsync()
        {
            IEnumerable<JobTitleDto> jobTitles = await _jobTitleRepository.GetAllAsync();

            return _mapper.Map<IEnumerable<JobTitleDto>, IEnumerable<Common.Models.JobTitle>>(jobTitles);
        }

        #endregion

        #region Admin

        public async Task<IEnumerable<Common.Models.JobTitle>> GetAllAsync()
        {
            IEnumerable<JobTitleDto> jobTitles = await _jobTitleRepositoryAdmin.GetAllAsync();

            return _mapper.Map<IEnumerable<JobTitleDto>, IEnumerable<Common.Models.JobTitle>>(jobTitles);
        }

        public async Task<int> CreateAsync(Common.Models.JobTitle jobTitle)
        {
            Guard.ArgumentIsNotNull(jobTitle, nameof(jobTitle));

            await ValdiateIfTechnologyWithSameNameExists(jobTitle);

            JobTitleDto dto = _mapper.Map<Common.Models.JobTitle, JobTitleDto>(jobTitle);

            return await _jobTitleRepositoryAdmin.CreateAsync(dto);
        }

        public async Task<int> UpdateAsync(Common.Models.JobTitle jobTitle)
        {
            Guard.ArgumentIsNotNull(jobTitle, nameof(jobTitle));

            await ValdiateIfTechnologyWithSameNameExists(jobTitle);

            JobTitleDto dto = _mapper.Map<Common.Models.JobTitle, JobTitleDto>(jobTitle);

            return await _jobTitleRepositoryAdmin.UpdateAsync(dto);
        }

        private async Task ValdiateIfTechnologyWithSameNameExists(Common.Models.JobTitle jobTitle)
        {
            JobTitleDto technologyWithSameName = await _jobTitleRepositoryAdmin.GetByNameAsync(jobTitle.Name);

            if (technologyWithSameName != null && technologyWithSameName.Id != jobTitle.Id)
            {
                throw new ValidationException("Job Title with same Name already exists.");
            }
        }

        public Task<int> DeleteAsync(int id)
        {
            return _jobTitleRepositoryAdmin.DeleteAsync(id);
        }

        #endregion
    }
}
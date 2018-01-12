using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using HRTools.Core.Repositories.Technology;
using HRTools.Crosscutting.Common;
using HRTools.Crosscutting.Common.Exceptions;

namespace HRTools.Core.Services.Technology
{
    public class TechnologyService : ITechnologyService, ITechnologyServiceAdmin
    {
        private readonly IMapper _mapper;
        private readonly ITechnologyRepository _technologyRepository;
        private readonly ITechnologyRepositoryAdmin _technologyRepositoryAdmin;

        public TechnologyService(
            ITechnologyRepository technologyRepository,
            ITechnologyRepositoryAdmin technologyRepositoryAdmin,
            IMapper mapper)
        {
            Guard.ConstructorArgumentIsNotNull(technologyRepository, nameof(technologyRepository));
            Guard.ConstructorArgumentIsNotNull(technologyRepositoryAdmin, nameof(technologyRepositoryAdmin));
            Guard.ConstructorArgumentIsNotNull(mapper, nameof(mapper));

            _technologyRepository = technologyRepository;
            _technologyRepositoryAdmin = technologyRepositoryAdmin;
            _mapper = mapper;
        }

        #region Public

        async Task<IEnumerable<Common.Models.Technology>> ITechnologyService.GetAllAsync()
        {
            IEnumerable<TechnologyDto> jobTitles = await _technologyRepository.GetAllAsync();

            return _mapper.Map<IEnumerable<TechnologyDto>, IEnumerable<Common.Models.Technology>>(jobTitles);
        }

        #endregion

        #region Admin

        public async Task<IEnumerable<Common.Models.Technology>> GetAllAsync()
        {
            IEnumerable<TechnologyDto> jobTitles = await _technologyRepositoryAdmin.GetAllAsync();

            return _mapper.Map<IEnumerable<TechnologyDto>, IEnumerable<Common.Models.Technology>>(jobTitles);
        }

        public async Task<int> CreateAsync(Common.Models.Technology technology)
        {
            Guard.ArgumentIsNotNull(technology, nameof(technology));

            await ValdiateIfTechnologyWithSameNameExists(technology);

            TechnologyDto dto = _mapper.Map<Common.Models.Technology, TechnologyDto>(technology);

            return await _technologyRepositoryAdmin.CreateAsync(dto);
        }

        public async Task<int> UpdateAsync(Common.Models.Technology technology)
        {
            Guard.ArgumentIsNotNull(technology, nameof(technology));

            await ValdiateIfTechnologyWithSameNameExists(technology);

            TechnologyDto dto = _mapper.Map<Common.Models.Technology, TechnologyDto>(technology);

            return await _technologyRepositoryAdmin.UpdateAsync(dto);
        }

        private async Task ValdiateIfTechnologyWithSameNameExists(Common.Models.Technology technology)
        {
            TechnologyDto technologyWithSameName = await _technologyRepositoryAdmin.GetByNameAsync(technology.Name);

            if (technologyWithSameName != null && technologyWithSameName.Id != technology.Id)
            {
                throw new ValidationException("Tehcnology with same Name already exists.");
            }
        }

        public Task<int> DeleteAsync(int id)
        {
            return _technologyRepositoryAdmin.DeleteAsync(id);
        }

        #endregion
    }
}
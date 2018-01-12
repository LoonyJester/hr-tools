using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using HRTools.Core.Common.Models;
using HRTools.Core.Repositories.Company;
using HRTools.Crosscutting.Common;

namespace HRTools.Core.Services.Company
{
    public class CompanyService : ICompanyService, ICompanyServiceAdmin
    {
        private readonly ICompanyRepository _companyRepository;
        private readonly ICompanyRepositoryAdmin _companyRepositoryAdmin;
        private readonly IMapper _mapper;

        public CompanyService(
            ICompanyRepository companyRepository,
            ICompanyRepositoryAdmin companyRepositoryAdmin,
            IMapper mapper)
        {
            Guard.ConstructorArgumentIsNotNull(companyRepository, nameof(companyRepository));
            Guard.ConstructorArgumentIsNotNull(companyRepositoryAdmin, nameof(companyRepositoryAdmin));
            Guard.ConstructorArgumentIsNotNull(mapper, nameof(mapper));

            _companyRepository = companyRepository;
            _companyRepositoryAdmin = companyRepositoryAdmin;
            _mapper = mapper;
        }

        #region Public

        async Task<IEnumerable<OfficeLocation>> ICompanyService.GetOfficeLocationListAsync()
        {
            IEnumerable<OfficeLocationDto> dtoList = await _companyRepository.GetOfficeLocationListAsync();

            IEnumerable<OfficeLocation> result =
                _mapper.Map<IEnumerable<OfficeLocationDto>, IEnumerable<OfficeLocation>>(dtoList);

            return result;
        }

        #endregion

        #region Admin

        async Task<IEnumerable<OfficeLocation>> ICompanyServiceAdmin.GetOfficeLocationListAsync()
        {
            IEnumerable<OfficeLocationDto> dtoList = await _companyRepositoryAdmin.GetOfficeLocationListAsync();

            IEnumerable<OfficeLocation> result =
                _mapper.Map<IEnumerable<OfficeLocationDto>, IEnumerable<OfficeLocation>>(dtoList);

            return result;
        }

        #endregion
    }
}

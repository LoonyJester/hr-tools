using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;
using HRTools.Crosscutting.Common.DataAccess;
using HRTools.Crosscutting.Common.Logging;
using HRTools.Crosscutting.Common.Master;

namespace HRTools.Core.Repositories.Company
{
    public class CompanyRepository : Repository, ICompanyRepository, ICompanyRepositoryAdmin
    {
        public CompanyRepository(IConnectionFactory connectionFactory, IConfigurationManager configurationManager,
            ILogger logger) :
            base(connectionFactory, configurationManager, logger)
        {
        }

        #region Public

        Task<IEnumerable<OfficeLocationDto>> ICompanyRepository.GetOfficeLocationListAsync()
        {
            DynamicParameters parameters = new DynamicParameters();

            return QueryAsync<OfficeLocationDto>(StoredProcedures.Company.GetOfficeLocationList, parameters);
        }

        #endregion

        #region Admin

        Task<IEnumerable<OfficeLocationDto>> ICompanyRepositoryAdmin.GetOfficeLocationListAsync()
        {
            DynamicParameters parameters = new DynamicParameters();

            return QueryAsync<OfficeLocationDto>(StoredProcedures.Company.GetOfficeLocationList, parameters);
        }

        #endregion
    }
}
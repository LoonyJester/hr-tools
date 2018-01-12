using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;
using HRTools.Crosscutting.Common;
using HRTools.Crosscutting.Common.DataAccess;
using HRTools.Crosscutting.Common.Logging;
using HRTools.Crosscutting.Common.Master;

namespace HRTools.Core.Repositories.JobTitle
{
    public class JobTitleRepository : Repository, IJobTitleRepository, IJobTitleRepositoryAdmin
    {
        public JobTitleRepository(IConnectionFactory connectionFactory,
            IConfigurationManager configurationManager, ILogger logger)
            : base(connectionFactory, configurationManager, logger)
        {
        }

        #region Public

        Task<IEnumerable<JobTitleDto>> IJobTitleRepository.GetAllAsync()
        {
            DynamicParameters parameters = new DynamicParameters();

            return QueryAsync<JobTitleDto>(StoredProcedures.JobTitle.GetAll, parameters);
        }

        #endregion

        #region Admin

        public Task<IEnumerable<JobTitleDto>> GetAllAsync()
        {
            DynamicParameters parameters = new DynamicParameters();

            return QueryAsync<JobTitleDto>(StoredProcedures.JobTitle.GetAll, parameters);
        }

        public async Task<int> CreateAsync(JobTitleDto jobTitleDto)
        {
            Guard.ArgumentIsNotNull(jobTitleDto, nameof(jobTitleDto));

            DynamicParameters parameters = new DynamicParameters();

            parameters.Add("Name", jobTitleDto.Name);

            return await QueryFirstOrDefaultAsync<int>(StoredProcedures.JobTitle.Create, parameters);
        }

        public async Task<int> UpdateAsync(JobTitleDto jobTitleDto)
        {
            Guard.ArgumentIsNotNull(jobTitleDto, nameof(jobTitleDto));

            DynamicParameters parameters = new DynamicParameters();

            parameters.Add("Id", jobTitleDto.Id);
            parameters.Add("Name", jobTitleDto.Name);

            return await ExecuteAsync(StoredProcedures.JobTitle.Update, parameters);
        }

        public Task<JobTitleDto> GetByNameAsync(string name)
        {
            Guard.ArgumentIsNotNullOrWhiteSpace(name, nameof(name));

            DynamicParameters parameters = new DynamicParameters();

            parameters.Add("Name", name);

            return QuerySingleOrDefaultAsync<JobTitleDto>(StoredProcedures.JobTitle.GetByName, parameters);
        }

        public async Task<int> DeleteAsync(int id)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add("Id", id);

            return await ExecuteAsync(StoredProcedures.JobTitle.Delete, parameters);
        }

        #endregion
    }
}
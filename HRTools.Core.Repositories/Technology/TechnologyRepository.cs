using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;
using HRTools.Crosscutting.Common;
using HRTools.Crosscutting.Common.DataAccess;
using HRTools.Crosscutting.Common.Logging;
using HRTools.Crosscutting.Common.Master;

namespace HRTools.Core.Repositories.Technology
{
    public class TechnologyRepository : Repository, ITechnologyRepository, ITechnologyRepositoryAdmin
    {
        public TechnologyRepository(IConnectionFactory connectionFactory,
            IConfigurationManager configurationManager, ILogger logger)
            : base(connectionFactory, configurationManager, logger)
        {
        }

        #region Public

        Task<IEnumerable<TechnologyDto>> ITechnologyRepository.GetAllAsync()
        {
            DynamicParameters parameters = new DynamicParameters();

            return QueryAsync<TechnologyDto>(StoredProcedures.Technology.GetAll, parameters);
        }

        #endregion

        #region Admin

        public Task<IEnumerable<TechnologyDto>> GetAllAsync()
        {
            DynamicParameters parameters = new DynamicParameters();

            return QueryAsync<TechnologyDto>(StoredProcedures.Technology.GetAll, parameters);
        }

        public async Task<int> CreateAsync(TechnologyDto technologyDto)
        {
            Guard.ArgumentIsNotNull(technologyDto, nameof(technologyDto));

            DynamicParameters parameters = new DynamicParameters();

            parameters.Add("Name", technologyDto.Name);

            return await QueryFirstOrDefaultAsync<int>(StoredProcedures.Technology.Create, parameters);
        }

        public async Task<int> UpdateAsync(TechnologyDto technologyDto)
        {
            Guard.ArgumentIsNotNull(technologyDto, nameof(technologyDto));

            DynamicParameters parameters = new DynamicParameters();

            parameters.Add("Id", technologyDto.Id);
            parameters.Add("Name", technologyDto.Name);

            return await ExecuteAsync(StoredProcedures.Technology.Update, parameters);
        }

        public Task<TechnologyDto> GetByNameAsync(string name)
        {
            Guard.ArgumentIsNotNullOrWhiteSpace(name, nameof(name));

            DynamicParameters parameters = new DynamicParameters();

            parameters.Add("Name", name);

            return QuerySingleOrDefaultAsync<TechnologyDto>(StoredProcedures.Technology.GetByName, parameters);
        }

        public async Task<int> DeleteAsync(int id)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add("Id", id);

            return await ExecuteAsync(StoredProcedures.Technology.Delete, parameters);
        }

        #endregion
    }
}
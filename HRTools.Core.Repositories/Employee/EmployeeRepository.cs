using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using Dapper;
using HRTools.Core.Common.Enums;
using HRTools.Core.Common.Models.Employee;
using HRTools.Crosscutting.Common;
using HRTools.Crosscutting.Common.DataAccess;
using HRTools.Crosscutting.Common.Logging;
using HRTools.Crosscutting.Common.Master;

namespace HRTools.Core.Repositories.Employee
{
    public class EmployeeRepository : Repository, IEmployeeRepository, IEmployeeRepositoryAdmin
    {
        public EmployeeRepository(IConnectionFactory connectionFactory,
            IConfigurationManager configurationManager, ILogger logger)
            : base(connectionFactory, configurationManager, logger)
        {
        }

        Task<IEnumerable<EmployeeDto>> IEmployeeRepository.GetAllAsync(EmployeeGridSettings settings)
        {
            Guard.ArgumentIsNotNull(settings, nameof(settings));
            Guard.ArgumentIsNotNull(settings.PagingSettings, nameof(settings.PagingSettings));
            Guard.ArgumentIsNotNull(settings.SortingSettings, nameof(settings.SortingSettings));
            Guard.ArgumentIsNotNull(settings.EmployeeFilter, nameof(settings.EmployeeFilter));

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("CurrentPage", settings.PagingSettings.CurrentPage);
            parameters.Add("ItemsPerPage", settings.PagingSettings.ItemsPerPage);

            parameters.Add("SortColumnName", settings.SortingSettings.SortColumnName);
            parameters.Add("IsDescending", settings.SortingSettings.IsDescending);

            parameters.Add("SearchKeyword", settings.SearchKeyword ?? string.Empty);
            parameters.Add("CountryFilter", settings.EmployeeFilter.Country ?? string.Empty);
            parameters.Add("CityFilter", settings.EmployeeFilter.City ?? string.Empty);

            if (settings.EmployeeFilter.Status == EmployeeStatus.NotDefined)
            {
                parameters.Add("StatusFilter", string.Empty);
            }
            else
            {
                parameters.Add("StatusFilter", settings.EmployeeFilter.Status);
            }

            return QueryAsync<EmployeeDto>(StoredProcedures.Employee.GetAll, parameters);
        }

        Task<int> IEmployeeRepository.GetTotalCountAsync(EmployeeGridSettings settings)
        {
            Guard.ArgumentIsNotNull(settings, nameof(settings));
            Guard.ArgumentIsNotNull(settings.EmployeeFilter, nameof(settings.EmployeeFilter));

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("SearchKeyword", settings.SearchKeyword ?? string.Empty);
            parameters.Add("CountryFilter", settings.EmployeeFilter.Country ?? string.Empty);
            parameters.Add("CityFilter", settings.EmployeeFilter.City ?? string.Empty);

            if (settings.EmployeeFilter.Status == EmployeeStatus.NotDefined)
            {
                parameters.Add("StatusFilter", string.Empty);
            }
            else
            {
                parameters.Add("StatusFilter", settings.EmployeeFilter.Status);
            }

            return QuerySingleOrDefaultAsync<int>(StoredProcedures.Employee.GetTotalCount, parameters);
        }

        #region Admin

        Task<IEnumerable<EmployeeDto>> IEmployeeRepositoryAdmin.GetAllAsync(EmployeeGridSettings settings)
        {
            Guard.ArgumentIsNotNull(settings, nameof(settings));
            Guard.ArgumentIsNotNull(settings.PagingSettings, nameof(settings.PagingSettings));
            Guard.ArgumentIsNotNull(settings.SortingSettings, nameof(settings.SortingSettings));
            Guard.ArgumentIsNotNull(settings.EmployeeFilter, nameof(settings.EmployeeFilter));

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("CurrentPage", settings.PagingSettings.CurrentPage);
            parameters.Add("ItemsPerPage", settings.PagingSettings.ItemsPerPage);
            parameters.Add("SortColumnName", settings.SortingSettings.SortColumnName);
            parameters.Add("IsDescending", settings.SortingSettings.IsDescending);

            parameters.Add("SearchKeyword", settings.SearchKeyword ?? string.Empty);
            parameters.Add("CountryFilter", settings.EmployeeFilter.Country ?? string.Empty);
            parameters.Add("CityFilter", settings.EmployeeFilter.City ?? string.Empty);

            if (settings.EmployeeFilter.Status == EmployeeStatus.NotDefined)
            {
                parameters.Add("StatusFilter", string.Empty);
            }
            else
            {
                parameters.Add("StatusFilter", settings.EmployeeFilter.Status);
            }

            return QueryAsync<EmployeeDto>(StoredProcedures.Employee.GetAllAdmin, parameters);
        }

        Task<int> IEmployeeRepositoryAdmin.GetTotalCountAsync(EmployeeGridSettings settings)
        {
            Guard.ArgumentIsNotNull(settings, nameof(settings));
            Guard.ArgumentIsNotNull(settings.PagingSettings, nameof(settings.PagingSettings));
            Guard.ArgumentIsNotNull(settings.EmployeeFilter, nameof(settings.EmployeeFilter));

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("SearchKeyword", settings.SearchKeyword ?? string.Empty);
            parameters.Add("CountryFilter", settings.EmployeeFilter.Country ?? string.Empty);
            parameters.Add("CityFilter", settings.EmployeeFilter.City ?? string.Empty);

            if (settings.EmployeeFilter.Status == EmployeeStatus.NotDefined)
            {
                parameters.Add("StatusFilter", string.Empty);
            }
            else
            {
                parameters.Add("StatusFilter", settings.EmployeeFilter.Status);
            }

            return QuerySingleOrDefaultAsync<int>(StoredProcedures.Employee.GetTotalCount, parameters);
        }

        Task<EmployeeDto> IEmployeeRepositoryAdmin.GetByIdAsync(Guid id)
        {
            Guard.ArgumentIsNotNullOrEmpty(id, nameof(id));

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("EmployeeId", id);

            return QuerySingleOrDefaultAsync<EmployeeDto>(StoredProcedures.Employee.GetById, parameters);
        }

        async Task<Guid?> IEmployeeRepositoryAdmin.CreateAsync(EmployeeDto dto)
        {
            Guard.ArgumentIsNotNull(dto, nameof(dto));

            DynamicParameters parameters = new DynamicParameters();

            parameters.Add("EmployeeId", dto.EmployeeId);
            parameters.Add("FullName", dto.FullName);
            parameters.Add("FullNameCyrillic", dto.FullNameCyrillic);
            parameters.Add("PatronymicCyrillic", dto.PatronymicCyrillic);
            parameters.Add("JobTitle", dto.JobTitle);
            parameters.Add("DepartmentName", dto.DepartmentName);
            parameters.Add("Technology", dto.Technology);
            parameters.Add("ProjectName", dto.ProjectName);
            parameters.Add("CompanyEmail", dto.CompanyEmail);
            parameters.Add("PersonalEmail", dto.PersonalEmail);
            parameters.Add("MessengerName", dto.MessengerName);
            parameters.Add("MessengerLogin", dto.MessengerLogin);
            parameters.Add("MobileNumber", dto.MobileNumber);
            parameters.Add("AdditionalMobileNumber", dto.AdditionalMobileNumber);
            parameters.Add("Birthday", dto.Birthday);
            parameters.Add("Status", dto.Status);
            parameters.Add("StartDate", dto.StartDate);
            parameters.Add("TerminationDate", dto.TerminationDate);
            parameters.Add("DaysSkipped", dto.DaysSkipped);
            parameters.Add("BioUrl", dto.BioUrl);
            parameters.Add("Notes", dto.Notes);
            parameters.Add("PhotoUrl", dto.PhotoUrl);

            parameters.Add("Country", dto.Country);
            parameters.Add("City", dto.City);

            return await ExecuteAsync(StoredProcedures.Employee.Create, parameters) != 0 ? (Guid?) dto.EmployeeId : null;
        }

        async Task<int> IEmployeeRepositoryAdmin.UpdateAsync(EmployeeDto dto)
        {
            Guard.ArgumentIsNotNull(dto, nameof(dto));

            DynamicParameters parameters = new DynamicParameters();

            parameters.Add("EmployeeId", dto.EmployeeId);
            parameters.Add("FullName", dto.FullName);
            parameters.Add("FullNameCyrillic", dto.FullNameCyrillic);
            parameters.Add("PatronymicCyrillic", dto.PatronymicCyrillic);
            parameters.Add("JobTitle", dto.JobTitle);
            parameters.Add("DepartmentName", dto.DepartmentName);
            parameters.Add("Technology", dto.Technology);
            parameters.Add("ProjectName", dto.ProjectName);
            parameters.Add("CompanyEmail", dto.CompanyEmail);
            parameters.Add("PersonalEmail", dto.PersonalEmail);
            parameters.Add("MessengerName", dto.MessengerName);
            parameters.Add("MessengerLogin", dto.MessengerLogin);
            parameters.Add("MobileNumber", dto.MobileNumber);
            parameters.Add("AdditionalMobileNumber", dto.AdditionalMobileNumber);
            parameters.Add("Birthday", dto.Birthday);
            parameters.Add("Status", dto.Status);
            parameters.Add("StartDate", dto.StartDate);
            parameters.Add("TerminationDate", dto.TerminationDate);
            parameters.Add("DaysSkipped", dto.DaysSkipped);
            parameters.Add("BioUrl", dto.BioUrl);
            parameters.Add("Notes", dto.Notes);
            parameters.Add("PhotoUrl", dto.PhotoUrl);

            parameters.Add("Country", dto.Country);
            parameters.Add("City", dto.City);

            int result = await ExecuteAsync(StoredProcedures.Employee.Update, parameters);

            return result;
        }

        public Task<EmployeeDto> GetByCompanyEmailAsync(string companyEmail)
        {
            Guard.ArgumentIsNotNullOrEmpty(companyEmail, nameof(companyEmail));

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("CompanyEmail", companyEmail);

            return QueryFirstOrDefaultAsync<EmployeeDto>(StoredProcedures.Employee.GetByCompanyEmail, parameters);
        }

        Task<int> IEmployeeRepositoryAdmin.DeleteAsync(Guid id)
        {
            Guard.ArgumentIsNotNullOrEmpty(id, nameof(id));

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("EmployeeId", id);

            return ExecuteAsync(StoredProcedures.Employee.Delete, parameters);
        }

        public Task<int> DeleteBioUrlAsync(Guid id)
        {
            Guard.ArgumentIsNotNullOrEmpty(id, nameof(id));

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("EmployeeId", id);

            return ExecuteAsync(StoredProcedures.Employee.DeleteBioUrl, parameters);
        }

        public Task<int> DeletePhotoUrlAsync(Guid id)
        {
            Guard.ArgumentIsNotNullOrEmpty(id, nameof(id));

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("EmployeeId", id);

            return ExecuteAsync(StoredProcedures.Employee.DeletePhotoUrl, parameters);
        }

        public async Task<Dictionary<string, string>> GetAllCompanyEmailFullNameAsync()
        {
            DynamicParameters parameters = new DynamicParameters();

            IEnumerable<EmployeeCompanyEmailFullNameDto> result = await QueryAsync<EmployeeCompanyEmailFullNameDto>(StoredProcedures.Employee.GetAllCompanyEmailFullName, parameters);

            Dictionary<string, string> resultDictionary = result.ToDictionary(x => x.CompanyEmail.ToString(), x => x.FullName.ToString());

            return resultDictionary;
        }

        #endregion
    }

    class EmployeeCompanyEmailFullNameDto
    {
        public string CompanyEmail { get; set; }
        public string FullName { get; set; }
    }
}
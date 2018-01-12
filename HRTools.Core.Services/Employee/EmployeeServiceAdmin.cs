using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using HRTools.Core.Common.Models.Employee;
using HRTools.Core.Repositories.Employee;
using HRTools.Crosscutting.Common;
using HRTools.Crosscutting.Common.Exceptions;
using HRTools.Crosscutting.Common.Models;
using HRTools.Crosscutting.Messaging;
using HRTools.Crosscutting.Messaging.Events.Employee;

namespace HRTools.Core.Services.Employee
{
    public class EmployeeServiceAdmin : IEmployeeServiceAdmin
    {
        private const string CdnUrl = @"d:\HR Tools\HR_Tools\CDN\";
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IEmployeeRepositoryAdmin _employeeRepositoryAdmin;
        private readonly IEventPublisher _eventPublisher;
        private readonly IMapper _mapper;

        public EmployeeServiceAdmin(
            IEmployeeRepositoryAdmin employeeRepositoryAdmin,
            IEventPublisher eventPublisher,
            IMapper mapper)
        {
            Guard.ConstructorArgumentIsNotNull(employeeRepositoryAdmin, nameof(employeeRepositoryAdmin));
            Guard.ConstructorArgumentIsNotNull(eventPublisher, nameof(eventPublisher));
            Guard.ConstructorArgumentIsNotNull(mapper, nameof(mapper));

            _employeeRepositoryAdmin = employeeRepositoryAdmin;
            _eventPublisher = eventPublisher;
            _mapper = mapper;
        }
        
        #region Admin

        async Task<GrigData<Common.Models.Employee.Employee>> IEmployeeServiceAdmin.GetAllAsync(
            EmployeeGridSettings settings)
        {
            Guard.ArgumentIsNotNull(settings, nameof(settings));

            IEnumerable<EmployeeDto> dtoList = await _employeeRepositoryAdmin.GetAllAsync(settings);
            int totalCount = await _employeeRepositoryAdmin.GetTotalCountAsync(settings);

            IEnumerable<Common.Models.Employee.Employee> result = _mapper.Map<IEnumerable<EmployeeDto>, IEnumerable<Common.Models.Employee.Employee>>(dtoList);

            return new GrigData<Common.Models.Employee.Employee>
            {
                Data = result,
                TotalCount = totalCount
            };
        }

        async Task<Common.Models.Employee.Employee> IEmployeeServiceAdmin.GetByIdAsync(Guid id)
        {
            Guard.ArgumentIsNotNullOrEmpty(id, nameof(id));

            EmployeeDto dto = await _employeeRepositoryAdmin.GetByIdAsync(id);

            Common.Models.Employee.Employee result =
                _mapper.Map<EmployeeDto, Common.Models.Employee.Employee>(dto);

            return result;
        }

        public async Task<Common.Models.Employee.Employee> GetByCompanyEmailAsync(string companyEmail)
        {
            Guard.ArgumentIsNotNullOrEmpty(companyEmail, nameof(companyEmail));

            EmployeeDto dto = await _employeeRepositoryAdmin.GetByCompanyEmailAsync(companyEmail);

            return _mapper.Map<EmployeeDto, Common.Models.Employee.Employee>(dto);
        }

        async Task<Guid?> IEmployeeServiceAdmin.CreateAsync(Common.Models.Employee.Employee employee)
        {
            Guard.ArgumentIsNotNull(employee, nameof(employee));

            EmployeeValidator.Validate(employee);
            await ValdiateIfUserWithSameCompanyEmailExists(employee);
            
            employee.EmployeeId = Guid.NewGuid();
            EmployeeDto dto = _mapper.Map<Common.Models.Employee.Employee, EmployeeDto>(employee);

            Guid? employeeId = await _employeeRepositoryAdmin.CreateAsync(dto);

            if (employeeId == null)
            {
                return null;
            }

            await _eventPublisher.PublishAsync<IEmployeeCreatedEvent>(new EmployeeCreatedEvent
            {
                EmployeeId = employee.EmployeeId,
                FullName = employee.FullName,
                JobTitle = employee.JobTitle,
                Technology = employee.Technology,
                StartDate = employee.StartDate
            });

            return employeeId;
        }

        async Task<int> IEmployeeServiceAdmin.UpdateAsync(Common.Models.Employee.Employee employee)
        {
            Guard.ArgumentIsNotNull(employee, nameof(employee));

            EmployeeValidator.Validate(employee);
            await ValdiateIfUserWithSameCompanyEmailExists(employee);

            string bioUrl = MoveBioFromTempFolder(employee.EmployeeIdForFiles, employee.EmployeeId);
            string photoUrl = MovePhotoFromTempFolder(employee.EmployeeIdForFiles, employee.EmployeeId);
            
            DeleteTempFolder(employee.EmployeeIdForFiles);

            employee.BioUrl = bioUrl;
            employee.PhotoUrl = photoUrl;
            EmployeeDto dto = _mapper.Map<Common.Models.Employee.Employee, EmployeeDto>(employee);

            int updatedRowsCount = await _employeeRepositoryAdmin.UpdateAsync(dto);

            if (updatedRowsCount == 0)
            {
                return 0;
            }

            await _eventPublisher.PublishAsync<IEmployeeUpdatedEvent>(
                new EmployeeUpdatedEvent
                {
                    EmployeeId = employee.EmployeeId,
                    FullName = employee.FullName,
                    JobTitle = employee.JobTitle,
                    Technology = employee.Technology,
                    StartDate = employee.StartDate
                });

            return updatedRowsCount;
        }

        private static string MoveBioFromTempFolder(string employeeIdForFiles, Guid employeeId)
        {
            string sourceFileDirectory = CdnUrl + $@"Employees\Temp\{employeeIdForFiles}\Bio\";
            string destFileDirectory = CdnUrl + $@"Employees\{employeeId}\Bio\";

            string[] bioUrls = MoveFilesFromTempFolder(sourceFileDirectory, destFileDirectory);
                
            return bioUrls?.FirstOrDefault();
        }

        private static string MovePhotoFromTempFolder(string employeeIdForFiles, Guid employeeId)
        {
            string sourceFileDirectory = CdnUrl + $@"Employees\Temp\{employeeIdForFiles}\Photo\";
            string destFileDirectory = CdnUrl + $@"Employees\{employeeId}\Photo\";

            string[] photoUrls = MoveFilesFromTempFolder(sourceFileDirectory, destFileDirectory);

            return photoUrls?.FirstOrDefault();
        }

        private static string[] MoveFilesFromTempFolder(string sourceFileDirectory, string destFileDirectory)
        {
            try
            {
                List<string> destFileNames = new List<string>();

                if (!Directory.Exists(sourceFileDirectory))
                {
                    // no files to copy or directory is invalid.
                    return null;
                }

                string[] files = Directory.GetFiles(sourceFileDirectory);

                foreach (string file in files)
                {
                    string fileName = Path.GetFileName(file);

                    if (string.IsNullOrWhiteSpace(fileName))
                    {
                        return null;
                    }

                    if (!Directory.Exists(destFileDirectory))
                    {
                        Directory.CreateDirectory(destFileDirectory);
                    }

                    string destFile = Path.Combine(destFileDirectory, fileName);
                    File.Move(file, destFile);

                    destFileNames.Add(destFile);
                }

                return destFileNames.ToArray();
            }
            catch (Exception)
            {
                return null;
            }
        }

        private static void DeleteTempFolder(string employeeIdForFiles)
        {
            if (Directory.Exists(CdnUrl + $@"Employees\Temp\{employeeIdForFiles}"))
            {
                Directory.Delete(CdnUrl + $@"Employees\Temp\{employeeIdForFiles}", true);
            }
        }

        private async Task ValdiateIfUserWithSameCompanyEmailExists(Common.Models.Employee.Employee employee)
        {
            EmployeeDto employeeWithSameCompanyEmail =
                await _employeeRepositoryAdmin.GetByCompanyEmailAsync(employee.CompanyEmail);

            if (employeeWithSameCompanyEmail != null && employeeWithSameCompanyEmail.EmployeeId != employee.EmployeeId)
            {
                throw new ValidationException("Employee with same Company Email already exists.");
            }
        }

        async Task<int> IEmployeeServiceAdmin.DeleteAsync(Guid id)
        {
            Guard.ArgumentIsNotNullOrEmpty(id, nameof(id));

            int deletedRowsCount = await _employeeRepositoryAdmin.DeleteAsync(id);

            if (deletedRowsCount == 0)
            {
                return 0;
            }

            await _eventPublisher.PublishAsync<IEmployeeDeletedEvent>(new EmployeeDeletedEvent
            {
                EmployeeId = id
            });

            return deletedRowsCount;
        }

        public Task<int> DeleteBioUrlAsync(Guid id)
        {
            Guard.ArgumentIsNotNullOrEmpty(id, nameof(id));

            return _employeeRepositoryAdmin.DeleteBioUrlAsync(id);
        }

        public Task<int> DeletePhotoUrlAsync(Guid id)
        {
            Guard.ArgumentIsNotNullOrEmpty(id, nameof(id));

            return _employeeRepositoryAdmin.DeletePhotoUrlAsync(id);
        }

        #endregion
    }
}
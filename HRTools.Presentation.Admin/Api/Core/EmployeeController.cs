using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Script.Serialization;
using HRTools.Core.Common.Models;
using HRTools.Core.Common.Models.Employee;
using HRTools.Core.Services.Company;
using HRTools.Core.Services.Employee;
using HRTools.Crosscutting.Common;
using HRTools.Crosscutting.Common.Exceptions;
using HRTools.Crosscutting.Common.Logging;
using HRTools.Crosscutting.Common.Models;

namespace HRTools.Presentation.Admin.Api.Core
{
    public class EmployeeController : BaseCoreApiController
    {
        private const string CdnUrl = @"d:\HR Tools\HR_Tools\CDN\";
        private const int MegabyteInBytes = 10000000;
        
        private readonly Lazy<ICompanyServiceAdmin> _companyService;
        private readonly Lazy<IEmployeeServiceAdmin> _employeeService;

        public EmployeeController(
            Lazy<IEmployeeServiceAdmin> employeeService,
            Lazy<ICompanyServiceAdmin> companyService,
            Lazy<ILogger> logger)
            : base(logger)
        {
            Guard.ConstructorArgumentIsNotNull(employeeService, nameof(employeeService));
            Guard.ConstructorArgumentIsNotNull(companyService, nameof(companyService));
            
            _employeeService = employeeService;
            _companyService = companyService;
        }

        private IEmployeeServiceAdmin EmployeeService => _employeeService.Value;
        private ICompanyServiceAdmin CompanyService => _companyService.Value;

        [HttpGet]
        [Route("Test")]
        public IEnumerable<object> Test(string gridSettings)
        {
            var identity = User.Identity as ClaimsIdentity;

            return identity.Claims.Select(c => new
            {
                Type = c.Type,
                Value = c.Value
            });
        }

        [HttpGet]
        [Route("Api/GetAllEmployees")]
        public Task<HttpResponseMessage> GetAllAsync(string gridSettings)
        {
            return TryExecuteAsync(async () =>
            {
                Guard.ArgumentIsNotNullOrWhiteSpace(gridSettings, nameof(gridSettings));

                EmployeeGridSettings settings =
                    new JavaScriptSerializer().Deserialize<EmployeeGridSettings>(gridSettings);

                GrigData<Employee> employeeListGridData = await EmployeeService.GetAllAsync(settings);

                return Request.CreateResponse(HttpStatusCode.OK, employeeListGridData);
            });
        }

        [HttpGet]
        [Route("Api/GetCountriesWithCities")]
        public Task<HttpResponseMessage> GetCountriesWithCitiesAsync()
        {
            return TryExecuteAsync(async () =>
            {
                IEnumerable<OfficeLocation> officeLocationList = await CompanyService.GetOfficeLocationListAsync();

                return Request.CreateResponse(HttpStatusCode.OK, officeLocationList.GroupBy(p => p.Country,
                    p => p.City,
                    (key, g) => new
                    {
                        Country = key,
                        Cities = g.ToList()
                    }));
            });
        }

        [HttpPost]
        [Route("Api/CreateEmployee")]
        public Task<HttpResponseMessage> CreateAsync(Employee employee)
        {
            return TryExecuteAsync(async () =>
            {
                Guard.ArgumentIsNotNull(employee, nameof(employee));

                Guid? id = await EmployeeService.CreateAsync(employee);

                return Request.CreateResponse(HttpStatusCode.OK, id != null); // && wasMoved);
            });
        }

        [HttpPut]
        [Route("Api/UpdateEmployee")]
        //[InvalidateCacheOutput("GetAllAsync")]
        public Task<HttpResponseMessage> UpdateAsync([FromBody] Employee employee)
        {
            return TryExecuteAsync(async () =>
            {
                Guard.ArgumentIsNotNull(employee, nameof(employee));

                int updatedRowsCount = await EmployeeService.UpdateAsync(employee);

                return updatedRowsCount == 0
                    ? Request.CreateResponse(HttpStatusCode.InternalServerError, "Employee was not updated")
                    : Request.CreateResponse(HttpStatusCode.OK, updatedRowsCount);
            });
        }
        
        [HttpDelete]
        [Route("Api/DeleteEmployee")]
        public Task<HttpResponseMessage> DeleteAsync([FromBody] Guid id)
        {
            return TryExecuteAsync(async () =>
            {
                Guard.ArgumentIsNotNullOrEmpty(id, nameof(id));

                int deletedRowsCount = await EmployeeService.DeleteAsync(id);

                return deletedRowsCount == 0
                    ? Request.CreateResponse(HttpStatusCode.InternalServerError, "Employee was not deleted")
                    : Request.CreateResponse(HttpStatusCode.OK, deletedRowsCount);
            });
        }

        [HttpPost]
        [Route("Api/UploadPhoto")]
        public HttpResponseMessage UploadPhoto()
        {
            return TryExecute(() =>
            {
                string id = HttpContext.Current.Request.Headers["EmployeeIdForFiles"];

                if (string.IsNullOrWhiteSpace(id))
                {
                    return Request.CreateResponse(HttpStatusCode.OK, false);
                }

                string fileDirectory = $@"d:\HR Tools\HR_Tools\CDN\Employees\Temp\{id}\Photo\";
                string[] supportedFormats = { ".jpeg", ".jpg", ".png" };

                SaveFileFromContext(fileDirectory, supportedFormats);
                
                return Request.CreateResponse(HttpStatusCode.OK, true);
            });
        }

        [HttpPost]
        [Route("Api/UploadBio")]
        public HttpResponseMessage UploadBio()
        {
            return TryExecute(() =>
            {
                string id = HttpContext.Current.Request.Headers["EmployeeIdForFiles"];

                if (string.IsNullOrWhiteSpace(id))
                {
                    return Request.CreateResponse(HttpStatusCode.OK, false);
                }

                string fileDirectory = $@"d:\HR Tools\HR_Tools\CDN\Employees\Temp\{id}\Bio\";
                string[] supportedFormats = { ".docx", ".doc" };

                SaveFileFromContext(fileDirectory, supportedFormats);
                
                return Request.CreateResponse(HttpStatusCode.OK, true);
            });
        }

        private static void SaveFileFromContext(string fileDirectory, string[] supportedFormats)
        {
            HttpPostedFile file = HttpContext.Current.Request.Files.Count > 0
                ? HttpContext.Current.Request.Files[0]
                : null;

            if (file == null || file.ContentLength <= 0)
            {
                return;
            }

            ValidateFile(file, supportedFormats);
            
            string fileName = Path.GetFileName(file.FileName);
            
            if (!Directory.Exists(fileDirectory))
            {
                Directory.CreateDirectory(fileDirectory);
            }

            var path = fileDirectory + fileName;

            file.SaveAs(path);
        }

        private static void ValidateFile(HttpPostedFile file, string[] supportedFormats)
        {
            string fileExtenstion = Path.GetExtension(file.FileName);

            if (!supportedFormats.Contains(fileExtenstion.ToLower()))
            {
                throw new ValidationException("Uploaded file has an unsupported format");
            }

            if (file.ContentLength > MegabyteInBytes * 10)
            {
                throw new ValidationException("Uploaded file is too large");
            }
        }

        [HttpDelete]
        [Route("Api/DeleteBio")]
        public Task<HttpResponseMessage> DeleteBio([FromBody]dynamic model)
        {
            return TryExecuteAsync(async () =>
            {
                Guard.ArgumentIsNotNull(model, nameof(model));
                Guard.ArgumentIsNotNull(model.FileName, nameof(model.FileName));
                Guard.ArgumentIsNotNull(model.EmployeeIdForFiles, nameof(model.EmployeeIdForFiles));

                string fileName = model.FileName;
                string employeeIdForFiles = model.EmployeeIdForFiles;
                string employeeId = model.EmployeeId;

                if (!string.IsNullOrWhiteSpace(employeeId))
                {
                    Guid id;
                    Guid.TryParse(employeeId, out id);

                    int isDeleted = await EmployeeService.DeleteBioUrlAsync(id);
                    if (isDeleted == 0)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, false);
                    }
                }

                // Delete Temp folder
                string fileDirectory = CdnUrl + $@"Employees\Temp\{employeeIdForFiles}\Bio";
                DeleteFileAndFolderWithSubfoldersIfEmpty(fileDirectory, fileName);

                // Delete origin folder
                fileDirectory = CdnUrl + $@"Employees\{employeeId}\Bio";
                DeleteFileAndFolderWithSubfoldersIfEmpty(fileDirectory, fileName);

                return Request.CreateResponse(HttpStatusCode.OK, true);
            });
        }

        [HttpDelete]
        [Route("Api/DeletePhoto")]
        public Task<HttpResponseMessage> DeletePhoto([FromBody]dynamic model)
        {
            return TryExecuteAsync(async () =>
            {
                Guard.ArgumentIsNotNull(model, nameof(model));
                Guard.ArgumentIsNotNull(model.FileName, nameof(model.FileName));
                Guard.ArgumentIsNotNull(model.EmployeeIdForFiles, nameof(model.EmployeeIdForFiles));

                string fileName = model.FileName;
                string employeeIdForFiles = model.EmployeeIdForFiles;
                string employeeId = model.EmployeeId;

                if (!string.IsNullOrWhiteSpace(employeeId))
                {
                    Guid id;
                    Guid.TryParse(employeeId, out id);

                    int isDeleted = await EmployeeService.DeletePhotoUrlAsync(id);
                    if (isDeleted == 0)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, false);
                    }
                }

                // Delete Temp folder
                string fileDirectory = CdnUrl + $@"Employees\Temp\{employeeIdForFiles}\Photo";
                DeleteFileAndFolderWithSubfoldersIfEmpty(fileDirectory, fileName);

                // Delete origin folder
                fileDirectory = CdnUrl + $@"Employees\{employeeId}\Photo";
                DeleteFileAndFolderWithSubfoldersIfEmpty(fileDirectory, fileName);

                return Request.CreateResponse(HttpStatusCode.OK, true);
            });
        }

        private static void DeleteFileAndFolderWithSubfoldersIfEmpty(string directory, string fileName)
        {
            string filePath = directory + "\\" + fileName;

            if (File.Exists(filePath))
            {
                File.Delete(filePath);

                DirectoryInfo parentDirectory = Directory.GetParent(directory);
                DeleteForlerWithSubfoldersIfEmpty(parentDirectory.FullName);
            }
        }

        private static void DeleteForlerWithSubfoldersIfEmpty(string directory)
        {
            if (IsDirectoryEmpty(directory))
            {
                Directory.Delete(directory, true);
            }
        }

        private static bool IsDirectoryEmpty(string directory)
        {
            bool isEmpty = true;

            foreach (string innerFolder in Directory.GetDirectories(directory))
            {
                IsDirectoryEmpty(innerFolder);

                if (Directory.GetFiles(innerFolder).Length != 0)
                {
                    isEmpty = false;
                }
            }

            return isEmpty;
        }
    }
}
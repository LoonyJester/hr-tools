using System;

namespace HRTools.Crosscutting.Common.Helpers
{
    public static class CompanyHelper
    {
        private const string TeamInternationalCompanyId = "a4260c2b-3d9d-436b-ba99-b6667a374b89";
        private const string CompanyCompanyId = "9a0c9793-9064-48b0-b38f-58071d0f52e1";

		public static Guid GetCompanyIdByHost(string host)
        {
            Guard.ArgumentIsNotNull(host, nameof(host));

		    switch (host)
		    {
                case "teaminternational":
		        {
		            return new Guid(TeamInternationalCompanyId);
		        }
                case "teaminternational.admin":
		        {
		            return new Guid(TeamInternationalCompanyId);
		        }
                case "teaminternational.auth":
		        {
		            return new Guid(TeamInternationalCompanyId);
		        }
                case "company":
		        {
		            return new Guid(CompanyCompanyId);
		        }
                case "company.admin":
		        {
		            return new Guid(CompanyCompanyId);
		        }
                case "company.auth":
		        {
		            return new Guid(CompanyCompanyId);
		        }
		    }

		    return new Guid(TeamInternationalCompanyId);// Guid.Empty;
        }

        public static string GetCompanyNameByHost(string host)
        {
            Guard.ArgumentIsNotNull(host, nameof(host));

            switch (host)
            {
                case "teaminternational":
                    {
                        return "teaminternational";
                    }
                case "teaminternational.admin":
                    {
                        return "teaminternational";
                    }
                case "teaminternational.auth":
                    {
                        return "teaminternational";
                    }
                case "company":
                    {
                        return "company";
                    }
                case "company.admin":
                    {
                        return "company";
                    }
                case "company.auth":
                    {
                        return "company";
                    }
            }

            return "teaminternational";
        }

        public static string GetAuthUrlByHost(string host)
        {
            Guard.ArgumentIsNotNull(host, nameof(host));

            switch (host)
            {
                case "teaminternational":
                    {
                        return "https://teaminternational.auth:44311/core/";
                    }
                case "teaminternational.admin":
                    {
                        return "https://teaminternational.auth:44311/core/";
                    }
                case "company":
                    {
                        return "https://company.auth:44314/core/";
                    }
                case "company.admin":
                    {
                        return "https://company.auth:44314/core/";
                    }
            }

            return string.Empty;
        }
    }
}

using System.Text.RegularExpressions;
using HRTools.Crosscutting.Common.Exceptions;
using CoreModels = HRTools.Crosscutting.Common.Models.User;

namespace HRTools.AuthorizationServer.Services
{
    public static class UserValidator
    {
        private const int FieldLength = 250;
        private const int MinPasswordLength = 8;
        private const int MaxPasswordLength = 50;

        private const string EmailPattern =
            @"^(([^<>()\[\]\.,;:\s@\']+(\.[^<>()\[\]\.,;:\s@\']+)*)|(\'.+\'))@(([^<>()[\]\.,;:\s@\']+\.)+[^<>()[\]\.,;:\s@\']{2,})$";

        public static void Validate(CoreModels.User user)
        {
            if (string.IsNullOrWhiteSpace(user.Login))
            {
                throw new ValidationException("Login is required");
            }

            if (user.Login.Length > FieldLength)
            {
                throw new ValidationException($"Login length can not be more than {FieldLength} symbols");
            }

            if (user.Login != null && !Regex.IsMatch(user.Login, EmailPattern))
            {
                throw new ValidationException("Login does not have an email format");
            }

            if (string.IsNullOrWhiteSpace(user.FullName))
            {
                throw new ValidationException("FullName is required");
            }

            if (user.FullName.Length > FieldLength)
            {
                throw new ValidationException($"FullName length can not be more than {FieldLength} symbols");
            }

            //if ((user.Roles & Roles.SuperAdmin) != Roles.SuperAdmin
            //    && (user.Roles & Roles.Admin) != Roles.Admin
            //    && (user.Roles & Roles.Employee) != Roles.Employee
            //    && (user.Roles & Roles.Manager) != Roles.Manager
            //    && (user.Roles & Roles.Recruiter) != Roles.Recruiter)
            //{
            //    throw new ValidationException("Roles are not valid");
            //}

            if (!string.IsNullOrWhiteSpace(user.Password))
            {
                ValidatePassword(user.Password);
            }
        }

        private static void ValidatePassword(string password)
        {
            if (password.Length < MinPasswordLength)
            {
                throw new ValidationException($"Minimal password length is {MinPasswordLength} symbols");
            }

            if (password.Length >= MaxPasswordLength)
            {
                throw new ValidationException($"Maximum password length is {MaxPasswordLength} symbols");
            }

            Regex reg = new Regex(@"\d");

            if (!reg.IsMatch(password))
            {
                throw new ValidationException("Password should contain at least one digit.");
            }

            reg = new Regex(@"[a-z]");

            if (!reg.IsMatch(password))
            {
                throw new ValidationException("Password should contain at least one lowercase character.");
            }

            reg = new Regex(@"[A-Z]");

            if (!reg.IsMatch(password))
            {
                throw new ValidationException("Password should contain at least one uppercase character.");
            }

            reg = new Regex(@"[^0-9a-zA-Z]");

            if (reg.IsMatch(password))
            {
                throw new ValidationException(
                    "Password should contain at least one digit, one uppercase and one lowercase character.");
            }
        }
    }
}
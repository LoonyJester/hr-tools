using System;

namespace HRTools.Crosscutting.Common.Models.User
{
    // TODO: reorganise
    [Flags]
    public enum Roles
    {
        Admin = 0x01,
        Employee = 0x02,
        Manager = 0x04,
        Recruiter = 0x08,
        SuperAdmin = 0x10
    }
}
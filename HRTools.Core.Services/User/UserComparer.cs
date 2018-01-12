using System.Collections.Generic;
using HRTools.Core.Repositories.User;

internal class UserComparer : IEqualityComparer<UserDto>
{
    public bool Equals(UserDto x, UserDto y)
    {
        if (x == y)
        {
            return true;
        }
        if (x == null || y == null)
        {
            return false;
        }

        return x.Subject == y.Subject;
    }


    public int GetHashCode(UserDto pay)
    {
        return base.GetHashCode();
    }
}
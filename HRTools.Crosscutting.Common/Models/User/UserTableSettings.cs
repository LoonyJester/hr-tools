namespace HRTools.Crosscutting.Common.Models.User
{
    public class UserTableSettings
    {
        public PagingSettings PagingSettings { get; set; }

        public string SearchKeyword { get; set; }

        public UserFilter UserFilter { get; set; }
    }
}
using System.Data;
using HRTools.Crosscutting.Common.Master;

namespace HRTools.Crosscutting.Common.DataAccess
{
    public interface IConnectionFactory
    {
        IDbConnection Create(ClientConfiguration configuration);
    }
}

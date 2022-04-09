using System.Data.SqlClient;

namespace Domain.Repository
{
    public interface IConnectionFactory
    {
        SqlConnection CreateConnection();
    }
}

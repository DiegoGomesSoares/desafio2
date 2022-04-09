using Domain.Repository;
using System;
using System.Data.SqlClient;

namespace Infrastructure.Repository
{
    public class ConnectionFactory : IConnectionFactory
    {
        public string Connection { get; }
        public ConnectionFactory(string connection)
        {
            Connection = connection ?? throw new ArgumentNullException(nameof(connection));
        }

        public SqlConnection CreateConnection() => new(Connection);       
    }
}

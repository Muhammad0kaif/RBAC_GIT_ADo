using Microsoft.Data.SqlClient;

namespace AdoApi2.Infrastructure
{
    public class DbConnectionFactory
    {
        private readonly IConfiguration _configuration;

        public DbConnectionFactory(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public SqlConnection CreateConnection()
        {
            var connStr = _configuration.GetConnectionString("Sql");

            Console.WriteLine("FACTORY SQL => " + connStr);

            if (string.IsNullOrWhiteSpace(connStr))
            {
                throw new Exception("Connection string 'Sql' is missing");
            }

            return new SqlConnection(connStr);
        }
    }
}
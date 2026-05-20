using Microsoft.Data.SqlClient;
using System.Data;

namespace AdoApi2.Infrastructure
{
    public class BaseRepository(DbConnectionFactory factory)
    {
        private readonly DbConnectionFactory _factory = factory;

        protected SqlConnection CreateConnection()
        {
            return _factory.CreateConnection();
        }

        protected SqlCommand CreateCommand(string spName, SqlConnection conn)
        {
            return new SqlCommand(spName, conn)
            {
                CommandType = CommandType.StoredProcedure
            };
        }

        // FIXED: NO manual connection handling here
        protected async Task<int> ExecuteNonQuery(SqlCommand cmd)
        {
            await cmd.Connection.OpenAsync();
            return await cmd.ExecuteNonQueryAsync();
        }

        protected async Task<object?> ExecuteScalar(SqlCommand cmd)
        {
            await cmd.Connection.OpenAsync();

            var result = await cmd.ExecuteScalarAsync();
            return result;
        }

        protected async Task<SqlDataReader> ExecuteReader(SqlCommand cmd)
        {
            await cmd.Connection.OpenAsync();

            return await cmd.ExecuteReaderAsync(CommandBehavior.CloseConnection);
        }
    }
}
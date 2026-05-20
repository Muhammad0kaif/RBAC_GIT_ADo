using Microsoft.Data.SqlClient;
using AdoApi2.Infrastructure;
using AdoApi2.Repositories.Interfaces;

namespace AdoApi2.Repositories.Implemenetation
{
    public class ReportRepository(DbConnectionFactory factory) : BaseRepository(factory), IReportRepository
    {
        public async Task<(int TotalOrders, decimal TotalSales)> GetDashboard()
        {
            using var conn = CreateConnection();
            using var cmd = CreateCommand("sp_GetDashboardReport", conn);

            await conn.OpenAsync();

            using var reader = await cmd.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                return (
                    Convert.ToInt32(reader["TotalOrders"]),
                    Convert.ToDecimal(reader["TotalSales"])
                );
            }

            return (0, 0);
        }
    }
}
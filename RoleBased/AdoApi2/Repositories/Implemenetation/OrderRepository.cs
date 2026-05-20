using AdoApi2.Infrastructure;
using AdoApi2.Repositories.Interfaces;
using Microsoft.Data.SqlClient;
using PocoClasses;
using PocoClasses.PocoClasses;
using System.Data;

namespace AdoApi2.Repositories.Implemenetation
{
    public class OrderRepository(DbConnectionFactory factory) : BaseRepository(factory), IOrderRepository
    {

        #region CREATE ORDER

        public async Task CreateOrder(Order order)
        {
            using var conn = CreateConnection();
            using var cmd = CreateCommand("sp_CreateOrder", conn);

            cmd.Parameters.AddWithValue("@Id", order.Id);
            cmd.Parameters.AddWithValue("@ProductName", order.ProductName);
            cmd.Parameters.AddWithValue("@Quantity", order.Quantity);
            cmd.Parameters.AddWithValue("@Price", order.Price);
            cmd.Parameters.AddWithValue("@UserId", order.UserId);

            await ExecuteNonQuery(cmd);
        }

        #endregion

        #region UPDATE ORDER

        public async Task UpdateOrder(Order order)
        {
            using var conn = CreateConnection();
            using var cmd = CreateCommand("sp_UpdateOrder", conn);

            cmd.Parameters.AddWithValue("@Id", order.Id);
            cmd.Parameters.AddWithValue("@ProductName", order.ProductName);
            cmd.Parameters.AddWithValue("@Quantity", order.Quantity);
            cmd.Parameters.AddWithValue("@Price", order.Price);

            await ExecuteNonQuery(cmd);
        }

        #endregion

        #region GET ORDERS (PAGINATION)

        public async Task<(List<Order>, int)> GetOrders(Guid userId, bool isAdmin, int page, int pageSize)
        {
            var list = new List<Order>();
            int totalCount = 0;

            using var conn = CreateConnection();
            using var cmd = CreateCommand("sp_GetOrdersPaged", conn);

            cmd.Parameters.AddWithValue("@UserId", userId);
            cmd.Parameters.AddWithValue("@IsAdmin", isAdmin);
            cmd.Parameters.AddWithValue("@Page", page);
            cmd.Parameters.AddWithValue("@PageSize", pageSize);

            await conn.OpenAsync();

            using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                list.Add(new Order
                {
                    Id = reader["Id"] != DBNull.Value? Guid.Parse(reader["Id"].ToString()!) : Guid.Empty,

                    ProductName = reader["ProductName"].ToString(),

                    Quantity = Convert.ToInt32(reader["Quantity"]),

                    Price = Convert.ToDecimal(reader["Price"]),

                    UserId = reader["UserId"] != DBNull.Value? Guid.Parse(reader["UserId"].ToString()!): Guid.Empty
                });

                totalCount = Convert.ToInt32(reader["TotalCount"]);
            }

            return (list, totalCount);
        }

        #endregion
    }
}
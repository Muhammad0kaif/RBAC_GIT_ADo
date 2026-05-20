using PocoClasses;
using PocoClasses.PocoClasses;

namespace AdoApi2.Repositories.Interfaces
{
    public interface IOrderRepository
    {
        Task CreateOrder(Order order);
        Task UpdateOrder(Order order);
        Task<(List<Order>, int)> GetOrders(Guid userId, bool isAdmin, int page, int pageSize);
    }
}
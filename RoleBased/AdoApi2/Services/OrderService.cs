using AdoApi2.Repositories.Interfaces;
using PocoClasses;
using PocoClasses.PocoClasses;
using System.Security.Claims;

namespace AdoApi2.Services
{
    public class OrderService
    {
        private readonly IOrderRepository _repo;

        public OrderService(IOrderRepository repo)
        {
            _repo = repo;
        }

        public Task CreateOrder(Order order)
        {
            return _repo.CreateOrder(order);
        }

        public Task UpdateOrder(Order order)
        {
            return _repo.UpdateOrder(order);
        }

        public Task<(List<Order>, int)> GetOrders(Guid userId, bool isAdmin, int page, int pageSize)
        {
            return _repo.GetOrders(userId, isAdmin, page, pageSize);
        }
    }
}
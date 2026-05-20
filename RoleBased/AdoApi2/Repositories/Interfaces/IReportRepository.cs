namespace AdoApi2.Repositories.Interfaces
{
    public interface IReportRepository
    {
        Task<(int TotalOrders, decimal TotalSales)> GetDashboard();
    }
}
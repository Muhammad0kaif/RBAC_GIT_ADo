using AdoApi2.Repositories.Interfaces;

namespace AdoApi2.Services
{
    public class ReportService(IReportRepository repo)
    {
        public Task<(int TotalOrders, decimal TotalSales)> GetDashboard()
        {
            return repo.GetDashboard();
        }
    }
}
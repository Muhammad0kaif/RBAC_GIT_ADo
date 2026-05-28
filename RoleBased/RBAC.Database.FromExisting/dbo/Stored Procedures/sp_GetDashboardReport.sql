CREATE   PROCEDURE sp_GetDashboardReport
AS
BEGIN
    SELECT
        COUNT(*) AS TotalOrders,
        ISNULL(SUM(Price * Quantity), 0) AS TotalSales
    FROM Orders
END
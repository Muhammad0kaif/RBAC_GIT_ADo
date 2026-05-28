CREATE PROCEDURE sp_GetOrdersPaged
    @UserId UNIQUEIDENTIFIER,
    @IsAdmin BIT,
    @Page INT,
    @PageSize INT
AS
BEGIN
    SELECT
        Id,
        ProductName,
        Quantity,
        Price,
        UserId,
        COUNT(*) OVER() AS TotalCount
    FROM Orders
    WHERE
        @IsAdmin = 1
        OR UserId = @UserId
    ORDER BY Id
    OFFSET (@Page - 1) * @PageSize ROWS
    FETCH NEXT @PageSize ROWS ONLY
END
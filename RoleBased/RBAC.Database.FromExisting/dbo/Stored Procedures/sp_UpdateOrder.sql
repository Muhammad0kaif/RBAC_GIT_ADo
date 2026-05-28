CREATE PROCEDURE sp_UpdateOrder
    @Id UNIQUEIDENTIFIER,
    @ProductName NVARCHAR(100),
    @Quantity INT,
    @Price DECIMAL(18,2)
AS
BEGIN
    UPDATE Orders
    SET
        ProductName = @ProductName,
        Quantity = @Quantity,
        Price = @Price
    WHERE Id = @Id
END
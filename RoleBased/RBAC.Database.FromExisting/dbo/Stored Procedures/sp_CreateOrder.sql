CREATE PROCEDURE sp_CreateOrder
    @Id UNIQUEIDENTIFIER,
    @ProductName NVARCHAR(100),
    @Quantity INT,
    @Price DECIMAL(18,2),
    @UserId UNIQUEIDENTIFIER
AS
BEGIN
    INSERT INTO Orders
    (
        Id,
        ProductName,
        Quantity,
        Price,
        UserId
    )
    VALUES
    (
        @Id,
        @ProductName,
        @Quantity,
        @Price,
        @UserId
    )
END
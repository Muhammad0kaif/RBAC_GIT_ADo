CREATE TABLE [dbo].[Orders]
(
    [Id] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,

    [ProductName] NVARCHAR(100) NOT NULL,

    [Quantity] INT NOT NULL,

    [Price] DECIMAL(18,2) NOT NULL,

    [UserId] UNIQUEIDENTIFIER NOT NULL,

    CONSTRAINT [FK_Orders_Users]
    FOREIGN KEY ([UserId]) REFERENCES [dbo].[Users]([Id])
);
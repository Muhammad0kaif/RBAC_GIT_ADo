CREATE TABLE [dbo].[Orders] (
    [Id]          UNIQUEIDENTIFIER NOT NULL,
    [ProductName] NVARCHAR (100)   NOT NULL,
    [Quantity]    INT              NOT NULL,
    [Price]       DECIMAL (18, 2)  NOT NULL,
    [UserId]      UNIQUEIDENTIFIER NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);


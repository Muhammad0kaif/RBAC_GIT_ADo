CREATE TABLE [dbo].[Permissions]
(
    [Id] INT NOT NULL PRIMARY KEY,

    [RoleId] INT NOT NULL,

    [PageName] NVARCHAR(100) NULL,

    [CanRead] BIT NULL,

    [CanWrite] BIT NULL,

    [CanDelete] BIT NULL,

    CONSTRAINT [FK_Permissions_Roles]
        FOREIGN KEY ([RoleId]) REFERENCES [dbo].[Roles]([Id])
);
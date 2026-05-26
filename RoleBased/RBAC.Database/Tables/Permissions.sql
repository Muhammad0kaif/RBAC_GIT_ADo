CREATE TABLE [dbo].[Permissions]
(
    [Id] INT NOT NULL PRIMARY KEY,
    [RoleId] INT NOT NULL,
    [PageName] NVARCHAR(100) NULL,
    [CanRead] BIT NOT NULL DEFAULT 0,
    [CanWrite] BIT NOT NULL DEFAULT 0,
    [CanDelete] BIT NOT NULL DEFAULT 0,

    CONSTRAINT [FK_Permissions_Roles]
        FOREIGN KEY ([RoleId]) REFERENCES [dbo].[Roles]([Id])
);
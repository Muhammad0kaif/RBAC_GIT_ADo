CREATE TABLE [dbo].[Permissions] (
    [Id]       INT            IDENTITY (1, 1) NOT NULL,
    [RoleId]   INT            NOT NULL,
    [PageName] NVARCHAR (100) NULL,
    [CanRead]  BIT            NULL,
    [CanWrite] BIT            NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    FOREIGN KEY ([RoleId]) REFERENCES [dbo].[Roles] ([Id])
);


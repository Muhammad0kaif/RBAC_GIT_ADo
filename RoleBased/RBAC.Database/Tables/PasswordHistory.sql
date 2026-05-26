CREATE TABLE [dbo].[PasswordHistory]
(
    [Id] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,

    [UserId] UNIQUEIDENTIFIER NOT NULL,

    [PasswordHash] NVARCHAR(500) NOT NULL,

    [CreatedAt] DATETIME2 NOT NULL,

    CONSTRAINT [FK_PasswordHistory_Users]
    FOREIGN KEY ([UserId]) REFERENCES [dbo].[Users]([Id])
);
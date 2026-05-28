CREATE TABLE [dbo].[PasswordHistory] (
    [Id]           UNIQUEIDENTIFIER NOT NULL,
    [UserId]       UNIQUEIDENTIFIER NOT NULL,
    [PasswordHash] NVARCHAR (500)   NOT NULL,
    [CreatedAt]    DATETIME2 (7)    NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_PasswordHistory_Users] FOREIGN KEY ([UserId]) REFERENCES [dbo].[Users] ([Id])
);


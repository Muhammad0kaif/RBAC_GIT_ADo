CREATE TABLE [dbo].[RefreshTokens]
(
     [Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,

    [Token] NVARCHAR(500) NOT NULL,

    [UserId] UNIQUEIDENTIFIER NOT NULL,

    [ExpiryDate] DATETIME2 NOT NULL,

    [IsRevoked] BIT NOT NULL DEFAULT 0,

    CONSTRAINT [FK_RefreshTokens_Users]
    FOREIGN KEY ([UserId]) REFERENCES [dbo].[Users]([Id])
);
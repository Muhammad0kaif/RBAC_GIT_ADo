CREATE TABLE [dbo].[RefreshTokens]
(
    [Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,

    [Token] NVARCHAR(500) NULL,

    [UserId] UNIQUEIDENTIFIER NULL,

    [ExpiryDate] DATETIME2 NULL,

    [IsRevoked] BIT NULL,

    CONSTRAINT [FK_RefreshTokens_Users]
        FOREIGN KEY ([UserId]) REFERENCES [dbo].[Users]([Id])
);
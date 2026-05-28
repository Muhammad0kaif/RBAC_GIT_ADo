CREATE TABLE [dbo].[RefreshTokens] (
    [Id]         INT              IDENTITY (1, 1) NOT NULL,
    [Token]      NVARCHAR (500)   NULL,
    [ExpiryDate] DATETIME         NULL,
    [IsRevoked]  BIT              DEFAULT ((0)) NULL,
    [UserId]     UNIQUEIDENTIFIER NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    FOREIGN KEY ([UserId]) REFERENCES [dbo].[Users] ([Id])
);


CREATE TABLE [dbo].[Users] (
    [Id]                  UNIQUEIDENTIFIER NOT NULL,
    [Name]                NVARCHAR (100)   NOT NULL,
    [Email]               NVARCHAR (150)   NOT NULL,
    [PasswordHash]        NVARCHAR (255)   NOT NULL,
    [RoleId]              INT              NOT NULL,
    [ProfilePicture]      NVARCHAR (500)   NULL,
    [MustChangePassword]  BIT              DEFAULT ((0)) NOT NULL,
    [FailedLoginAttempts] INT              DEFAULT ((0)) NOT NULL,
    [IsLocked]            BIT              DEFAULT ((0)) NOT NULL,
    [LockedAt]            DATETIME2 (7)    NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    UNIQUE NONCLUSTERED ([Email] ASC)
);


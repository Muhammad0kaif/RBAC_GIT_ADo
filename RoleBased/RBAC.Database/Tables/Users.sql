CREATE TABLE [dbo].[Users]
(
    [Id] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,

    [Name] NVARCHAR(100) NOT NULL,

    [Email] NVARCHAR(150) NOT NULL,

    [PasswordHash] NVARCHAR(500) NULL,

    [RoleId] INT NOT NULL,

    [MustChangePassword] BIT NOT NULL DEFAULT 0,

    [ProfilePicture] NVARCHAR(500) NULL,

    [FailedLoginAttempts] INT NOT NULL DEFAULT 0,

    [IsLocked] BIT NOT NULL DEFAULT 0,

    [LockedAt] DATETIME2 NULL,
CONSTRAINT [UQ_Users_Email]
    UNIQUE ([Email]),

CONSTRAINT [FK_Users_Roles]
    FOREIGN KEY ([RoleId]) REFERENCES [dbo].[Roles]([Id])
);

CREATE TABLE [dbo].[AuditLogs] (
    [Id]        UNIQUEIDENTIFIER NOT NULL,
    [UserId]    UNIQUEIDENTIFIER NULL,
    [Action]    NVARCHAR (200)   NOT NULL,
    [Timestamp] DATETIME2 (7)    NOT NULL,
    [IP]        NVARCHAR (100)   NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);


CREATE PROCEDURE sp_InsertAuditLog
    @Id UNIQUEIDENTIFIER,
    @UserId UNIQUEIDENTIFIER,
    @Action NVARCHAR(200),
    @Timestamp DATETIME2,
    @IP NVARCHAR(100)
AS
BEGIN
    INSERT INTO AuditLogs
    (
        Id,
        UserId,
        Action,
        Timestamp,
        IP
    )
    VALUES
    (
        @Id,
        @UserId,
        @Action,
        @Timestamp,
        @IP
    )
END
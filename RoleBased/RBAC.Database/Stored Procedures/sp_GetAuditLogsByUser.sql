CREATE PROCEDURE [dbo].[sp_GetAuditLogsByUser]
    @UserId UNIQUEIDENTIFIER
AS
BEGIN
    SELECT
        Id,
        UserId,
        Action,
        Timestamp,
        IP
    FROM AuditLogs
    WHERE UserId = @UserId
    ORDER BY Timestamp DESC
END
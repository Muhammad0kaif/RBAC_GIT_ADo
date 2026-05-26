CREATE PROCEDURE [dbo].[sp_GetAuditLogs]
    @Page INT,
    @PageSize INT
AS
BEGIN
    SELECT
        Id,
        UserId,
        Action,
        Timestamp,
        IP,
        COUNT(*) OVER() AS TotalCount
    FROM AuditLogs
    ORDER BY Timestamp DESC
    OFFSET (@Page - 1) * @PageSize ROWS
    FETCH NEXT @PageSize ROWS ONLY
END
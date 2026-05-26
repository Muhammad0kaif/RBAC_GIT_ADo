CREATE PROCEDURE [dbo].[sp_GetLastPasswordHistory]
    @UserId UNIQUEIDENTIFIER
AS
BEGIN
    SELECT TOP 3
        PasswordHash
    FROM PasswordHistory
    WHERE UserId = @UserId
    ORDER BY CreatedAt DESC
END
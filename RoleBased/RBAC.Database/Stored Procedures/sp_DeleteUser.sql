CREATE PROCEDURE [dbo].[sp_DeleteUser]
    @Id UNIQUEIDENTIFIER
AS
BEGIN
    DELETE FROM RefreshTokens
    WHERE UserId = @Id;

    DELETE FROM PasswordHistory
    WHERE UserId = @Id;

    DELETE FROM Orders
    WHERE UserId = @Id;

    DELETE FROM AuditLogs
    WHERE UserId = @Id;

    DELETE FROM Users
    WHERE Id = @Id;
END
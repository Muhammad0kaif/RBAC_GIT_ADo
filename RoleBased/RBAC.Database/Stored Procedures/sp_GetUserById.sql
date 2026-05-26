CREATE PROCEDURE [dbo].[sp_GetUserById]
    @Id UNIQUEIDENTIFIER
AS
BEGIN
    SELECT
        Id,
        Name,
        Email,
        PasswordHash,
        RoleId,
        MustChangePassword,
        ProfilePicture,
        FailedLoginAttempts,
        IsLocked,
        LockedAt
    FROM Users
    WHERE Id = @Id
END
CREATE PROCEDURE sp_GetUserByEmail
    @Email NVARCHAR(100)
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
    WHERE Email = @Email
END
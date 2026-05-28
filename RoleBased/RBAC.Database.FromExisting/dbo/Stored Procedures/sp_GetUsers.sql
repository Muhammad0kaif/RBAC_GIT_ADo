CREATE   PROCEDURE sp_GetUsers
AS
BEGIN
    SELECT
        Id,
        Name,
        Email,
        RoleId,
        MustChangePassword,
        ProfilePicture,
        FailedLoginAttempts,
        IsLocked,
        LockedAt
    FROM Users
END
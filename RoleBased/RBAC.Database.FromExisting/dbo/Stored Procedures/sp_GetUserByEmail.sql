CREATE PROCEDURE [dbo].[sp_GetUserByEmail]
    @Email NVARCHAR(150)
AS
BEGIN
    SELECT
        u.Id,
        u.Name,
        u.Email,
        u.PasswordHash,
        u.RoleId,
        u.DepartmentId,
        d.Name AS DepartmentName,
        u.MustChangePassword,
        u.ProfilePicture,
        u.FailedLoginAttempts,
        u.IsLocked,
        u.LockedAt
    FROM Users u
    LEFT JOIN Departments d
        ON u.DepartmentId = d.Id
    WHERE u.Email = @Email
END
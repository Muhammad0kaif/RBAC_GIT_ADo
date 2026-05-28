CREATE PROCEDURE [dbo].[sp_GetUsers]
AS
BEGIN
    SELECT
        u.Id,
        u.Name,
        u.Email,
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
END
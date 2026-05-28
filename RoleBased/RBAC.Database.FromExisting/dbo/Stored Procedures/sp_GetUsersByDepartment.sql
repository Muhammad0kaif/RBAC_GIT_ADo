CREATE PROCEDURE [dbo].[sp_GetUsersByDepartment]
    @DepartmentId UNIQUEIDENTIFIER
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
    FROM [dbo].[Users] u
    LEFT JOIN [dbo].[Departments] d
        ON u.DepartmentId = d.Id
    WHERE u.DepartmentId = @DepartmentId
END
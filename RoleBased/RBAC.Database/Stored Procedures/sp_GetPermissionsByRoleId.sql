CREATE PROCEDURE [dbo].[sp_GetPermissionsByRoleId]
    @RoleId INT
AS
BEGIN
    SELECT
        Id,
        PageName,
        CanRead,
        CanWrite,
        CanDelete
    FROM Permissions
    WHERE RoleId = @RoleId
END
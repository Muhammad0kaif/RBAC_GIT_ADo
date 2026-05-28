CREATE PROCEDURE sp_GetPermissionsByRoleId
    @RoleId INT
AS
BEGIN
    SELECT
        PageName,
        CanRead,
        CanWrite
    FROM Permissions
    WHERE RoleId = @RoleId
END
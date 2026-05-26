CREATE PROCEDURE [dbo].[sp_GetRoleById]
    @RoleId INT
AS
BEGIN
    SELECT
        Id,
        RoleName
    FROM Roles
    WHERE Id = @RoleId
END
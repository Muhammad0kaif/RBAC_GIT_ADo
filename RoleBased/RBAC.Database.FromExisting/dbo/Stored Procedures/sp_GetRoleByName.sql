CREATE PROCEDURE sp_GetRoleByName
    @RoleName NVARCHAR(50)
AS
BEGIN
    SELECT Id, RoleName
    FROM Roles
    WHERE LOWER(RoleName) = LOWER(@RoleName)
END
CREATE PROCEDURE [dbo].[sp_GetRoles]
AS
BEGIN
    SELECT
        Id,
        RoleName
    FROM Roles
END
CREATE   PROCEDURE sp_CheckPermission
@RoleId INT,
@PageName NVARCHAR(100)
AS
BEGIN
    SELECT COUNT(1)
    FROM Permissions
    WHERE RoleId = @RoleId
      AND LOWER(PageName) = LOWER(@PageName)
      AND CanRead = 1
END
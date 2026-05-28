CREATE   PROCEDURE sp_HasPermission
    @RoleId INT,
    @PageName NVARCHAR(100)
AS
BEGIN
    SELECT CASE 
        WHEN EXISTS (
            SELECT 1 
            FROM Permissions
            WHERE RoleId = @RoleId
              AND LOWER(PageName) = LOWER(@PageName)
              AND CanRead = 1
        )
        THEN 1
        ELSE 0
    END
END
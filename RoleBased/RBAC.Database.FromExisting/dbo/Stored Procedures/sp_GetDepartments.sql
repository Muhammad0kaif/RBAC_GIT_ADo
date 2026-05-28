CREATE PROCEDURE [dbo].[sp_GetDepartments]
AS
BEGIN
    SELECT
        Id,
        Name
    FROM [dbo].[Departments]
    ORDER BY Name
END
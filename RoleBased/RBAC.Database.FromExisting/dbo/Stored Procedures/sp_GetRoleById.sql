CREATE   PROCEDURE sp_GetRoleById
@Id INT
AS
BEGIN
    SELECT Id, RoleName
    FROM Roles
    WHERE Id = @Id
END
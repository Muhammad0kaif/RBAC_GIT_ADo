CREATE PROCEDURE [dbo].[sp_UpdateUserByAdmin]
    @Id UNIQUEIDENTIFIER,
    @Name NVARCHAR(100),
    @Email NVARCHAR(150),
    @RoleId INT,
    @DepartmentId UNIQUEIDENTIFIER = NULL
AS
BEGIN
    UPDATE Users
    SET
        Name = @Name,
        Email = @Email,
        RoleId = @RoleId,
        DepartmentId = @DepartmentId
    WHERE Id = @Id
END
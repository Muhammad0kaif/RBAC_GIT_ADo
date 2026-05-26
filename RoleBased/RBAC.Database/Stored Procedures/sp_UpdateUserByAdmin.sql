CREATE PROCEDURE [dbo].[sp_UpdateUserByAdmin]
    @Id UNIQUEIDENTIFIER,
    @Name NVARCHAR(100),
    @Email NVARCHAR(150),
    @RoleId INT
AS
BEGIN
    UPDATE Users
    SET
        Name = @Name,
        Email = @Email,
        RoleId = @RoleId
    WHERE Id = @Id
END
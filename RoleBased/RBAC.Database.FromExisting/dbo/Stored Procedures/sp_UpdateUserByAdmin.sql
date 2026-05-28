CREATE   PROCEDURE sp_UpdateUserByAdmin
    @Id UNIQUEIDENTIFIER,
    @Name NVARCHAR(100),
    @Email NVARCHAR(100),
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
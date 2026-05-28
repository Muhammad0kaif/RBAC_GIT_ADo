CREATE   PROCEDURE sp_UpdateUser
@Id UNIQUEIDENTIFIER,
@Name NVARCHAR(100),
@Email NVARCHAR(150),
@PasswordHash NVARCHAR(255),
@RoleId INT
AS
BEGIN
    UPDATE Users
    SET 
        Name = @Name,
        Email = @Email,
        PasswordHash = @PasswordHash,
        RoleId = @RoleId
    WHERE Id = @Id
END
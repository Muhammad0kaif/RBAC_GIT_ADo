CREATE   PROCEDURE sp_RegisterUser
@Id UNIQUEIDENTIFIER,
@Name NVARCHAR(100),
@Email NVARCHAR(150),
@PasswordHash NVARCHAR(255),
@RoleId INT
AS
BEGIN
    INSERT INTO Users (Id, Name, Email, PasswordHash, RoleId)
    VALUES (@Id, @Name, @Email, @PasswordHash, @RoleId)
END
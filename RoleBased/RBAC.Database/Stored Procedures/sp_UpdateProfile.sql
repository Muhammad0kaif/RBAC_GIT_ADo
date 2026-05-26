CREATE PROCEDURE [dbo].[sp_UpdateProfile]
    @Id UNIQUEIDENTIFIER,
    @Name NVARCHAR(100),
    @Email NVARCHAR(150)
AS
BEGIN
    UPDATE Users
    SET
        Name = @Name,
        Email = @Email
    WHERE Id = @Id
END
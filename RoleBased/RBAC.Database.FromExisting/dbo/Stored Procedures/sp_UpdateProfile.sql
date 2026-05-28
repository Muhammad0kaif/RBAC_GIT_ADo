CREATE   PROCEDURE sp_UpdateProfile
    @Id UNIQUEIDENTIFIER,
    @Name NVARCHAR(100),
    @Email NVARCHAR(100)
AS
BEGIN
    UPDATE Users
    SET
        Name = @Name,
        Email = @Email
    WHERE Id = @Id
END
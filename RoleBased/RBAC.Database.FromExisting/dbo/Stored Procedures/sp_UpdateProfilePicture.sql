CREATE PROCEDURE sp_UpdateProfilePicture
    @Id UNIQUEIDENTIFIER,
    @ProfilePicture NVARCHAR(500)
AS
BEGIN
    UPDATE Users
    SET ProfilePicture = @ProfilePicture
    WHERE Id = @Id
END
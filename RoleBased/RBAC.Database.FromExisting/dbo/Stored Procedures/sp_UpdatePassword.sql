CREATE PROCEDURE sp_UpdatePassword
    @Id UNIQUEIDENTIFIER,
    @PasswordHash NVARCHAR(500),
    @MustChangePassword BIT
AS
BEGIN
    UPDATE Users
    SET
        PasswordHash = @PasswordHash,
        MustChangePassword = @MustChangePassword
    WHERE Id = @Id
END
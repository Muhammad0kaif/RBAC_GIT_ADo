CREATE   PROCEDURE sp_ResetFailedLogin
    @UserId UNIQUEIDENTIFIER
AS
BEGIN
    UPDATE Users
    SET
        FailedLoginAttempts = 0,
        IsLocked = 0,
        LockedAt = NULL
    WHERE Id = @UserId
END
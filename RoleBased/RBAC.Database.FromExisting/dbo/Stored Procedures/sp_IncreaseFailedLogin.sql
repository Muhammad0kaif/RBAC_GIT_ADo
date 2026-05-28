CREATE   PROCEDURE sp_IncreaseFailedLogin
    @UserId UNIQUEIDENTIFIER
AS
BEGIN
    UPDATE Users
    SET
        FailedLoginAttempts = FailedLoginAttempts + 1,
        IsLocked =
            CASE
                WHEN FailedLoginAttempts + 1 >= 5 THEN 1
                ELSE IsLocked
            END,
        LockedAt =
            CASE
                WHEN FailedLoginAttempts + 1 >= 5 THEN SYSUTCDATETIME()
                ELSE LockedAt
            END
    WHERE Id = @UserId
END
CREATE   PROCEDURE sp_GetPasswordHistory
    @UserId UNIQUEIDENTIFIER
AS
BEGIN
    SELECT
        ph.Id,
        ph.UserId,
        u.Name AS UserName,
        u.Email,
        ph.PasswordHash,
        ph.CreatedAt
    FROM PasswordHistory ph
    INNER JOIN Users u
        ON ph.UserId = u.Id
    WHERE ph.UserId = @UserId
    ORDER BY ph.CreatedAt DESC
END
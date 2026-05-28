CREATE   PROCEDURE sp_InsertRefreshToken
@Token NVARCHAR(500),
@ExpiryDate DATETIME,
@UserId UNIQUEIDENTIFIER
AS
BEGIN
    INSERT INTO RefreshTokens (Token, ExpiryDate, IsRevoked, UserId)
    VALUES (@Token, @ExpiryDate, 0, @UserId)
END
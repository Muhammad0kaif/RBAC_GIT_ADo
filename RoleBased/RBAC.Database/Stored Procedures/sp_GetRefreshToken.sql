CREATE PROCEDURE [dbo].[sp_GetRefreshToken]
    @Token NVARCHAR(500)
AS
BEGIN
    SELECT
        Id,
        Token,
        UserId,
        ExpiryDate,
        IsRevoked
    FROM RefreshTokens
    WHERE Token = @Token
END
CREATE PROCEDURE [dbo].[sp_RevokeRefreshToken]
    @Token NVARCHAR(500)
AS
BEGIN
    UPDATE RefreshTokens
    SET IsRevoked = 1
    WHERE Token = @Token
END
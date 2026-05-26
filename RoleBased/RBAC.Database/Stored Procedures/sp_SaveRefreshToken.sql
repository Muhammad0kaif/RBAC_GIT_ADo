CREATE PROCEDURE [dbo].[sp_SaveRefreshToken]
    @Token NVARCHAR(500),
    @UserId UNIQUEIDENTIFIER,
    @ExpiryDate DATETIME2,
    @IsRevoked BIT
AS
BEGIN
    INSERT INTO RefreshTokens
    (
        Token,
        UserId,
        ExpiryDate,
        IsRevoked
    )
    VALUES
    (
        @Token,
        @UserId,
        @ExpiryDate,
        @IsRevoked
    )
END
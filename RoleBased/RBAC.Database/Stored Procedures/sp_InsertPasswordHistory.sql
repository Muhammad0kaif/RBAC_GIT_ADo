CREATE PROCEDURE [dbo].[sp_InsertPasswordHistory]
    @Id UNIQUEIDENTIFIER,
    @UserId UNIQUEIDENTIFIER,
    @PasswordHash NVARCHAR(500),
    @CreatedAt DATETIME2
AS
BEGIN
    INSERT INTO PasswordHistory
    (
        Id,
        UserId,
        PasswordHash,
        CreatedAt
    )
    VALUES
    (
        @Id,
        @UserId,
        @PasswordHash,
        @CreatedAt
    )
END
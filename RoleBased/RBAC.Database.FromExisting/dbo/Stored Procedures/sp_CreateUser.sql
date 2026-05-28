CREATE PROCEDURE sp_CreateUser
    @Id UNIQUEIDENTIFIER,
    @Name NVARCHAR(100),
    @Email NVARCHAR(100),
    @PasswordHash NVARCHAR(500),
    @RoleId INT,
    @MustChangePassword BIT
AS
BEGIN
    INSERT INTO Users
    (
        Id,
        Name,
        Email,
        PasswordHash,
        RoleId,
        MustChangePassword
    )
    VALUES
    (
        @Id,
        @Name,
        @Email,
        @PasswordHash,
        @RoleId,
        @MustChangePassword
    )
END
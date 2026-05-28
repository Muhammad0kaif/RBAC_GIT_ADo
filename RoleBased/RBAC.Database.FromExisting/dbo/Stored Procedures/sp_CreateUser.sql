CREATE PROCEDURE [dbo].[sp_CreateUser]
    @Id UNIQUEIDENTIFIER,
    @Name NVARCHAR(100),
    @Email NVARCHAR(150),
    @PasswordHash NVARCHAR(500),
    @RoleId INT,
    @DepartmentId UNIQUEIDENTIFIER = NULL,
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
        DepartmentId,
        MustChangePassword,
        FailedLoginAttempts,
        IsLocked,
        LockedAt
    )
    VALUES
    (
        @Id,
        @Name,
        @Email,
        @PasswordHash,
        @RoleId,
        @DepartmentId,
        @MustChangePassword,
        0,
        0,
        NULL
    )
END
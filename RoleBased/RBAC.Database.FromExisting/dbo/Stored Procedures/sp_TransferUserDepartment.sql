CREATE  PROCEDURE [dbo].[sp_TransferUserDepartment]
    @UserId UNIQUEIDENTIFIER,
    @DepartmentId UNIQUEIDENTIFIER
AS
BEGIN
    UPDATE [dbo].[Users]
    SET DepartmentId = @DepartmentId
    WHERE Id = @UserId
END
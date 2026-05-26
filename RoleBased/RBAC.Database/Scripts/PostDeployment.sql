IF NOT EXISTS (SELECT 1 FROM Roles WHERE Id = 1)
BEGIN
    INSERT INTO Roles (Id, RoleName)
    VALUES
    (1, 'Admin'),
    (2, 'User');
END

IF NOT EXISTS (SELECT 1 FROM Permissions WHERE Id = 1)
BEGIN
    INSERT INTO Permissions
    (
        Id,
        RoleId,
        PageName,
        CanRead,
        CanWrite,
        CanDelete
    )
    VALUES
    -- Admin
    (1, 1, 'Dashboard', 1, 1, 1),
    (2, 1, 'Users', 1, 1, 1),
    (3, 1, 'Orders', 1, 1, 1),
    (4, 1, 'Reports', 1, 1, 1),
    (5, 1, 'Profile', 1, 1, 1),
    (6, 1, 'AuditLogs', 1, 1, 1),

    -- User
    (7, 2, 'Dashboard', 1, 0, 0),
    (8, 2, 'Orders', 1, 1, 0),
    (9, 2, 'Profile', 1, 1, 0);
END
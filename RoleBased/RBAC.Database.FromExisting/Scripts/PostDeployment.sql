IF NOT EXISTS (SELECT 1 FROM Roles WHERE RoleName = 'Manager')
BEGIN
    INSERT INTO Roles (RoleName)
    VALUES ('Manager');
END
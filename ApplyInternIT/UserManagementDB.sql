USE master
CREATE DATABASE UserManagement;
GO

USE UserManagement;
GO

CREATE TABLE Users (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Username NVARCHAR(50) NOT NULL UNIQUE,
    PasswordHash NVARCHAR(255) NOT NULL,
    Email NVARCHAR(100) NOT NULL UNIQUE,
    FullName NVARCHAR(100),
    CreatedAt DATETIME DEFAULT GETDATE(),
    UpdatedAt DATETIME DEFAULT GETDATE()
);
GO
USE UserManagement;
GO
INSERT INTO Users (Username, PasswordHash, Email, FullName, CreatedAt, UpdatedAt)
VALUES 
('user1', 'hashedpassword1', 'user1@example.com', 'User One', GETDATE(), GETDATE()),
('user2', 'hashedpassword2', 'user2@example.com', 'User Two', GETDATE(), GETDATE()),
('user3', 'hashedpassword3', 'user3@example.com', 'User Three', GETDATE(), GETDATE()),
('user4', 'hashedpassword4', 'user4@example.com', 'User Four', GETDATE(), GETDATE()),
('user5', 'hashedpassword5', 'user5@example.com', 'User Five', GETDATE(), GETDATE()),
('user6', 'hashedpassword6', 'user6@example.com', 'User Six', GETDATE(), GETDATE()),
('user7', 'hashedpassword7', 'user7@example.com', 'User Seven', GETDATE(), GETDATE()),
('user8', 'hashedpassword8', 'user8@example.com', 'User Eight', GETDATE(), GETDATE()),
('user9', 'hashedpassword9', 'user9@example.com', 'User Nine', GETDATE(), GETDATE()),
('user10', 'hashedpassword10', 'user10@example.com', 'User Ten', GETDATE(), GETDATE());
GO
DROP TABLE Users;




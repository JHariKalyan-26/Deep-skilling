SET NOCOUNT ON;

IF OBJECT_ID('dbo.EmployeeChanges','U') IS NOT NULL DROP TABLE dbo.EmployeeChanges;
IF OBJECT_ID('dbo.Employees','U') IS NOT NULL DROP TABLE dbo.Employees;
IF OBJECT_ID('dbo.Departments','U') IS NOT NULL DROP TABLE dbo.Departments;
GO

CREATE TABLE Departments
(
    DepartmentID INT PRIMARY KEY,
    DepartmentName VARCHAR(100)
);

CREATE TABLE Employees
(
    EmployeeID INT PRIMARY KEY,
    FirstName VARCHAR(50),
    LastName VARCHAR(50),
    DepartmentID INT FOREIGN KEY REFERENCES Departments(DepartmentID),
    Salary DECIMAL(10,2),
    AnnualSalary DECIMAL(12,2),
    JoinDate DATE
);

CREATE TABLE EmployeeChanges
(
    ChangeID INT IDENTITY(1,1) PRIMARY KEY,
    EmployeeID INT,
    OldSalary DECIMAL(10,2),
    NewSalary DECIMAL(10,2),
    ChangedAt DATETIME DEFAULT GETDATE()
);
GO

INSERT INTO Departments VALUES
(1,'HR'),(2,'Finance'),(3,'IT'),(4,'Marketing');

INSERT INTO Employees VALUES
(1,'John','Doe',1,5000,60000,'2022-01-15'),
(2,'Jane','Smith',2,6000,72000,'2021-03-22'),
(3,'Michael','Johnson',3,7000,84000,'2020-07-30'),
(4,'Emily','Davis',4,5500,66000,'2019-11-05');
GO

CREATE OR ALTER TRIGGER dbo.trg_Employees_LogSalaryChange
ON dbo.Employees
AFTER UPDATE
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO EmployeeChanges(EmployeeID,OldSalary,NewSalary)
    SELECT i.EmployeeID,d.Salary,i.Salary
    FROM inserted i
    JOIN deleted d ON d.EmployeeID=i.EmployeeID
    WHERE ISNULL(i.Salary,0)<>ISNULL(d.Salary,0);
END;
GO

CREATE OR ALTER TRIGGER dbo.trg_Employees_PreventDelete
ON dbo.Employees
INSTEAD OF DELETE
AS
BEGIN
    RAISERROR('Deletion of employee records is not allowed.',16,1);
END;
GO

CREATE OR ALTER TRIGGER dbo.trg_Employees_UpdateAnnualSalary
ON dbo.Employees
AFTER INSERT,UPDATE
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE e
    SET AnnualSalary=i.Salary*12
    FROM Employees e
    JOIN inserted i ON i.EmployeeID=e.EmployeeID
    WHERE e.AnnualSalary<>i.Salary*12 OR e.AnnualSalary IS NULL;
END;
GO

UPDATE Employees SET Salary=5250 WHERE EmployeeID=1;
SELECT * FROM EmployeeChanges;
SELECT EmployeeID,Salary,AnnualSalary FROM Employees;

/*
LOGON TRIGGER - REVIEW BEFORE EXECUTING.
It is intentionally left commented because an incorrect LOGON trigger can lock users out.

CREATE OR ALTER TRIGGER trg_BlockLogonDuringMaintenance
ON ALL SERVER
FOR LOGON
AS
BEGIN
    IF DATEPART(HOUR,GETDATE())>=2 AND DATEPART(HOUR,GETDATE())<3
    BEGIN
        ROLLBACK;
    END
END;
GO

To remove:
DROP TRIGGER trg_BlockLogonDuringMaintenance ON ALL SERVER;
*/

/*
Delete trigger example:
DROP TRIGGER dbo.trg_Employees_LogSalaryChange;
*/
GO

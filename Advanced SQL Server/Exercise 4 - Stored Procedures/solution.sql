SET NOCOUNT ON;

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
    EmployeeID INT IDENTITY(1,1) PRIMARY KEY,
    FirstName VARCHAR(50),
    LastName VARCHAR(50),
    DepartmentID INT FOREIGN KEY REFERENCES Departments(DepartmentID),
    Salary DECIMAL(10,2),
    JoinDate DATE
);
GO

INSERT INTO Departments VALUES
(1,'HR'),(2,'Finance'),(3,'IT'),(4,'Marketing');

SET IDENTITY_INSERT Employees ON;
INSERT INTO Employees(EmployeeID,FirstName,LastName,DepartmentID,Salary,JoinDate) VALUES
(1,'John','Doe',1,5000,'2020-01-15'),
(2,'Jane','Smith',2,6000,'2019-03-22'),
(3,'Michael','Johnson',3,7000,'2018-07-30'),
(4,'Emily','Davis',4,5500,'2021-11-05');
SET IDENTITY_INSERT Employees OFF;
GO

CREATE OR ALTER PROCEDURE dbo.sp_GetEmployeesByDepartment
    @DepartmentID INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT EmployeeID,FirstName,LastName,DepartmentID,Salary,JoinDate
    FROM Employees
    WHERE DepartmentID=@DepartmentID;
END;
GO

CREATE OR ALTER PROCEDURE dbo.sp_InsertEmployee
    @FirstName VARCHAR(50),
    @LastName VARCHAR(50),
    @DepartmentID INT,
    @Salary DECIMAL(10,2),
    @JoinDate DATE
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO Employees(FirstName,LastName,DepartmentID,Salary,JoinDate)
    VALUES(@FirstName,@LastName,@DepartmentID,@Salary,@JoinDate);
END;
GO

CREATE OR ALTER PROCEDURE dbo.sp_GetEmployeeCountByDepartment
    @DepartmentID INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT COUNT(*) AS TotalEmployees
    FROM Employees
    WHERE DepartmentID=@DepartmentID;
END;
GO

CREATE OR ALTER PROCEDURE dbo.sp_GetTotalSalaryByDepartment
    @DepartmentID INT,
    @TotalSalary DECIMAL(12,2) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT @TotalSalary=COALESCE(SUM(Salary),0)
    FROM Employees
    WHERE DepartmentID=@DepartmentID;
END;
GO

CREATE OR ALTER PROCEDURE dbo.sp_UpdateEmployeeSalary
    @EmployeeID INT,
    @NewSalary DECIMAL(10,2)
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE Employees
    SET Salary=@NewSalary
    WHERE EmployeeID=@EmployeeID;
END;
GO

EXEC dbo.sp_GetEmployeesByDepartment @DepartmentID=3;

EXEC dbo.sp_InsertEmployee
    @FirstName='Hari',
    @LastName='Kalyan',
    @DepartmentID=3,
    @Salary=6500,
    @JoinDate='2026-07-18';

EXEC dbo.sp_GetEmployeeCountByDepartment @DepartmentID=3;

DECLARE @Total DECIMAL(12,2);
EXEC dbo.sp_GetTotalSalaryByDepartment
    @DepartmentID=3,
    @TotalSalary=@Total OUTPUT;
SELECT @Total AS TotalSalaryForDepartment3;

EXEC dbo.sp_UpdateEmployeeSalary @EmployeeID=1,@NewSalary=5250;
SELECT * FROM Employees ORDER BY EmployeeID;

/*
Delete example (kept commented so later tests continue to work):
DROP PROCEDURE dbo.sp_GetEmployeesByDepartment;
*/
GO

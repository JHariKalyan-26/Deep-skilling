SET NOCOUNT ON;

IF OBJECT_ID('dbo.vw_EmployeeReport','V') IS NOT NULL DROP VIEW dbo.vw_EmployeeReport;
IF OBJECT_ID('dbo.vw_EmployeeAnnualSalary','V') IS NOT NULL DROP VIEW dbo.vw_EmployeeAnnualSalary;
IF OBJECT_ID('dbo.vw_EmployeeFullName','V') IS NOT NULL DROP VIEW dbo.vw_EmployeeFullName;
IF OBJECT_ID('dbo.vw_EmployeeBasicInfo','V') IS NOT NULL DROP VIEW dbo.vw_EmployeeBasicInfo;
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
    JoinDate DATE
);
GO

INSERT INTO Departments VALUES
(1,'HR'),(2,'Finance'),(3,'IT'),(4,'Marketing');

INSERT INTO Employees VALUES
(1,'John','Doe',1,5000,'2020-01-15'),
(2,'Jane','Smith',2,6000,'2019-03-22'),
(3,'Michael','Johnson',3,7000,'2018-07-30'),
(4,'Emily','Davis',4,5500,'2021-11-05');
GO

CREATE VIEW dbo.vw_EmployeeBasicInfo
AS
SELECT e.EmployeeID,e.FirstName,e.LastName,d.DepartmentName
FROM Employees e
JOIN Departments d ON d.DepartmentID=e.DepartmentID;
GO

CREATE VIEW dbo.vw_EmployeeFullName
AS
SELECT
    e.EmployeeID,
    CONCAT(e.FirstName,' ',e.LastName) AS FullName,
    d.DepartmentName
FROM Employees e
JOIN Departments d ON d.DepartmentID=e.DepartmentID;
GO

CREATE VIEW dbo.vw_EmployeeAnnualSalary
AS
SELECT
    e.EmployeeID,
    e.FirstName,
    e.LastName,
    CAST(e.Salary*12 AS DECIMAL(12,2)) AS AnnualSalary
FROM Employees e;
GO

CREATE VIEW dbo.vw_EmployeeReport
AS
SELECT
    e.EmployeeID,
    CONCAT(e.FirstName,' ',e.LastName) AS FullName,
    d.DepartmentName,
    CAST(e.Salary*12 AS DECIMAL(12,2)) AS AnnualSalary,
    CAST((e.Salary*12)*0.10 AS DECIMAL(12,2)) AS Bonus
FROM Employees e
JOIN Departments d ON d.DepartmentID=e.DepartmentID;
GO

SELECT * FROM dbo.vw_EmployeeBasicInfo;
SELECT * FROM dbo.vw_EmployeeFullName;
SELECT * FROM dbo.vw_EmployeeAnnualSalary;
SELECT * FROM dbo.vw_EmployeeReport;
GO

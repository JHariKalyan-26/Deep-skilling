SET NOCOUNT ON;

IF OBJECT_ID('dbo.fn_CalculateTotalCompensation','FN') IS NOT NULL DROP FUNCTION dbo.fn_CalculateTotalCompensation;
IF OBJECT_ID('dbo.fn_GetEmployeesByDepartment','IF') IS NOT NULL DROP FUNCTION dbo.fn_GetEmployeesByDepartment;
IF OBJECT_ID('dbo.fn_CalculateBonus','FN') IS NOT NULL DROP FUNCTION dbo.fn_CalculateBonus;
IF OBJECT_ID('dbo.fn_CalculateAnnualSalary','FN') IS NOT NULL DROP FUNCTION dbo.fn_CalculateAnnualSalary;
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

CREATE FUNCTION dbo.fn_CalculateAnnualSalary(@Salary DECIMAL(10,2))
RETURNS DECIMAL(12,2)
AS
BEGIN
    RETURN @Salary*12;
END;
GO

CREATE FUNCTION dbo.fn_GetEmployeesByDepartment(@DepartmentID INT)
RETURNS TABLE
AS
RETURN
(
    SELECT EmployeeID,FirstName,LastName,DepartmentID,Salary,JoinDate
    FROM Employees
    WHERE DepartmentID=@DepartmentID
);
GO

CREATE FUNCTION dbo.fn_CalculateBonus(@Salary DECIMAL(10,2))
RETURNS DECIMAL(12,2)
AS
BEGIN
    RETURN @Salary*0.15;
END;
GO

CREATE FUNCTION dbo.fn_CalculateTotalCompensation(@Salary DECIMAL(10,2))
RETURNS DECIMAL(12,2)
AS
BEGIN
    RETURN dbo.fn_CalculateAnnualSalary(@Salary)
         + dbo.fn_CalculateBonus(@Salary);
END;
GO

SELECT
    EmployeeID,
    FirstName,
    LastName,
    Salary,
    dbo.fn_CalculateAnnualSalary(Salary) AS AnnualSalary,
    dbo.fn_CalculateBonus(Salary) AS Bonus,
    dbo.fn_CalculateTotalCompensation(Salary) AS TotalCompensation
FROM Employees;

SELECT * FROM dbo.fn_GetEmployeesByDepartment(3);

SELECT dbo.fn_CalculateAnnualSalary(
    (SELECT Salary FROM Employees WHERE EmployeeID=1)
) AS Employee1AnnualSalary;
GO

/*
Delete demonstration:
DROP FUNCTION dbo.fn_CalculateBonus;
*/

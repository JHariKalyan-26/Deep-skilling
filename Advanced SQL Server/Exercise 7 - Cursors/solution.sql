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
    EmployeeID INT PRIMARY KEY,
    FirstName VARCHAR(50),
    LastName VARCHAR(50),
    DepartmentID INT FOREIGN KEY REFERENCES Departments(DepartmentID),
    Salary DECIMAL(10,2),
    JoinDate DATE
);

INSERT INTO Departments VALUES (1,'HR'),(2,'IT'),(3,'Finance');

INSERT INTO Employees VALUES
(1,'John','Doe',1,5000,'2020-01-15'),
(2,'Jane','Smith',2,6000,'2019-03-22'),
(3,'Bob','Johnson',3,5500,'2021-07-30');
GO

DECLARE
    @EmployeeID INT,
    @FirstName VARCHAR(50),
    @LastName VARCHAR(50),
    @DepartmentID INT,
    @Salary DECIMAL(10,2),
    @JoinDate DATE;

DECLARE EmployeeCursor CURSOR LOCAL FAST_FORWARD FOR
SELECT EmployeeID,FirstName,LastName,DepartmentID,Salary,JoinDate
FROM Employees
ORDER BY EmployeeID;

OPEN EmployeeCursor;
FETCH NEXT FROM EmployeeCursor
INTO @EmployeeID,@FirstName,@LastName,@DepartmentID,@Salary,@JoinDate;

WHILE @@FETCH_STATUS=0
BEGIN
    PRINT CONCAT(
        'EmployeeID=',@EmployeeID,
        ', Name=',@FirstName,' ',@LastName,
        ', DepartmentID=',@DepartmentID,
        ', Salary=',@Salary,
        ', JoinDate=',CONVERT(VARCHAR(10),@JoinDate,120)
    );

    FETCH NEXT FROM EmployeeCursor
    INTO @EmployeeID,@FirstName,@LastName,@DepartmentID,@Salary,@JoinDate;
END

CLOSE EmployeeCursor;
DEALLOCATE EmployeeCursor;
GO

-- STATIC cursor
DECLARE StaticCursor CURSOR LOCAL STATIC FOR
SELECT EmployeeID,FirstName FROM Employees;
OPEN StaticCursor;
FETCH NEXT FROM StaticCursor INTO @EmployeeID,@FirstName;
WHILE @@FETCH_STATUS=0
BEGIN
    PRINT CONCAT('STATIC: ',@EmployeeID,' - ',@FirstName);
    FETCH NEXT FROM StaticCursor INTO @EmployeeID,@FirstName;
END
CLOSE StaticCursor;
DEALLOCATE StaticCursor;
GO

-- DYNAMIC cursor
DECLARE DynamicCursor CURSOR LOCAL DYNAMIC FOR
SELECT EmployeeID,FirstName FROM Employees;
OPEN DynamicCursor;
FETCH NEXT FROM DynamicCursor INTO @EmployeeID,@FirstName;
WHILE @@FETCH_STATUS=0
BEGIN
    PRINT CONCAT('DYNAMIC: ',@EmployeeID,' - ',@FirstName);
    FETCH NEXT FROM DynamicCursor INTO @EmployeeID,@FirstName;
END
CLOSE DynamicCursor;
DEALLOCATE DynamicCursor;
GO

-- FORWARD_ONLY cursor
DECLARE ForwardCursor CURSOR LOCAL FORWARD_ONLY FOR
SELECT EmployeeID,FirstName FROM Employees;
OPEN ForwardCursor;
FETCH NEXT FROM ForwardCursor INTO @EmployeeID,@FirstName;
WHILE @@FETCH_STATUS=0
BEGIN
    PRINT CONCAT('FORWARD_ONLY: ',@EmployeeID,' - ',@FirstName);
    FETCH NEXT FROM ForwardCursor INTO @EmployeeID,@FirstName;
END
CLOSE ForwardCursor;
DEALLOCATE ForwardCursor;
GO

-- KEYSET cursor
DECLARE KeysetCursor CURSOR LOCAL KEYSET FOR
SELECT EmployeeID,FirstName FROM Employees;
OPEN KeysetCursor;
FETCH NEXT FROM KeysetCursor INTO @EmployeeID,@FirstName;
WHILE @@FETCH_STATUS=0
BEGIN
    PRINT CONCAT('KEYSET: ',@EmployeeID,' - ',@FirstName);
    FETCH NEXT FROM KeysetCursor INTO @EmployeeID,@FirstName;
END
CLOSE KeysetCursor;
DEALLOCATE KeysetCursor;
GO

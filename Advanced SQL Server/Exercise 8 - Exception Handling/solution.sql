SET NOCOUNT ON;

IF OBJECT_ID('dbo.AuditLog','U') IS NOT NULL DROP TABLE dbo.AuditLog;
IF OBJECT_ID('dbo.Employees','U') IS NOT NULL DROP TABLE dbo.Employees;
IF OBJECT_ID('dbo.Departments','U') IS NOT NULL DROP TABLE dbo.Departments;
GO

CREATE TABLE Departments
(
    DepartmentID INT PRIMARY KEY,
    DepartmentName VARCHAR(100) NOT NULL
);

CREATE TABLE Employees
(
    EmployeeID INT PRIMARY KEY,
    FirstName VARCHAR(50),
    LastName VARCHAR(50),
    Email VARCHAR(100) UNIQUE,
    Salary DECIMAL(10,2),
    DepartmentID INT,
    FOREIGN KEY(DepartmentID) REFERENCES Departments(DepartmentID)
);

CREATE TABLE AuditLog
(
    LogID INT IDENTITY(1,1) PRIMARY KEY,
    Action VARCHAR(100),
    ErrorMessage VARCHAR(4000),
    ActionDate DATETIME DEFAULT GETDATE()
);

INSERT INTO Departments VALUES
(1,'HR'),(2,'Finance'),(3,'IT'),(4,'Marketing');
GO

CREATE OR ALTER PROCEDURE dbo.AddEmployee
    @EmployeeID INT,
    @FirstName VARCHAR(50),
    @LastName VARCHAR(50),
    @Email VARCHAR(100),
    @Salary DECIMAL(10,2),
    @DepartmentID INT
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        IF @Salary<0
            RAISERROR('Salary cannot be negative.',16,1);

        IF @Salary<1000
            RAISERROR('Warning: Salary is below 1000.',10,1);

        IF @Salary=0
            RAISERROR('Salary must be greater than zero.',16,1);

        INSERT INTO Employees
            (EmployeeID,FirstName,LastName,Email,Salary,DepartmentID)
        VALUES
            (@EmployeeID,@FirstName,@LastName,@Email,@Salary,@DepartmentID);
    END TRY
    BEGIN CATCH
        INSERT INTO AuditLog(Action,ErrorMessage)
        VALUES('AddEmployee',ERROR_MESSAGE());

        THROW;
    END CATCH
END;
GO

CREATE OR ALTER PROCEDURE dbo.TransferEmployee
    @EmployeeID INT,
    @NewDepartmentID INT
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        BEGIN TRY
            IF NOT EXISTS
                (SELECT 1 FROM Departments WHERE DepartmentID=@NewDepartmentID)
            BEGIN
                RAISERROR('The destination department does not exist.',16,1);
            END

            UPDATE Employees
            SET DepartmentID=@NewDepartmentID
            WHERE EmployeeID=@EmployeeID;

            IF @@ROWCOUNT=0
                RAISERROR('Employee does not exist.',16,1);
        END TRY
        BEGIN CATCH
            INSERT INTO AuditLog(Action,ErrorMessage)
            VALUES('TransferEmployee - Inner Catch',ERROR_MESSAGE());
            THROW;
        END CATCH
    END TRY
    BEGIN CATCH
        INSERT INTO AuditLog(Action,ErrorMessage)
        VALUES('TransferEmployee - Outer Catch',ERROR_MESSAGE());
        THROW;
    END CATCH
END;
GO

CREATE OR ALTER PROCEDURE dbo.BatchInsertEmployees
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        BEGIN TRANSACTION;

        INSERT INTO Employees VALUES
        (10,'Batch','One','batch1@example.com',3000,1);

        INSERT INTO Employees VALUES
        (11,'Batch','Two','batch2@example.com',3500,2);

        -- Duplicate email intentionally demonstrates rollback.
        INSERT INTO Employees VALUES
        (12,'Batch','Three','batch1@example.com',4000,3);

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT>0 ROLLBACK TRANSACTION;

        INSERT INTO AuditLog(Action,ErrorMessage)
        VALUES('BatchInsertEmployees',ERROR_MESSAGE());
    END CATCH
END;
GO

EXEC dbo.AddEmployee
    @EmployeeID=1,
    @FirstName='John',
    @LastName='Doe',
    @Email='john@example.com',
    @Salary=5000,
    @DepartmentID=1;

BEGIN TRY
    EXEC dbo.AddEmployee
        @EmployeeID=2,
        @FirstName='Jane',
        @LastName='Smith',
        @Email='john@example.com',
        @Salary=6000,
        @DepartmentID=2;
END TRY
BEGIN CATCH
    PRINT CONCAT('Duplicate-email test: ',ERROR_MESSAGE());
END CATCH;

BEGIN TRY
    EXEC dbo.TransferEmployee @EmployeeID=1,@NewDepartmentID=99;
END TRY
BEGIN CATCH
    PRINT CONCAT('Transfer test: ',ERROR_MESSAGE());
END CATCH;

EXEC dbo.BatchInsertEmployees;

SELECT * FROM Employees ORDER BY EmployeeID;
SELECT * FROM AuditLog ORDER BY LogID;
GO

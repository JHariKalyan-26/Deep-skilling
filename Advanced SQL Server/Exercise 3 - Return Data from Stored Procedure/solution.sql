-- Exercise 3: Return Data from a Stored Procedure
-- Run Exercise 2 first.

CREATE OR ALTER PROCEDURE sp_GetEmployeeCountByDepartment
    @DepartmentID INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        @DepartmentID AS DepartmentID,
        COUNT(*) AS TotalEmployees
    FROM Employees
    WHERE DepartmentID = @DepartmentID;
END;
GO

EXEC sp_GetEmployeeCountByDepartment @DepartmentID = 3;
GO

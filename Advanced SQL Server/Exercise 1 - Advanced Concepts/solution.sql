/*
Advanced SQL Exercises for Online Retail Store
SQL Server
*/

SET NOCOUNT ON;

-- Clean-up for repeatable execution
IF OBJECT_ID('dbo.OrderDetails','U') IS NOT NULL DROP TABLE dbo.OrderDetails;
IF OBJECT_ID('dbo.Orders','U') IS NOT NULL DROP TABLE dbo.Orders;
IF OBJECT_ID('dbo.StagingProducts','U') IS NOT NULL DROP TABLE dbo.StagingProducts;
IF OBJECT_ID('dbo.Products','U') IS NOT NULL DROP TABLE dbo.Products;
IF OBJECT_ID('dbo.Customers','U') IS NOT NULL DROP TABLE dbo.Customers;
GO

CREATE TABLE Customers
(
    CustomerID INT PRIMARY KEY,
    Name VARCHAR(100) NOT NULL,
    Region VARCHAR(50) NOT NULL
);

CREATE TABLE Products
(
    ProductID INT PRIMARY KEY,
    ProductName VARCHAR(100) NOT NULL,
    Category VARCHAR(50) NOT NULL,
    Price DECIMAL(10,2) NOT NULL
);

CREATE TABLE Orders
(
    OrderID INT PRIMARY KEY,
    CustomerID INT NOT NULL,
    OrderDate DATE NOT NULL,
    CONSTRAINT FK_Orders_Customers
        FOREIGN KEY (CustomerID) REFERENCES Customers(CustomerID)
);

CREATE TABLE OrderDetails
(
    OrderDetailID INT PRIMARY KEY,
    OrderID INT NOT NULL,
    ProductID INT NOT NULL,
    Quantity INT NOT NULL,
    CONSTRAINT FK_OrderDetails_Orders
        FOREIGN KEY (OrderID) REFERENCES Orders(OrderID),
    CONSTRAINT FK_OrderDetails_Products
        FOREIGN KEY (ProductID) REFERENCES Products(ProductID)
);
GO

INSERT INTO Customers VALUES
(1,'Alice','North'),(2,'Bob','South'),(3,'Charlie','East'),
(4,'David','West'),(5,'Eva','North');

INSERT INTO Products VALUES
(1,'Laptop','Electronics',1200.00),
(2,'Smartphone','Electronics',800.00),
(3,'Tablet','Electronics',600.00),
(4,'Headphones','Accessories',150.00),
(5,'Keyboard','Accessories',80.00),
(6,'Mouse','Accessories',40.00),
(7,'Monitor','Electronics',600.00);

INSERT INTO Orders VALUES
(1,1,'2025-01-03'),(2,1,'2025-01-08'),(3,1,'2025-01-15'),
(4,1,'2025-01-20'),(5,2,'2025-01-05'),(6,3,'2025-01-11'),
(7,4,'2025-01-18'),(8,5,'2025-01-25');

INSERT INTO OrderDetails VALUES
(1,1,1,1),(2,1,4,2),(3,2,2,1),(4,3,3,2),
(5,4,5,3),(6,5,2,2),(7,6,6,4),(8,7,7,1),
(9,8,4,3),(10,8,5,1);
GO

PRINT 'EXERCISE 1: ROW_NUMBER, RANK AND DENSE_RANK';
WITH ProductRanks AS
(
    SELECT
        ProductID,
        ProductName,
        Category,
        Price,
        ROW_NUMBER() OVER
            (PARTITION BY Category ORDER BY Price DESC) AS RowNumberRank,
        RANK() OVER
            (PARTITION BY Category ORDER BY Price DESC) AS RankValue,
        DENSE_RANK() OVER
            (PARTITION BY Category ORDER BY Price DESC) AS DenseRankValue
    FROM Products
)
SELECT *
FROM ProductRanks
WHERE DenseRankValue <= 3
ORDER BY Category, Price DESC, ProductID;
GO

PRINT 'EXERCISE 2A: GROUPING SETS';
SELECT
    COALESCE(c.Region,'ALL REGIONS') AS Region,
    COALESCE(p.Category,'ALL CATEGORIES') AS Category,
    SUM(od.Quantity) AS TotalQuantity
FROM OrderDetails od
JOIN Orders o ON o.OrderID = od.OrderID
JOIN Customers c ON c.CustomerID = o.CustomerID
JOIN Products p ON p.ProductID = od.ProductID
GROUP BY GROUPING SETS
(
    (c.Region,p.Category),
    (c.Region),
    (p.Category),
    ()
)
ORDER BY Region, Category;
GO

PRINT 'EXERCISE 2B: ROLLUP';
SELECT
    COALESCE(c.Region,'ALL REGIONS') AS Region,
    COALESCE(p.Category,'ALL CATEGORIES') AS Category,
    SUM(od.Quantity) AS TotalQuantity
FROM OrderDetails od
JOIN Orders o ON o.OrderID = od.OrderID
JOIN Customers c ON c.CustomerID = o.CustomerID
JOIN Products p ON p.ProductID = od.ProductID
GROUP BY ROLLUP(c.Region,p.Category);
GO

PRINT 'EXERCISE 2C: CUBE';
SELECT
    COALESCE(c.Region,'ALL REGIONS') AS Region,
    COALESCE(p.Category,'ALL CATEGORIES') AS Category,
    SUM(od.Quantity) AS TotalQuantity
FROM OrderDetails od
JOIN Orders o ON o.OrderID = od.OrderID
JOIN Customers c ON c.CustomerID = o.CustomerID
JOIN Products p ON p.ProductID = od.ProductID
GROUP BY CUBE(c.Region,p.Category);
GO

PRINT 'EXERCISE 3A: RECURSIVE CTE CALENDAR';
WITH CalendarCTE AS
(
    SELECT CAST('2025-01-01' AS DATE) AS CalendarDate
    UNION ALL
    SELECT DATEADD(DAY,1,CalendarDate)
    FROM CalendarCTE
    WHERE CalendarDate < '2025-01-31'
)
SELECT CalendarDate
FROM CalendarCTE
OPTION (MAXRECURSION 31);
GO

PRINT 'EXERCISE 3B: MERGE';
CREATE TABLE StagingProducts
(
    ProductID INT PRIMARY KEY,
    ProductName VARCHAR(100),
    Category VARCHAR(50),
    Price DECIMAL(10,2)
);

INSERT INTO StagingProducts VALUES
(1,'Laptop','Electronics',1150.00),
(2,'Smartphone','Electronics',780.00),
(8,'Webcam','Accessories',95.00);

MERGE Products AS Target
USING StagingProducts AS Source
ON Target.ProductID = Source.ProductID
WHEN MATCHED THEN
    UPDATE SET
        Target.ProductName = Source.ProductName,
        Target.Category = Source.Category,
        Target.Price = Source.Price
WHEN NOT MATCHED BY TARGET THEN
    INSERT(ProductID,ProductName,Category,Price)
    VALUES(Source.ProductID,Source.ProductName,Source.Category,Source.Price);

SELECT * FROM Products ORDER BY ProductID;
GO

PRINT 'EXERCISE 4A: PIVOT';
WITH MonthlySales AS
(
    SELECT
        p.ProductName,
        'Jan' AS SalesMonth,
        od.Quantity
    FROM OrderDetails od
    JOIN Orders o ON o.OrderID = od.OrderID
    JOIN Products p ON p.ProductID = od.ProductID
    WHERE YEAR(o.OrderDate)=2025 AND MONTH(o.OrderDate)=1
)
SELECT ProductName, ISNULL([Jan],0) AS Jan
FROM MonthlySales
PIVOT
(
    SUM(Quantity) FOR SalesMonth IN ([Jan])
) p
ORDER BY ProductName;
GO

PRINT 'EXERCISE 4B: UNPIVOT';
WITH MonthlySales AS
(
    SELECT
        p.ProductName,
        'Jan' AS SalesMonth,
        od.Quantity
    FROM OrderDetails od
    JOIN Orders o ON o.OrderID = od.OrderID
    JOIN Products p ON p.ProductID = od.ProductID
),
Pivoted AS
(
    SELECT ProductName, ISNULL([Jan],0) AS Jan
    FROM MonthlySales
    PIVOT(SUM(Quantity) FOR SalesMonth IN ([Jan])) p
)
SELECT ProductName, SalesMonth, Quantity
FROM Pivoted
UNPIVOT
(
    Quantity FOR SalesMonth IN ([Jan])
) u
ORDER BY ProductName;
GO

PRINT 'EXERCISE 5: CUSTOMERS WITH MORE THAN 3 ORDERS';
WITH CustomerOrderCounts AS
(
    SELECT CustomerID, COUNT(OrderID) AS OrderCount
    FROM Orders
    GROUP BY CustomerID
)
SELECT c.CustomerID,c.Name,coc.OrderCount
FROM CustomerOrderCounts coc
JOIN Customers c ON c.CustomerID = coc.CustomerID
WHERE coc.OrderCount > 3;
GO

SET NOCOUNT ON;

IF OBJECT_ID('dbo.OrderDetails','U') IS NOT NULL DROP TABLE dbo.OrderDetails;
IF OBJECT_ID('dbo.Orders','U') IS NOT NULL DROP TABLE dbo.Orders;
IF OBJECT_ID('dbo.Products','U') IS NOT NULL DROP TABLE dbo.Products;
IF OBJECT_ID('dbo.Customers','U') IS NOT NULL DROP TABLE dbo.Customers;
GO

CREATE TABLE Customers
(
    CustomerID INT PRIMARY KEY,
    Name VARCHAR(100),
    Region VARCHAR(50)
);

CREATE TABLE Products
(
    ProductID INT PRIMARY KEY,
    ProductName VARCHAR(100),
    Category VARCHAR(50),
    Price DECIMAL(10,2)
);

-- Heap is used here so OrderDate can receive the requested clustered index.
CREATE TABLE Orders
(
    OrderID INT NOT NULL UNIQUE NONCLUSTERED,
    CustomerID INT,
    OrderDate DATE,
    FOREIGN KEY(CustomerID) REFERENCES Customers(CustomerID)
);

CREATE TABLE OrderDetails
(
    OrderDetailID INT PRIMARY KEY,
    OrderID INT,
    ProductID INT,
    Quantity INT,
    FOREIGN KEY(OrderID) REFERENCES Orders(OrderID),
    FOREIGN KEY(ProductID) REFERENCES Products(ProductID)
);
GO

INSERT INTO Customers VALUES
(1,'Alice','North'),(2,'Bob','South'),(3,'Charlie','East'),(4,'David','West');

INSERT INTO Products VALUES
(1,'Laptop','Electronics',1200.00),
(2,'Smartphone','Electronics',800.00),
(3,'Tablet','Electronics',600.00),
(4,'Headphones','Accessories',150.00);

INSERT INTO Orders VALUES
(1,1,'2023-01-15'),(2,2,'2023-02-20'),
(3,3,'2023-03-25'),(4,4,'2023-04-30');

INSERT INTO OrderDetails VALUES
(1,1,1,1),(2,2,2,2),(3,3,3,1),(4,4,4,3);
GO

PRINT 'Before non-clustered index';
SELECT * FROM Products WHERE ProductName='Laptop';

CREATE NONCLUSTERED INDEX IX_Products_ProductName
ON Products(ProductName);
GO

PRINT 'After non-clustered index';
SELECT * FROM Products WHERE ProductName='Laptop';
GO

PRINT 'Before clustered index';
SELECT * FROM Orders WHERE OrderDate='2023-01-15';

CREATE CLUSTERED INDEX IX_Orders_OrderDate
ON Orders(OrderDate);
GO

PRINT 'After clustered index';
SELECT * FROM Orders WHERE OrderDate='2023-01-15';
GO

PRINT 'Before composite index';
SELECT * FROM Orders
WHERE CustomerID=1 AND OrderDate='2023-01-15';

CREATE NONCLUSTERED INDEX IX_Orders_CustomerID_OrderDate
ON Orders(CustomerID,OrderDate);
GO

PRINT 'After composite index';
SELECT * FROM Orders
WHERE CustomerID=1 AND OrderDate='2023-01-15';
GO

SELECT
    OBJECT_NAME(i.object_id) AS TableName,
    i.name AS IndexName,
    i.type_desc AS IndexType
FROM sys.indexes i
WHERE OBJECT_NAME(i.object_id) IN ('Products','Orders')
  AND i.name IS NOT NULL
ORDER BY TableName,IndexName;
GO

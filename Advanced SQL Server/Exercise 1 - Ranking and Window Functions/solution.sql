-- Exercise 1: Ranking and Window Functions

IF OBJECT_ID('dbo.Products', 'U') IS NOT NULL
    DROP TABLE dbo.Products;
GO

CREATE TABLE Products
(
    ProductID INT PRIMARY KEY,
    ProductName VARCHAR(100) NOT NULL,
    Category VARCHAR(50) NOT NULL,
    Price DECIMAL(10,2) NOT NULL
);
GO

INSERT INTO Products VALUES
(1, 'Laptop', 'Electronics', 75000.00),
(2, 'Smartphone', 'Electronics', 50000.00),
(3, 'Tablet', 'Electronics', 30000.00),
(4, 'Smart Watch', 'Electronics', 30000.00),
(5, 'Headphones', 'Electronics', 5000.00),
(6, 'Sofa', 'Furniture', 45000.00),
(7, 'Dining Table', 'Furniture', 35000.00),
(8, 'Bed', 'Furniture', 30000.00),
(9, 'Chair', 'Furniture', 5000.00),
(10, 'Novel', 'Books', 900.00),
(11, 'Dictionary', 'Books', 700.00),
(12, 'Notebook', 'Books', 150.00),
(13, 'Story Book', 'Books', 150.00);
GO

WITH RankedProducts AS
(
    SELECT
        ProductID,
        ProductName,
        Category,
        Price,
        ROW_NUMBER() OVER
        (
            PARTITION BY Category
            ORDER BY Price DESC
        ) AS RowNumberRank,
        RANK() OVER
        (
            PARTITION BY Category
            ORDER BY Price DESC
        ) AS RankValue,
        DENSE_RANK() OVER
        (
            PARTITION BY Category
            ORDER BY Price DESC
        ) AS DenseRankValue
    FROM Products
)
SELECT
    ProductID,
    ProductName,
    Category,
    Price,
    RowNumberRank,
    RankValue,
    DenseRankValue
FROM RankedProducts
WHERE DenseRankValue <= 3
ORDER BY Category, Price DESC, ProductID;
GO

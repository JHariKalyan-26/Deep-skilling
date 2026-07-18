# Entity Framework Core - Retail Inventory System

Status: Completed

This module implements all 15 guided hands-on labs from the official **EF Core 8.0 HOL**:

1. ORM concepts and EF Core setup
2. Models and `DbContext`
3. Migrations
4. Insert data
5. Retrieve data
6. Update and delete
7. LINQ queries and projection
8. Schema change with `StockQuantity`
9. Seed data with `HasData`
10. Eager, explicit and lazy-loading configuration
11. One-to-one and many-to-many relationships
12. DTO projection to avoid circular references
13. `AsNoTracking` and compiled queries
14. Batch updates using `ExecuteUpdateAsync`
15. Optimistic concurrency with `RowVersion`

## Run

The demonstration uses EF Core InMemory so it runs without SQL Server:

```powershell
cd RetailInventory
dotnet restore
dotnet run -- --in-memory
```

## SQL Server mode

Update the connection string in `appsettings.json`, then run:

```powershell
dotnet tool install --global dotnet-ef --version 8.*
dotnet ef migrations add InitialCreate
dotnet ef database update
dotnet run
```

The code uses SQL Server by default and switches to InMemory only when `--in-memory` is supplied.

## Project structure

```text
RetailInventory/
├── Data/
│   ├── AppDbContext.cs
│   └── AppDbContextFactory.cs
├── DTOs/
│   └── ProductDto.cs
├── Models/
│   ├── Category.cs
│   ├── Product.cs
│   ├── ProductDetail.cs
│   └── Tag.cs
├── Services/
│   └── InventoryLabService.cs
├── appsettings.json
├── Program.cs
└── RetailInventory.csproj
```

Actual console output is saved in `Output/output.txt`.

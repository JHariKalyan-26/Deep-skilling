# EF Core migrations

Generate SQL Server migrations from the project directory:

```powershell
dotnet tool install --global dotnet-ef --version 8.*
dotnet ef migrations add InitialCreate
dotnet ef database update
```

After adding `StockQuantity`, the corresponding schema-change command is:

```powershell
dotnet ef migrations add AddStockQuantity
dotnet ef database update
```

The final model already includes `StockQuantity`, relationship mappings, seed data and `RowVersion`.

using Microsoft.Extensions.Configuration;
using RetailInventory.Data;
using RetailInventory.Services;

var configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: true)
    .Build();

var useInMemory = args.Any(
    argument => argument.Equals(
        "--in-memory",
        StringComparison.OrdinalIgnoreCase));

var connectionString = configuration.GetConnectionString("RetailInventory")
    ?? "Server=(localdb)\\MSSQLLocalDB;Database=RetailInventoryDb;Trusted_Connection=True;TrustServerCertificate=True";

Console.WriteLine("EF CORE 8.0 - RETAIL INVENTORY SYSTEM");
Console.WriteLine($"Provider: {(useInMemory ? "InMemory demonstration" : "SQL Server")}");

await using var context = new AppDbContext(connectionString, useInMemory);
var service = new InventoryLabService(context);

try
{
    Console.WriteLine("\nLABS 1-4, 8 AND 9 - SETUP, CONTEXT, MIGRATIONS AND DATA");
    await service.EnsureDemoDataAsync();
    Console.WriteLine("Database initialized and initial data available.");
    Console.WriteLine("StockQuantity is included in the Product model.");
    Console.WriteLine("HasData seed configuration is included in AppDbContext.");

    await service.RunRetrievalLabsAsync();
    await service.RunUpdateDeleteLabAsync();
    await service.RunLinqLabAsync();
    await service.RunLoadingLabAsync();
    await service.RunRelationshipAndDtoLabsAsync();
    await service.RunPerformanceLabAsync();
    await service.RunBatchLabAsync();
    await service.RunConcurrencyLabAsync();

    Console.WriteLine("\nALL 15 EF CORE LABS COMPLETED SUCCESSFULLY.");
}
catch (Exception exception)
{
    Console.Error.WriteLine($"Execution failed: {exception.Message}");
    Environment.ExitCode = 1;
}

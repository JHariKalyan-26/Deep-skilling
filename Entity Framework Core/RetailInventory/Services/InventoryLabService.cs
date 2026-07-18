using Microsoft.EntityFrameworkCore;
using RetailInventory.Data;
using RetailInventory.DTOs;
using RetailInventory.Models;

namespace RetailInventory.Services;

public sealed class InventoryLabService
{
    private readonly AppDbContext _context;

    public InventoryLabService(AppDbContext context)
    {
        _context = context;
    }

    public async Task EnsureDemoDataAsync()
    {
        await _context.Database.EnsureCreatedAsync();

        if (await _context.Products.AnyAsync())
        {
            return;
        }

        var electronics = new Category { Name = "Electronics" };
        var groceries = new Category { Name = "Groceries" };

        await _context.Categories.AddRangeAsync(electronics, groceries);

        var laptop = new Product
        {
            Name = "Laptop",
            Price = 75000m,
            StockQuantity = 10,
            Category = electronics,
            ProductDetail = new ProductDetail
            {
                WarrantyInfo = "Two-year manufacturer warranty"
            }
        };

        var riceBag = new Product
        {
            Name = "Rice Bag",
            Price = 1200m,
            StockQuantity = 40,
            Category = groceries
        };

        var saleTag = new Tag { Name = "On Sale" };
        var newArrivalTag = new Tag { Name = "New Arrival" };

        laptop.Tags.Add(newArrivalTag);
        riceBag.Tags.Add(saleTag);

        await _context.Products.AddRangeAsync(laptop, riceBag);
        await _context.SaveChangesAsync();
    }

    public async Task RunRetrievalLabsAsync()
    {
        Console.WriteLine("\nLAB 5 - RETRIEVING DATA");

        var products = await _context.Products
            .OrderBy(product => product.Id)
            .ToListAsync();

        foreach (var product in products)
        {
            Console.WriteLine($"{product.Name} - Rs.{product.Price:0.00}");
        }

        var byId = await _context.Products.FindAsync(products.First().Id);
        Console.WriteLine($"FindAsync: {byId?.Name}");

        var expensive = await _context.Products
            .FirstOrDefaultAsync(product => product.Price > 50000m);

        Console.WriteLine($"First expensive product: {expensive?.Name ?? "None"}");
    }

    public async Task RunUpdateDeleteLabAsync()
    {
        Console.WriteLine("\nLAB 6 - UPDATE AND DELETE");

        var laptop = await _context.Products
            .FirstOrDefaultAsync(product => product.Name == "Laptop");

        if (laptop is not null)
        {
            laptop.Price = 70000m;
            await _context.SaveChangesAsync();
            Console.WriteLine($"Updated Laptop price: Rs.{laptop.Price:0.00}");
        }

        var riceBag = await _context.Products
            .FirstOrDefaultAsync(product => product.Name == "Rice Bag");

        if (riceBag is not null)
        {
            _context.Products.Remove(riceBag);
            await _context.SaveChangesAsync();
            Console.WriteLine("Deleted product: Rice Bag");
        }
    }

    public async Task RunLinqLabAsync()
    {
        Console.WriteLine("\nLAB 7 - LINQ");

        var filtered = await _context.Products
            .Where(product => product.Price > 1000m)
            .OrderByDescending(product => product.Price)
            .ToListAsync();

        foreach (var product in filtered)
        {
            Console.WriteLine($"Filtered: {product.Name} - Rs.{product.Price:0.00}");
        }

        var projections = await _context.Products
            .Select(product => new { product.Name, product.Price })
            .ToListAsync();

        foreach (var product in projections)
        {
            Console.WriteLine($"Projection: {product.Name}, Rs.{product.Price:0.00}");
        }
    }

    public async Task RunLoadingLabAsync()
    {
        Console.WriteLine("\nLAB 10 - RELATED DATA LOADING");

        var eagerLoaded = await _context.Products
            .Include(product => product.Category)
            .OrderBy(product => product.Id)
            .ToListAsync();

        foreach (var product in eagerLoaded)
        {
            Console.WriteLine($"Eager: {product.Name} -> {product.Category?.Name}");
        }

        var explicitProduct = await _context.Products
            .OrderBy(product => product.Id)
            .FirstAsync();

        await _context.Entry(explicitProduct)
            .Reference(product => product.Category)
            .LoadAsync();

        Console.WriteLine(
            $"Explicit: {explicitProduct.Name} -> {explicitProduct.Category?.Name}");

        // Lazy-loading proxies are enabled; virtual navigation properties support it.
        var lazyProduct = await _context.Products
            .OrderBy(product => product.Id)
            .FirstAsync();

        Console.WriteLine(
            $"Lazy: {lazyProduct.Name} -> {lazyProduct.Category?.Name}");
    }

    public async Task RunRelationshipAndDtoLabsAsync()
    {
        Console.WriteLine("\nLABS 11 AND 12 - RELATIONSHIPS AND DTO");

        var product = await _context.Products
            .Include(item => item.ProductDetail)
            .Include(item => item.Tags)
            .OrderBy(item => item.Id)
            .FirstAsync();

        Console.WriteLine(
            $"One-to-one detail: {product.ProductDetail?.WarrantyInfo ?? "Not available"}");

        Console.WriteLine(
            $"Many-to-many tags: {string.Join(", ", product.Tags.Select(tag => tag.Name))}");

        var productDtos = await _context.Products
            .Include(item => item.Category)
            .Select(item => new ProductDto
            {
                Name = item.Name,
                Price = item.Price,
                StockQuantity = item.StockQuantity,
                CategoryName = item.Category == null
                    ? "Unknown"
                    : item.Category.Name
            })
            .ToListAsync();

        foreach (var dto in productDtos)
        {
            Console.WriteLine(
                $"DTO: {dto.Name}, {dto.CategoryName}, Stock={dto.StockQuantity}");
        }
    }

    public async Task RunPerformanceLabAsync()
    {
        Console.WriteLine("\nLAB 13 - PERFORMANCE");

        var readOnlyProducts = await _context.Products
            .AsNoTracking()
            .ToListAsync();

        Console.WriteLine($"AsNoTracking count: {readOnlyProducts.Count}");

        var compiledQuery = EF.CompileAsyncQuery(
            (AppDbContext context, decimal minimumPrice) =>
                context.Products
                    .AsNoTracking()
                    .Count(product => product.Price > minimumPrice));

        var compiledCount = await compiledQuery(_context, 10000m);

        Console.WriteLine($"Compiled query count above Rs.10000: {compiledCount}");
    }

    public async Task RunBatchLabAsync()
    {
        Console.WriteLine("\nLAB 14 - BATCH UPDATE");

        if (_context.Database.IsRelational())
        {
            var affected = await _context.Products
                .Where(product => product.StockQuantity < 20)
                .ExecuteUpdateAsync(setters => setters
                    .SetProperty(
                        product => product.StockQuantity,
                        product => product.StockQuantity + 10));

            Console.WriteLine($"ExecuteUpdate affected rows: {affected}");
        }
        else
        {
            var products = await _context.Products
                .Where(product => product.StockQuantity < 20)
                .ToListAsync();

            foreach (var product in products)
            {
                product.StockQuantity += 10;
            }

            await _context.SaveChangesAsync();
            Console.WriteLine($"InMemory batch fallback affected rows: {products.Count}");
        }
    }

    public async Task RunConcurrencyLabAsync()
    {
        Console.WriteLine("\nLAB 15 - CONCURRENCY");

        var product = await _context.Products
            .OrderBy(item => item.Id)
            .FirstAsync();

        product.StockQuantity += 1;

        try
        {
            await _context.SaveChangesAsync();
            Console.WriteLine(
                "RowVersion configured; update completed without a conflict.");
        }
        catch (DbUpdateConcurrencyException)
        {
            Console.WriteLine("Concurrency conflict detected.");
        }
    }
}


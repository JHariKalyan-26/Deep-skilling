using Microsoft.EntityFrameworkCore;
using RetailInventory.Models;

namespace RetailInventory.Data;

public class AppDbContext : DbContext
{
    private readonly string? _connectionString;
    private readonly bool _useInMemory;

    public AppDbContext()
    {
    }

    public AppDbContext(string connectionString, bool useInMemory = false)
    {
        _connectionString = connectionString;
        _useInMemory = useInMemory;
    }

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<Product> Products => Set<Product>();

    public DbSet<Category> Categories => Set<Category>();

    public DbSet<ProductDetail> ProductDetails => Set<ProductDetail>();

    public DbSet<Tag> Tags => Set<Tag>();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (optionsBuilder.IsConfigured)
        {
            return;
        }

        if (_useInMemory)
        {
            optionsBuilder
                .UseLazyLoadingProxies()
                .UseInMemoryDatabase("RetailInventoryDemo");
            return;
        }

        var connectionString = _connectionString
            ?? "Server=(localdb)\\MSSQLLocalDB;Database=RetailInventoryDb;Trusted_Connection=True;TrustServerCertificate=True";

        optionsBuilder
            .UseLazyLoadingProxies()
            .UseSqlServer(connectionString);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Category>()
            .Property(category => category.Name)
            .HasMaxLength(100)
            .IsRequired();

        modelBuilder.Entity<Product>()
            .Property(product => product.Price)
            .HasPrecision(18, 2);

        modelBuilder.Entity<Product>()
            .HasOne(product => product.Category)
            .WithMany(category => category.Products)
            .HasForeignKey(product => product.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Product>()
            .HasOne(product => product.ProductDetail)
            .WithOne(detail => detail.Product)
            .HasForeignKey<ProductDetail>(detail => detail.ProductId);

        modelBuilder.Entity<Product>()
            .HasMany(product => product.Tags)
            .WithMany(tag => tag.Products)
            .UsingEntity(join => join.ToTable("ProductTags"));

        modelBuilder.Entity<Product>()
            .Property(product => product.RowVersion)
            .IsRowVersion();

        // Lab 9 - seed data during migrations.
        modelBuilder.Entity<Category>().HasData(
            new Category { Id = 1, Name = "Electronics" },
            new Category { Id = 2, Name = "Groceries" }
        );

        modelBuilder.Entity<Product>().HasData(
            new Product
            {
                Id = 1,
                Name = "Smartphone",
                Price = 25000m,
                CategoryId = 1,
                StockQuantity = 50
            },
            new Product
            {
                Id = 2,
                Name = "Wheat Flour",
                Price = 800m,
                CategoryId = 2,
                StockQuantity = 100
            }
        );
    }
}

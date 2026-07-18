namespace RetailInventory.DTOs;

public sealed class ProductDto
{
    public string Name { get; init; } = string.Empty;

    public decimal Price { get; init; }

    public int StockQuantity { get; init; }

    public string CategoryName { get; init; } = string.Empty;
}

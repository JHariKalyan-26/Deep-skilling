namespace ProductApi.Models;

public class Product
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public string Category { get; set; } = "General";
    public decimal Price { get; set; }
    public int Stock { get; set; }
}

public class Product
{
    public int ProductId { get; set; }
    public string ProductName { get; set; }
    public int Quantity { get; set; }
    public double Price { get; set; }

    public Product(int productId, string productName, int quantity, double price)
    {
        ProductId = productId;
        ProductName = productName;
        Quantity = quantity;
        Price = price;
    }
}
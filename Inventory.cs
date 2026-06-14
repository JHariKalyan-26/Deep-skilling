using System;
using System.Collections.Generic;

public class Inventory
{
    private Dictionary<int, Product> products = new Dictionary<int, Product>();

    public void AddProduct(Product product)
    {
        products[product.ProductId] = product;
        Console.WriteLine("Product Added Successfully");
    }

    public void UpdateProduct(int productId, int quantity, double price)
    {
        if (products.ContainsKey(productId))
        {
            products[productId].Quantity = quantity;
            products[productId].Price = price;
            Console.WriteLine("Product Updated Successfully");
        }
        else
        {
            Console.WriteLine("Product Not Found");
        }
    }

    public void DeleteProduct(int productId)
    {
        if (products.Remove(productId))
        {
            Console.WriteLine("Product Deleted Successfully");
        }
        else
        {
            Console.WriteLine("Product Not Found");
        }
    }

    public void DisplayProducts()
    {
        Console.WriteLine("\nInventory Products:");

        foreach (var product in products.Values)
        {
            Console.WriteLine($"ID: {product.ProductId}, Name: {product.ProductName}, Quantity: {product.Quantity}, Price: {product.Price}");
        }
    }
}
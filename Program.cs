using System;

class Program
{
    static void Main(string[] args)
    {
        Inventory inventory = new Inventory();

        inventory.AddProduct(new Product(1, "Laptop", 10, 50000));
        inventory.AddProduct(new Product(2, "Mouse", 50, 500));
        inventory.AddProduct(new Product(3, "Keyboard", 25, 1500));

        inventory.DisplayProducts();

        inventory.UpdateProduct(1, 15, 55000);

        inventory.DeleteProduct(2);

        Console.WriteLine("\nAfter Update and Delete:");

        inventory.DisplayProducts();
    }
}
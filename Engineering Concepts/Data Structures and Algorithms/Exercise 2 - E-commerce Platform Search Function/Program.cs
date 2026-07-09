
using System;

class Product
{
    public int ProductId;
    public string ProductName;
    public string Category;

    public Product(int id, string name, string category)
    {
        ProductId = id;
        ProductName = name;
        Category = category;
    }
}

class Program
{
    // Linear Search
    static int LinearSearch(Product[] products, string key)
    {
        for (int i = 0; i < products.Length; i++)
        {
            if (products[i].ProductName.Equals(key, StringComparison.OrdinalIgnoreCase))
                return i;
        }
        return -1;
    }

    // Binary Search (Sorted Array)
    static int BinarySearch(Product[] products, string key)
    {
        int low = 0;
        int high = products.Length - 1;

        while (low <= high)
        {
            int mid = (low + high) / 2;

            int result = string.Compare(products[mid].ProductName, key, true);

            if (result == 0)
                return mid;
            else if (result < 0)
                low = mid + 1;
            else
                high = mid - 1;
        }

        return -1;
    }

    static void Main()
    {
        Product[] products =
        {
            new Product(101,"Headphones","Electronics"),
            new Product(102,"Keyboard","Electronics"),
            new Product(103,"Laptop","Electronics"),
            new Product(104,"Mouse","Electronics"),
            new Product(105,"Phone","Electronics")
        };

        Console.Write("Enter Product Name to Search: ");
        string search = Console.ReadLine();

        int linearIndex = LinearSearch(products, search);

        if (linearIndex != -1)
        {
            Console.WriteLine("\nLinear Search Result");
            Console.WriteLine("Product Found");
            Console.WriteLine("ID : " + products[linearIndex].ProductId);
            Console.WriteLine("Name : " + products[linearIndex].ProductName);
            Console.WriteLine("Category : " + products[linearIndex].Category);
        }
        else
        {
            Console.WriteLine("\nProduct Not Found using Linear Search");
        }

        int binaryIndex = BinarySearch(products, search);

        if (binaryIndex != -1)
        {
            Console.WriteLine("\nBinary Search Result");
            Console.WriteLine("Product Found");
            Console.WriteLine("ID : " + products[binaryIndex].ProductId);
            Console.WriteLine("Name : " + products[binaryIndex].ProductName);
            Console.WriteLine("Category : " + products[binaryIndex].Category);
        }
        else
        {
            Console.WriteLine("\nProduct Not Found using Binary Search");
        }

        Console.WriteLine("\nTime Complexity");
        Console.WriteLine("Linear Search : O(n)");
        Console.WriteLine("Binary Search : O(log n)");
    }
}
using System;

class CustomerOrder
{
    public int OrderId;
    public string CustomerName;
    public double TotalPrice;

    public CustomerOrder(int id, string name, double price)
    {
        OrderId = id;
        CustomerName = name;
        TotalPrice = price;
    }
}

class Program
{
    static void BubbleSort(CustomerOrder[] orders)
    {
        int n = orders.Length;

        for (int i = 0; i < n - 1; i++)
        {
            for (int j = 0; j < n - i - 1; j++)
            {
                if (orders[j].TotalPrice > orders[j + 1].TotalPrice)
                {
                    CustomerOrder temp = orders[j];
                    orders[j] = orders[j + 1];
                    orders[j + 1] = temp;
                }
            }
        }
    }

    static int Partition(CustomerOrder[] orders, int low, int high)
    {
        double pivot = orders[high].TotalPrice;
        int i = low - 1;

        for (int j = low; j < high; j++)
        {
            if (orders[j].TotalPrice < pivot)
            {
                i++;

                CustomerOrder temp = orders[i];
                orders[i] = orders[j];
                orders[j] = temp;
            }
        }

        CustomerOrder t = orders[i + 1];
        orders[i + 1] = orders[high];
        orders[high] = t;

        return i + 1;
    }

    static void QuickSort(CustomerOrder[] orders, int low, int high)
    {
        if (low < high)
        {
            int pi = Partition(orders, low, high);

            QuickSort(orders, low, pi - 1);
            QuickSort(orders, pi + 1, high);
        }
    }

    static void Display(CustomerOrder[] orders)
    {
        foreach (CustomerOrder order in orders)
        {
            Console.WriteLine(order.OrderId + "  " +
                              order.CustomerName + "  Rs." +
                              order.TotalPrice);
        }
    }

    static CustomerOrder[] CopyArray(CustomerOrder[] original)
    {
        CustomerOrder[] copy = new CustomerOrder[original.Length];

        for (int i = 0; i < original.Length; i++)
        {
            copy[i] = new CustomerOrder(
                original[i].OrderId,
                original[i].CustomerName,
                original[i].TotalPrice);
        }

        return copy;
    }

    static void Main()
    {
        CustomerOrder[] orders =
        {
            new CustomerOrder(101,"Hari",2500),
            new CustomerOrder(102,"Kiran",1200),
            new CustomerOrder(103,"Ravi",4500),
            new CustomerOrder(104,"Ajay",1800),
            new CustomerOrder(105,"Rahul",3200)
        };

        CustomerOrder[] bubble = CopyArray(orders);
        CustomerOrder[] quick = CopyArray(orders);

        BubbleSort(bubble);

        Console.WriteLine("Bubble Sort Result");
        Display(bubble);

        QuickSort(quick, 0, quick.Length - 1);

        Console.WriteLine("\nQuick Sort Result");
        Display(quick);

        Console.WriteLine("\nTime Complexity");
        Console.WriteLine("Bubble Sort : O(n^2)");
        Console.WriteLine("Quick Sort  : O(n log n) (Average)");
    }
}
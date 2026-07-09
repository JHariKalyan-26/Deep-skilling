using System;

class Book
{
    public int BookId;
    public string Title;
    public string Author;

    public Book(int id, string title, string author)
    {
        BookId = id;
        Title = title;
        Author = author;
    }
}

class Program
{
    static int LinearSearch(Book[] books, string title)
    {
        for (int i = 0; i < books.Length; i++)
        {
            if (books[i].Title.Equals(title, StringComparison.OrdinalIgnoreCase))
                return i;
        }
        return -1;
    }

    static int BinarySearch(Book[] books, string title)
    {
        int low = 0, high = books.Length - 1;

        while (low <= high)
        {
            int mid = (low + high) / 2;
            int result = string.Compare(books[mid].Title, title, true);

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
        Book[] books =
        {
            new Book(101, "CSharp Basics", "John"),
            new Book(102, "Data Structures", "Alice"),
            new Book(103, "Java Programming", "Robert"),
            new Book(104, "Python Guide", "David")
        };

        Console.Write("Enter Book Title to Search: ");
        string title = Console.ReadLine();

        int linearIndex = LinearSearch(books, title);
        int binaryIndex = BinarySearch(books, title);

        Console.WriteLine(linearIndex != -1 ? "\nBook Found using Linear Search" : "\nBook Not Found using Linear Search");
        Console.WriteLine(binaryIndex != -1 ? "Book Found using Binary Search" : "Book Not Found using Binary Search");

        Console.WriteLine("\nTime Complexity");
        Console.WriteLine("Linear Search : O(n)");
        Console.WriteLine("Binary Search : O(log n)");
    }
}
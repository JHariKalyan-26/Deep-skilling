using System;

class Task
{
    public int TaskId;
    public string TaskName;
    public string Status;
    public Task Next;

    public Task(int id, string name, string status)
    {
        TaskId = id;
        TaskName = name;
        Status = status;
        Next = null;
    }
}

class TaskManager
{
    Task head = null;

    public void AddTask(int id, string name, string status)
    {
        Task newTask = new Task(id, name, status);

        if (head == null)
        {
            head = newTask;
        }
        else
        {
            Task temp = head;
            while (temp.Next != null)
                temp = temp.Next;

            temp.Next = newTask;
        }
    }

    public void DisplayTasks()
    {
        Console.WriteLine("Task List");

        Task temp = head;

        while (temp != null)
        {
            Console.WriteLine(temp.TaskId + "  " +
                              temp.TaskName + "  " +
                              temp.Status);
            temp = temp.Next;
        }
    }

    public void SearchTask(int id)
    {
        Task temp = head;

        while (temp != null)
        {
            if (temp.TaskId == id)
            {
                Console.WriteLine("\nTask Found");
                Console.WriteLine("ID : " + temp.TaskId);
                Console.WriteLine("Name : " + temp.TaskName);
                Console.WriteLine("Status : " + temp.Status);
                return;
            }

            temp = temp.Next;
        }

        Console.WriteLine("\nTask Not Found");
    }

    public void DeleteTask(int id)
    {
        if (head == null)
        {
            Console.WriteLine("No tasks available");
            return;
        }

        if (head.TaskId == id)
        {
            head = head.Next;
            Console.WriteLine("\nTask Deleted");
            return;
        }

        Task temp = head;

        while (temp.Next != null && temp.Next.TaskId != id)
        {
            temp = temp.Next;
        }

        if (temp.Next == null)
        {
            Console.WriteLine("\nTask Not Found");
        }
        else
        {
            temp.Next = temp.Next.Next;
            Console.WriteLine("\nTask Deleted");
        }
    }
}

class Program
{
    static void Main()
    {
        TaskManager manager = new TaskManager();

        manager.AddTask(1, "Complete Assignment", "Pending");
        manager.AddTask(2, "Attend Meeting", "Completed");
        manager.AddTask(3, "Submit Report", "Pending");

        manager.DisplayTasks();

        manager.SearchTask(2);

        manager.DeleteTask(1);

        Console.WriteLine("\nAfter Deletion:");
        manager.DisplayTasks();

        Console.WriteLine("\nTime Complexity");
        Console.WriteLine("Add Task : O(n)");
        Console.WriteLine("Search Task : O(n)");
        Console.WriteLine("Delete Task : O(n)");
        Console.WriteLine("Traverse Tasks : O(n)");
    }
}
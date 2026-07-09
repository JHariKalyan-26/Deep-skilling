using System;

class Employee
{
    public int EmployeeId;
    public string Name;
    public string Position;
    public double Salary;
    public Employee Next;

    public Employee(int id, string name, string position, double salary)
    {
        EmployeeId = id;
        Name = name;
        Position = position;
        Salary = salary;
        Next = null;
    }
}

class EmployeeManagement
{
    Employee head = null;

    // Add Employee
    public void AddEmployee(int id, string name, string position, double salary)
    {
        Employee newEmployee = new Employee(id, name, position, salary);

        if (head == null)
        {
            head = newEmployee;
        }
        else
        {
            Employee temp = head;
            while (temp.Next != null)
                temp = temp.Next;

            temp.Next = newEmployee;
        }
    }

    // Display Employees
    public void DisplayEmployees()
    {
        Console.WriteLine("Employee Details");

        Employee temp = head;

        while (temp != null)
        {
            Console.WriteLine(temp.EmployeeId + "  " +
                              temp.Name + "  " +
                              temp.Position + "  Rs." +
                              temp.Salary);

            temp = temp.Next;
        }
    }

    // Search Employee
    public void SearchEmployee(int id)
    {
        Employee temp = head;

        while (temp != null)
        {
            if (temp.EmployeeId == id)
            {
                Console.WriteLine("\nEmployee Found");
                Console.WriteLine("ID : " + temp.EmployeeId);
                Console.WriteLine("Name : " + temp.Name);
                Console.WriteLine("Position : " + temp.Position);
                Console.WriteLine("Salary : Rs." + temp.Salary);
                return;
            }

            temp = temp.Next;
        }

        Console.WriteLine("\nEmployee Not Found");
    }
}

class Program
{
    static void Main()
    {
        EmployeeManagement emp = new EmployeeManagement();

        emp.AddEmployee(101, "Hari", "Developer", 50000);
        emp.AddEmployee(102, "Kiran", "Tester", 40000);
        emp.AddEmployee(103, "Ravi", "Manager", 70000);

        emp.DisplayEmployees();

        Console.Write("\nEnter Employee ID to Search: ");
        int id = Convert.ToInt32(Console.ReadLine());

        emp.SearchEmployee(id);

        Console.WriteLine("\nTime Complexity");
        Console.WriteLine("Add Employee : O(n)");
        Console.WriteLine("Search Employee : O(n)");
    }
}
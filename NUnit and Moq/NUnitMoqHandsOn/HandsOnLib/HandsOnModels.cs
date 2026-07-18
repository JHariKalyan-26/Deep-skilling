using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace HandsOnLib;

// HANDSON 1 - Calculator
public class SimpleCalculator
{
    private double _result;

    public double Addition(double a, double b) => _result = a + b;
    public double Subtraction(double a, double b) => _result = a - b;
    public double Multiplication(double a, double b) => _result = a * b;

    public double Division(double a, double b)
    {
        if (b == 0) throw new ArgumentException("Second parameter cannot be zero.");
        return _result = a / b;
    }

    public void AllClear() => _result = 0;
    public double GetResult => _result;
}

// HANDSON 2 - Converter
public interface IDollarToEuroExchangeRateFeed
{
    double GetActualUSDollarValue();
}

public class Converter
{
    private readonly IDollarToEuroExchangeRateFeed _feed;

    public Converter(IDollarToEuroExchangeRateFeed feed) => _feed = feed;

    public double CelsiusToKelvin(double value) => value + 273.15;
    public double KilogramToPound(double value) => value * 2.205;
    public double KilometerToMile(double value) => value / 1.609;
    public double LiterToGallon(double value) => value / 3.785;
    public double USDToEuro(double value) => value * _feed.GetActualUSDollarValue();
}

// HANDSON 3 - URL parser
public class UrlHostNameParser
{
    public string ParseHostName(string url)
    {
        if (!Uri.TryCreate(url, UriKind.Absolute, out var uri) ||
            (uri.Scheme != Uri.UriSchemeHttp && uri.Scheme != Uri.UriSchemeHttps))
        {
            throw new FormatException("URL is not in correct format.");
        }

        return uri.Host;
    }
}

// HANDSON 4 - Accounts manager
public class AccountsManager
{
    public string ValidateUser(string userId, string password)
    {
        if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(password))
            throw new FormatException("Both user id and password are mandatory.");

        return (userId, password) switch
        {
            ("user_11", "secret@user11") => "Welcome user_11!!!",
            ("user_22", "secret@user22") => "Welcome user_22!!!",
            _ => "Invalid user id/password"
        };
    }
}

// HANDSON 5 - Collections
public class Employee : IEquatable<Employee>
{
    public int EmpId { get; set; }
    public string EmpName { get; set; } = string.Empty;
    public double Salary { get; set; }
    public DateTime DOJ { get; set; }

    public bool Equals(Employee? other) => other is not null && EmpId == other.EmpId;
    public override bool Equals(object? obj) => Equals(obj as Employee);
    public override int GetHashCode() => EmpId.GetHashCode();
}

public class EmployeeManager
{
    private readonly List<Employee> _employees =
    [
        new() { EmpId = 100, EmpName = "John", DOJ = DateTime.Today.AddYears(-5), Salary = 30000 },
        new() { EmpId = 101, EmpName = "Mary", DOJ = DateTime.Today.AddYears(-2), Salary = 10000 },
        new() { EmpId = 102, EmpName = "Steve", DOJ = DateTime.Today.AddYears(-2), Salary = 10000 },
        new() { EmpId = 103, EmpName = "Allen", DOJ = DateTime.Today.AddYears(-7), Salary = 50000 }
    ];

    public List<Employee> GetEmployees() => _employees;
    public List<Employee> GetEmployeesWhoJoinedInPreviousYears() =>
        _employees.Where(x => x.DOJ < DateTime.Today).ToList();
}

// HANDSON 6 - Seasons
public class SeasonTeller
{
    public string DisplaySeasonBy(string monthName) =>
        monthName.ToLowerInvariant() switch
        {
            "february" or "march" => "Spring",
            "april" or "may" or "june" => "Summer",
            "july" or "august" or "september" => "Monsoon",
            "october" or "november" => "Autumn",
            "december" or "january" => "Winter",
            _ => "Invalid Season"
        };
}

// HANDSON 7 - Leap year
public class LeapYearCalculator
{
    public int IsLeapYear(int year)
    {
        if (year < 1753 || year > 9999) return -1;
        return DateTime.IsLeapYear(year) ? 1 : 0;
    }
}

// HANDSON 8 - User manager
public class User
{
    public string PANCardNo { get; set; } = string.Empty;

    public string ValidatePANCardNumber(string panCard)
    {
        if (string.IsNullOrEmpty(panCard))
            throw new NullReferenceException("Invalid PAN card number.");

        if (panCard.Length != 10)
            throw new FormatException("PAN card number should contain 10 characters.");

        return "Valid";
    }

    public void CreateUser(User user) => ValidatePANCardNumber(user.PANCardNo);
}

// MOQ - Mail sender
public interface IMailSender
{
    bool SendMail(string toAddress, string message);
}

public class CustomerComm
{
    private readonly IMailSender _mailSender;

    public CustomerComm(IMailSender mailSender) => _mailSender = mailSender;

    public bool SendMailToCustomer() =>
        _mailSender.SendMail("cust123@abc.com", "Some Message");
}

// MOQ - Files
public interface IDirectoryExplorer
{
    ICollection<string> GetFiles(string path);
}

public class DirectoryExplorer : IDirectoryExplorer
{
    public ICollection<string> GetFiles(string path) => Directory.GetFiles(path);
}

// MOQ - Player database
public interface IPlayerMapper
{
    bool IsPlayerNameExistsInDb(string name);
    void AddNewPlayerIntoDb(string name);
}

public class Player
{
    public string Name { get; }

    private Player(string name) => Name = name;

    public static Player RegisterNewPlayer(string name, IPlayerMapper playerMapper)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Player name cannot be empty.");

        if (playerMapper.IsPlayerNameExistsInDb(name))
            throw new InvalidOperationException("Player already exists.");

        playerMapper.AddNewPlayerIntoDb(name);
        return new Player(name);
    }
}

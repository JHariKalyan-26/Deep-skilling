using HandsOnLib;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HandsOnLib.Tests;

[TestFixture]
public class CalculatorTests
{
    private SimpleCalculator _calculator = null!;

    [SetUp]
    public void Setup() => _calculator = new SimpleCalculator();

    [TestCase(10, 20, 30)]
    [TestCase(-5, 5, 0)]
    public void Addition_ValidNumbers_ReturnsSum(double a, double b, double expected) =>
        Assert.That(_calculator.Addition(a, b), Is.EqualTo(expected));

    [TestCase(10, 5, 2)]
    public void Division_ValidNumbers_ReturnsQuotient(double a, double b, double expected) =>
        Assert.That(_calculator.Division(a, b), Is.EqualTo(expected));

    [Test]
    public void Division_ZeroDivisor_ThrowsArgumentException() =>
        Assert.That(() => _calculator.Division(10, 0), Throws.TypeOf<ArgumentException>());

    [Test]
    public void AllClear_AfterCalculation_ResultBecomesZero()
    {
        _calculator.Addition(10, 5);
        _calculator.AllClear();
        Assert.That(_calculator.GetResult, Is.Zero);
    }
}

[TestFixture]
public class ConverterTests
{
    private Converter CreateConverter(double rate = 0.9)
    {
        var feed = new Mock<IDollarToEuroExchangeRateFeed>();
        feed.Setup(x => x.GetActualUSDollarValue()).Returns(rate);
        return new Converter(feed.Object);
    }

    [TestCase(0, 273.15)]
    [TestCase(100, 373.15)]
    public void CelsiusToKelvin_ValidInput_ReturnsExpected(double input, double expected) =>
        Assert.That(CreateConverter().CelsiusToKelvin(input), Is.EqualTo(expected).Within(0.001));

    [TestCase(10, 0.9, 9)]
    [TestCase(100, 0.85, 85)]
    public void USDToEuro_MockedRate_ReturnsExpected(double dollars, double rate, double expected) =>
        Assert.That(CreateConverter(rate).USDToEuro(dollars), Is.EqualTo(expected).Within(0.001));
}

[TestFixture]
public class UrlHostNameParserTests
{
    [TestCase("https://www.cognizant.com/in/en", "www.cognizant.com")]
    [TestCase("http://example.com/test", "example.com")]
    public void ParseHostName_ValidUrl_ReturnsHost(string url, string expected) =>
        Assert.That(new UrlHostNameParser().ParseHostName(url), Is.EqualTo(expected));

    [Test]
    public void ParseHostName_InvalidProtocol_ThrowsFormatException() =>
        Assert.That(() => new UrlHostNameParser().ParseHostName("ftp://example.com"),
            Throws.TypeOf<FormatException>());
}

[TestFixture]
public class AccountsManagerTests
{
    [TestCase("user_11", "secret@user11", "Welcome user_11!!!")]
    [TestCase("user_22", "secret@user22", "Welcome user_22!!!")]
    [TestCase("wrong", "wrong", "Invalid user id/password")]
    public void ValidateUser_Credentials_ReturnExpectedMessage(
        string user, string password, string expected) =>
        Assert.That(new AccountsManager().ValidateUser(user, password), Is.EqualTo(expected));

    [TestCase("", "password")]
    [TestCase("user", "")]
    public void ValidateUser_MissingCredential_ThrowsFormatException(string user, string password) =>
        Assert.That(() => new AccountsManager().ValidateUser(user, password),
            Throws.TypeOf<FormatException>());
}

[TestFixture]
public class EmployeeManagerTests
{
    private List<Employee> Employees => new EmployeeManager().GetEmployees();

    [Test]
    public void GetEmployees_Result_HasNoNullValues() =>
        Assert.That(Employees, Has.None.Null);

    [Test]
    public void GetEmployees_Result_ContainsEmployee100() =>
        Assert.That(Employees.Any(x => x.EmpId == 100), Is.True);

    [Test]
    public void GetEmployees_Result_IsUnique() =>
        Assert.That(Employees.Distinct().Count(), Is.EqualTo(Employees.Count));

    [Test]
    public void PreviousEmployees_Result_EqualsAllEmployees() =>
        Assert.That(new EmployeeManager().GetEmployeesWhoJoinedInPreviousYears(),
            Is.EquivalentTo(Employees));
}

[TestFixture]
public class SeasonTellerTests
{
    public static IEnumerable<TestCaseData> SeasonCases()
    {
        yield return new TestCaseData("February", "Spring");
        yield return new TestCaseData("April", "Summer");
        yield return new TestCaseData("August", "Monsoon");
        yield return new TestCaseData("October", "Autumn");
        yield return new TestCaseData("December", "Winter");
        yield return new TestCaseData("Unknown", "Invalid Season");
    }

    [TestCaseSource(nameof(SeasonCases))]
    public void DisplaySeasonBy_Month_ReturnsExpectedSeason(string month, string expected) =>
        Assert.That(new SeasonTeller().DisplaySeasonBy(month), Is.EqualTo(expected));
}

[TestFixture]
public class LeapYearCalculatorTests
{
    [TestCase(2000, 1)]
    [TestCase(2024, 1)]
    [TestCase(1900, 0)]
    [TestCase(2023, 0)]
    [TestCase(1752, -1)]
    [TestCase(10000, -1)]
    public void IsLeapYear_Year_ReturnsExpected(int year, int expected) =>
        Assert.That(new LeapYearCalculator().IsLeapYear(year), Is.EqualTo(expected));
}

[TestFixture]
public class UserTests
{
    [Test]
    public void CreateUser_NullPan_ThrowsNullReferenceException()
    {
        var user = new User { PANCardNo = null! };
        Assert.That(() => user.CreateUser(user), Throws.TypeOf<NullReferenceException>());
    }

    [Test]
    public void CreateUser_InvalidLength_ThrowsFormatException()
    {
        var user = new User { PANCardNo = "ABCDE123" };
        Assert.That(() => user.CreateUser(user), Throws.TypeOf<FormatException>());
    }

    [Test]
    public void CreateUser_ValidPan_DoesNotThrow()
    {
        var user = new User { PANCardNo = "ABCDE1234F" };
        Assert.That(() => user.CreateUser(user), Throws.Nothing);
    }
}

[TestFixture]
public class CustomerCommTests
{
    [Test]
    public void SendMailToCustomer_MockedSender_ReturnsTrue()
    {
        var sender = new Mock<IMailSender>();
        sender.Setup(x => x.SendMail(It.IsAny<string>(), It.IsAny<string>())).Returns(true);

        var result = new CustomerComm(sender.Object).SendMailToCustomer();

        Assert.That(result, Is.True);
    }
}

[TestFixture]
public class DirectoryExplorerTests
{
    [Test]
    public void GetFiles_MockedDirectory_ReturnsExpectedFiles()
    {
        const string file1 = "file.txt";
        const string file2 = "file2.txt";

        var explorer = new Mock<IDirectoryExplorer>();
        explorer.Setup(x => x.GetFiles(It.IsAny<string>()))
            .Returns(new List<string> { file1, file2 });

        var files = explorer.Object.GetFiles("dummy-path");

        Assert.That(files, Is.EquivalentTo(new[] { file1, file2 }));
    }
}

[TestFixture]
public class PlayerTests
{
    [Test]
    public void RegisterNewPlayer_NewName_AddsPlayer()
    {
        var mapper = new Mock<IPlayerMapper>();
        mapper.Setup(x => x.IsPlayerNameExistsInDb("Hari")).Returns(false);

        var player = Player.RegisterNewPlayer("Hari", mapper.Object);

        Assert.That(player.Name, Is.EqualTo("Hari"));
        mapper.Verify(x => x.AddNewPlayerIntoDb("Hari"), Times.Once);
    }

    [Test]
    public void RegisterNewPlayer_ExistingName_ThrowsException()
    {
        var mapper = new Mock<IPlayerMapper>();
        mapper.Setup(x => x.IsPlayerNameExistsInDb("Hari")).Returns(true);

        Assert.That(() => Player.RegisterNewPlayer("Hari", mapper.Object),
            Throws.TypeOf<InvalidOperationException>());
    }
}

using System.ComponentModel.DataAnnotations;
using Xunit.Sdk;

namespace HealthCare.Tests;

public class AssertionsConcern : TestClass
{
    [Fact]
    public void ValidateAppointmentDateShouldBeValid()
    {
        var currentDate = DateTime.Now;
        var validFutureDate = currentDate.AddDays(1);
        var pastDate = currentDate.AddDays(-1);

        Assert.True(IsValidAppointmentDate(validFutureDate));

        Assert.False(IsValidAppointmentDate(pastDate));

        Assert.False(IsValidAppointmentDate(currentDate));
    }

    private bool IsValidAppointmentDate(DateTime appointmentDate)
    {
        return appointmentDate > DateTime.Now;
    }

    [Fact]
    public void EmailShouldBeValid()
    {
        var validEmail = "test@example.com";
        var invalidEmail = "patient.com";

        Assert.True(IsValidEmail(validEmail));
        Assert.False(IsValidEmail(invalidEmail));
    }

    private bool IsValidEmail(string email)
    {
        return new EmailAddressAttribute().IsValid(email);
    }

    [Fact]
    public void IdentityUserNameShouldBeValid() // <= 100 characters
    {
        var validUserName = "validuser123";
        var invalidUserName1 = new string('a', 101);
        var invalidUserName2 = "invalid user";

        Assert.True(IsValidIdentityUserName(validUserName));
        Assert.False(IsValidIdentityUserName(invalidUserName1));
        Assert.False(IsValidIdentityUserName(invalidUserName2));
        Assert.False(IsValidIdentityUserName(null));
        Assert.False(IsValidIdentityUserName(string.Empty));
    }

    private bool IsValidIdentityUserName(string userName)
    {
        if (string.IsNullOrEmpty(userName))
        {
            return false;
        }

        if (userName.Length > 100)
        {
            return false;
        }

        foreach (char c in userName)
        {
            if (!char.IsLetterOrDigit(c) && c != '_')
            {
                return false;
            }
        }

        return true;
    }

    [Fact]
    public void IdentityPasswordShouldBeValid() // <= 100 characters
    {
        var validPassword = "Password123!";
        var invalidPassword1 = new string('a', 101);
        var invalidPassword2 = "short";
        var invalidPassword3 = "password";
        var invalidPassword4 = "PASSWORD";
        var invalidPassword5 = "Password";
        var invalidPassword6 = "Password1";

        Assert.True(IsValidIdentityPassword(validPassword));
        Assert.False(IsValidIdentityPassword(invalidPassword1));
        Assert.False(IsValidIdentityPassword(invalidPassword2));
        Assert.False(IsValidIdentityPassword(invalidPassword3));
        Assert.False(IsValidIdentityPassword(invalidPassword4));
        Assert.False(IsValidIdentityPassword(invalidPassword5));
        Assert.False(IsValidIdentityPassword(invalidPassword6));
        Assert.False(IsValidIdentityPassword(null));
        Assert.False(IsValidIdentityPassword(string.Empty));
    }

    private bool IsValidIdentityPassword(string password)
    {
        if (string.IsNullOrEmpty(password))
        {
            return false;
        }

        if (password.Length > 100 || password.Length < 8)
        {
            return false;
        }

        bool hasUpperCase = false;
        bool hasLowerCase = false;
        bool hasDigit = false;
        bool hasSpecialChar = false;
        string specialChars = "!@#$%^&*()-_=+[]{}|;:'\",.<>/?";

        foreach (char c in password)
        {
            if (char.IsUpper(c))
                hasUpperCase = true;
            else if (char.IsLower(c))
                hasLowerCase = true;
            else if (char.IsDigit(c))
                hasDigit = true;
            else if (specialChars.Contains(c))
                hasSpecialChar = true;
        }

        return hasUpperCase && hasLowerCase && hasDigit && hasSpecialChar;
    }
}
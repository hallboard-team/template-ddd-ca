using Ca.Domain.Modules.AccessControl.Enums;
using Ca.Domain.Modules.Auth.ValueObjects;
using Ca.Domain.Modules.Common.Exceptions;

namespace Ca.Domain.Modules.Auth.Aggregates;

// Aggregate root
public class AppUser
{
    // Used to create a new AppUser object
    private AppUser()
    {
    }

    public Name FirstName { get; private set; }
    public Name LastName { get; private set; }
    public Email Email { get; private set; }
    public Name UserName { get; private set; }
    public Password? Password { get; private set; }

    public static AppUser CreateSuperAdmin(
        string firstNameRaw, string lastNameRaw, string emailRaw, string userNameRaw, string roleNameRaw,
        string passwordRaw
    )
    {
        if (roleNameRaw != AccessRoleType.SuperAdmin.ToString()) // Enforce SuperAdmin role name
            throw new DomainException("SuperAdmin user name must be 'SuperAdmin'.");

        return Create(
            firstNameRaw, lastNameRaw, emailRaw, userNameRaw, passwordRaw
        );
    }

    public static AppUser Create(
        string? firstNameRaw, string? lastNameRaw, string? emailRaw, string? userNameRaw, string? passwordRaw
    )
    {
        var firstName = Name.Create(firstNameRaw); // value object handles validation
        var lastName = Name.Create(lastNameRaw);
        var email = Email.Create(emailRaw);
        var userName = Name.Create(userNameRaw);
        var password = Password.Create(passwordRaw);
        
        return new AppUser
        {
            FirstName = firstName, // Use of ValueObject
            LastName = lastName,
            Email = email,
            UserName = userName,
            Password = password
        };
    }

    public static AppUser Rehydrate(string firstNameRaw, string lastNameRaw, string emailRaw, string userNameRaw) =>
        new()
        {
            FirstName = Name.Create(firstNameRaw),
            LastName = Name.Create(lastNameRaw),
            Email = Email.Create(emailRaw),
            UserName = Name.Create(emailRaw)
        };

    public void ChangeFirstName(string? newFirstName)
    {
        FirstName = Name.Create(newFirstName);
    }

    public void ChangeLastName(string? newLastName)
    {
        LastName = Name.Create(newLastName);
    }

    public void ChangePassword(string newPasswordRaw) =>
        Password = Password.Create(newPasswordRaw);
}
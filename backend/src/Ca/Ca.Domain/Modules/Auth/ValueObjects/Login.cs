using Ca.Domain.Modules.Common.Exceptions;

namespace Ca.Domain.Modules.Auth.ValueObjects;

public sealed class Login
{
    private Login(Email? email, Name? userName, Password password)
    {
        Email = email;
        UserName = userName;
        Password = password;
    }

    public Email? Email { get; }
    public Name? UserName { get; }
    public Password Password { get; }

    public bool IsEmail => Email is not null;

    public static Login Create(string credentialRaw, string passwordRaw)
    {
        if (string.IsNullOrEmpty(credentialRaw))
            throw new DomainException("Value cannot be null or empty.", nameof(credentialRaw));
        if (string.IsNullOrEmpty(passwordRaw))
            throw new DomainException("Value cannot be null or empty.", nameof(passwordRaw));

        if (credentialRaw.Contains(value: '@'))
        {
            var email = Email.Create(credentialRaw);
            return new Login(email, userName: null, Password.Create(passwordRaw));
        }

        var userName = Name.Create(credentialRaw);
        return new Login(email: null, userName, Password.Create(passwordRaw));
    }
}
using Ca.Domain.Modules.Common.Exceptions;
using Ca.Domain.Modules.Common.Validations;
using Ca.Domain.Shared;

namespace Ca.Domain.Modules.Auth.ValueObjects;

public sealed class Password
{
    private Password(string value) => Value = value;

    public string Value { get; }

    public static Password Create(string? passwordRaw)
    {
        string? validationError = passwordRaw.ValidateValue(
            nameof(passwordRaw), SharedLengths.PasswordMin, SharedLengths.PasswordMax, regex: null
        );

        if (validationError is not null)
            throw new DomainException(validationError);

        return new Password(passwordRaw!);
    }

    //overrides the default ToString() behavior to hide the actual password value when the object is printed or logged.
    public override string ToString() => "[PROTECTED]";
}
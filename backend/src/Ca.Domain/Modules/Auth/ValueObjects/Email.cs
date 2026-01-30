using System.Text.RegularExpressions;
using Ca.Domain.Modules.Common.Base;
using Ca.Domain.Modules.Common.Exceptions;
using Ca.Domain.Modules.Common.Validations;
using Ca.Domain.Shared;

namespace Ca.Domain.Modules.Auth.ValueObjects;

public sealed partial class Email : ValueObject
{
    private Email(string value) => Value = value;

    public string Value { get; }


    public static Email Create(string? emailRaw)
    {
        string? validationError = emailRaw.ValidateValue(
            nameof(emailRaw), minLength: -1, SharedLengths.EmailMax,
            EmailRegex()
        );

        if (validationError is not null)
            throw new DomainException(validationError);

        return new Email(emailRaw!.Trim().ToLowerInvariant());
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }

    public override string ToString() => Value;

    // Source-generated, cached singleton under the hood.
    [GeneratedRegex(
        @"^(?=.{3,254}$)(?=.{1,64}@)[A-Za-z0-9.!#$%&'*+/=?^_`{|}~-]+@([A-Za-z0-9](?:[A-Za-z0-9-]{0,61}[A-Za-z0-9])\.)+[A-Za-z]{2,63}$",
        RegexOptions.CultureInvariant /* + RegexOptions.IgnoreCase if you prefer */
    )]
    private static partial Regex EmailRegex();
}
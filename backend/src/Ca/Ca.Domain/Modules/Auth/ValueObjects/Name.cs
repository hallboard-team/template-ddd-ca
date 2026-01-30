using Ca.Domain.Modules.Common.Base;
using Ca.Domain.Modules.Common.Exceptions;
using Ca.Domain.Modules.Common.Validations;
using Ca.Domain.Shared;

namespace Ca.Domain.Modules.Auth.ValueObjects;

public sealed class Name : ValueObject
{
    public Name()
    {
    }

    public Name(string value) => Value = value;
    // {
    //     Value = value;
    // }

    public string Value { get; } // Read-only

    public static Name Create(string? nameRaw)
    {
        string? validationError = nameRaw.ValidateValue(nameof(nameRaw), SharedLengths.NameMin, SharedLengths.NameMax);

        if (validationError is not null)
            throw new DomainException(validationError);

        return new Name(nameRaw!.Trim());
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }

    public override string ToString() => Value;
}
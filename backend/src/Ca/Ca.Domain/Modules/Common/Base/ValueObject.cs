namespace Ca.Domain.Modules.Common.Base;

/// <summary>
/// Base class for value objects. Implements value-based equality logic.
/// </summary>
public abstract class ValueObject
{
    /// <summary>
    /// Override this in your value object to define which properties are used for equality.
    /// </summary>
    protected abstract IEnumerable<object?> GetEqualityComponents();

    public override bool Equals(object? obj)
    {
        if (obj is null || obj.GetType() != GetType())
            return false;

        var other = (ValueObject)obj;
        return GetEqualityComponents().SequenceEqual(other.GetEqualityComponents());
    }

    public override int GetHashCode()
    {
        return GetEqualityComponents().Aggregate(seed: 1, (current, obj) => current * 23 + (obj?.GetHashCode() ?? 0));
    }

    public static bool operator ==(ValueObject? left, ValueObject? right) => Equals(left, right);

    public static bool operator !=(ValueObject? left, ValueObject? right) => !Equals(left, right);
}
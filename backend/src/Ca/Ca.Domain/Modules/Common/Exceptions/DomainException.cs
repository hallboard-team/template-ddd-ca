namespace Ca.Domain.Modules.Common.Exceptions;

public class DomainException : Exception
{
    public string? PropertyName { get; }
    
    public DomainException(string message) : base(message)
    {
    }
    
    public DomainException(string propertyName, string message)
        : base(message)
    {
        PropertyName = propertyName;
    }

    public DomainException(string message, Exception innerException) : base(message, innerException)
    {
    }
}
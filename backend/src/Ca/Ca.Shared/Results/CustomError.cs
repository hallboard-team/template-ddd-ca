namespace Ca.Shared.Results;

public record CustomError(
    Enum Code,
    [Optional] string? Message
);
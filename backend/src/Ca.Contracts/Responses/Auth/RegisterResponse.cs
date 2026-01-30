namespace Ca.Contracts.Responses.Auth;

public record RegisterResponse(
    string FirstName,
    string LastName,
    string Email,
    string UserName
);
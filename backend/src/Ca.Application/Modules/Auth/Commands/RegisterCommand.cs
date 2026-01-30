namespace Ca.Application.Modules.Auth.Commands;

public record RegisterCommand(
    string FirstName,
    string LastName,
    string Email,
    string UserName,
    string Password
);
namespace Ca.Contracts.Responses.Auth;

public record LoginResponse(
    bool IsRecaptchaTokenInvalid,
    bool IsWrongCreds,
    string? RecaptchaToken = null,
    string? Email = null, // Used only to verify the account. Will always return null if the account is verified.
    IEnumerable<string>? RolesStr = null,
    string? FirstName = null,
    string? LastName = null,
    string? UserName = null,
    string? ProfilePhotoUrl = null,
    List<string>? Errors = null,
    bool? IsProfileCompleted = null,
    bool? IsEmailNotConfirmed = null
);
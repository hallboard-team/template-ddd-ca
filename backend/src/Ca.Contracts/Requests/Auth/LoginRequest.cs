using System.ComponentModel.DataAnnotations;
using Ca.Domain.Modules.Auth.Constants;
using Ca.Domain.Shared;

namespace Ca.Contracts.Requests.Auth;

public record LoginRequest(
    [MaxLength(SharedLengths.EmailMax)] string Credential,
    [DataType(DataType.Password)]
    [Length(AuthLengths.PasswordMin, AuthLengths.PasswordMax)]
    [RegularExpression(
        @"^(?=.*[A-Z])(?=.*\d).+$",
        ErrorMessage = "Needs: 8 to 50 characters. An uppercase character(ABC). A number(123)"
    )]
    string Password
);
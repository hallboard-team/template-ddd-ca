using System.ComponentModel.DataAnnotations;
using Ca.Domain.Modules.Auth.Constants;
using Ca.Domain.Shared;

namespace Ca.Contracts.Requests.Auth;

public record RegisterRequest(
    [Length(SharedLengths.NameMin, SharedLengths.NameMax)]
    string FirstName,
    [Length(SharedLengths.NameMin, SharedLengths.NameMax)]
    string LastName,
    [MaxLength(SharedLengths.EmailMax)] string Email,
    [Length(SharedLengths.NameMin, SharedLengths.NameMax)]
    string UserName,
    [DataType(DataType.Password)]
    [Length(AuthLengths.PasswordMin, AuthLengths.PasswordMax)]
    [RegularExpression(
        @"^(?=.*[A-Z])(?=.*\d).+$",
        ErrorMessage = "Needs: 8 to 50 characters. An uppercase character(ABC). A number(123)"
    )]
    string Password,
    string ConfirmPassword
);
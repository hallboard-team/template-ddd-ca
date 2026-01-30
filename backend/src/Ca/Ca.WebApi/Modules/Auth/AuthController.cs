using Ca.Application.Modules.Auth.Commands;
using Ca.Application.Modules.Auth.Interfaces;
using Ca.Contracts.Requests.Auth;
using Ca.Contracts.Responses.Auth;
using Ca.Domain.Modules.Auth.Enums;
using Ca.Shared.Results;
using Ca.WebApi.Helpers;
using Microsoft.AspNetCore.Mvc;
using UAParser;
using UAParser.Objects;

namespace Ca.WebApi.Modules.Auth;

public class AuthController(IAuthService authService) : BaseApiController
{
    [HttpPost("register")]
    public async Task<ActionResult<RegisterResponse>> Create(RegisterRequest request)
    {
        if (request.Password != request.ConfirmPassword)
            return BadRequest("Passwords do not match!");

        RegisterCommand command = AuthRequestMapper.MapRegisterRequestToRegisterCommand(request);

        OperationResult<RegisterResponse> result = await authService.CreateAsync(command);

        return result.IsSuccess
            ? result.Result
            : result.Error?.Code switch
            {
                AuthUserCreationErrorType.EmailAlreadyExists => BadRequest(result.Error.Message),
                AuthUserCreationErrorType.UsernameAlreadyExists => BadRequest(result.Error.Message),
                AuthUserCreationErrorType.AddRoleFailed => BadRequest(result.Error.Message),
                _ => BadRequest(result.Error?.Message)
            };
    }

    [HttpPost("login")]
    public async Task<ActionResult<LoginResponse>> Login(LoginRequest request)
    {
        LoginCommand command = AuthRequestMapper.MapLoginRequestToLoginCommand(request, ExtractSessionMetadata());

        OperationResult<LoginResponse> result = await authService.LoginAsync(command);

        return result.IsSuccess
            ? result.Result
            : result.Error?.Code switch
            {
                AuthLoginErrorType.WrongCredentials => Unauthorized(result.Error.Message),
                AuthLoginErrorType.Unknown => BadRequest(result.Error.Message),
                _ => BadRequest(result.Error?.Message)
            };
    }

    private SessionMetadataDto ExtractSessionMetadata()
    {
        var userAgent = HttpContext.Request.Headers["User-Agent"].ToString();
        Parser? parser = Parser.GetDefault();
        ClientInfo? client = parser.Parse(userAgent);

        string deviceType = client.Device.IsSpider ? "Bot" :
            string.IsNullOrWhiteSpace(client.Device.Family) ? "Unknown" : client.Device.Family;
        var os = $"{client.OS.Family} {client.OS.Major}";
        var browser = $"{client.Browser.Family} {client.Browser.Major}";

        var deviceName = $"{os} - {browser}";

        string ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "Unknown";

        // Optionally get location from headers (e.g., behind Cloudflare)
        string location = HttpContext.Request.Headers["CF-IPCountry"].FirstOrDefault() ?? "Unknown";

        return new SessionMetadataDto(
            deviceType,
            deviceName,
            string.IsNullOrWhiteSpace(userAgent) ? "Unknown" : userAgent,
            ipAddress,
            location
        );
    }
}
using Ca.Contracts.Requests.Auth;

namespace Ca.Application.Modules.Auth.Commands;

public record LoginCommand(
    string Credential,
    string Password,
    SessionMetadataDto SessionMetadata
);
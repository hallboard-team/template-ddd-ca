using Ca.Domain.Modules.Auth.ValueObjects;

namespace Ca.Application.Modules.Auth.Commands;

public class RefreshTokenCommand(
    string TokenValueRaw,
    string JtiValue,
    SessionMetadata SessionMetadata
);
using System.ComponentModel.DataAnnotations;
using Ca.Domain.Modules.Auth.Constants;

namespace Ca.Contracts.Requests.Auth;

public record RefreshTokenRequest(
    [Length(AuthLengths.TokenValueMin, AuthLengths.TokenValueMax)]
    string TokenValueRaw,
    [Length(AuthLengths.JtiValueMin, AuthLengths.JtiValueMax)]
    string JtiValue,
    SessionMetadataDto SessionMetadata
);
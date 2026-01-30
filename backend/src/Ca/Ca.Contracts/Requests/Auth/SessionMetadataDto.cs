using System.ComponentModel.DataAnnotations;
using Ca.Domain.Modules.Auth.Constants;

namespace Ca.Contracts.Requests.Auth;

public record SessionMetadataDto(
    [Length(SessionMetadataLengths.DeviceTypeMin, SessionMetadataLengths.DeviceTypeMax)]
    string DeviceType,
    [Length(SessionMetadataLengths.DeviceNameMin, SessionMetadataLengths.DeviceNameMax)]
    string DeviceName,
    [Length(SessionMetadataLengths.UserAgentMin, SessionMetadataLengths.UserAgentMax)]
    string UserAgent,
    [Length(
        SessionMetadataLengths.IpAddressMin, SessionMetadataLengths.IpAddressMax
    )] // TODO: Validate IP4/IP6 with FluentValidation
    string IpAddress,
    [Length(SessionMetadataLengths.LocationMin, SessionMetadataLengths.LocationMax)]
    string Location
);
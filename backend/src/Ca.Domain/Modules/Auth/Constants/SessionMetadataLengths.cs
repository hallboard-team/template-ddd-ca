namespace Ca.Domain.Modules.Auth.Constants;

public static class SessionMetadataLengths
{
    public const int DeviceTypeMin = 1;
    public const int DeviceTypeMax = 64;
    public const int DeviceNameMin = 1;
    public const int DeviceNameMax = 128;
    public const int UserAgentMin = 1;
    public const int UserAgentMax = 512;
    public const int IpAddressMin = 1;
    public const int IpAddressMax = 64;
    public const int LocationMin = 1;
    public const int LocationMax = 64;
}
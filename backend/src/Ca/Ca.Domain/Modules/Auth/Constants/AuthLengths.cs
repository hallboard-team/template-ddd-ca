namespace Ca.Domain.Modules.Auth.Constants;

public static class AuthLengths
{
    public const int PasswordMin = 8;
    public const int PasswordMax = 30;
    public const int TokenValueMin = 10;
    public const int TokenValueMax = 256;
    public const int JtiValueMin = 10;
    public const int JtiValueMax = 128;
}
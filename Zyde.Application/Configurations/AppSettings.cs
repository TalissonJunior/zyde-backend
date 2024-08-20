using Coffee.Application.Configurations;

namespace Zyde.Application.Configurations;

public class AppSettings : CoffeeAppSettings
{
    public JwtSettings JwtSettings { get; set; }
}

public class JwtSettings
{
    public string Secret { get; set; }
    public int TokenDurationDays { get; set; }
    public string Issuer { get; set; }
    public string Audience { get; set; }
}
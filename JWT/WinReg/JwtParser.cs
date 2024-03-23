using System.IdentityModel.Tokens.Jwt;

namespace SchedulerDesktop.JWT.WinReg;

public static class JwtParser
{
    public static string ParseId(string token)
    {
        if (string.IsNullOrEmpty(token)) return string.Empty;

        var handler = new JwtSecurityTokenHandler();
        var jwtToken = handler.ReadJwtToken(token);

        return jwtToken.Payload.Sub;
    }
    
    public static DateTime ParseExpirationDateTime(string token)
    {
        if (string.IsNullOrEmpty(token)) return DateTime.MinValue;

        var handler = new JwtSecurityTokenHandler();
        var jwtToken = handler.ReadJwtToken(token);

        return jwtToken.ValidTo;
    }

    public static bool ValidThrough(string token, DateTime dateTime)
    {
        return ParseExpirationDateTime(token) > dateTime;
    }

    public static bool CurrentlyValid(string token)
    {
        return ValidThrough(token, DateTime.UtcNow);
    }
}
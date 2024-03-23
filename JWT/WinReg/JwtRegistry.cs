using Microsoft.Win32;

namespace SchedulerDesktop.JWT.WinReg;

public static class JwtRegistry
{
    private const string RegistryKeyPath = @"Software\Scheduler";

    public static void RegisterToken(string token)
    {
        var key = Registry.CurrentUser.CreateSubKey(RegistryKeyPath);
        key.SetValue("AuthToken", token);
        key.Close();
    }

    public static string? FetchToken()
    {
        var key = Registry.CurrentUser.OpenSubKey(RegistryKeyPath);
        if (key == null) return null;
        
        var token = key.GetValue("AuthToken")?.ToString();
        key.Close();
        return token;
    }
}

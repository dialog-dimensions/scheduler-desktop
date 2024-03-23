using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using SchedulerDesktop.JWT.Interfaces;
using SchedulerDesktop.Views.Account;

namespace SchedulerDesktop.JWT.WinReg;

public class JwtTools : IJwtTools
{
    private readonly IServiceProvider _serviceProvider;

    public JwtTools(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }
    
    public string? TryGetToken()
    {
        var token = JwtRegistry.FetchToken();
        if (token.IsNullOrEmpty())
        {
            return null;
        }

        if (JwtParser.CurrentlyValid(token))
        {
            return token;
        }
        
        return null;
    }

    public string? TryGetId()
    {
        var token = JwtRegistry.FetchToken();
        if (token.IsNullOrEmpty())
        {
            return null;
        }

        return JwtParser.ParseId(token!);
    }

    public bool ValidateToken()
    {
        var token = JwtRegistry.FetchToken();
        if (!token.IsNullOrEmpty() && JwtParser.CurrentlyValid(token!)) return true;
        
        CatchInvalidToken();
        return false;
    }

    public void RedirectToLogin()
    {
        MessageBox.Show("Please login to continue.");
        var view = _serviceProvider.GetRequiredService<Login>();
        view.Show();
    }

    public void CatchInvalidToken()
    {
        JwtRegistry.RegisterToken(string.Empty);
    }
}

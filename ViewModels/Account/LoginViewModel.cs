using System.Windows.Input;
using SchedulerDesktop.Commands;
using SchedulerDesktop.Services.Api.Interfaces;

namespace SchedulerDesktop.ViewModels.Account;

public class LoginViewModel
{
    private readonly IUserApiService _apiService;

    public ICommand LoginCommand { get; }

    public LoginViewModel(IUserApiService apiService)
    {
        _apiService = apiService;
        LoginCommand = new AsyncRelayCommand(Login, CanLogin);
    }
    public string? Id { get; set; }
    public string? Password { get; set; }

    private async Task Login()
    {
        await _apiService.LoginAsync(Id!, Password!);
    }

    private bool CanLogin()
    {
        return !(string.IsNullOrEmpty(Id) | string.IsNullOrEmpty(Password));
    }
}
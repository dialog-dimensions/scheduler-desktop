using System.Windows.Input;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using SchedulerDesktop.Commands;
using SchedulerDesktop.JWT.Interfaces;
using SchedulerDesktop.JWT.WinReg;
using SchedulerDesktop.Services.Api.Interfaces;
using SchedulerDesktop.ViewModels.BaseClasses;
using SchedulerDesktop.ViewModels.EmployeeManager;
using SchedulerDesktop.ViewModels.ExceptionsManager;
using SchedulerDesktop.ViewModels.ScheduleManager;
using SchedulerDesktop.Views.Account;
using SchedulerDesktop.Views.ScheduleManager;
using SchedulerDesktop.Views.ScheduleManager.ScheduleDashboard;

namespace SchedulerDesktop.ViewModels;

public class MainViewModel : ViewModelBase
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IConfiguration _configuration;
    private readonly IJwtTools _jwtTools;
    private readonly IEmployeeApiService _employeeApi;
    private readonly IScheduleApiService _scheduleApi;

    private string _userName;
    public string UserName
    {
        get => _userName;
        set
        {
            if (SetField(ref _userName, value))
            {
                OnPropertyChanged(nameof(GreetingMessage));
                OnPropertyChanged(nameof(LoggedIn));
            }
        }
    }

    public bool LoggedIn => !(UserName.IsNullOrEmpty() || UserName == _configuration["Content:MainWindow:GuestName"]);

    public string GreetingMessage => $"שלום, {UserName}!";

    public ICommand OpenLoginCommand { get; }
    public ICommand LogoutCommand { get; }
    public ICommand OpenEmployeeManagerCommand { get; }
    public ICommand OpenNewScheduleFormCommand { get; }
    public ICommand OpenScheduleDashboardCommand { get; }
    public ICommand OpenExceptionsManagerCommand { get; }
    public ICommand WakeScannerCommand { get; }
    public ICommand TerminateScannerCommand { get; }

    public MainViewModel(IServiceProvider serviceProvider, IJwtTools jwtTools, IEmployeeApiService employeeApi, 
        IScheduleApiService scheduleApi, IConfiguration configuration)
    {
        _serviceProvider = serviceProvider;
        _configuration = configuration;
        _jwtTools = jwtTools;
        _employeeApi = employeeApi;
        _scheduleApi = scheduleApi;
        
        OpenLoginCommand = new RelayCommand(OpenLogin, _ => true);
        LogoutCommand = new RelayCommand(Logout, CanLogout);
        OpenEmployeeManagerCommand = new AsyncRelayCommand(OpenEmployeeManager, ProgramEnabled);
        OpenNewScheduleFormCommand = new AsyncRelayCommand(OpenNewScheduleForm, ProgramEnabled);
        OpenScheduleDashboardCommand = new AsyncRelayCommand(OpenScheduleDashboard, ProgramEnabled);
        OpenExceptionsManagerCommand = new AsyncRelayCommand(OpenExceptionsManager, ProgramEnabled);
        WakeScannerCommand = new AsyncRelayCommand(WakeScannerAsync, ProgramEnabled);
        TerminateScannerCommand = new AsyncRelayCommand(TerminateScannerAsync, ProgramEnabled);
    }

    private static bool ProgramEnabled()
    {
        return !string.IsNullOrEmpty(JwtRegistry.FetchToken());
    }

    private void OpenLogin(object? parameter)
    {
            var view = _serviceProvider.GetRequiredService<Login>();
            view.Show();
    }

    private async Task OpenEmployeeManager()
    {
        if (_jwtTools.ValidateToken())
        {
            var view = _serviceProvider.GetRequiredService<Views.EmployeeManager.EmployeeManager>();
            if (view.DataContext is EmployeeManagerViewModel vm)
            {
                await vm.RefreshEmployees();
            }
        
            view.Show();
        }
        else
        {
            _jwtTools.RedirectToLogin();
        }
    }

    private void Logout(object? parameter)
    {
        JwtRegistry.RegisterToken(string.Empty);
    }

    private bool CanLogout(object? parameter)
    {
        return !JwtRegistry.FetchToken().IsNullOrEmpty();
    }

    private async Task OpenNewScheduleForm()
    {
        if (_jwtTools.ValidateToken())
        {
            var view = _serviceProvider.GetRequiredService<NewScheduleForm>();
            if (view.DataContext is NewScheduleFormViewModel vm)
            {
                await vm.SetContinuationToLatestSchedule();
            }
        
            view.Show();
        }
        else
        {
            _jwtTools.RedirectToLogin();
        }
    }

    private async Task OpenScheduleDashboard()
    {
        if (_jwtTools.ValidateToken())
        {
            var view = _serviceProvider.GetRequiredService<ScheduleDashboard>();
            if (view.DataContext is ScheduleDashboardViewModel vm)
            {
                await vm.LoadSchedulesAsync();
            }
        
            view.Show();
        }
        else
        {
            _jwtTools.RedirectToLogin();
        }
    }

    private async Task OpenExceptionsManager()
    {
        if (_jwtTools.ValidateToken())
        {
            var view = _serviceProvider.GetRequiredService<Views.ExceptionsManager.ExceptionsManager>();
            if (view.DataContext is ExceptionsManagerViewModel vm)
            {
                await vm.Initialize();
            }

            view.Show();
        }
        else
        {
            _jwtTools.RedirectToLogin();
        }
    }

    public async Task GetEmployeeUserName()
    {
        if (_jwtTools.ValidateToken())
        {
            var parseId = int.TryParse(_jwtTools.TryGetId(), out var idInt);
            if (!parseId) return;

            var userEmployeeObject = await _employeeApi.GetEmployeeAsync(idInt);
            if (userEmployeeObject is null) return;

            var userName = userEmployeeObject.Name;
            UserName = userName;
        }
        else
        {
            UserName = _configuration["Content:MainWindow:GuestName"]!;
        }
    }

    private async Task WakeScannerAsync()
    {
        await _scheduleApi.WakeScannerAsync();
    }

    private async Task TerminateScannerAsync()
    {
        await _scheduleApi.TerminateScannerAsync();
    }
}

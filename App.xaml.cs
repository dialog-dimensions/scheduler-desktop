using System.Configuration;
using System.Data;
using System.IO;
using System.Net.Http;
using System.Windows;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SchedulerDesktop.JWT.Interfaces;
using SchedulerDesktop.JWT.WinReg;
using SchedulerDesktop.Services.Api.Classes;
using SchedulerDesktop.Services.Api.Interfaces;
using SchedulerDesktop.ViewModels;
using SchedulerDesktop.ViewModels.Account;
using SchedulerDesktop.ViewModels.EmployeeManager;
using SchedulerDesktop.ViewModels.ExceptionsManager;
using SchedulerDesktop.ViewModels.ScheduleManager;
using SchedulerDesktop.Views.Account;
using SchedulerDesktop.Views.EmployeeManager;
using SchedulerDesktop.Views.ExceptionsManager;
using SchedulerDesktop.Views.ScheduleManager;
using SchedulerDesktop.Views.ScheduleManager.ScheduleDashboard;

namespace SchedulerDesktop;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{private readonly IHost _host;
    // public static string Token { get; set; } = "";

    public App()
    {
        _host = Host.CreateDefaultBuilder()
            .ConfigureAppConfiguration((_, builder) =>
            {
                builder.SetBasePath(Directory.GetCurrentDirectory());
                builder.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            })
            .ConfigureServices((context, services) =>
            {
                services.AddSingleton(context.Configuration);
                services.AddSingleton<IJwtTools, JwtTools>();
                services.AddSingleton<MainWindow>();
                services.AddSingleton<MainViewModel>();

                var httpClient = new HttpClient { BaseAddress = new Uri(context.Configuration["Api:BaseUrl"]!), Timeout = TimeSpan.FromHours(1)};
                services.AddSingleton(httpClient);

                services.AddScoped<IUserApiService, UserApiService>();
                services.AddScoped<IEmployeeApiService, EmployeeApiService>();
                services.AddScoped<IScheduleApiService, ScheduleApiService>();
                services.AddScoped<IShiftApiService, ShiftApiService>();
                services.AddScoped<IShiftExceptionApiService, ShiftExceptionApiService>();
                services.AddScoped<IShiftSwapApiService, ShiftSwapApiService>();
                
                services.AddScoped<LoginViewModel>();
                services.AddTransient<Login>();
                
                services.AddTransient<EmployeeManager>();
                services.AddScoped<EmployeeViewModel>();
                services.AddScoped<EmployeeManagerViewModel>();
                
                services.AddTransient<NewScheduleForm>();
                services.AddScoped<NewScheduleFormViewModel>();
                
                services.AddTransient<ScheduleDashboard>();
                services.AddScoped<ScheduleDashboardViewModel>();
                services.AddScoped<ScheduleDisplayViewModel>();
                services.AddTransient<ShiftCubeViewModel>();
                services.AddTransient<ShiftViewModel>();

                services.AddTransient<ExceptionsManager>();
                services.AddScoped<ExceptionsManagerViewModel>();
            })
            .Build();
    }

    protected override async void OnStartup(StartupEventArgs e)
    {
        await _host.StartAsync();
        var mainWindow = _host.Services.GetRequiredService<MainWindow>();
        mainWindow.Show();
        base.OnStartup(e);
    }

    protected override async void OnExit(ExitEventArgs e)
    {
        using (_host)
        {
            await _host.StopAsync(TimeSpan.FromSeconds(5));
        }
        base.OnExit(e);
    }
}
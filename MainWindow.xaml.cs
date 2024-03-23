using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Extensions.DependencyInjection;
using SchedulerDesktop.ViewModels;
using SchedulerDesktop.Views.Account;

namespace SchedulerDesktop;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private readonly IServiceProvider _serviceProvider;
    
    public MainWindow(IServiceProvider serviceProvider, MainViewModel viewModel)
    {
        _serviceProvider = serviceProvider;
        InitializeComponent();
        DataContext = viewModel;
    }

    private void LoginMenuItem_OnClick(object sender, RoutedEventArgs e)
    {
        var view = _serviceProvider.GetRequiredService<Login>();
        view.Show();
    }

    private async Task GetUserName()
    {
        if (DataContext is MainViewModel vm)
        {
            
            await vm.GetEmployeeUserName();
        }
    }

    private async void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
    {
        await GetUserName();
    }

    private async void MainWindow_OnActivated(object? sender, EventArgs e)
    {
        await GetUserName();
    }
}
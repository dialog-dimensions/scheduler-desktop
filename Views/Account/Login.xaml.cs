using System.Windows;
using SchedulerDesktop.ViewModels.Account;

namespace SchedulerDesktop.Views.Account;

public partial class Login : Window
{
    public Login(LoginViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
    }

    private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
    {
        Hide();
    }
}

using System.Windows;
using SchedulerDesktop.ViewModels.EmployeeManager;

namespace SchedulerDesktop.Views.EmployeeManager;

public partial class EmployeeManager : Window
{
    public EmployeeManager(EmployeeManagerViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
    }
}

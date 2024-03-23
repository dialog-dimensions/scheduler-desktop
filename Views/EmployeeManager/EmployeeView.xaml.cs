using System.Windows.Controls;
using System.Windows.Input;
using SchedulerDesktop.ViewModels.EmployeeManager;

namespace SchedulerDesktop.Views.EmployeeManager;

public partial class EmployeeView : UserControl
{
    public EmployeeView()
    {
        InitializeComponent();
    }

    private void UIElement_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        if (DataContext is EmployeeViewModel vm)
        {
            vm.ShowCallToRegisterFlow = true;
        }
    }
}

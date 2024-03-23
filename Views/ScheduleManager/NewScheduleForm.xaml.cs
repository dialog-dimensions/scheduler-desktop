using System.Windows;
using SchedulerDesktop.ViewModels.ScheduleManager;

namespace SchedulerDesktop.Views.ScheduleManager;

public partial class NewScheduleForm : Window
{
    public NewScheduleForm(NewScheduleFormViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
    }
}
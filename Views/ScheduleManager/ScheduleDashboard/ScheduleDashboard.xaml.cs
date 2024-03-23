using System.Windows;
using System.Windows.Controls;
using SchedulerDesktop.Models.Entities.Other_Objects;
using SchedulerDesktop.ViewModels.ScheduleManager;

namespace SchedulerDesktop.Views.ScheduleManager.ScheduleDashboard;

public partial class ScheduleDashboard : Window
{
    public ScheduleDashboard(ScheduleDashboardViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
    }

    private async void Selector_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (DataContext is not ScheduleDashboardViewModel vm) return;
        if (sender is ComboBox { SelectedItem: FlatSchedule })
        {
            await vm.UpdateDisplayAsync();
        }
    }
}

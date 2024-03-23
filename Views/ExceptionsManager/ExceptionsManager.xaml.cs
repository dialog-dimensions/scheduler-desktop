using System.Windows;
using System.Windows.Controls;
using SchedulerDesktop.Models.Entities.Other_Objects;
using SchedulerDesktop.ViewModels.ExceptionsManager;

namespace SchedulerDesktop.Views.ExceptionsManager;

public partial class ExceptionsManager : Window
{
    public ExceptionsManager(ExceptionsManagerViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
    }

    private async void Selector_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (sender is not ComboBox comboBox) return;
        if (DataContext is ExceptionsManagerViewModel vm)
        {
            await vm.LoadShifts(((FlatSchedule)comboBox.SelectedItem).StartDateTime);
        }
    }
}
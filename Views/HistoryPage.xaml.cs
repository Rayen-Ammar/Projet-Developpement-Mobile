using Microsoft.Maui.Controls;
using ChahriyaPro.ViewModels;
using ChahriyaPro.Services;

namespace ChahriyaPro.Views;

public partial class HistoryPage : ContentPage
{
    private readonly MainViewModel _viewModel;

    public HistoryPage()
    {
        InitializeComponent();

        var dataService = new DataService();
        _viewModel = new MainViewModel(dataService);
        BindingContext = _viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _viewModel.LoadDashboardAsync();
    }
}
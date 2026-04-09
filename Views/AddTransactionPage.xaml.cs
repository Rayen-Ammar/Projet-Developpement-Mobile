using Microsoft.Maui.Controls;
using ChahriyaPro.ViewModels;
using ChahriyaPro.Services;

namespace ChahriyaPro.Views;

public partial class AddTransactionPage : ContentPage
{
    private readonly AddTransactionViewModel _viewModel;

    public AddTransactionPage()
    {
        InitializeComponent();

        var dataService = new DataService();
        _viewModel = new AddTransactionViewModel(dataService);
        BindingContext = _viewModel;
    }
}
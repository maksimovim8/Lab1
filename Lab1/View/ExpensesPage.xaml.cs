using Lab1.ViewModels;
namespace Lab1.Views;

public partial class ExpensesPage : ContentPage
{
    private readonly ExpensesViewModel _viewModel;

    public ExpensesPage()
    {
        InitializeComponent();

        _viewModel = new ExpensesViewModel();

        BindingContext = _viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        await _viewModel.LoadRatesAsync();
    }
}
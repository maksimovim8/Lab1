
using System.Diagnostics;
using Lab1.Services;
using Lab1.ViewModels;

namespace Lab1.Views;

public partial class ExpensesPage : ContentPage
{
    private readonly ExpensesViewModel _viewModel;

    public ExpensesPage(
    CategoryService categoryService,
    ExpenseStore expenseStore)
    {
        InitializeComponent();

        _viewModel = new ExpensesViewModel(
        categoryService,
        expenseStore);

        BindingContext = _viewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        Debug.WriteLine(
            $"[Page Lifecycle] ExpensesPage OnAppearing: {DateTime.Now:HH:mm:ss}");

        _viewModel.LoadCommand.Execute(null);
    }

    protected override void OnDisappearing()
    {
        Debug.WriteLine(
            $"[Page Lifecycle] ExpensesPage OnDisappearing: {DateTime.Now:HH:mm:ss}");

        base.OnDisappearing();
    }
}

using Lab1.ViewModels;

namespace Lab1.Views;

public partial class ExpenseDetailPage : ContentPage
{
    public ExpenseDetailPage()
    {
        InitializeComponent();

        BindingContext =
            new ExpenseDetailViewModel();
    }
}
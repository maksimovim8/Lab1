using Lab1.Services;
using Lab1.ViewModels;

namespace Lab1.Views;

public partial class ExpenseDetailPage : ContentPage
{
    public ExpenseDetailPage(CategoryService categoryService)
    {
        InitializeComponent();

        BindingContext =
            new ExpenseDetailViewModel(categoryService);
    }
}
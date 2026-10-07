using Lab1.ViewModels;
using Lab1.Services;

namespace Lab1.Views;

public partial class CategoriesPage : ContentPage
{
    public CategoriesPage(CategoryService categoryService)
    {
        InitializeComponent();

        BindingContext = new CategoriesViewModel(categoryService);
    }
}
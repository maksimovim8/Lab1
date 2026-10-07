using Lab1.Views;

namespace Lab1;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();

        Routing.RegisterRoute("expensedetail", typeof(ExpenseDetailPage));
        Routing.RegisterRoute("statistics", typeof(StatisticsPage));
        Routing.RegisterRoute("categories", typeof(CategoriesPage));
    }
}
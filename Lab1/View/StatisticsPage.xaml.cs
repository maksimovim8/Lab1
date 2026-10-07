using Lab1.ViewModels;

namespace Lab1.Views;

public partial class StatisticsPage : ContentPage
{
    private readonly StatisticsViewModel _viewModel;

    public StatisticsPage()
    {
        InitializeComponent();

        _viewModel = new StatisticsViewModel();

        BindingContext = _viewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        _viewModel.Refresh();
    }
}
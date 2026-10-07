using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Lab1.Models;

namespace Lab1.ViewModels;

public class StatisticsViewModel :
    IQueryAttributable,
    INotifyPropertyChanged
{
    private ObservableCollection<Expense> _expenses;

    private decimal _totalAmount;
    private decimal _averageAmount;
    private decimal _largestExpense;

    private int _expenseCount;

    private string _largestCategory =
        "Немає даних";

    public ObservableCollection<CategoryStatistic>
        CategoryStatistics
    { get; set; }

    public decimal TotalAmount
    {
        get => _totalAmount;
        private set
        {
            if (_totalAmount != value)
            {
                _totalAmount = value;
                OnPropertyChanged();
            }
        }
    }

    public int ExpenseCount
    {
        get => _expenseCount;
        private set
        {
            if (_expenseCount != value)
            {
                _expenseCount = value;
                OnPropertyChanged();
            }
        }
    }

    public decimal AverageAmount
    {
        get => _averageAmount;
        private set
        {
            if (_averageAmount != value)
            {
                _averageAmount = value;
                OnPropertyChanged();
            }
        }
    }

    public decimal LargestExpense
    {
        get => _largestExpense;
        private set
        {
            if (_largestExpense != value)
            {
                _largestExpense = value;
                OnPropertyChanged();
            }
        }
    }

    public string LargestCategory
    {
        get => _largestCategory;
        private set
        {
            if (_largestCategory != value)
            {
                _largestCategory = value;
                OnPropertyChanged();
            }
        }
    }

    public ICommand GoBackCommand { get; }

    public StatisticsViewModel()
    {
        _expenses =
            new ObservableCollection<Expense>();

        CategoryStatistics =
            new ObservableCollection<CategoryStatistic>();

        GoBackCommand =
            new Command(
                async () => await GoBackAsync());
    }

    public void ApplyQueryAttributes(
        IDictionary<string, object> query)
    {
        if (!query.TryGetValue(
                "Expenses",
                out var value))
        {
            return;
        }

        if (value is not ObservableCollection<Expense> expenses)
        {
            return;
        }

        _expenses = expenses;

        CalculateStatistics();
    }

    public void Refresh()
    {
        CalculateStatistics();
    }

    private void CalculateStatistics()
    {
        if (_expenses.Count == 0)
        {
            TotalAmount = 0;
            ExpenseCount = 0;
            AverageAmount = 0;
            LargestExpense = 0;
            LargestCategory = "Немає даних";

            CategoryStatistics.Clear();

            return;
        }

        TotalAmount =
            _expenses.Sum(
                expense => expense.Amount);

        ExpenseCount =
            _expenses.Count;

        AverageAmount =
            TotalAmount / ExpenseCount;

        LargestExpense =
            _expenses.Max(
                expense => expense.Amount);

        var largestCategory =
            _expenses
                .GroupBy(
                    expense => expense.Category)
                .Select(
                    group => new
                    {
                        Category = group.Key,

                        Amount =
                            group.Sum(
                                expense =>
                                    expense.Amount)
                    })
                .OrderByDescending(
                    item => item.Amount)
                .FirstOrDefault();

        LargestCategory =
            largestCategory?.Category
            ?? "Немає даних";

        CalculateCategoryStatistics();
    }

    private void CalculateCategoryStatistics()
    {
        CategoryStatistics.Clear();

        var statistics =
            _expenses
                .GroupBy(
                    expense => expense.Category)
                .Select(
                    group => new CategoryStatistic
                    {
                        Category = group.Key,

                        Amount =
                            group.Sum(
                                expense =>
                                    expense.Amount),

                        Count = group.Count()
                    })
                .OrderByDescending(
                    statistic =>
                        statistic.Amount);

        foreach (var statistic in statistics)
        {
            CategoryStatistics.Add(statistic);
        }
    }

    private async Task GoBackAsync()
    {
        await Shell.Current.GoToAsync("..");
    }

    public event PropertyChangedEventHandler?
        PropertyChanged;

    protected void OnPropertyChanged(
        [CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(
            this,
            new PropertyChangedEventArgs(
                propertyName));
    }
}
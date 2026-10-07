using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Lab1.Models;
using Lab1.Services;

namespace Lab1.ViewModels;

public class ExpensesViewModel :
    IQueryAttributable,
    INotifyPropertyChanged
{
    private readonly CurrencyRateService _rateService;
    private readonly CurrencyConverter _currencyConverter;
    private readonly CategoryService _categoryService;

    private decimal _amount;
    private string _category = string.Empty;
    private string _description = string.Empty;
    private DateTime _currentDate = DateTime.Today;

    private string _selectedCurrency = "UAH";

    private decimal _usdRate;
    private decimal _eurRate;
    private string _rateDate = string.Empty;

    public ObservableCollection<Expense> Expenses { get; set; }

    public ObservableCollection<string> Currencies { get; set; }

    public ObservableCollection<string> Categories =>
        _categoryService.Categories;

    public decimal Amount
    {
        get => _amount;
        set
        {
            if (_amount != value)
            {
                _amount = value;
                OnPropertyChanged();
            }
        }
    }

    public string Category
    {
        get => _category;
        set
        {
            if (_category != value)
            {
                _category = value;
                OnPropertyChanged();
            }
        }
    }

    public string Description
    {
        get => _description;
        set
        {
            if (_description != value)
            {
                _description = value;
                OnPropertyChanged();
            }
        }
    }

    public DateTime CurrentDate
    {
        get => _currentDate;
        set
        {
            if (_currentDate != value)
            {
                _currentDate = value;
                OnPropertyChanged();
            }
        }
    }

    public string SelectedCurrency
    {
        get => _selectedCurrency;
        set
        {
            if (_selectedCurrency != value)
            {
                _selectedCurrency = value;
                OnPropertyChanged();
            }
        }
    }

    public decimal UsdRate
    {
        get => _usdRate;
        private set
        {
            if (_usdRate != value)
            {
                _usdRate = value;
                OnPropertyChanged();
            }
        }
    }

    public decimal EurRate
    {
        get => _eurRate;
        private set
        {
            if (_eurRate != value)
            {
                _eurRate = value;
                OnPropertyChanged();
            }
        }
    }

    public string RateDate
    {
        get => _rateDate;
        private set
        {
            if (_rateDate != value)
            {
                _rateDate = value;
                OnPropertyChanged();
            }
        }
    }

    public decimal TotalAmount =>
        Expenses.Sum(expense => expense.Amount);

    public ICommand AddExpenseCommand { get; }
    public ICommand DeleteExpenseCommand { get; }
    public ICommand OpenDetailsCommand { get; }
    public ICommand OpenStatisticsCommand { get; }
    public ICommand OpenCategoriesCommand { get; }

    public ExpensesViewModel(CategoryService categoryService)
    {
        _categoryService = categoryService;

        _rateService = new CurrencyRateService();
        _currencyConverter = new CurrencyConverter();

        Expenses = new ObservableCollection<Expense>();

        Currencies = new ObservableCollection<string>
        {
            "UAH",
            "USD",
            "EUR"
        };

        AddExpenseCommand = new Command(AddExpense);

        DeleteExpenseCommand =
            new Command<Expense>(DeleteExpense);

        OpenDetailsCommand =
            new Command<Expense>(
                async expense => await OpenDetailsAsync(expense));

        OpenStatisticsCommand =
            new Command(
                async () => await OpenStatisticsAsync());

        OpenCategoriesCommand =
            new Command(
                async () => await OpenCategoriesAsync());
    }

    private void AddExpense()
    {
        if (Amount <= 0)
            return;

        if (string.IsNullOrWhiteSpace(Category))
            return;

        // Конвертуємо вибрану валюту в гривні
        decimal amountInUah = _currencyConverter.ConvertToUah(
            Amount,
            SelectedCurrency,
            UsdRate,
            EurRate);

        var expense = new Expense
        {
            Id = Expenses.Count + 1,

            // У витраті зберігаємо вже суму в гривнях
            Amount = amountInUah,

            Category = Category,
            Description = Description,
            Date = CurrentDate,

            // Зберігаємо валюту, яку вибрав користувач
            Currency = SelectedCurrency
        };

        Expenses.Add(expense);

        Amount = 0;
        Category = string.Empty;
        Description = string.Empty;
        CurrentDate = DateTime.Today;
        SelectedCurrency = "UAH";

        OnPropertyChanged(nameof(TotalAmount));
    }

    private void DeleteExpense(Expense expense)
    {
        if (expense == null)
            return;

        Expenses.Remove(expense);

        OnPropertyChanged(nameof(TotalAmount));
    }

    private async Task OpenDetailsAsync(Expense expense)
    {
        if (expense == null)
            return;

        var parameters = new Dictionary<string, object>
        {
            { "SelectedExpense", expense }
        };

        await Shell.Current.GoToAsync(
            "expensedetail",
            parameters);
    }

    private async Task OpenStatisticsAsync()
    {
        var parameters = new Dictionary<string, object>
        {
            { "Expenses", Expenses }
        };

        await Shell.Current.GoToAsync(
            "statistics",
            parameters);
    }

    private async Task OpenCategoriesAsync()
    {
        await Shell.Current.GoToAsync("categories");
    }

    public async Task LoadRatesAsync()
    {
        try
        {
            var rates = await _rateService.GetRatesAsync();

            var usd = rates.FirstOrDefault(
                rate => rate.Currency == "USD");

            var eur = rates.FirstOrDefault(
                rate => rate.Currency == "EUR");

            if (usd != null)
                UsdRate = usd.Rate;

            if (eur != null)
                EurRate = eur.Rate;

            if (usd != null)
                RateDate = usd.Date;
        }
        catch
        {
            UsdRate = 0;
            EurRate = 0;
            RateDate = "Помилка завантаження";
        }
    }

    public void ApplyQueryAttributes(
        IDictionary<string, object> query)
    {
        if (!query.TryGetValue(
                "UpdatedExpense",
                out var value))
            return;

        if (value is not Expense updatedExpense)
            return;

        var existingExpense = Expenses.FirstOrDefault(
            expense => expense.Id == updatedExpense.Id);

        if (existingExpense == null)
            return;

        var index = Expenses.IndexOf(existingExpense);

        Expenses[index] = updatedExpense;

        OnPropertyChanged(nameof(TotalAmount));
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected void OnPropertyChanged(
        [CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(
            this,
            new PropertyChangedEventArgs(propertyName));
    }
}
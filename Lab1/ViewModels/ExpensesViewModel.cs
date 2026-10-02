using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Lab1.Models;
using Lab1.Services;

namespace Lab1.ViewModels;

public class ExpensesViewModel : INotifyPropertyChanged
{
    private readonly CurrencyConverter _currencyConverter;
    private readonly CurrencyRateService _currencyRateService;

    private decimal _totalAmount;

    private decimal _usdRate;
    private decimal _eurRate;

    private string _rateDate = "Курс не завантажено";

    private string _amount = string.Empty;
    private string _category = string.Empty;
    private string _description = string.Empty;

    private string _selectedCurrency = "UAH";

    private DateTime _currentDate = DateTime.Today;

    public ObservableCollection<Expense> Expenses { get; set; }

    public ObservableCollection<string> Currencies { get; set; }

    public string Amount
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

    public ICommand AddExpenseCommand { get; }

    public ICommand DeleteExpenseCommand { get; }

    public ExpensesViewModel()
    {
        _currencyConverter = new CurrencyConverter();
        _currencyRateService = new CurrencyRateService();

        Expenses = new ObservableCollection<Expense>();

        Currencies = new ObservableCollection<string>
        {
            "UAH",
            "USD",
            "EUR"
        };

        AddExpenseCommand = new Command(AddExpense);
        DeleteExpenseCommand = new Command<Expense>(DeleteExpense);
    }

    public async Task LoadRatesAsync()
    {
        try
        {
            var rates = await _currencyRateService.GetRatesAsync();

            var usd = rates.FirstOrDefault(rate => rate.Currency == "USD");
            var eur = rates.FirstOrDefault(rate => rate.Currency == "EUR");

            if (usd != null)
                UsdRate = usd.Rate;

            if (eur != null)
                EurRate = eur.Rate;

            if (usd != null)
                RateDate = usd.Date;
        }
        catch
        {
            RateDate = "Не вдалося завантажити курс";
        }
    }

    private void AddExpense()
    {
        if (!decimal.TryParse(Amount, out decimal amount))
            return;

        if (amount <= 0)
            return;

        if (string.IsNullOrWhiteSpace(Category))
            return;

        decimal amountInUah = _currencyConverter.ConvertToUah(
            amount,
            SelectedCurrency,
            UsdRate,
            EurRate);

        var expense = new Expense
        {
            Id = Expenses.Count + 1,

            Amount = amountInUah,

            Category = Category,

            Description = Description,

            Date = CurrentDate,

            Currency = "UAH"
        };

        Expenses.Add(expense);

        UpdateTotalAmount();

        ClearForm();
    }

    private void DeleteExpense(Expense? expense)
    {
        if (expense == null)
            return;

        Expenses.Remove(expense);

        UpdateTotalAmount();
    }

    private void UpdateTotalAmount()
    {
        TotalAmount = Expenses.Sum(expense => expense.Amount);
    }

    private void ClearForm()
    {
        Amount = string.Empty;
        Category = string.Empty;
        Description = string.Empty;

        SelectedCurrency = "UAH";

        CurrentDate = DateTime.Today;
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
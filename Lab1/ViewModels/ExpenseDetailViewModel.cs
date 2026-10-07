using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Lab1.Models;
using Lab1.Services;

namespace Lab1.ViewModels;

public class ExpenseDetailViewModel :
    IQueryAttributable,
    INotifyPropertyChanged
{
    private readonly CategoryService _categoryService;

    private Expense? _expense;

    private string _amount = string.Empty;
    private string _category = string.Empty;
    private string _description = string.Empty;
    private DateTime _date = DateTime.Today;

    public IEnumerable<string> Categories =>
        _categoryService.Categories;

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

    public DateTime Date
    {
        get => _date;
        set
        {
            if (_date != value)
            {
                _date = value;
                OnPropertyChanged();
            }
        }
    }

    public ICommand SaveCommand { get; }
    public ICommand GoBackCommand { get; }

    public ExpenseDetailViewModel(CategoryService categoryService)
    {
        _categoryService = categoryService;

        SaveCommand = new Command(
            async () => await SaveAsync());

        GoBackCommand = new Command(
            async () => await GoBackAsync());
    }

    public void ApplyQueryAttributes(
        IDictionary<string, object> query)
    {
        if (!query.TryGetValue(
                "SelectedExpense",
                out var value))
            return;

        if (value is not Expense expense)
            return;

        _expense = expense;

        Amount = expense.Amount.ToString("F2");
        Category = expense.Category;
        Description = expense.Description;
        Date = expense.Date;
    }

    private async Task SaveAsync()
    {
        if (_expense == null)
            return;

        if (!decimal.TryParse(
                Amount,
                out decimal amount))
            return;

        if (amount <= 0)
            return;

        if (string.IsNullOrWhiteSpace(Category))
            return;

        var updatedExpense = new Expense
        {
            Id = _expense.Id,
            Amount = amount,
            Category = Category,
            Description = Description,
            Date = Date,
            Currency = _expense.Currency
        };

        var parameters = new Dictionary<string, object>
        {
            { "UpdatedExpense", updatedExpense }
        };

        await Shell.Current.GoToAsync(
            "..",
            parameters);
    }

    private async Task GoBackAsync()
    {
        await Shell.Current.GoToAsync("..");
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
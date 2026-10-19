
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
    private readonly IAppStateService _appStateService;

    private Expense? _expense;
    private bool _isRestoringState;

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
                SaveDraftIfNeeded();
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
                SaveDraftIfNeeded();
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
                SaveDraftIfNeeded();
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
                SaveDraftIfNeeded();
            }
        }
    }

    public ICommand SaveCommand { get; }
    public ICommand GoBackCommand { get; }

    public ExpenseDetailViewModel(
        CategoryService categoryService,
        IAppStateService appStateService)
    {
        _categoryService = categoryService;
        _appStateService = appStateService;

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

        var draft = _appStateService.GetExpenseDraft();

        _isRestoringState = true;

        try
        {
            if (draft != null &&
                draft.ExpenseId == expense.Id)
            {
                Amount = draft.Amount;
                Category = draft.Category;
                Description = draft.Description;
                Date = draft.Date;
            }
            else
            {
                Amount = expense.Amount.ToString("F2");
                Category = expense.Category;
                Description = expense.Description;
                Date = expense.Date;
            }
        }
        finally
        {
            _isRestoringState = false;
        }

        SaveDraftIfNeeded();
    }

    private void SaveDraftIfNeeded()
    {
        if (_expense == null || _isRestoringState)
            return;

        _appStateService.SaveExpenseDraft(
            new ExpenseEditState
            {
                ExpenseId = _expense.Id,
                Amount = Amount,
                Category = Category,
                Description = Description,
                Date = Date
            });
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

        _appStateService.ClearExpenseDraft();

        await Shell.Current.GoToAsync(
            "..",
            parameters);
    }

    private async Task GoBackAsync()
    {
        _appStateService.ClearExpenseDraft();

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

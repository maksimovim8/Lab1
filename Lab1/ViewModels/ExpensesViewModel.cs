using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Lab1.Models;

namespace Lab1.ViewModels;

public class ExpensesViewModel : INotifyPropertyChanged
{
    private decimal _totalAmount;

    private string _amount = string.Empty;
    private string _category = string.Empty;
    private string _description = string.Empty;
    private DateTime _currentDate = DateTime.Today;

    public ObservableCollection<Expense> Expenses { get; set; }

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

    public ICommand AddExpenseCommand { get; }

    public ICommand DeleteExpenseCommand { get; }

    public ExpensesViewModel()
    {
        Expenses = new ObservableCollection<Expense>();

        AddExpenseCommand = new Command(AddExpense);
        DeleteExpenseCommand = new Command<Expense>(DeleteExpense);
    }

    private void AddExpense()
    {
        if (!decimal.TryParse(Amount, out decimal amount))
            return;

        if (amount <= 0)
            return;

        if (string.IsNullOrWhiteSpace(Category))
            return;

        var expense = new Expense
        {
            Id = Expenses.Count + 1,
            Amount = amount,
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
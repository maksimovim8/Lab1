using Lab1.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using System.Xml.Linq;

namespace Lab1.ViewModels;

public class ExpensesViewModel : INotifyPropertyChanged
{
    private decimal _totalAmount;
    private DateTime _currentDate = DateTime.Today;

    public ObservableCollection<Expense> Expenses { get; set; }

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
        // Тимчасово створюємо тестову витрату.
        // Пізніше тут будемо отримувати дані з Entry.
        var expense = new Expense
        {
            Id = Expenses.Count + 1,
            Amount = 100,
            Category = "Їжа",
            Description = "Тестова витрата",
            Date = CurrentDate,
            Currency = "UAH"
        };

        Expenses.Add(expense);

        UpdateTotalAmount();
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

    public event PropertyChangedEventHandler? PropertyChanged;

    protected void OnPropertyChanged(
        [CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(
            this,
            new PropertyChangedEventArgs(propertyName));
    }
}

using System.Collections.ObjectModel;
using Lab1.Models;

namespace Lab1.Services;

public class ExpenseStore
{
    public ObservableCollection<Expense> Expenses { get; } = new();
}

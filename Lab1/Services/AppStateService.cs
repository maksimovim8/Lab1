
using Lab1.Models;

namespace Lab1.Services;

public class AppStateService : IAppStateService
{
    private ExpenseEditState? _expenseDraft;

    public ExpenseEditState? GetExpenseDraft()
    {
        return _expenseDraft;
    }

    public void SaveExpenseDraft(ExpenseEditState state)
    {
        _expenseDraft = state;
    }

    public void ClearExpenseDraft()
    {
        _expenseDraft = null;
    }
}

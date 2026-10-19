
using Lab1.Models;

namespace Lab1.Services;

public interface IAppStateService
{
    ExpenseEditState? GetExpenseDraft();

    void SaveExpenseDraft(ExpenseEditState state);

    void ClearExpenseDraft();
}

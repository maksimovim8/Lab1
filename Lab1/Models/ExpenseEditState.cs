
namespace Lab1.Models;

public class ExpenseEditState
{
    public int ExpenseId { get; set; }

    public string Amount { get; set; } = string.Empty;

    public string Category { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public DateTime Date { get; set; } = DateTime.Today;
}

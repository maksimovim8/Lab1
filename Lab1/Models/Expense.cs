namespace Lab1.Models;

public class Expense
{
    public int Id { get; set; }

    public decimal Amount { get; set; }

    public string Category { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public DateTime Date { get; set; }

    public string Currency { get; set; } = "UAH";


}
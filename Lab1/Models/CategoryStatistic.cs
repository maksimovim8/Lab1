namespace Lab1.Models;

public class CategoryStatistic
{
    public string Category { get; set; } = string.Empty;

    public decimal Amount { get; set; }

    public int Count { get; set; }

    public string AmountText =>
        $"{Amount:F2} UAH";
}
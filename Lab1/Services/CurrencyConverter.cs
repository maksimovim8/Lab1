namespace Lab1.Services;

public class CurrencyConverter
{
    public decimal ConvertToUah(
        decimal amount,
        string currency,
        decimal usdRate,
        decimal eurRate)
    {
        return currency switch
        {
            "UAH" => amount,

            "USD" => amount * usdRate,

            "EUR" => amount * eurRate,

            _ => throw new ArgumentException(
                $"Невідома валюта: {currency}")
        };
    }
}
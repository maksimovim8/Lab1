using System.Net.Http.Json;
using Lab1.Models;

namespace Lab1.Services;

public class CurrencyRateService
{
    private readonly HttpClient _httpClient;

    public CurrencyRateService()
    {
        _httpClient = new HttpClient();
    }

    public async Task<List<CurrencyRate>> GetRatesAsync()
    {
        const string url =
            "https://bank.gov.ua/NBUStatService/v1/statdirectory/exchangenew?json";

        var rates = await _httpClient.GetFromJsonAsync<List<NbuCurrencyRate>>(url);

        if (rates == null)
            throw new Exception("Не вдалося отримати курси валют.");

        return rates
            .Where(rate => rate.cc == "USD" || rate.cc == "EUR")
            .Select(rate => new CurrencyRate
            {
                Currency = rate.cc,
                Rate = rate.rate,
                Date = rate.exchangedate
            })
            .ToList();
    }

    private class NbuCurrencyRate
    {
        public int r030 { get; set; }

        public string txt { get; set; } = string.Empty;

        public decimal rate { get; set; }

        public string cc { get; set; } = string.Empty;

        public string exchangedate { get; set; } = string.Empty;
    }
}
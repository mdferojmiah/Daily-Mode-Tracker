using System.Text.Json;
using MoodTracker.Models.Entities;
using MoodTracker.ServiceContracts;

namespace MoodTracker.Services;

public class QuoteService: IQuoteService
{
    private readonly IConfiguration _configuration;
    private readonly IHttpClientFactory _clientFactory;

    public QuoteService(IConfiguration configuration, IHttpClientFactory clientFactory)
    {
        _configuration = configuration;
        _clientFactory = clientFactory;
    }

    public async Task<Quote> GetRandomQuote()
    {
        Quote? randomQuote = null;
        HttpClient client = _clientFactory.CreateClient("ZenQuoteAPI");
        HttpResponseMessage response = await client.GetAsync("/api/random");
        response.EnsureSuccessStatusCode();
        string responseBody = await response.Content.ReadAsStringAsync();
        List<Quote>? quotes = JsonSerializer.Deserialize<List<Quote>>(responseBody);
        if (quotes != null)
        {
            randomQuote = quotes[0];
        }
        return randomQuote!;
    }
}
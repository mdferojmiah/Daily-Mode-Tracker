using MoodTracker.Models.Entities;

namespace MoodTracker.ServiceContracts;

public interface IQuoteService
{
    Task<Quote> GetRandomQuote();
}
using System.Text.Json.Serialization;

namespace MoodTracker.Models.Entities;

public class Quote
{
    [JsonPropertyName("q")]
    public string QuoteText { get; set; } = string.Empty;

    [JsonPropertyName("a")]
    public string Author { get; set; } = string.Empty;

    [JsonPropertyName("h")]
    public string HtmlVersion { get; set; } =  string.Empty;
}
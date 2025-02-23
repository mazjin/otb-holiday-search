using System.Text.Json.Serialization;

namespace HolidaySearch.Models;

public record Airport()
{
    [JsonPropertyName("name")]
    public string Name { get; init; }
    
    [JsonPropertyName("code")]
    public string Code { get; init; }
    
    [JsonPropertyName("region")]
    public string Region { get; set; }
    // should probably put country here but out of scope
};
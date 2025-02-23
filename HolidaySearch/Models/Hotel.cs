using System.Text.Json.Serialization;

namespace HolidaySearch.Models;

public record Hotel
{
    [JsonPropertyName("id")] public int Id { get; set; }

    [JsonPropertyName("name")] public string Name { get; set; }

    [JsonPropertyName("arrival_date")] public DateOnly ArrivalDate { get; set; }

    [JsonPropertyName("price_per_night")] public int PricePerNight { get; set; }

    [JsonPropertyName("local_airports")] public List<string> LocalAirports { get; set; }

    [JsonPropertyName("nights")] public int Nights { get; set; }
}
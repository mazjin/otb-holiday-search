using System.Text.Json.Serialization;

namespace HolidaySearch.Models;

public record Flight
{
    [JsonPropertyName("id")] public int Id { get; init; }

    [JsonPropertyName("airline")] public string Airline { get; init; }

    [JsonPropertyName("from")] public string From { get; init; }

    [JsonPropertyName("to")] public string To { get; init; }

    [JsonPropertyName("price")] public int Price { get; init; }

    [JsonPropertyName("departure_date")] public DateOnly DepartureDate { get; init; }
}
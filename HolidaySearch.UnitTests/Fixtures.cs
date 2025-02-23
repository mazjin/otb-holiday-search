using System.Text.Json;
using HolidaySearch.Models;

namespace HolidaySearch.UnitTests;

public static class Fixtures
{
    private static List<T> GetFixture<T>(string filePath, Func<T, bool>? filter) where T : class
    {
        List<T>? data = new();
        using (var sr = new StreamReader(filePath))
        {
            var json = sr.ReadToEnd();
            data = JsonSerializer.Deserialize<List<T>>(json);
        }

        if (data is null) throw new FileNotFoundException();
        return filter is null ? data : data.Where(filter).ToList();
    }

    public static List<Hotel> GetHotels(Func<Hotel, bool>? filter)
    {
        return GetFixture("Data/hotels.json", filter);
    }

    public static List<Flight> GetFlights(Func<Flight, bool>? filter)
    {
        return GetFixture("Data/flights.json", filter);
    }

    public static List<Airport> GetAirports(Func<Airport, bool>? filter)
    {
        return GetFixture("Data/airports.json", filter);
    }
}
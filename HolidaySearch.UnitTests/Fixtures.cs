using System.Text.Json;
using HolidaySearch.Models;

namespace HolidaySearch.UnitTests;

public static class Fixtures
{
    public static List<Hotel> GetHotels(Func<Hotel, bool>? filter)
    {
        List<Hotel>? hotels = new();
        using (StreamReader sr = new StreamReader("hotels.json"))
        {
            string json = sr.ReadToEnd();
            hotels = JsonSerializer.Deserialize<List<Hotel>>(json);
        }
        if (hotels is null) throw new FileNotFoundException();
        return filter is null ? hotels : hotels.Where(filter).ToList();
    }
    public static List<Flight> GetFlights(Func<Flight, bool>? filter)
    {
        List<Flight>? flights = new();
        using (StreamReader sr = new StreamReader("flights.json"))
        {
            string json = sr.ReadToEnd();
            flights = JsonSerializer.Deserialize<List<Flight>>(json);
        }
        if (flights is null) throw new FileNotFoundException();
        return filter is null ? flights : flights.Where(filter).ToList();
    }
    
    
}
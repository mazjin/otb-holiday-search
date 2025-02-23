using System.Text.Json;
using HolidaySearch.Models;

namespace HolidaySearch.UnitTests;

public static class Fixtures
{
    public static List<Hotel> GetHotels()
    {
        List<Hotel>? hotels = new();
        using (StreamReader sr = new StreamReader("hotels.json"))
        {
            string json = sr.ReadToEnd();
            hotels = JsonSerializer.Deserialize<List<Hotel>>(json);
        }
        if (hotels is null) throw new FileNotFoundException();
        return hotels;
    }
    public static List<Flight> GetFlights()
    {
        List<Flight>? flights = new();
        using (StreamReader sr = new StreamReader("flights.json"))
        {
            string json = sr.ReadToEnd();
            flights = JsonSerializer.Deserialize<List<Flight>>(json);
        }
        if (flights is null) throw new FileNotFoundException();
        return flights;
    }
    
    
}
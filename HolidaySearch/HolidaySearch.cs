using HolidaySearch.Models;

namespace HolidaySearch;

public class HolidaySearch
{
    public List<HolidaySearchResult> Results { get; set; }
    
    public HolidaySearch(IHotelsSearch hotelsSearch, IFlightsSearch flightsSearch)
    {
        var hotels = hotelsSearch.GetHotels();
        var flights = flightsSearch.GetFlights();
        Results = hotels.Take(5).SelectMany(hotel => hotel.LocalAirports, (hotel, airport) => new HolidaySearchResult
        {
            Flight = flights.FirstOrDefault(x => x.To == airport),
            Hotel = hotel,
        }).OrderBy(x => x.TotalPrice).ToList();
    }
}
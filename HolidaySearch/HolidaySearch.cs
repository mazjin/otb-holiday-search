using HolidaySearch.Models;

namespace HolidaySearch;

public class HolidaySearch
{
    public List<HolidaySearchResult> Results { get; set; }
    
    public HolidaySearch(IHotelsSearch hotelsSearch, IFlightsSearch flightsSearch)
    {
        var hotels = hotelsSearch.GetHotels();
        var flights = flightsSearch.GetFlights();
        Results = hotels.Take(5).Select((hotel, index) => new HolidaySearchResult
        {
            Flight = flights[index],
            Hotel = hotel,
        }).OrderBy(x => x.TotalPrice).ToList();
    }
}
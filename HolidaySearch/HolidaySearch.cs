using HolidaySearch.Models;

namespace HolidaySearch;

public class HolidaySearch
{
    private readonly IFlightsSearch _flightsSearch;
    private readonly IHotelsSearch _hotelsSearch;
    public List<HolidaySearchResult> Results { get; set; }

    public HolidaySearch(IHotelsSearch hotelsSearch, IFlightsSearch flightsSearch)
    {
        _hotelsSearch = hotelsSearch;
        _flightsSearch = flightsSearch;
    }
    public void Search(HolidaySearchQuery query)
    {
        var hotels = _hotelsSearch.GetHotels();
        var flights = _flightsSearch.GetFlights();
        Results = hotels
            .Where(hotel => hotel.LocalAirports.Contains(query.TravelingTo))
            .SelectMany(hotel => hotel.LocalAirports, (hotel, airport) => new HolidaySearchResult
        {
            Flight = flights.FirstOrDefault(x => x.To == query.TravelingTo),
            Hotel = hotel,
        }).OrderBy(x => x.TotalPrice).ToList();
    }
}
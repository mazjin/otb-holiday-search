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
            .Select(hotel => new HolidaySearchResult
            {
                Flight = flights.FirstOrDefault(flight => flight.To == query.TravelingTo
                                                          && flight.From == query.DepartingFrom
                                                          && MatchesDepartureDate(query,flight)),
                Hotel = hotel,
            }).OrderBy(x => x.TotalPrice).ToList();
    }

    private bool MatchesDepartureDate(HolidaySearchQuery query, Flight flight)
    {
        return string.IsNullOrEmpty(query.DepartureDate) || DateOnly.Parse(query.DepartureDate).Equals(flight.DepartureDate);
    }
}
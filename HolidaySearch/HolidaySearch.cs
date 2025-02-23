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
            .Where(hotel => MatchesTravellingTo(query, hotel))
            .SelectMany(hotel => hotel.LocalAirports, (hotel, airport) => new HolidaySearchResult
            {
                Flight = flights.FirstOrDefault(flight => hotel.LocalAirports.Contains(flight.To) 
                                                          && MatchesTravellingTo(query, flight)
                                                          && MatchesDepartingFrom(query, flight)
                                                          && MatchesDepartureDate(query, flight)),
                Hotel = hotel,
            })
            .Where(result => result.Flight is not null)
            .OrderBy(x => x.TotalPrice)
            .ToList();
    }

    private bool MatchesDepartingFrom(HolidaySearchQuery query, Flight flight)
    {
        return string.IsNullOrEmpty(query.DepartingFrom) || query.DepartingFrom.Equals(flight.From);
    }

    private bool MatchesTravellingTo(HolidaySearchQuery query, Flight flight)
    {
        return string.IsNullOrEmpty(query.TravelingTo) || query.TravelingTo.Equals(flight.To);
    }

    private bool MatchesTravellingTo(HolidaySearchQuery query, Hotel hotel)
    {
        return string.IsNullOrEmpty(query.TravelingTo) || hotel.LocalAirports.Contains(query.TravelingTo);
    }

    private bool MatchesDepartureDate(HolidaySearchQuery query, Flight flight)
    {
        return string.IsNullOrEmpty(query.DepartureDate) ||
               DateOnly.Parse(query.DepartureDate).Equals(flight.DepartureDate);
    }
}
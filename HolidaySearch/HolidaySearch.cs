using HolidaySearch.Models;

namespace HolidaySearch;

public class HolidaySearch
{
    private readonly IFlightsSearch _flightsSearch;
    private readonly IHotelsSearch _hotelsSearch;
    private readonly IAirportsSearch _airportsSearch;
    public List<HolidaySearchResult> Results { get; set; }

    public HolidaySearch(IHotelsSearch hotelsSearch, IFlightsSearch flightsSearch, IAirportsSearch airportsSearch)
    {
        _hotelsSearch = hotelsSearch;
        _flightsSearch = flightsSearch;
        _airportsSearch = airportsSearch;
    }

    public void Search(HolidaySearchQuery query)
    {
        var hotels = _hotelsSearch.GetHotels(hotel => MatchesTravellingTo(query, hotel)
                                                      && MatchesDuration(query, hotel)
                                                      && MatchesDepartureDate(query, hotel));
        var destinations = hotels.SelectMany(hotel => hotel.LocalAirports, (hotel, airport) => new { hotel, airport })
            .ToList();
        var airports = destinations.Select(x => x.airport).Distinct().ToList();
        var flights = _flightsSearch.GetFlights(flight => airports.Contains(flight.To)
                                                          && MatchesTravellingTo(query, flight)
                                                          && MatchesDepartingFrom(query, flight)
                                                          && MatchesDepartureDate(query, flight));
        Results = destinations
            .Join(flights, destination => destination.airport, flight => flight.To,
                (destination, flight) => new HolidaySearchResult() { Hotel = destination.hotel, Flight = flight })
            .OrderBy(x => x.TotalPrice)
            .ToList();
    }

    private bool MatchesDuration(HolidaySearchQuery query, Hotel hotel)
    {
        return query.Duration is null || query.Duration.Equals(hotel.Nights);
    }

    private bool MatchesDepartingFrom(HolidaySearchQuery query, Flight flight)
    {
        var departureAirportCodes = query.DepartingFromType is LocationType.Region ? _airportsSearch.GetAirports(airport => airport.Region == query.DepartingFrom).Select(airport => airport.Code).ToList() : new List<string>(){query.DepartingFrom};
        return string.IsNullOrEmpty(query.DepartingFrom) || departureAirportCodes.Contains(flight.From);
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

    private bool MatchesDepartureDate(HolidaySearchQuery query, Hotel hotel)
    {
        return string.IsNullOrEmpty(query.DepartureDate) ||
               DateOnly.Parse(query.DepartureDate).Equals(hotel.ArrivalDate);
    }
}
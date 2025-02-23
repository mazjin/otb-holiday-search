using HolidaySearch.Models;

namespace HolidaySearch;

public interface IFlightsSearch
{
    List<Flight> GetFlights(Func<Flight, bool>? filter);
}
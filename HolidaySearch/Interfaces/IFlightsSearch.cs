using HolidaySearch.Models;

namespace HolidaySearch.Interfaces;

public interface IFlightsSearch
{
    List<Flight> GetFlights(Func<Flight, bool>? filter);
}
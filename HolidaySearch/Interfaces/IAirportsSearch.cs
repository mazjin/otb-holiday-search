using HolidaySearch.Models;

namespace HolidaySearch;

public interface IAirportsSearch
{
    List<Airport> GetAirports(Func<Airport, bool>? filter);
}
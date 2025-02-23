using HolidaySearch.Models;

namespace HolidaySearch.Interfaces;

public interface IAirportsSearch
{
    List<Airport> GetAirports(Func<Airport, bool>? filter);
}
using HolidaySearch.Models;

namespace HolidaySearch;

public interface IHotelsSearch
{
    List<Hotel> GetHotels(Func<Hotel, bool>? filter);
}
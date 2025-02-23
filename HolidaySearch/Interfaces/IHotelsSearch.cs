using HolidaySearch.Models;

namespace HolidaySearch.Interfaces;

public interface IHotelsSearch
{
    List<Hotel> GetHotels(Func<Hotel, bool>? filter);
}
using HolidaySearch.Models.Enums;

namespace HolidaySearch.Models;

public class HolidaySearchQuery
{
    public string? TravelingTo { get; set; }
    public string? DepartingFrom { get; set; }
    public LocationType DepartingFromType { get; set; } = LocationType.Airport;
    public string? DepartureDate { get; set; }
    public int? Duration { get; set; }
}
namespace HolidaySearch.Models;

public class HolidaySearchQuery
{
    public string? TravelingTo { get; set; }
    public string? DepartingFrom { get; set; }
    public string? DepartureDate { get; set; }
    public int? Duration { get; set; }
}
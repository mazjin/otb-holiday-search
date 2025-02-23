namespace HolidaySearch.Models;

public class HolidaySearchResult
{
    public Flight? Flight { get; set; }
    public Hotel Hotel { get; set; }
    public int TotalPrice => (Flight?.Price ?? 0) + (Hotel.PricePerNight * Hotel.Nights);
}
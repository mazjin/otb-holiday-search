using NSubstitute;
using Shouldly;

namespace HolidaySearch.UnitTests;

public class HolidaySearchTests
{
    [Fact]
    public void HolidaySearch_ShouldReturnListOfHolidaysOrderedByTotalPrice()
    {
        // Arrange
        var hotelsSearch = Substitute.For<IHotelsSearch>();
        hotelsSearch.GetHotels().Returns(Fixtures.GetHotels());
        var flightSearch = Substitute.For<IFlightsSearch>();
        flightSearch.GetFlights().Returns(Fixtures.GetFlights());
        
        // Act
        var search = new HolidaySearch(hotelsSearch, flightSearch);

        // Assert
        search.Results.First().ShouldBe(search.Results.MinBy(x => x.TotalPrice));
    }

    [Fact]
    public void HolidaySearch_ShouldOnlyReturnHolidaysWhereFlightAndHotelLocationMatch()
    {
        // Arrange
        var hotelsSearch = Substitute.For<IHotelsSearch>();
        hotelsSearch.GetHotels().Returns(Fixtures.GetHotels());
        var flightSearch = Substitute.For<IFlightsSearch>();
        flightSearch.GetFlights().Returns(Fixtures.GetFlights());
        
        // Act
        var search = new HolidaySearch(hotelsSearch, flightSearch);
        search.Results.ForEach(result => result.Hotel.LocalAirports.ShouldContain(result.Flight.To));
    }
    
}



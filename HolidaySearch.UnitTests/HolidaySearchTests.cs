using NSubstitute;
using Shouldly;

namespace HolidaySearch.UnitTests;

public class HolidaySearchTests
{
    private readonly IHotelsSearch _hotelsSearch;
    private readonly IFlightsSearch _flightSearch;

    public HolidaySearchTests()
    {
        _hotelsSearch = Substitute.For<IHotelsSearch>();
        _hotelsSearch.GetHotels().Returns(Fixtures.GetHotels());
        _flightSearch = Substitute.For<IFlightsSearch>();
        _flightSearch.GetFlights().Returns(Fixtures.GetFlights());
    }
    
    [Fact]
    public void HolidaySearch_ShouldReturnListOfHolidaysOrderedByTotalPrice()
    {
        // Act
        var search = new HolidaySearch(_hotelsSearch, _flightSearch);

        // Assert
        search.Results.First().ShouldBe(search.Results.MinBy(x => x.TotalPrice));
    }

    [Fact]
    public void HolidaySearch_ShouldOnlyReturnHolidaysWhereFlightAndHotelLocationMatch()
    {
        // Act
        var search = new HolidaySearch(_hotelsSearch, _flightSearch);
        
        // Assert
        search.Results.ForEach(result => result.Hotel.LocalAirports.ShouldContain(result.Flight.To));
    }
    [Theory]
    [InlineData("AGP")]
    [InlineData("PMI")]
    [InlineData("LPA")]
    [InlineData("TFS")]
    public void HolidaySearch_ShouldOnlyReturnHolidaysWhereHotelMatchesQueriedDestination(string destination)
    {
        // Arrange
        
        // Act
        var search = new HolidaySearch(_hotelsSearch, _flightSearch);
        
        // Assert
        search.Results.ForEach(result => result.Hotel.LocalAirports.ShouldContain(destination));
    }
}



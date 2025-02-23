using HolidaySearch.Models;
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
        // Arrange
        var search = new HolidaySearch(_hotelsSearch, _flightSearch);

        // Act
        search.Search(new HolidaySearchQuery() { TravelingTo = "AGP", DepartingFrom = "MAN"});

        // Assert
        search.Results.First().ShouldBe(search.Results.MinBy(x => x.TotalPrice));
    }

    [Fact]
    public void HolidaySearch_ShouldOnlyReturnHolidaysWhereFlightAndHotelLocationMatch()
    {
        // Arrange
        var search = new HolidaySearch(_hotelsSearch, _flightSearch);

        // Act
        search.Search(new HolidaySearchQuery() { TravelingTo = "AGP", DepartingFrom = "MAN" });

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
        var query = new HolidaySearchQuery
        {
            DepartingFrom = "MAN",
            TravelingTo = destination,
        };
        var search = new HolidaySearch(_hotelsSearch, _flightSearch);

        // Act
        search.Search(query);

        // Assert
        search.Results.ForEach(result => result.Hotel.LocalAirports.ShouldContain(destination));
    }

    [Theory]
    [InlineData("MAN", "TFS")]
    [InlineData("MAN", "AGP")]
    [InlineData("MAN", "PMI")]
    [InlineData("MAN", "LPA")]
    [InlineData("LTN", "PMI")]
    [InlineData("LGW", "AGP")]
    [InlineData("LGW", "PMI")]
    public void HolidaySearch_ShouldOnlyReturnHolidaysWhereFlightMatchesQueriedDepartureAirport(string departureAirport,
        string destination)
    {
        // Arrange
        var query = new HolidaySearchQuery
        {
            DepartingFrom = departureAirport,
            TravelingTo = destination,
        };
        var search = new HolidaySearch(_hotelsSearch, _flightSearch);

        // Act
        search.Search(query);

        // Assert
        search.Results.ForEach(result => result.Flight.From.ShouldBe(departureAirport));
    }
}
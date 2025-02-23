using HolidaySearch.Interfaces;
using HolidaySearch.Models;
using HolidaySearch.Models.Enums;
using NSubstitute;
using Shouldly;

namespace HolidaySearch.UnitTests;

public class HolidaySearchTests
{
    private readonly IAirportsSearch _airportSearch;
    private readonly IFlightsSearch _flightSearch;
    private readonly IHotelsSearch _hotelsSearch;

    public HolidaySearchTests()
    {
        _hotelsSearch = Substitute.For<IHotelsSearch>();
        _hotelsSearch.GetHotels(Arg.Any<Func<Hotel, bool>>()).Returns(x => Fixtures.GetHotels((Func<Hotel, bool>)x[0]));
        _flightSearch = Substitute.For<IFlightsSearch>();
        _flightSearch.GetFlights(Arg.Any<Func<Flight, bool>>())
            .Returns(x => Fixtures.GetFlights((Func<Flight, bool>)x[0]));
        _airportSearch = Substitute.For<IAirportsSearch>();
        _airportSearch.GetAirports(Arg.Any<Func<Airport, bool>>())
            .Returns(x => Fixtures.GetAirports((Func<Airport, bool>)x[0]));
    }

    [Fact]
    public void HolidaySearch_ShouldReturnListOfHolidaysOrderedByTotalPrice()
    {
        // Arrange
        var search = new HolidaySearch(_hotelsSearch, _flightSearch, _airportSearch);

        // Act
        search.Search(new HolidaySearchQuery());

        // Assert
        search.Results.First().ShouldBe(search.Results.MinBy(x => x.TotalPrice));
    }

    [Fact]
    public void HolidaySearch_ShouldOnlyReturnHolidaysWhereFlightAndHotelLocationMatch()
    {
        // Arrange
        var search = new HolidaySearch(_hotelsSearch, _flightSearch, _airportSearch);

        // Act
        search.Search(new HolidaySearchQuery());

        // Assert
        search.Results.Count.ShouldBeGreaterThan(0);
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
            TravelingTo = destination
        };
        var search = new HolidaySearch(_hotelsSearch, _flightSearch, _airportSearch);

        // Act
        search.Search(query);

        // Assert
        search.Results.Count.ShouldBeGreaterThan(0);
        search.Results.ForEach(result => result.Hotel.LocalAirports.ShouldContain(destination));
    }

    [Theory]
    [InlineData("MAN")]
    [InlineData("LTN")]
    [InlineData("LGW")]
    public void HolidaySearch_ShouldOnlyReturnHolidaysWhereFlightMatchesQueriedDepartureAirport(string departureAirport)
    {
        // Arrange
        var query = new HolidaySearchQuery
        {
            DepartingFrom = departureAirport
        };
        var search = new HolidaySearch(_hotelsSearch, _flightSearch, _airportSearch);

        // Act
        search.Search(query);

        // Assert
        search.Results.Count.ShouldBeGreaterThan(0);
        search.Results.ForEach(result => result.Flight.From.ShouldBe(departureAirport));
    }

    [Theory]
    [InlineData("London", "LGW", "LTN")]
    [InlineData("Manchester", "MAN")]
    public void HolidaySearch_ShouldOnlyReturnHolidaysWhereFlightMatchesQueriedDepartureRegion(string departureRegion,
        params string[] expectedDepartureRegions)
    {
        // Arrange
        var query = new HolidaySearchQuery
        {
            DepartingFrom = departureRegion,
            DepartingFromType = LocationType.Region
        };
        var search = new HolidaySearch(_hotelsSearch, _flightSearch, _airportSearch);

        // Act
        search.Search(query);

        // Assert
        search.Results.Count.ShouldBeGreaterThan(0);
        search.Results.ForEach(result => expectedDepartureRegions.ShouldContain(result.Flight.From));
    }

    [Theory]
    [InlineData(2023, 06, 15)]
    [InlineData(2023, 07, 01)]
    [InlineData(2022, 11, 10)]
    public void HolidaySearch_ShouldOnlyReturnHolidaysWhereDepartureDateMatchesQueriedDepartureDate(int year, int month,
        int day)
    {
        // Arrange
        var query = new HolidaySearchQuery
        {
            DepartureDate = $"{year}/{month}/{day}"
        };
        var search = new HolidaySearch(_hotelsSearch, _flightSearch, _airportSearch);

        // Act
        search.Search(query);

        // Assert
        search.Results.Count.ShouldBeGreaterThan(0);
        search.Results.ForEach(result =>
        {
            result.Flight.DepartureDate.ShouldBeEquivalentTo(new DateOnly(year, month, day));
            result.Hotel.ArrivalDate.ShouldBeEquivalentTo(new DateOnly(year, month, day));
        });
    }

    [Theory]
    [InlineData(7)]
    [InlineData(10)]
    [InlineData(14)]
    public void HolidaySearch_ShouldOnlyReturnHolidaysWhereStayDurationMatchesHotel(int duration)
    {
        // Arrange
        var query = new HolidaySearchQuery
        {
            Duration = duration
        };
        var search = new HolidaySearch(_hotelsSearch, _flightSearch, _airportSearch);

        // Act
        search.Search(query);

        // Assert
        search.Results.Count.ShouldBeGreaterThan(0);
        search.Results.ForEach(result => result.Hotel.Nights.ShouldBe(duration));
    }

    [Fact]
    public void HolidaySearch_ShouldReturnCorrectValuesForExampleCase1()
    {
        // Arrange
        var query = new HolidaySearchQuery
            { DepartingFrom = "MAN", TravelingTo = "AGP", DepartureDate = "2023/07/01", Duration = 7 };
        var search = new HolidaySearch(_hotelsSearch, _flightSearch, _airportSearch);

        // Act
        search.Search(query);

        // Assert
        search.Results.Count.ShouldBeGreaterThan(0);
        search.Results.First().Flight.Id.ShouldBe(2);
        search.Results.First().Hotel.Id.ShouldBe(9);
    }

    [Fact]
    public void HolidaySearch_ShouldReturnCorrectValuesForExampleCase2()
    {
        // Arrange
        var query = new HolidaySearchQuery
        {
            DepartingFrom = "London", DepartingFromType = LocationType.Region, TravelingTo = "PMI",
            DepartureDate = "2023/06/15", Duration = 10
        };
        var search = new HolidaySearch(_hotelsSearch, _flightSearch, _airportSearch);

        // Act
        search.Search(query);

        // Assert
        search.Results.Count.ShouldBeGreaterThan(0);
        search.Results.First().Flight.Id.ShouldBe(6);
        search.Results.First().Hotel.Id.ShouldBe(5);
    }

    [Fact]
    public void HolidaySearch_ShouldReturnCorrectValuesForExampleCase3()
    {
        // Arrange
        var query = new HolidaySearchQuery
            { DepartingFrom = null, TravelingTo = "LPA", DepartureDate = "2022/11/10", Duration = 14 };
        var search = new HolidaySearch(_hotelsSearch, _flightSearch, _airportSearch);

        // Act
        search.Search(query);

        // Assert
        search.Results.Count.ShouldBeGreaterThan(0);
        search.Results.First().Flight.Id.ShouldBe(7);
        search.Results.First().Hotel.Id.ShouldBe(6);
    }
}
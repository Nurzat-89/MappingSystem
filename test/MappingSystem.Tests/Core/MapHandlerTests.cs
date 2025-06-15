using FluentAssertions;
using Google;
using MappingSystem.Core;
using MappingSystem.Mappers.Reservations;

namespace MappingSystem.Tests.Core;

public class MapHandlerTests
{
    [Fact]
    public void Should_Return_GoogleReservation_WithValidArguments()
    {
        
        // Arrange
        var mappers = new List<IMapper>
        {
            new ReservationFromGoogleMapper(),
            new ReservationToGoogleMapper()
        };

        var source = new Model.Reservation
        {
            Id = 1,
            FirstName = "Will",
            LastName = "Smith",
            Email = "test@example.com",
            Phone = "+123456789",
            CheckInDate = new DateTime(2025, 6, 20),
            CheckOutDate = new DateTime(2025, 6, 25),
            TotalNumberOfGuests = 2,
            TotalPrice = 500,
            ReservationCode = "12345"
        };
        
        var sut = new MapHandler(mappers);

        // Act
        var result = sut.Map(source, "Model.Reservation", "Google.Reservation");
        
        // Assert
        result.Should().BeOfType<Google.Reservation>();
        result.Should().NotBeNull();
        var model = (result as Google.Reservation)!;

        model.GuestFullName.Should().Be("Will Smith");
        model.CheckInDate.Should().Be(new DateTime(2025, 6, 20));
        model.CheckOutDate.Should().Be(new DateTime(2025, 6, 25));
        model.GuestEmail.Should().Be("test@example.com");
        model.GuestPhoneNumber.Should().Be("+123456789");
        model.Code.Should().Be("12345");
        model.NumberOfGuests.Should().Be(2);
        model.TotalPrice.Should().Be(500);
    }

    [Fact]
    public void Should_Return_DIRS21Reservation_WithValidArguments()
    {

        // Arrange
        var mappers = new List<IMapper>
        {
            new ReservationFromGoogleMapper(),
            new ReservationToGoogleMapper()
        };

        var source = new Reservation
        {
            CheckInDate = new DateTime(2025, 6, 20),
            CheckOutDate = new DateTime(2025, 6, 25),
            GuestFullName = "Will Smith",
            GuestEmail = "test@example.com",
            GuestPhoneNumber = "+1234567890",
            NumberOfGuests = 3,
            Code = "1234",
            HotelName = "Hilton",
            TotalPrice = 1000
        };

        var sut = new MapHandler(mappers);

        // Act
        var result = sut.Map(source, "Google.Reservation", "Model.Reservation");

        // Assert
        result.Should().BeOfType<Model.Reservation>();
        result.Should().NotBeNull();
        var model = (result as Model.Reservation)!;
        model.FirstName.Should().Be("Will");
        model.LastName.Should().Be("Smith");
        model.CheckInDate.Should().Be(new DateTime(2025, 6, 20));
        model.CheckOutDate.Should().Be(new DateTime(2025, 6, 25));
        model.Email.Should().Be("test@example.com");
        model.Phone.Should().Be("+1234567890");
        model.ReservationCode.Should().Be("1234");
        model.TotalNumberOfGuests.Should().Be(3);
        model.TotalPrice.Should().Be(1000);
    }

    [Fact]
    public void Should_Throw_CastException_With_InvalidSource()
    {
        // Arrange
        var mappers = new List<IMapper>
        {
            new ReservationFromGoogleMapper(),
            new ReservationToGoogleMapper()
        };

        var source = new Model.Reservation
        {
            Id = 1,
            FirstName = "Will",
            LastName = "Smith",
            Email = "test@example.com",
            Phone = "+123456789",
            CheckInDate = new DateTime(2025, 6, 20),
            CheckOutDate = new DateTime(2025, 6, 25),
            TotalNumberOfGuests = 2,
            TotalPrice = 500,
            ReservationCode = "12345"
        };

        var sut = new MapHandler(mappers);

        // Act
        Action act = () => sut.Map(source, "Google.Reservation", "Model.Reservation");
        
        act.Should().Throw<InvalidCastException>().WithMessage("Source object is not type of 'Google.Reservation'");
    }

    [Fact]
    public void Should_Throw_InvalidOperationException_With_WithInvalidSource()
    {
        // Arrange
        var mappers = new List<IMapper>
        {
            new ReservationFromGoogleMapper(),
            new ReservationToGoogleMapper()
        };

        var source = new Model.Reservation
        {
            Id = 1,
            FirstName = "Will",
            LastName = "Smith",
            Email = "test@example.com",
            Phone = "+123456789",
            CheckInDate = new DateTime(2025, 6, 20),
            CheckOutDate = new DateTime(2025, 6, 25),
            TotalNumberOfGuests = 2,
            TotalPrice = 500,
            ReservationCode = "12345"
        };

        var sut = new MapHandler(mappers);

        // Act
        Action act = () => sut.Map(source, "Invalid.Reservation", "Model.Reservation");

        act.Should().Throw<InvalidOperationException>().WithMessage("No mapper found for Invalid.Reservation -> Model.Reservation");
    }

    [Fact]
    public void Should_Throw_InvalidOperationException_With_WithInvalidSource2()
    {
        // Arrange
        var mappers = new List<IMapper>
        {
            new ReservationFromGoogleMapper(),
            new ReservationToGoogleMapper()
        };

        var source = new Reservation
        {
            CheckInDate = new DateTime(2025, 6, 20),
            CheckOutDate = new DateTime(2025, 6, 25),
            GuestFullName = "Will Smith",
            GuestEmail = "test@example.com",
            GuestPhoneNumber = "+1234567890",
            NumberOfGuests = 3,
            Code = "1234",
            TotalPrice = 1000
        };

        var sut = new MapHandler(mappers);

        // Act
        Action act = () => sut.Map(source, "Google.Reservation", "Invalid.Reservation");

        act.Should().Throw<InvalidOperationException>().WithMessage("No mapper found for Google.Reservation -> Invalid.Reservation");
    }
}

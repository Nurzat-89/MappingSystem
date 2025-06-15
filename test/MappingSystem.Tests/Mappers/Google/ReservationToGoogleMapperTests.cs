using System.ComponentModel.DataAnnotations;
using FluentAssertions;
using MappingSystem.Mappers.Reservations;

namespace MappingSystem.Tests.Mappers.Google;

public class ReservationToGoogleMapperTests
{
    private readonly ReservationToGoogleMapper _mapper = new();

    [Fact]
    public void Should_MapTo_Google_From_DIRS21_Reservation()
    {
        // Arrange
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

        // Act
        var result = _mapper.Map(source);

        // Assert
        result.Should().NotBeNull();
        result.GuestFullName.Should().Be("Will Smith");
        result.GuestEmail.Should().Be(source.Email);
        result.Code.Should().Be(source.ReservationCode);
        result.CheckInDate.Should().Be(source.CheckInDate);
        result.CheckOutDate.Should().Be(source.CheckOutDate);
        result.NumberOfGuests.Should().Be(source.TotalNumberOfGuests);
        result.TotalPrice.Should().Be(source.TotalPrice);
    }

    [Theory]
    [InlineData(null, "Smith", "test@example.com", "12345", 1, "Guest first name is required.")]
    [InlineData("Will", null, "test@example.com", "12345", 1, "Guest last name is required.")]
    [InlineData("Will", "Smith", null, "12345", 1, "Guest email name is required.")]
    [InlineData("Will", "Smith", "test@example.com", null, 1, "ReservationCode is required.")]
    [InlineData("Will", "Smith", "test@example.com", "12345", 0,
        "The total number of guests must be at least 1 person.")]
    public void Should_Throw_ValidationException_When_InvalidFields(
        string firstName, string lastName, string email, string reservationCode, int guests, string expectedMessage)
    {
        // Arrange
        var reservation = new Model.Reservation
        {
            Id = 1,
            FirstName = firstName,
            LastName = lastName,
            Email = email,
            ReservationCode = reservationCode,
            TotalNumberOfGuests = guests,
            CheckInDate = new DateTime(2025, 6, 20),
            CheckOutDate = new DateTime(2025, 6, 25),
            TotalPrice = 500,
        };

        // Act
        Action act = () => _mapper.Map(reservation);

        // Assert
        act.Should().Throw<ValidationException>().WithMessage(expectedMessage);
    }

    [Fact]
    public void Map_InvalidDates_ThrowsValidationException()
    {
        // Arrange
        var source = new Model.Reservation
        {
            Id = 1,
            FirstName = "Will",
            LastName = "Smith",
            Email = "test@example.com",
            Phone = "+123456789",
            CheckInDate = new DateTime(2025, 6, 20),
            CheckOutDate = new DateTime(2025, 6, 15),
            TotalNumberOfGuests = 2,
            TotalPrice = 500,
            ReservationCode = "12345"
        };

        // Act
        Action act = () => _mapper.Map(source);

        // Assert
        act.Should().Throw<ValidationException>().WithMessage("Check-out must be after check-in.");
    }
}

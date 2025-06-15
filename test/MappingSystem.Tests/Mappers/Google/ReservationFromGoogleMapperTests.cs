using FluentAssertions;
using MappingSystem.Mappers.Reservations;
using System.ComponentModel.DataAnnotations;
using Google;

namespace MappingSystem.Tests.Mappers.Google;

public class ReservationFromGoogleMapperTests
{
    private readonly ReservationFromGoogleMapper _mapper = new();

    [Fact]
    public void Should_MapTo_DIRS21_From_Google_Reservation()
    {
        var model = new Reservation
        {
            CheckInDate = new DateTime(2025, 6, 20),
            CheckOutDate = new DateTime(2025, 6, 25),
            GuestFullName = "User1 User2",
            GuestEmail = "test@example.com",
            GuestPhoneNumber = "+1234567890",
            NumberOfGuests = 3,
            Code = "1234",
            HotelName = "Hilton",
            TotalPrice = 1000
        };

        Model.Reservation result = _mapper.Map(model);

        result.FirstName.Should().Be("User1");
        result.LastName.Should().Be("User2");
        result.Email.Should().Be(model.GuestEmail);
        result.Phone.Should().Be(model.GuestPhoneNumber);
        result.ReservationCode.Should().Be(model.Code);
        result.CheckOutDate.Should().Be(model.CheckOutDate);
        result.CheckInDate.Should().Be(model.CheckInDate);
        result.TotalPrice.Should().Be(model.TotalPrice);
        result.TotalNumberOfGuests.Should().Be(model.NumberOfGuests);
    }

    [Theory]
    [InlineData(null, "email@test.com", "Hotel", "ABC", "Missing name")]
    [InlineData("John Doe", null, "Hotel", "ABC", "Missing email")]
    [InlineData("John Doe", "email@test.com", null, "ABC", "Missing hotel name")]
    [InlineData("John Doe", "email@test.com", "Hotel", null, "Missing reservation code")]
    public void Validate_InvalidFields_ThrowsValidationException(
        string fullName, string email, string hotel, string code, string reason)
    {
        // Arrange
        var reservation = new Reservation
        {
            GuestFullName = fullName,
            GuestEmail = email,
            HotelName = hotel,
            Code = code,
            CheckInDate = DateTime.Today,
            CheckOutDate = DateTime.Today.AddDays(1)
        };

        // Act
        Action act = () => _mapper.Validate(reservation);

        // Assert
        act.Should().Throw<ValidationException>().WithMessage("*");
    }

    [Fact]
    public void Validate_InvalidDateRange_ThrowsValidationException()
    {
        // Arrange
        var reservation = new Reservation
        {
            GuestFullName = "Jane Doe",
            GuestEmail = "jane@example.com",
            HotelName = "Test Hotel",
            Code = "XYZ987",
            CheckInDate = new DateTime(2025, 6, 25),
            CheckOutDate = new DateTime(2025, 6, 20)
        };

        // Act
        Action act = () => _mapper.Validate(reservation);

        // Assert
        act.Should().Throw<ValidationException>().WithMessage("Check-out must be after check-in.");
    }

}

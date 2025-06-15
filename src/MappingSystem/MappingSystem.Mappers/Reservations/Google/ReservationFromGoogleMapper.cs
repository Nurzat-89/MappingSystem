using MappingSystem.Mappers.Base;
using MappingSystem.Mappers.Utils;
using System.ComponentModel.DataAnnotations;

namespace MappingSystem.Mappers.Reservations;

public class ReservationFromGoogleMapper : MapperBase<Google.Reservation, Model.Reservation>
{
    public override Model.Reservation Map(Google.Reservation source)
    {
        Validate(source);

        return new Model.Reservation
        {
            CheckInDate = source.CheckInDate,
            CheckOutDate = source.CheckOutDate,
            Email = source.GuestEmail,
            Phone = source.GuestPhoneNumber,
            TotalNumberOfGuests = source.NumberOfGuests,
            FirstName = NameParser.ParseFullName(source.GuestFullName).FirstName,
            LastName = NameParser.ParseFullName(source.GuestFullName).LastName,
            ReservationCode = source.Code,
            TotalPrice = source.TotalPrice,
        };
    }

    public override bool Validate(Google.Reservation source)
    {
        if (string.IsNullOrWhiteSpace(source.GuestFullName))
            throw new ValidationException("Guest full name is required.");

        if (string.IsNullOrWhiteSpace(source.GuestEmail))
            throw new ValidationException("Guest email name is required.");
        
        if (string.IsNullOrWhiteSpace(source.HotelName))
            throw new ValidationException("Hotel name is required.");
        
        if (string.IsNullOrWhiteSpace(source.Code))
            throw new ValidationException("Reservation Code is required.");

        if (source.CheckInDate >= source.CheckOutDate)
            throw new ValidationException("Check-out must be after check-in.");

        return true;
    }
}
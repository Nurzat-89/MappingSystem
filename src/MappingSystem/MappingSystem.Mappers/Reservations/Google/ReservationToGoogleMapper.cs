using MappingSystem.Mappers.Base;
using System.ComponentModel.DataAnnotations;

namespace MappingSystem.Mappers.Reservations;

public class ReservationToGoogleMapper : MapperBase<Model.Reservation, Google.Reservation>
{
    public override Google.Reservation Map(Model.Reservation source)
    {
        Validate(source);

        return new Google.Reservation
        {
            Code = source.ReservationCode,
            GuestFullName = $"{source.FirstName} {source.LastName}",
            GuestEmail = source.Email,
            GuestPhoneNumber = source.Phone,
            CheckOutDate = source.CheckOutDate,
            CheckInDate = source.CheckInDate,
            NumberOfGuests = source.TotalNumberOfGuests,
            TotalPrice = source.TotalPrice,
        };
    }

    public override bool Validate(Model.Reservation source)
    {
        if (string.IsNullOrWhiteSpace(source.FirstName))
            throw new ValidationException("Guest first name is required.");

        if (string.IsNullOrWhiteSpace(source.LastName))
            throw new ValidationException("Guest last name is required.");
        
        if (string.IsNullOrWhiteSpace(source.Email))
            throw new ValidationException("Guest email name is required.");

        if (source.TotalNumberOfGuests < 1)
            throw new ValidationException("The total number of guests must be at least 1 person.");

        if (string.IsNullOrWhiteSpace(source.ReservationCode))
            throw new ValidationException("ReservationCode is required.");

        if (source.CheckInDate >= source.CheckOutDate)
            throw new ValidationException("Check-out must be after check-in.");

        return true;
    }
}
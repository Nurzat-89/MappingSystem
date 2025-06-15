namespace Model;

/// <summary>
/// Represents a reservation entity containing details about a booking.
/// </summary>
public class Reservation
{
    public int Id { get; set; }
    public string ReservationCode { get; set; } = null!;
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Country { get; set; } = null!;
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public DateTime CheckInDate { get; set; }
    public DateTime CheckOutDate { get; set; }
    public int TotalNumberOfGuests { get; set; }
    public decimal TotalPrice { get; set; }
}
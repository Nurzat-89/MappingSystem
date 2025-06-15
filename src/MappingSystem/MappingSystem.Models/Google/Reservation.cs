namespace Google;

/// <summary>
/// Represents the data model for a Google reservation
/// </summary>
public class Reservation
{
    public string Code { get; set; } = null!;
    public string HotelName { get; set; } = null!;
    public string GuestFullName { get; set; } = null!;
    public string? GuestEmail { get; set; }
    public string? GuestPhoneNumber { get; set; }
    public DateTime CheckInDate { get; set; }
    public DateTime CheckOutDate { get; set; }
    public int NumberOfGuests { get; set; }
    public decimal TotalPrice { get; set; }
}
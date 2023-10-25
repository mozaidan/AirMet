using Microsoft.AspNetCore.Identity;

namespace AirMet.Models;
public class Customer 
{
    public virtual string CustomerId { get; set; } = string.Empty;
    public virtual string Name { get; set; } = string.Empty;
    public virtual string? Age { get; set; }
    public virtual string? Address { get; set; }
    public virtual string? PhoneNumber { get; set; }
    public virtual string Email { get; set; } = string.Empty;
    public virtual int? ReservationId { get; set; }
    // Add other properties as needed

    // Navigation property
    public virtual List<Reservation>? Reservations { get; set; }
    public virtual List<Property> Properties { get; set; } = new List<Property>();
    public virtual IdentityUser? IdentityUser { get; set; }
    
}


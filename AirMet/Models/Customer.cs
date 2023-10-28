using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace AirMet.Models;
public class Customer 
{
    // Unique identifier for a Customer
    public virtual string CustomerId { get; set; } = string.Empty;

    // Name of the Customer
    [RegularExpression(@"[0-9a-zA-ZæøåÆØÅ. \-]{2,20}", ErrorMessage = "The Name must be numbers or letters and between 2 to 20 characters.")]
    public virtual string Name { get; set; } = string.Empty;

    // Age of the Customer (Optional)
    public virtual string? Age { get; set; }

    // Address of the Customer (Optional)
    public virtual string? Address { get; set; }

    // Phone number of the Customer (Optional)
    public virtual string? PhoneNumber { get; set; }

    // Email of the Customer
    public virtual string Email { get; set; } = string.Empty;

    // Reservation ID if available
    public virtual int? ReservationId { get; set; }

    // Navigation property
    public virtual List<Reservation>? Reservations { get; set; }
    public virtual List<Property> Properties { get; set; } = new List<Property>();
    public virtual IdentityUser? IdentityUser { get; set; }
    
}


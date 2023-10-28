using System.ComponentModel.DataAnnotations;

namespace AirMet.Models
{
	public class Reservation
	{
        // Unique Identifier for a reservation
        public int ReservationId { get; set; }

        // Foreign key to the associated Property
        public int PropertyId { get; set; }

        // Foreign key to the associated User
        public string UserId { get; set; } = string.Empty;

        // Navigation property to the associated Property
        public virtual Customer Customer { get; set; } = default!;

        // Start date of the reservation
        public DateTime StartDate { get; set; }

        // End date of the reservation
        public DateTime EndDate { get; set; }

        // Number of guests for the reservation
        [Range(1, int.MaxValue, ErrorMessage = "Number of guests should be at least 1")]
        public int NumberOfGuests { get; set; }

        // Total price for the reservation
        public decimal TotalPrice { get; set; }

        // Total days of the reservation
        [Range(1, int.MaxValue, ErrorMessage = "Total days should be at least 1")]
        public int TotalDays { get; set; }

        // Navigation property to the associated Property
        public virtual Property Property { get; set; } = default!;
    }
}


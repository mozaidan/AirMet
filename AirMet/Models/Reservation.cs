using System;
using System.ComponentModel.DataAnnotations;

namespace AirMet.Models
{
	public class Reservation
	{
        public int ReservationId { get; set; }
        public int PropertyId { get; set; }
        [Required]
        public string? UserId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int NumberOfGuests { get; set; }
        //public DateTime Date { get; internal set; }
        public decimal TotalPrice { get; set; }
        public int TotalDays { get; set; }
        public virtual Property? Property { get; set; }
    }
}


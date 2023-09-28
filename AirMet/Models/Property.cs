using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AirMet.Models
{
	public class Property
	{
		public int PropertyId { get; set; }
		public decimal Price { get; set; }
        public string Address { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
		public string? ImageUrl { get; set; }
		[NotMapped]
		public IFormFile? File { get; set; }
    }
}


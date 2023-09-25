using System;
namespace AirMet.Models
{
	public class Property
	{
		public int PropertyId { get; set; }
		public decimal Price { get; set; }
		public string Description { get; set; } = string.Empty;
		public string ImageUrl { get; set; } = string.Empty;

	}
}


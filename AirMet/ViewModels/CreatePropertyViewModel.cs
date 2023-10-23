using System;
using Microsoft.AspNetCore.Mvc.Rendering;
using AirMet.Models;
namespace AirMet.ViewModels
{
	public class CreatePropertyViewModel
	{
		public Property Property { get; set; } = default!;
		public List<SelectListItem> PTypeSelectList { get; set; } = default!;
		public List<Amenity> Amenities { get; set; } = default!;
	}
}


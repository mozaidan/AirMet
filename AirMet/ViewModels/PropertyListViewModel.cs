
using System;
using System.Collections.Generic;
using AirMet.Models;

namespace AirMet.ViewModels
{
	public class PropertyListViewModel
	{
        public IEnumerable<Property> Properties;
        public string? CurrenViewName;

        public PropertyListViewModel(IEnumerable<Property> properties, string? currentViewName)
        {
            Properties = properties;
            CurrenViewName = currentViewName;
        }
	}
}

using AirMet.Models;


namespace AirMet.ViewModels
{
	public class PropertyListViewModel
	{
        public IEnumerable<Property>? Properties;
        public string? CurrenViewName;
        public Customer? CustomerInfo;

        public PropertyListViewModel(IEnumerable<Property>? properties, string? currentViewName, Customer? customerInfo)
        {
            Properties = properties;
            CurrenViewName = currentViewName;
            CustomerInfo = customerInfo;
        }
	}
}
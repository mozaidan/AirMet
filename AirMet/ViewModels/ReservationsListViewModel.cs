namespace AirMet.Models
{
    public class ReservationsListViewModel
    {
        public List<Reservation>? Reservations;
        public string? CurrenViewName;
        public Customer? CustomerInfo;
        public ReservationsListViewModel(List<Reservation>? reservations, string? currentViewName, Customer? customerInfo)
        {
            Reservations = reservations;
            CurrenViewName = currentViewName;
            CustomerInfo = customerInfo;
        }

    }
}
using System;
using AirMet.Models;
namespace AirMet.Models
{
    public class ReservationsListViewModel
    {
        public List<Reservation> Reservations;
<<<<<<< HEAD
        public ReservationsListViewModel(List<Reservation> reservations)
        {
            Reservations = reservations;
=======
        public string? CurrenViewName;
        public ReservationsListViewModel(List<Reservation> reservations, string? currentViewName)
        {
            Reservations = reservations;
            CurrenViewName = currentViewName;
>>>>>>> 86b410a596466e0daea38b2558ff038226c5088f
        }

    }
}
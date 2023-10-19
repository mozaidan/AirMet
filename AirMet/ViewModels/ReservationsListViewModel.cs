using System;
using AirMet.Models;
namespace AirMet.Models
{
    public class ReservationsListViewModel
    {
        public List<Reservation> Reservations;
        public string? CurrenViewName;
        public ReservationsListViewModel(List<Reservation> reservations, string? currentViewName)
        {
            Reservations = reservations;
            CurrenViewName = currentViewName;
        }

    }
}
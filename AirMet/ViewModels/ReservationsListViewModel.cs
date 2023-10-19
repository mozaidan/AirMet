using System;
using AirMet.Models;
namespace AirMet.Models
{
    public class ReservationsListViewModel
    {
        public List<Reservation> Reservations;
        public ReservationsListViewModel(List<Reservation> reservations)
        {
            Reservations = reservations;
        }

    }
}
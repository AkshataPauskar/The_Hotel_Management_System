using Hotel_Management_System.Models;

namespace Hotel_Management_System.Services
{
    public interface IReservationsService
    {
        List<Reservation> GetAllReservations();
        Reservation GetReservationById(string id);
        Reservation AddNewReservation(Reservation bookingData);
        void UpdateReservationDetails(string id, Reservation bookingDetails);
        void DeleteReservation(string id);
    }
}

using Hotel_Management_System.Models;
using MongoDB.Driver;

namespace Hotel_Management_System.Services
{
    public class ReservationsService : IReservationsService
    {
        private readonly IMongoCollection<Reservation> _reservations;
        public ReservationsService(IHotelManagementDatabaseSettings settings, IMongoClient mongoClient)
        {
            var database = mongoClient.GetDatabase(settings.DatabaseName);
            _reservations = database.GetCollection<Reservation>("reservations");
        }

        public Reservation AddNewReservation(Reservation bookingData)
        {
            _reservations.InsertOne(bookingData);
            return bookingData;
        }

        public void DeleteReservation(string id)
        {
            _reservations.DeleteOne(reservation => reservation.Id == id);
        }

        public List<Reservation> GetAllReservations()
        {
            return _reservations.Find(reservation => true).ToList();
        }

        public Reservation GetReservationById(string id)
        {
            return _reservations.Find(reservation => reservation.Id == id).FirstOrDefault();
        }

        public void UpdateReservationDetails(string id, Reservation bookingDetails)
        {
            _reservations.ReplaceOne(reservation => reservation.Id == id, bookingDetails);
        }
    }
}

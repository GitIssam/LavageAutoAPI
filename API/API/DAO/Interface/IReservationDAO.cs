using API.Models;

namespace API.DAO.Interface
{
    public interface IReservationDAO
    {
        // Récupère toutes les réservations
        List<Reservation> GetAllReservation();

        // Récupère une réservation par son ID
        Reservation GetReservationById(int id);

        // Ajoute une nouvelle réservation et retourne l'ID généré
        int CreateReservation(Reservation reservation);

        // Met à jour une réservation existante et retourne un booléen indiquant si l'opération a réussi
        bool UpdateReservation(int id, Reservation reservation);

        // Supprime une réservation par son ID et retourne un booléen indiquant si l'opération a réussi
        bool DeleteReservation(int id);

        List<Reservation> GetReservationsByUserId(int id);

        List<Reservation> GetReservationsByUserByDay(DateTime date, int? userId);

        List<Reservation> GetReservationsByDay(DateTime date);

        List<Reservation> GetReservationsByMonth(DateTime month);

        List<Reservation> GetReservationsByUserByMonth(int userId, DateTime month);

    }
}

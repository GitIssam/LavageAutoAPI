using API.Models;

namespace API.DTO.Reservation
{
    public class ReservationDto
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string ClientName { get; set; }
        public string ClientEmail { get; set; }
        public string ClientPhone { get; set; }
        public string CarType { get; set; }
        public int ReservationTypeId { get; set; } // Nom du type de réservation
        public string ReservationTypeName { get; set; } // Ajout pour le nom du type
        public DateTime ReservedAt { get; set; }
    }

}

namespace API.Models
{
    public class Reservation
    {
        public int Id { get; set; } // Correspond à la colonne id
        public DateTime Date { get; set; } // Correspond à la colonne date
        public int SlotId { get; set; } // Correspond à la colonne slot_id
        public int UserId { get; set; } // Correspond à la colonne user_id
        //public User User { get; set; } // Navigation property pour User
        public string ClientName { get; set; } // Correspond à la colonne client_name
        public string ClientEmail { get; set; } // Correspond à la colonne client_email
        public string ClientPhone { get; set; } // Correspond à la colonne client_phone
        public string CarType { get; set; } // Correspond à la colonne car_type
        public DateTime ReservedAt { get; set; } = DateTime.Now; // Correspond à la colonne reserved_at avec une valeur par défaut
        public int ReservationTypeId { get; set; } // Correspond à la colonne reservation_type_id
        public string ReservationTypeName { get; set; } // Ajout pour le nom du type

    }
}

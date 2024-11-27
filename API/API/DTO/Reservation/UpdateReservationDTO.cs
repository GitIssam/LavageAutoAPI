namespace API.DTO.Reservation
{
    public class UpdateReservationDto
    {
        public DateTime Date { get; set; }
        public int SlotId { get; set; }
        public int UserId { get; set; }
        public string ClientName { get; set; }
        public string ClientEmail { get; set; }
        public string ClientPhone { get; set; }
        public string CarType { get; set; }
        public DateTime ReservedAt { get; set; }
        public int ReservationTypeId { get; set; }
    }

}

using API.DTO.Reservation;
using API.Models;

namespace API.Mappers
{
    public static class ReservationMapper
    {
        public static ReservationDto ReservationToDto(Reservation reservation)
        {
            return new ReservationDto
            {
                Id = reservation.Id,
                Date = reservation.Date,
                ClientName = reservation.ClientName,
                ClientEmail = reservation.ClientEmail,
                ClientPhone = reservation.ClientPhone,
                CarType = reservation.CarType,
                ReservationTypeId = reservation.ReservationTypeId,
                ReservationTypeName = reservation.ReservationTypeName,
                ReservedAt = reservation.ReservedAt
            };
        }

        public static UpdateReservationDto UpdateReservationToDto(Reservation reservation)
        {
            return new UpdateReservationDto
            {
                Date = reservation.Date,
                SlotId = reservation.SlotId,
                UserId = reservation.UserId,
                ClientName = reservation.ClientName,
                ClientEmail = reservation.ClientEmail,
                ClientPhone = reservation.ClientPhone,
                CarType = reservation.CarType,
                ReservedAt = reservation.ReservedAt,
                ReservationTypeId = reservation.ReservationTypeId
            };
        }

        public static Reservation DtoToReservation(UpdateReservationDto dto)
        {
            return new Reservation
            {
                Date = dto.Date,
                SlotId = dto.SlotId,
                UserId = dto.UserId,
                ClientName = dto.ClientName,
                ClientEmail = dto.ClientEmail,
                ClientPhone = dto.ClientPhone,
                CarType = dto.CarType,
                ReservedAt = dto.ReservedAt,
                ReservationTypeId = dto.ReservationTypeId
            };
        }

    }
}

using API.DAO.Interface;
using API.Models;
using API.DTO; // Assurez-vous que votre DTO est dans ce namespace
using API.Mappers; // Assurez-vous que le mapper est dans ce namespace
using Microsoft.AspNetCore.Mvc;
using API.DTO.Reservation;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReservationController : ControllerBase
    {
        private readonly IReservationDAO _reservationDAO;

        public ReservationController(IReservationDAO reservationDAO)
        {
            _reservationDAO = reservationDAO;
        }

        [HttpGet]
        public ActionResult<List<ReservationDto>> GetReservations()
        {
            try
            {
                var reservations = _reservationDAO.GetAllReservation();
                var reservationDtos = reservations.Select(ReservationMapper.ReservationToDto).ToList();
                return Ok(reservationDtos);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Une erreur est survenue : {ex.Message}");
            }
        }

        [HttpGet("{id:int}")]
        public ActionResult<ReservationDto> GetReservationById(int id)
        {
            try
            {
                var reservation = _reservationDAO.GetReservationById(id);

                if (reservation == null)
                {
                    return NotFound($"Aucune réservation trouvée avec l'ID {id}.");
                }

                var reservationDto = ReservationMapper.ReservationToDto(reservation);
                return Ok(reservationDto);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Une erreur est survenue : {ex.Message}");
            }
        }

        [HttpPost]
        public ActionResult<int> CreateReservation([FromBody] Reservation reservation)
        {
            try
            {
                var createdReservationId = _reservationDAO.CreateReservation(reservation);
                return CreatedAtAction(nameof(GetReservations), new { id = createdReservationId }, createdReservationId);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Une erreur est survenue : {ex.Message}");
            }
        }

        [HttpDelete("{id:int}")]
        public ActionResult DeleteReservation(int id)
        {
            try
            {
                var isDeleted = _reservationDAO.DeleteReservation(id);

                if (isDeleted)
                {
                    return NoContent();
                }
                else
                {
                    return NotFound($"Aucune réservation trouvée avec l'ID {id}.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Une erreur est survenue : {ex.Message}");
            }
        }

        [HttpGet("user/{userId:int}")]
        public ActionResult<List<ReservationDto>> GetUserReservations(int userId)
        {
            try
            {
                // Récupère les réservations de l'utilisateur via le DAO
                var userReservations = _reservationDAO.GetReservationsByUserId(userId);

                if (userReservations == null || !userReservations.Any())
                {
                    return NotFound($"Aucune réservation trouvée pour l'utilisateur avec l'ID {userId}.");
                }

                // Mappe les modèles en DTO
                var reservationDtos = userReservations
                    .Select(ReservationMapper.ReservationToDto)
                    .ToList();

                // Retourne les DTOs avec un code 200 (OK)
                return Ok(reservationDtos);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Une erreur est survenue : {ex.Message}");
            }
        }

        [HttpGet("user/{userId:int}/day/{date}")]
        public ActionResult<List<ReservationDto>> GetReservationsByUserByDay(DateTime date, [FromQuery] int userId)
        {
            try
            {
                // Récupère les réservations pour la date donnée (et optionnellement l'utilisateur)
                var reservations = _reservationDAO.GetReservationsByUserByDay(date, userId);

                if (reservations == null || !reservations.Any())
                {
                    return NotFound($"Aucune réservation trouvée pour la date {date:yyyy-MM-dd}.");
                }

                // Mappe les modèles en DTO
                var reservationDtos = reservations
                    .Select(ReservationMapper.ReservationToDto)
                    .ToList();

                return Ok(reservationDtos);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Une erreur est survenue : {ex.Message}");
            }
        }

        [HttpGet("day/{date}")]
        public ActionResult<List<ReservationDto>> GetReservationsByDay(DateTime date)
        {
            try
            {
                // Récupère les réservations pour une date donnée
                var reservations = _reservationDAO.GetReservationsByDay(date);

                if (reservations == null || !reservations.Any())
                {
                    return NotFound($"Aucune réservation trouvée pour la date {date:yyyy-MM-dd}.");
                }

                // Mappe les modèles en DTO
                var reservationDtos = reservations
                    .Select(ReservationMapper.ReservationToDto)
                    .ToList();

                return Ok(reservationDtos);
            }
            catch (Exception ex)
            {
                // Gère les exceptions et retourne une erreur 500
                return StatusCode(StatusCodes.Status500InternalServerError, $"Une erreur est survenue : {ex.Message}");
            }
        }




        [HttpPut("{id:int}")]
        public ActionResult UpdateReservation(int id, [FromBody] UpdateReservationDto updateDto)
        {
            try
            {
                if (updateDto == null)
                {
                    return BadRequest("Les données de la réservation sont manquantes.");
                }

                // Convertit le DTO en modèle Reservation
                var reservation = ReservationMapper.DtoToReservation(updateDto);

                // Appelle la méthode DAO pour mettre à jour la réservation
                var isUpdated = _reservationDAO.UpdateReservation(id, reservation);

                if (isUpdated)
                {
                    return NoContent();
                }
                else
                {
                    return NotFound($"Aucune réservation trouvée avec l'ID {id}.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Une erreur est survenue : {ex.Message}");
            }
        }

    }

}

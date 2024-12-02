using API.DAO;
using API.DAO.Interface;
using API.DTO.NewFolder;
using API.DTO.User;
using API.Models;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using System;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IUserDAO _userDAO;

        public AuthController(IUserDAO userDAO)
        {
            _userDAO = userDAO;
        }

        // Endpoint pour l'authentification
        [HttpPost("login")]
        public ActionResult<User> Login([FromBody] LoginDTO loginDTO)
        {
            if (loginDTO == null || string.IsNullOrEmpty(loginDTO.Login) || string.IsNullOrEmpty(loginDTO.Password))
            {
                return BadRequest("Email et mot de passe sont requis.");
            }

            User user = _userDAO.Authenticate(loginDTO.Login, loginDTO.Password);

            // Si l'utilisateur n'est pas trouvé, retourne une réponse non autorisée
            if (user == null)
            {
                return Unauthorized("Email ou mot de passe incorrect.");
            }

            return Ok(user);
        }

        [HttpPost("create")]
        public ActionResult CreateUser([FromBody] CreateUserDTO userDto)
        {
            try
            {
                // Mapper le DTO en modèle User
                var user = new User
                {
                    FirstName = userDto.FirstName,
                    LastName = userDto.LastName,
                    Login = userDto.Login,
                    Password = userDto.Password
                };

                var isCreated = _userDAO.CreateUser(user);

                if (isCreated)
                {
                    return CreatedAtAction(nameof(CreateUser), new { login = userDto.Login }, null);
                }
                else
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "Impossible de créer l'utilisateur.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Une erreur est survenue : {ex.Message}");
            }
        }

    }
}

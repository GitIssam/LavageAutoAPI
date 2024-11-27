using API.DAO.Interface;
using API.Models;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;

namespace API.DAO
{
    public class UserDAO : IUserDAO
    {
        private readonly string _connectionString;

        public UserDAO(string connectionString)
        {
            _connectionString = connectionString;
        }

        private string HashPassword(string password)
        {
            // Création d'un objet SHA-256 pour le hashage
            using (SHA256 sha256 = SHA256.Create())
            {
                // Conversion du mot de passe en tableau de bytes et calcul du hash
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));

                // Conversion du tableau de bytes en chaîne hexadécimale
                StringBuilder builder = new StringBuilder();
                foreach (byte b in bytes)
                {
                    builder.Append(b.ToString("x2")); // Format hexadécimal
                }

                return builder.ToString(); // Retourne le mot de passe hashé
            }
        }

        public User? Authenticate(string username, string password)
        {
            // Hache le mot de passe pour la comparaison
            string hashedPassword = HashPassword(password);

            User? user = null;

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                // Prépare la requête SQL
                SqlCommand cmd = new SqlCommand(
                    "SELECT id, first_name, last_name, login " +
                    "FROM [User] " +
                    "WHERE login = @login AND password = @password", conn);

                // Ajoute les paramètres
                cmd.Parameters.AddWithValue("@login", username);
                cmd.Parameters.AddWithValue("@password", hashedPassword);

                // Exécute la requête et lit les résultats
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        // Crée une instance de l'utilisateur à partir des résultats
                        user = new User
                        {
                            Id = reader.GetInt32(0),
                            FirstName = reader.GetString(1),
                            LastName = reader.GetString(2),
                            Login = reader.GetString(3)
                        };
                    }
                }
            }

            return user;
        }

    }
}

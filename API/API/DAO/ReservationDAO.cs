using API.DAO.Interface;
using API.Models;
using System.Data.SqlClient;

namespace API.DAO
{
    public class ReservationDAO : IReservationDAO
    {
        private readonly string _connectionString;

        public ReservationDAO(string connectionString)
        {
            _connectionString = connectionString;
        }

        public int CreateReservation(Reservation reservation)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(
                    @"INSERT INTO Reservation 
                (date, slot_id, user_id, client_name, client_email, client_phone, car_type, reserved_at, reservation_type_id)
              VALUES
                (@Date, @SlotId, @UserId, @ClientName, @ClientEmail, @ClientPhone, @CarType, @ReservedAt, @ReservationTypeId);
              SELECT SCOPE_IDENTITY();", conn))
                {
                    // Ajout des paramètres
                    cmd.Parameters.AddWithValue("@Date", reservation.Date);
                    cmd.Parameters.AddWithValue("@SlotId", reservation.SlotId);
                    cmd.Parameters.AddWithValue("@UserId", reservation.UserId);
                    cmd.Parameters.AddWithValue("@ClientName", (object)reservation.ClientName ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@ClientEmail", (object)reservation.ClientEmail ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@ClientPhone", (object)reservation.ClientPhone ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@CarType", (object)reservation.CarType ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@ReservedAt", reservation.ReservedAt);
                    cmd.Parameters.AddWithValue("@ReservationTypeId", reservation.ReservationTypeId);

                    // Exécuter la commande et récupérer l'ID généré
                    return Convert.ToInt32(cmd.ExecuteScalar());
                }
            }
        }


        public bool DeleteReservation(int id)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(
                    @"DELETE FROM Reservation 
                WHERE id = @Id", conn))
                {
                    // Ajout du paramètre
                    cmd.Parameters.AddWithValue("@Id", id);

                    // Exécuter la commande et vérifier le nombre de lignes affectées
                    int rowsAffected = cmd.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
            }
        }


        public List<Reservation> GetAllReservation()
        {
            var list = new List<Reservation>();
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                // La commande SQL doit être exécutée dans le contexte de la connexion ouverte
                SqlCommand cmd = new SqlCommand(
                    "SELECT r.id, r.date, r.client_name, r.client_email, r.client_phone, r.car_type, r.reserved_at, r.slot_id, r.user_id, r.reservation_type_id, rt.name AS reservation_type_name FROM Reservation r LEFT JOIN ReservationType rt ON r.reservation_type_id = rt.id",
                    conn
                );

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(new Reservation
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("id")),
                            Date = reader.GetDateTime(reader.GetOrdinal("date")),
                            ClientName = reader.IsDBNull(reader.GetOrdinal("client_name")) ? null : reader.GetString(reader.GetOrdinal("client_name")),
                            ClientEmail = reader.IsDBNull(reader.GetOrdinal("client_email")) ? null : reader.GetString(reader.GetOrdinal("client_email")),
                            ClientPhone = reader.IsDBNull(reader.GetOrdinal("client_phone")) ? null : reader.GetString(reader.GetOrdinal("client_phone")),
                            CarType = reader.IsDBNull(reader.GetOrdinal("car_type")) ? null : reader.GetString(reader.GetOrdinal("car_type")),
                            ReservedAt = reader.GetDateTime(reader.GetOrdinal("reserved_at")),
                            SlotId = reader.GetInt32(reader.GetOrdinal("slot_id")),
                            UserId = reader.GetInt32(reader.GetOrdinal("user_id")),
                            ReservationTypeId = reader.GetInt32(reader.GetOrdinal("reservation_type_id")),
                            ReservationTypeName = reader.IsDBNull(reader.GetOrdinal("reservation_type_name")) ? null : reader.GetString(reader.GetOrdinal("reservation_type_name"))
                        });
                    }
                }
            }
            return list;
        }


        public Reservation? GetReservationById(int id)
        {
            Reservation? reservation = null;

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                // Création de la commande SQL avec une requête préparée
                SqlCommand cmd = new SqlCommand(
                    "SELECT id, date, client_name, client_email, client_phone, car_type, reserved_at, slot_id, user_id, reservation_type_id " +
                    "FROM Reservation WHERE id = @id",
                    conn
                );

                // Ajout du paramètre
                cmd.Parameters.AddWithValue("@id", id);

                // Exécution de la commande et lecture des résultats
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        reservation = new Reservation
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("id")),
                            Date = reader.GetDateTime(reader.GetOrdinal("date")),
                            ClientName = reader.GetString(reader.GetOrdinal("client_name")),
                            ClientEmail = reader.GetString(reader.GetOrdinal("client_email")),
                            ClientPhone = reader.GetString(reader.GetOrdinal("client_phone")),
                            CarType = reader.GetString(reader.GetOrdinal("car_type")),
                            ReservedAt = reader.GetDateTime(reader.GetOrdinal("reserved_at")),
                            SlotId = reader.GetInt32(reader.GetOrdinal("slot_id")),
                            UserId = reader.GetInt32(reader.GetOrdinal("user_id")),
                            ReservationTypeId = reader.GetInt32(reader.GetOrdinal("reservation_type_id"))
                        };
                    }
                }
            }

            return reservation;
        }

        public List<Reservation> GetReservationsByDay(DateTime date)
        {
            var reservations = new List<Reservation>();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                // Commande SQL pour récupérer les réservations selon une date donnée
                SqlCommand cmd = new SqlCommand(
                    @"SELECT id, date, client_name, client_email, client_phone, car_type, reserved_at, slot_id, user_id, reservation_type_id 
              FROM Reservation 
              WHERE CAST(date AS DATE) = @Date",
                    conn
                );

                // Ajoute la date en paramètre pour éviter les injections SQL
                cmd.Parameters.AddWithValue("@Date", date.Date);

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        reservations.Add(new Reservation
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("id")),
                            Date = reader.GetDateTime(reader.GetOrdinal("date")),
                            ClientName = reader.IsDBNull(reader.GetOrdinal("client_name")) ? null : reader.GetString(reader.GetOrdinal("client_name")),
                            ClientEmail = reader.IsDBNull(reader.GetOrdinal("client_email")) ? null : reader.GetString(reader.GetOrdinal("client_email")),
                            ClientPhone = reader.IsDBNull(reader.GetOrdinal("client_phone")) ? null : reader.GetString(reader.GetOrdinal("client_phone")),
                            CarType = reader.IsDBNull(reader.GetOrdinal("car_type")) ? null : reader.GetString(reader.GetOrdinal("car_type")),
                            ReservedAt = reader.GetDateTime(reader.GetOrdinal("reserved_at")),
                            SlotId = reader.GetInt32(reader.GetOrdinal("slot_id")),
                            UserId = reader.GetInt32(reader.GetOrdinal("user_id")),
                            ReservationTypeId = reader.GetInt32(reader.GetOrdinal("reservation_type_id"))
                        });
                    }
                }
            }

            return reservations;
        }


        public List<Reservation> GetReservationsByUserByDay(DateTime date, int? userId)
        {
            var list = new List<Reservation>();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                // Construisez la requête SQL
                var query = "SELECT id, date, client_name, client_email, client_phone, car_type, reserved_at, slot_id, user_id, reservation_type_id " +
                            "FROM Reservation WHERE CAST(date AS DATE) = @Date";

                // Ajoutez une clause pour filtrer par utilisateur si un `userId` est fourni
                if (userId.HasValue)
                {
                    query += " AND user_id = @UserId";
                }

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Date", date.Date);

                if (userId.HasValue)
                {
                    cmd.Parameters.AddWithValue("@UserId", userId.Value);
                }

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(new Reservation
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("id")),
                            Date = reader.GetDateTime(reader.GetOrdinal("date")),
                            ClientName = reader.IsDBNull(reader.GetOrdinal("client_name")) ? null : reader.GetString(reader.GetOrdinal("client_name")),
                            ClientEmail = reader.IsDBNull(reader.GetOrdinal("client_email")) ? null : reader.GetString(reader.GetOrdinal("client_email")),
                            ClientPhone = reader.IsDBNull(reader.GetOrdinal("client_phone")) ? null : reader.GetString(reader.GetOrdinal("client_phone")),
                            CarType = reader.IsDBNull(reader.GetOrdinal("car_type")) ? null : reader.GetString(reader.GetOrdinal("car_type")),
                            ReservedAt = reader.GetDateTime(reader.GetOrdinal("reserved_at")),
                            SlotId = reader.GetInt32(reader.GetOrdinal("slot_id")),
                            UserId = reader.GetInt32(reader.GetOrdinal("user_id")),
                            ReservationTypeId = reader.GetInt32(reader.GetOrdinal("reservation_type_id"))
                        });
                    }
                }
            }

            return list;
        }

        public List<Reservation> GetReservationsByUserId(int userId)
        {
            var list = new List<Reservation>();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                // Préparez une requête SQL pour récupérer les réservations d'un utilisateur spécifique
                SqlCommand cmd = new SqlCommand(
                    "SELECT id, date, client_name, client_email, client_phone, car_type, reserved_at, slot_id, user_id, reservation_type_id " +
                    "FROM Reservation WHERE user_id = @UserId",
                    conn
                );

                // Ajoutez le paramètre `UserId` pour éviter les injections SQL
                cmd.Parameters.AddWithValue("@UserId", userId);

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(new Reservation
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("id")),
                            Date = reader.GetDateTime(reader.GetOrdinal("date")),
                            ClientName = reader.IsDBNull(reader.GetOrdinal("client_name")) ? null : reader.GetString(reader.GetOrdinal("client_name")),
                            ClientEmail = reader.IsDBNull(reader.GetOrdinal("client_email")) ? null : reader.GetString(reader.GetOrdinal("client_email")),
                            ClientPhone = reader.IsDBNull(reader.GetOrdinal("client_phone")) ? null : reader.GetString(reader.GetOrdinal("client_phone")),
                            CarType = reader.IsDBNull(reader.GetOrdinal("car_type")) ? null : reader.GetString(reader.GetOrdinal("car_type")),
                            ReservedAt = reader.GetDateTime(reader.GetOrdinal("reserved_at")),
                            SlotId = reader.GetInt32(reader.GetOrdinal("slot_id")),
                            UserId = reader.GetInt32(reader.GetOrdinal("user_id")),
                            ReservationTypeId = reader.GetInt32(reader.GetOrdinal("reservation_type_id"))
                        });
                    }
                }
            }

            return list;
        }

        public bool UpdateReservation(int id, Reservation reservation)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                // Création de la commande SQL avec une requête préparée
                SqlCommand cmd = new SqlCommand(
                    "UPDATE Reservation " +
                    "SET date = @date, client_name = @clientName, client_email = @clientEmail, client_phone = @clientPhone, " +
                    "car_type = @carType, slot_id = @slotId, user_id = @userId, reservation_type_id = @reservationTypeId " +
                    "WHERE id = @id",
                    conn
                );

                // Ajout des paramètres
                cmd.Parameters.AddWithValue("@id", id);
                cmd.Parameters.AddWithValue("@date", reservation.Date);
                cmd.Parameters.AddWithValue("@clientName", reservation.ClientName);
                cmd.Parameters.AddWithValue("@clientEmail", reservation.ClientEmail);
                cmd.Parameters.AddWithValue("@clientPhone", reservation.ClientPhone);
                cmd.Parameters.AddWithValue("@carType", reservation.CarType);
                cmd.Parameters.AddWithValue("@slotId", reservation.SlotId);
                cmd.Parameters.AddWithValue("@userId", reservation.UserId);
                cmd.Parameters.AddWithValue("@reservationTypeId", reservation.ReservationTypeId);

                // Exécute la commande et retourne si une ligne a été affectée
                return cmd.ExecuteNonQuery() > 0;
            }
        }
    }
}

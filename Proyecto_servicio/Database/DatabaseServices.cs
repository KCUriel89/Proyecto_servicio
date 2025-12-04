using SQLite;
using System;
using System.IO;    
using System.Threading.Tasks;
using System.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;

namespace Proyecto_servicio.DataBase
{


    public abstract class ConnectionToSQL
    {
        private readonly string connectionString = "Server=DESKTOP-38IFLSE\\KCSQL;Database= moneki; integrated security=true";
        public ConnectionToSQL() { connectionString = "Server=DESKTOP-38IFLSE\\KCSQL;Database= moneki; integrated security=true"; }
        protected SqlConnection GetConnection()
        {
            return new SqlConnection(connectionString);
        }
    }
    public class DatabaseService : ConnectionToSQL
    {
        private readonly string connectionString= "Server=DESKTOP-38IFLSE\\KCSQL;Database= moneki; integrated security=true";

        // ---------- MÉTODO: Registrar Usuario ----------


        // ---------- MÉTODO: Verificar usuario por Username ----------
        public async Task<bool> UserExistsAsync(string email)
        {
            string q = "SELECT COUNT(*) FROM Usuarios WHERE Email = @e";

            using (SqlConnection con = GetConnection())
            using (SqlCommand cmd = new SqlCommand(q, con))
            {
                cmd.Parameters.AddWithValue("@e", email);

                await con.OpenAsync();
                int count = (int)await cmd.ExecuteScalarAsync();
                return count > 0;
            }
        }

        // ---------- MÉTODO: Login ----------
        public async Task<Dictionary<string, object>> LoginTrabajadorEmailAsync(string email, string password)
        {
            string q = @"SELECT * FROM Trabajadores 
                 WHERE Email = @e AND Password = @p";

            using (SqlConnection con = GetConnection())
            {
                await con.OpenAsync();

                using (SqlCommand cmd = new SqlCommand(q, con))
                {
                    cmd.Parameters.AddWithValue("@e", email);
                    cmd.Parameters.AddWithValue("@p", password);

                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            var result = new Dictionary<string, object>();

                            for (int i = 0; i < reader.FieldCount; i++)
                                result[reader.GetName(i)] = reader.GetValue(i);

                            return result;
                        }
                    }
                }
            }
            return null;
        }
        public async Task<Dictionary<string, object>> LoginAdminEmailAsync(string email, string password)
        {
            string q = @"SELECT * FROM Administradores 
                 WHERE EmailAdmin = @e AND PasswordA = @p";

            using (SqlConnection con = GetConnection())
            {
                await con.OpenAsync();

                using (SqlCommand cmd = new SqlCommand(q, con))
                {
                    cmd.Parameters.AddWithValue("@e", email);
                    cmd.Parameters.AddWithValue("@p", password);

                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            var result = new Dictionary<string, object>();

                            for (int i = 0; i < reader.FieldCount; i++)
                                result[reader.GetName(i)] = reader.GetValue(i);

                            return result;
                        }
                    }
                }
            }
            return null;
        }
        public async Task<Dictionary<string, object>> LoginUsuarioEmailAsync(string email, string password)
        {
            string q = @"SELECT * FROM Usuarios 
                 WHERE Email = @e AND Password = @p";

            using (SqlConnection con = GetConnection())
            {
                await con.OpenAsync();

                using (SqlCommand cmd = new SqlCommand(q, con))
                {
                    cmd.Parameters.AddWithValue("@e", email);
                    cmd.Parameters.AddWithValue("@p", password);

                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            var result = new Dictionary<string, object>();

                            for (int i = 0; i < reader.FieldCount; i++)
                                result[reader.GetName(i)] = reader.GetValue(i);

                            return result;
                        }
                    }
                }
            }
            return null;
        }



        // ---------- Helper ejecutor ----------
        public async Task<int> ExecuteAsync(string query, params SqlParameter[] parameters)
        {
            using (SqlConnection con = GetConnection())
            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                cmd.Parameters.AddRange(parameters);
                await con.OpenAsync();
                return await cmd.ExecuteNonQueryAsync();
            }
        }
        // ---------- MÉTODO: Registrar Usuario V2 ----------
        public async Task RegisterUserAsync(
        string nombre,
        string apP,
        string apM,
        string password,
        string email,
        string direccion,
        string telefono,
        DateTime fechaNacimiento,
        DateTime fechaRegistro
    )
        {
            using (SqlConnection conn = GetConnection())
            {
                await conn.OpenAsync();

                string query = @"INSERT INTO Usuarios
                    (NombreUsuario, ApellidoPaterno, ApellidoMaterno, Password, Email, 
                     DireccionUsuario, Telefono, FechaNacimiento, FechaRegistro)
                    VALUES
                    (@NombreUsuario, @ApP, @ApM, @Password, @Email, 
                     @DireccionUsuario, @Telefono, @FechaNacimiento, @FechaRegistro)";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@NombreUsuario", nombre);
                    cmd.Parameters.AddWithValue("@ApP", apP);
                    cmd.Parameters.AddWithValue("@ApM", apM);
                    cmd.Parameters.AddWithValue("@Password", password);
                    cmd.Parameters.AddWithValue("@Email", email);
                    cmd.Parameters.AddWithValue("@DireccionUsuario", direccion);
                    cmd.Parameters.AddWithValue("@Telefono", telefono);
                    cmd.Parameters.AddWithValue("@FechaNacimiento", fechaNacimiento);
                    cmd.Parameters.AddWithValue("@FechaRegistro", fechaRegistro);

                    await cmd.ExecuteNonQueryAsync();
                }
            }
        }
    }

}


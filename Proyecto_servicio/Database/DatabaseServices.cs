using SQLite;
using Proyecto_servicio.Models;
using System;
using System.IO;    
using System.Threading.Tasks;

namespace Proyecto_servicio.DataBase
{
    public class DatabaseService
    {
        private static SQLiteAsyncConnection _database;

        // Inicializa la base de datos y crea la tabla de usuarios si no existe
        public static async Task InitAsync()
        {
            if (_database != null)
                return;

            // Ruta local de la base de datos (por usuario del dispositivo)
            var dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "usuarios.db3");

            _database = new SQLiteAsyncConnection(dbPath);

            // Crear tabla de usuarios si no existe
            await _database.CreateTableAsync<User>();
        }

        // Inserta un nuevo usuario (registro)
        public static async Task<int> AddUserAsync(User user)
        {
            await InitAsync();
            return await _database.InsertAsync(user);
        }

        // Obtiene usuario por nombre de usuario
        public static async Task<User> GetUserByUsernameAsync(string username)
        {
            await InitAsync();
            return await _database.Table<User>().Where(u => u.Username == username).FirstOrDefaultAsync();
        }

        // Verifica login (usuario y contraseña)
        public static async Task<User> ValidateLoginAsync(string username, string password)
        {
            await InitAsync();
            return await _database.Table<User>()
                .Where(u => u.Username == username && u.Password == password)
                .FirstOrDefaultAsync();
        }

        // Elimina todos los usuarios (para pruebas o reinicio)
        public static async Task<int> DeleteAllUsersAsync()
        {
            await InitAsync();
            return await _database.DeleteAllAsync<User>();
        }
    }
}

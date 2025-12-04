
using Proyecto_servicio.DataBase;
namespace Proyecto_servicio.Paginas
{
    public partial class LoginPage : ContentPage
    {
        private readonly DatabaseService db = new DatabaseService();

        public LoginPage()
        {
            InitializeComponent();
        }

        private async void OnLoginClicked(object sender, EventArgs e)
        {
            string email = entryUsuario.Text;
            string password = entryPassword.Text;

            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
                await DisplayAlert("Error", "Ingresa email y contraseña.", "OK");
                return;
            }

            // 🔹 Buscar en tabla Usuarios
            var user = await db.LoginUsuarioEmailAsync(email, password);
            if (user != null)
            {
                await DisplayAlert("Bienvenido", $"Hola {user["Email"]}", "OK");
                await Navigation.PushAsync(new PaginaPrincipal());
                return;
            }

            // 🔹 Buscar en Administradores
            var admin = await db.LoginAdminEmailAsync(email, password);
            if (admin != null)
            {
                await DisplayAlert("Bienvenido", $"Hola Administrador {admin["Email"]}", "OK");
                await Navigation.PushAsync(new AdminPage());
                return;
            }

            // 🔹 Buscar en Trabajadores
            var trabajador = await db.LoginTrabajadorEmailAsync(email, password);
            if (trabajador != null)
            {
                await DisplayAlert("Bienvenido", $"Hola {trabajador["Email"]}", "OK");
                await Navigation.PushAsync(new ());
                return;
            }

            // ❌ No existe en ninguna tabla
            await DisplayAlert("Error", "Credenciales incorrectas.", "OK");
        }


        private async void OnRegisterClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new RegisterPage());
        }
    }
}
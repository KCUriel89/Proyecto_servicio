using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Proyecto_servicio.Services;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Storage;
using Database_prueba.DataBase;
using System.Threading.Tasks;
using System.Windows.Input;
using Paginas.Models;
using Proyecto_servicio.Paginas;

namespace Proyecto_servicio
{
    public partial class LoginViewModel : ObservableObject
    {
        // ?? Propiedades que se enlazan con la vista (XAML)
        [ObservableProperty]
        private string username;

        [ObservableProperty]
        private string password;

        // ?? Comando para iniciar sesión
        [RelayCommand]
        private async Task LoginAsync()
        {
            if (string.IsNullOrWhiteSpace(Username) || string.IsNullOrWhiteSpace(Password))
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Por favor ingresa usuario y contraseña.", "OK");
                return;
            }

            // Validar usuario en la base de datos
            var user = await DatabaseService.ValidateLoginAsync(Username, Password);

            if (user != null)
            {
                // Guardar sesión (opcional)
                await SecureStorage.SetAsync("usuario", user.Username);

                await Application.Current.MainPage.DisplayAlert("Bienvenido", $"Hola, {user.Nombre}", "Continuar");

                // Navegar a la página principal
                await Application.Current.MainPage.Navigation.PushAsync(new PaginaPrincipal());
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Usuario o contraseña incorrectos.", "OK");
            }
        }

        // ?? Comando para ir a la página de registro
        [RelayCommand]
        private async Task GoToRegisterAsync()
        {
            await Application.Current.MainPage.Navigation.PushAsync(new RegisterPage());
        }
    }
}

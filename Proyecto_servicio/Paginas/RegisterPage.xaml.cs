using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Storage;
using Database_prueba.DataBase;
using System.Threading.Tasks;
using System.Windows.Input;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Tablas.Models;
using System;
using Proyecto_servicio.Services;

namespace Paginas.Models
{
    public partial class RegisterViewModel : ObservableObject
    {
        [ObservableProperty] private string nombre;
        [ObservableProperty] private string apellidoPaterno;
        [ObservableProperty] private string apellidoMaterno;
        [ObservableProperty] private DateTime fechaNacimiento = DateTime.Now;
        [ObservableProperty] private string email;
        [ObservableProperty] private string username;
        [ObservableProperty] private string password;
        [ObservableProperty] private string confirmPassword;
        // Backing field for fingerprint token
        private string fingerprintToken;

        // Public property that raises notifications
        public string FingerprintToken
        {
            get => fingerprintToken;
            set => SetProperty(ref fingerprintToken, value);
        }

        public DateTime FechaMaxima => DateTime.Now;

        [RelayCommand]
        private async Task RegisterAsync()
        {
            if (string.IsNullOrWhiteSpace(Nombre) ||
                string.IsNullOrWhiteSpace(ApellidoPaterno) ||
                string.IsNullOrWhiteSpace(Email) ||
                string.IsNullOrWhiteSpace(Username) ||
                string.IsNullOrWhiteSpace(Password) ||
                string.IsNullOrWhiteSpace(ConfirmPassword))
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Por favor completa todos los campos.", "OK");
                return;
            }

            if (Password != ConfirmPassword)
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Las contraseñas no coinciden.", "OK");
                return;
            }

            // Verificar si el usuario ya existe
            var existingUser = await DatabaseService.GetUserByUsernameAsync(Username);
            if (existingUser != null)
            {
                await Application.Current.MainPage.DisplayAlert("Error", "El nombre de usuario ya está en uso.", "OK");
                return;
            }

            var user = new User
            {
                Nombre = Nombre,
                ApellidoPaterno = ApellidoPaterno,
                ApellidoMaterno = ApellidoMaterno,
                FechaNacimiento = FechaNacimiento,
                Email = Email,
                Username = Username,
                Password = Password,
                ConfirmPassword = ConfirmPassword,
                FingerprintToken = FingerprintToken
            };

            await DatabaseService.AddUserAsync(user);

            await Application.Current.MainPage.DisplayAlert("Éxito", "Usuario registrado correctamente.", "OK");

            // Redirigir al login
            await Shell.Current.GoToAsync("..");
        }

        [RelayCommand]
        private async Task GoToLoginAsync()
        {
            await Shell.Current.GoToAsync("..");
        }

        [RelayCommand]
        private async Task RegisterFingerprintAsync()
        {
            try
            {
                var biometric = new BiometricService();
                var isAuthenticated = await biometric.AuthenticateAsync();
                if (!isAuthenticated)
                {
                    await Application.Current.MainPage.DisplayAlert("Error", "Autenticación biométrica fallida o no disponible.", "OK");
                    return;
                }

                // Generate a token to represent the enrolled fingerprint for this user
                FingerprintToken = Guid.NewGuid().ToString();

                await Application.Current.MainPage.DisplayAlert("Éxito", "Huella registrada correctamente.", "OK");
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", $"No se pudo registrar la huella: {ex.Message}", "OK");
            }
        }

    }
}

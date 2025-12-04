using Proyecto_servicio.DataBase;
namespace Proyecto_servicio.Paginas;

public partial class RegisterPage : ContentPage
{
    private readonly DatabaseService db = new DatabaseService();

    public RegisterPage()
    {
        InitializeComponent();
        pickerFechaNacimiento.MaximumDate = DateTime.Now;
        pickerFechaNacimiento.MinimumDate = new DateTime(1950, 1, 1);
    }

    private async void OnRegisterClicked(object sender, EventArgs e)
    {
        string nombre = entryNombre.Text;
        string apP = entryApP.Text;
        string apM = entryApM.Text;
        string email = entryEmail.Text;
        string telefono = entryTelefono.Text;
        string direccion = entryDireccion.Text;

        string password = entryPassword.Text;
        string confirmPassword = entryConfirm.Text;

        DateTime fechaNacimiento = pickerFechaNacimiento.Date;
        DateTime fechaRegistro = DateTime.Now; // SE GUARDA AUTOMÁTICAMENTE

        if (string.IsNullOrWhiteSpace(nombre) ||
            string.IsNullOrWhiteSpace(apP) ||
            string.IsNullOrWhiteSpace(apM) ||
            string.IsNullOrWhiteSpace(email) ||
            string.IsNullOrWhiteSpace(telefono) ||
            string.IsNullOrWhiteSpace(password))
        {
            await DisplayAlert("Error", "Por favor completa todos los campos obligatorios.", "OK");
            return;
        }

        if (password != confirmPassword)
        {
            await DisplayAlert("Error", "Las contraseñas no coinciden.", "OK");
            return;
        }

        // Validar si ya existe usuario
        if (await db.UserExistsAsync(email))
        {
            await DisplayAlert("Error", "Este nombre de usuario ya está registrado.", "OK");
            return;
        }

        // Registrar en SQL (YA NO INCLUYE TipoUsuario)
        await db.RegisterUserAsync(
            nombre,
            apP,
            apM,
            password,
            email,
            direccion,
            telefono,
            fechaNacimiento,
            fechaRegistro
        );

        await DisplayAlert("Éxito", "Usuario registrado correctamente.", "OK");
        await Navigation.PopAsync();
    }
    private async void OnGoLoginClicked(object sender, EventArgs e) 
    { 
    await Navigation.PopAsync();
    }
}
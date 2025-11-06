using Proyecto_servicio.Models;

namespace Proyecto_servicio.Paginas;
public partial class RegisterPage : ContentPage
{
    public RegisterPage()
    {
        InitializeComponent();
        BindingContext = new RegisterViewModel();
    }
}
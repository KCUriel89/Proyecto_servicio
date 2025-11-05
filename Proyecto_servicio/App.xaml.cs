using Microsoft.Maui;
using Microsoft.Maui.Controls;

namespace Proyecto_servicio
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            // Set the Shell as the main page
            MainPage = new AppShell();

            // Ensure navigation runs after MainPage is set and on the UI thread
            MainPage.Dispatcher.Dispatch(async () =>
            {
                // Navigate to the LoginPage route (defined in AppShell.xaml)
                await Shell.Current.GoToAsync("//LoginPage");
            });
        }
    }
}
using Microsoft.Maui.Controls;

namespace Proyecto_servicio
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            // Run navigation after the Shell is ready on the UI thread
            Dispatcher.Dispatch(async () =>
            {
                // Use absolute route to ensure navigation goes to the root route
                await GoToAsync("//LoginPage");
            });
        }
    }
}

using Microsoft.Extensions.DependencyInjection;

namespace EcoSpotterPostPrototype
{
    public partial class App : Application
    {
        public App()
        {
            try
            {
                InitializeComponent();
            }
            catch (Exception ex)
            {
                // Handle the exception, e.g., log it or display an error message
                System.Diagnostics.Debug.WriteLine($"Error initializing components: {ex.Message}");
                throw;
            }
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            return new Window(new AppShell());
        }
    }
}
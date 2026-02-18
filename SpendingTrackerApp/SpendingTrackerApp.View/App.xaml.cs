using SpendingTrackerApp.AddShells;
using SpendingTrackerApp.Pages;

namespace SpendingTrackerApp
{
    public partial class App : Application
    {
        public App(IServiceProvider serviceProvider)
        {
            InitializeComponent();

		}


		protected override Window CreateWindow(IActivationState? activationState)
        {
            return new Window(new AppShellLogin());
        }
    }
}
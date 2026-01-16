using SpendingTrackerApp.Pages;
using SpendingTrackerApp.Pages.LoginPages;

namespace SpendingTrackerApp.AddShells
{
    public partial class AppShellLogin : Shell
    {
        public AppShellLogin()
        {
            InitializeComponent();

			Routing.RegisterRoute(nameof(CreateAcountPage), typeof(CreateAcountPage));
			Routing.RegisterRoute(nameof(ResetPasswordPage), typeof(ResetPasswordPage));

		}
    }
}

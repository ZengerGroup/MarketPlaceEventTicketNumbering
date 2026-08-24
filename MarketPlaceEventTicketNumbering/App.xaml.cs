using Microsoft.Extensions.DependencyInjection;

namespace MarketPlaceEventTicketNumbering
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            var window = new Window(new AppShell());
            window.Width = 1000;
            window.MinimumWidth = 1000;
            window.MaximumWidth = 1000;
            window.Height = 600;
            window.MinimumHeight = 600;
            window.MaximumHeight = 600;
            return window;
        }
    }
}
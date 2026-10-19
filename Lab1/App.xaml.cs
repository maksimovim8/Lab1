
using System.Diagnostics;

namespace Lab1
{
    public partial class App : Application
    {
        public App()
        {
            var savedTheme = Preferences.Get(
                "AppTheme",
                "Системна");

            UserAppTheme = savedTheme switch
            {
                "Світла" => AppTheme.Light,
                "Темна" => AppTheme.Dark,
                _ => AppTheme.Unspecified
            };

            InitializeComponent();
        }

        protected override Window CreateWindow(
            IActivationState? activationState)
        {
            var window = new Window(new AppShell());

            window.Stopped += OnWindowStopped;
            window.Resumed += OnWindowResumed;
            window.Destroying += OnWindowDestroying;

            return window;
        }

        private void OnWindowStopped(object? sender, EventArgs e)
        {
            Debug.WriteLine(
                $"[Lifecycle] Stopped: {DateTime.Now:HH:mm:ss}");
        }

        private void OnWindowResumed(object? sender, EventArgs e)
        {
            Debug.WriteLine(
                $"[Lifecycle] Resumed: {DateTime.Now:HH:mm:ss}");
        }

        private void OnWindowDestroying(object? sender, EventArgs e)
        {
            Debug.WriteLine(
                $"[Lifecycle] Destroying: {DateTime.Now:HH:mm:ss}");
        }
    }
}

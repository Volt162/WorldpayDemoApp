using System.Diagnostics;
using WpMAUIApp.Lib.Services;

namespace WpMAUIApp;

public partial class App : Application
{
	public App()
	{
		InitializeComponent();

		MainPage = new AppShell();

        Application.Current.UserAppTheme = AppTheme.Light;
        this.RequestedThemeChanged += (s, e) => { Application.Current.UserAppTheme = AppTheme.Light; };
    }

    protected override Window CreateWindow(IActivationState activationState)
    {
        Window window = base.CreateWindow(activationState);

        window.Destroying += (s, e) =>
        {
            Application.Current.Quit();
        };

        return window;
    }
}


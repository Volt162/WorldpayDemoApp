using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using Raygun4Maui;
using WpMAUIApp.Lib;
using WpMAUIApp.Pages;
using WpMAUIApp.ViewModels;

namespace WpMAUIApp;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
            .UseMauiCommunityToolkit()
            .UsePaymentLibrary()
            .AddRaygun(options => {
                options.RaygunSettings.ApiKey = "Tj32CF7AlAPSwtQJniKc9A";
                options.RaygunSettings.CatchUnhandledExceptions = true;
            })
            .ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});

#if DEBUG
		builder.Logging.AddDebug();
#endif
        builder.Services.AddTransientWithShellRoute<MainPage, MainPageViewModel>(nameof(MainPageViewModel));

        return builder.Build();
	}
}


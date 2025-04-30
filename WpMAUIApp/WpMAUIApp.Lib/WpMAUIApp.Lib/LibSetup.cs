using WpMAUIApp.Lib.Services;

namespace WpMAUIApp.Lib;

public static class LibSetup
{
    public static MauiAppBuilder UsePaymentLibrary(this MauiAppBuilder builder)
    {
        var services = builder.Services;

#if IOS
        services.AddSingleton<IPaymentSdkService, Platforms.iOS.Services.PaymentSdkService>();
#elif ANDROID
        services.AddSingleton<IPaymentSdkService, Platforms.Android.Services.PaymentSdkService>();
#endif

        return builder;
    }
}


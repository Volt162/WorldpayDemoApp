using Microsoft.Maui.Platform;
using WpMAUIApp.Utils;
using WpMAUIApp.ViewModels;

namespace WpMAUIApp.Pages;

public class BaseContentPage : ContentPage
{
    protected virtual string AndroidNavBarColor => string.Empty;
    public BaseContentPage()
    {
        Shell.SetNavBarIsVisible(this, false);
        this.SetBinding(TitleProperty, "Title");
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        if (BindingContext is BaseViewModel viewModel)
        {
            await viewModel.InitializeAsync();
        }

#if ANDROID
        if (string.IsNullOrEmpty(AndroidNavBarColor))
        {
            return;
        }

        var color = ResourcesHelper.TryGetResource<Color>(AndroidNavBarColor);
        if (color != null)
        {
            Platform.CurrentActivity?.Window?.SetNavigationBarColor(color.ToPlatform());
        }
#endif
    }
}
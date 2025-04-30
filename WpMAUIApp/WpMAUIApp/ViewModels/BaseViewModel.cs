using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Mvvm.ComponentModel;
using WpMAUIApp.Utils;

namespace WpMAUIApp.ViewModels;

public partial class BaseViewModel : ObservableObject
{
    [ObservableProperty]
    private bool isBusy;

    [ObservableProperty]
    private string busyMessage;

    [ObservableProperty]
    private bool _fullScreenLoaderVisible;

    [ObservableProperty]
    private string _title;

    public Task InitializeAsync() => OnInitializeAsync();

    public virtual async Task OnInitializeAsync() { }

    protected virtual void OnIsBusyChanged()
    {
    }

    protected void ShowLoader(string message)
    {
        BusyMessage = message ?? string.Empty;
        IsBusy = true;
    }

    protected void HideLoader()
    {
        IsBusy = false;
    }

    protected Task ShowSnackBarAsync(string message, bool success)
    {
        var options = GetSnackbarOptions(success);

        var snackBar = Snackbar.Make(message, () => { }, "", visualOptions: options, duration: TimeSpan.FromSeconds(3));
        return snackBar.Show();
    }

    private SnackbarOptions GetSnackbarOptions(bool success)
    {
        var result = new SnackbarOptions
        {
            BackgroundColor = ResourcesHelper.TryGetResource<Color>(success ? "Gray900" : "Gray900"),
            TextColor = ResourcesHelper.TryGetResource<Color>("BackgroundWhiteColor"),
            CornerRadius = 8
        };

        return result;
    }

    partial void OnIsBusyChanged(bool value)
    {
        OnIsBusyChanged();
    }
}

using Android;
using Android.App;
using Android.Content.PM;
using Android.OS;
using AndroidX.Core.App;

namespace WpMAUIApp;

[Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density, ScreenOrientation = ScreenOrientation.Portrait)]
public class MainActivity : MauiAppCompatActivity
{
    private const int PermissionsRequestCode = 1;

    protected override void OnCreate(Bundle savedInstanceState)
    {
        base.OnCreate(savedInstanceState);

        var permissions = new[]
        {
                Manifest.Permission.AccessFineLocation,
                Manifest.Permission.AccessCoarseLocation,
                Manifest.Permission.WriteExternalStorage,
                Manifest.Permission.Bluetooth,
                Manifest.Permission.BluetoothAdmin,
                Manifest.Permission.BluetoothConnect,
                Manifest.Permission.BluetoothScan
            };

        ActivityCompat.RequestPermissions(this, permissions, PermissionsRequestCode);
    }

    public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Permission[] grantResults)
    {
        base.OnRequestPermissionsResult(requestCode, permissions, grantResults);

        if (requestCode != PermissionsRequestCode)
        {
            return;
        }
    }
}


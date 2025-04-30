using Com.Paysimpleandroid.Paysimpleandroidsdk;

namespace WpMAUIApp.Lib.Platforms.Android.Utils;

internal static class StringExtensions
{
    public static PsEnvironment GetPsEnvironment(this string value)
    {
        return value switch
        {
            "SBX" => PsEnvironment.Sbx,
            "PROD" => PsEnvironment.Prod,
            _ => PsEnvironment.Sbx
        };
    }
}
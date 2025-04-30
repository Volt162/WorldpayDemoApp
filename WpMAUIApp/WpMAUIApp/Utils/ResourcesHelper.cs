namespace WpMAUIApp.Utils;

public static class ResourcesHelper
{
    public static T TryGetResource<T>(string resourceKey) where T : class
    {
        if (Application.Current.Resources.TryGetValue(resourceKey, out var resource))
        {
            return resource as T;
        }

        return null;
    }
}

using CommunityToolkit.Maui.Alerts;

namespace LinksStorage.Services;

public class BrowserLauncherService
{
    public async Task Open(string uri)
    {
        try
        {
            await Browser.OpenAsync(uri);
        }
        catch
        {
            await Toast.Make("Sorry can't open that link, try edit it").Show();
        }
    }
}
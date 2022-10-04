namespace LinksStorage.Services;

public class BrowserLauncherService
{
    public async ValueTask Open(string uri)
    {
        try
        {
            await Browser.OpenAsync(uri);
        }
        catch (Exception e)
        {
            
        }
    }
}
using System;
using System.ComponentModel;
using System.Threading.Tasks;
using PuppeteerSharp;

namespace TullaLLM.Tools;

public static class WebPageTool
{
    [Description("Get the HTML of the provided website.")]
    public static async Task<string> GetWebPage([Description("The web page to load")] string url)
    {
        try
        {
            AppContext.SetData("APP_CONTEXT_BASE_DIRECTORY", "C:\\Users\\ms\\AppData\\Roaming\\XIVLauncher\\pluginConfigs\\TullaLLM");
            var options = new LaunchOptions
            {
                Headless = true
            };
            await new BrowserFetcher().DownloadAsync();

            await using var browser = await Puppeteer.LaunchAsync(options);

            var page = await browser.NewPageAsync();
            await page.GoToAsync(url);
            await page.WaitForNetworkIdleAsync();

            return await page.GetContentAsync();
        }
        catch (Exception e) { }

        return "Page load failed.";
    }
}

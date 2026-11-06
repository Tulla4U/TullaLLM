using System.ComponentModel;
using PuppeteerSharp;

namespace TullaLLMLib.Tools;

public class WebPageTool(LlmSettings config)
{
    [Description("Get the HTML of the provided website.")]
    public async Task<string> GetWebPage([Description("The web page to load")] string url)
    {
        try
        {
            AppContext.SetData("APP_CONTEXT_BASE_DIRECTORY",  config.WorkingDirectory);
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
        catch
        {
            // ignored
        }

        return "Page load failed.";
    }
}

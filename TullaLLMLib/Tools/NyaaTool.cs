using System.ComponentModel;

namespace TullaLLMLib.Tools;

public class NyaaTool
{
    private const string BaseUrl = "https://nyaa.si/?f=0&c=1_2&q=";

    [Description("Search anime torrents on Nyaa.si")]
    public static async Task<string> GetNyaaTorrents([Description("Name of the anime to search")] string name)
    {
        using var client = new HttpClient();
        var response = await client.GetAsync($"{BaseUrl}{name}");
        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadAsStringAsync();
        }

        return "Could not load torrents.";
    }
}

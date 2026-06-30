using System.ComponentModel;
using CsAnilist.Models.Enums;
using CsAnilist.Models.Media;
using CsAnilist.Services;

namespace TullaLLMLib.Tools;

public class AniListTools
{
    private readonly CsAniListService client = new();

    [Description("Get a manga or anime by name.")]
    public async Task<AniMedia> GetAnimeOrManga(
        [Description("The name of the media to search")] string name,
        [Description("The type of media to search: anime or manga")] MediaType mediaType)
    {
        return await client.SearchMedia(name, mediaType, false);
    }

    [Description("Get trending anime or manga for the given media type.")]
    public async Task<List<AniMedia>> GetTrendingAnimeOrManga([Description("The type of media to search: anime or manga")] MediaType mediaType)
    {
        var resultPage = await client.GetAllMediaTrendsAsync(mediaType, 1, 20);
        return resultPage.media ?? [];
    }
}

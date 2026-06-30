using System.ComponentModel;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Agents.AI;

namespace TullaLLMLib.Tools;

public class WebSearchTool : IDisposable
{
    private readonly HttpClient client;
    public WebSearchTool(string searchApiToken)
    {
        client = new HttpClient();
        client.BaseAddress = new Uri("https://api.langsearch.com/v1/web-search");
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", searchApiToken);
    }

    [Description("Do a websearch for the given query.")]
    public async Task<string> DoSearch([Description("The query for the search")] string query)
    {
        try
        {
            var requestMessage = new HttpRequestMessage(HttpMethod.Post, "");
            requestMessage.Content =
                new StringContent(JsonSerializer.Serialize(new SearchPostBody { Query = query, Summary = true, Count = 10}),
                                  Encoding.UTF8, "application/json");
            var response = await client.SendAsync(requestMessage);
            var resultMessage = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                return resultMessage;
            }
        }
        catch (Exception ex)
        {
            // Plugin.Log.Error(ex.Message);
        }

        return "Search could not be executed";
    }
    
    public async Task<IEnumerable<TextSearchProvider.TextSearchResult>> SearchAdapter(string query, CancellationToken cancellationToken)
    {
        var response = await DoSearch(query);
        var parsed = JsonSerializer.Deserialize<SearchResult>(response);
        if (parsed is null)
        {
            return [];
        }
        return parsed.Data.WebPages.Value.Select(x => new TextSearchProvider.TextSearchResult
        {
            SourceLink = x.Url,
            SourceName = x.Name,
            Text = x.Summary
        });
    }

    public void Dispose()
    {
        client.Dispose();
    }
}

class SearchPostBody
{
    [JsonPropertyName("query")]
    public string Query { get; set; }
    [JsonPropertyName("summary")]
    public bool Summary { get; set; }
    [JsonPropertyName("count")]
    public int Count { get; set; }
}

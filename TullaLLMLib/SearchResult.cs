namespace TullaLLMLib;

using System.Collections.Generic;
using System.Text.Json.Serialization;

public class SearchResult
{
    [JsonPropertyName("code")]
    public int Code { get; set; }

    [JsonPropertyName("log_id")]
    public string LogId { get; set; }

    [JsonPropertyName("msg")]
    public object Msg { get; set; } 

    [JsonPropertyName("data")]
    public SearchResponse Data { get; set; }
}

public class SearchResponse
{
    // The search response structure is complex. It contains the query context and a container for web results info.
    [JsonPropertyName("_type")]
    public string Type { get; set; }

    [JsonPropertyName("queryContext")]
    public QueryContext QueryContext { get; set; }

    // This object holds metadata like webSearchUrl, totalEstimatedMatches, and the list of results ('value').
    [JsonPropertyName("webPages")] 
    public WebPagesInfo WebPages { get; set; } 
}

public class WebPagesInfo
{
    [JsonPropertyName("webSearchUrl")]
    public string WebSearchUrl { get; set; }

    [JsonPropertyName("totalEstimatedMatches")]
    public int? TotalEstimatedMatches { get; set; } 

    // This list holds the actual search results.
    [JsonPropertyName("value")]
    public List<WebViewPage> Value { get; set; } 
}


public class QueryContext
{
    [JsonPropertyName("originalQuery")]
    public string OriginalQuery { get; set; }
}

// Note: The keys and types for the fields must match the JSON exactly.

public class WebViewPage
{
    [JsonPropertyName("id")]
    public string Id { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("url")]
    public string Url { get; set; }

    [JsonPropertyName("displayUrl")]
    public string DisplayUrl { get; set; }

    [JsonPropertyName("snippet")]
    public string Snippet { get; set; }

    [JsonPropertyName("summary")]
    public string Summary { get; set; }

    // Fields that might be null/strings in the JSON
    [JsonPropertyName("datePublished")]
    public string DatePublished { get; set; } 

    [JsonPropertyName("dateLastCrawled")]
    public string DateLastCrawled { get; set; }
}

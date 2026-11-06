using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace TullaLLMLib;

[Serializable]
public class LlmSettings
{
    public string BaseUrl { get; set; } = "http://127.0.0.1:11434";
    public string Model { get; set; } = "gemma4:latest";
    public string LangSearchApiToken { get; set; } = "";
    [JsonIgnore]
    [Required]
    public string WorkingDirectory { get; init; }
}

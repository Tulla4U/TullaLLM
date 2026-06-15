using System;
using Dalamud.Configuration;

namespace TullaLLM;

[Serializable]
public class Configuration : IPluginConfiguration
{
    public int Version { get; set; } = 0;
    public LlmSettings LlmSettings { get; set; } = new();

    // The below exists just to make saving less cumbersome
    public void Save()
    {
        Plugin.PluginInterface.SavePluginConfig(this);
    }
}
[Serializable]
public class LlmSettings
{
    public string BaseUrl { get; set; } = "http://127.0.0.1:11434";
    public string Model { get; set; } = "gemma4:latest";
    public string LangSearchApiToken { get; set; } = "";
}

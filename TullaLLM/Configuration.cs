using System;
using System.IO;
using Dalamud.Configuration;
using Translate;
using TullaLLMLib;

namespace TullaLLM;

[Serializable]
public class Configuration : IPluginConfiguration
{
    public int Version { get; set; } = 0;
    public LlmSettings LlmSettings { get; set; } = new()
    {
        WorkingDirectory = Path.Combine(Plugin.PluginInterface.GetPluginConfigDirectory(), "TullaLLM")
    };

    public TranslationConfig TranslationConfig { get; set; } = new()
    {
        IncomingLanguage = Language.German,
        OutgoingLanguage = Language.English,
        OllamaUri = "http://127.0.0.1:11434",
        ModelName = "translategemma:4b"
    };

    // The below exists just to make saving less cumbersome
    public void Save()
    {
        Plugin.PluginInterface.SavePluginConfig(this);
    }
}

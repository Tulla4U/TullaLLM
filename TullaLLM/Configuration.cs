using System;
using Dalamud.Configuration;
using TullaLLMLib;

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

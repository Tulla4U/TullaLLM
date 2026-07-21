using System;
using System.Collections.Generic;
using System.Numerics;
using System.Threading.Tasks;
using Dalamud.Bindings.ImGui;
using Dalamud.Interface.Windowing;
using TullaLLMLib;
using TullaLLMLib.Util;

namespace TullaLLM.Windows;

public class ConfigWindow : Window, IDisposable
{
    private readonly Configuration configuration;
    private readonly AnswerBot answerBot;
    private List<string> availableModels = [];

    // We give this window a constant ID using ###.
    // This allows for labels to be dynamic, like "{FPS Counter}fps###XYZ counter window",
    // and the window ID will always be "###XYZ counter window" for ImGui
    public ConfigWindow(Plugin plugin, AnswerBot answerBot) : base("A Wonderful Configuration Window###With a constant ID")
    {
        Flags = ImGuiWindowFlags.NoCollapse;

        Size = new Vector2(300, 90);
        SizeCondition = ImGuiCond.FirstUseEver;

        configuration = plugin.Configuration;
        this.answerBot = answerBot;
        Task.Run(GetAvailableModels);
    }

    public void Dispose() { }

    public override void PreDraw()
    {
        // Flags must be added or removed before Draw() is being called, or they won't apply
    }

    public override void Draw()
    {
        // Can't ref a property, so use a local copy
        var baseUrl = configuration.LlmSettings.BaseUrl;
        if (ImGui.InputText("URL to your Ollama Endpoint", ref baseUrl))
        {
            configuration.LlmSettings.BaseUrl = baseUrl;
            configuration.Save();
            Task.Run(GetAvailableModels);
        }

        var selectedModelName = configuration.LlmSettings.Model;
        var selectedModelIndex = availableModels.IndexOf(selectedModelName);
        if (ImGui.Combo("Model", ref selectedModelIndex, availableModels) && selectedModelIndex >= 0)
        {
            selectedModelName = availableModels[selectedModelIndex];
            answerBot.SelectModel(selectedModelName);
            configuration.LlmSettings.Model = selectedModelName;
            configuration.Save();
        }

        var langSearchApiToken = configuration.LlmSettings.LangSearchApiToken;
        if (ImGui.InputText("Token for Lang Search API", ref langSearchApiToken))
        {
            configuration.LlmSettings.LangSearchApiToken = langSearchApiToken;
            configuration.Save();
        }
    }

    private async Task GetAvailableModels()
    {
        if (string.IsNullOrEmpty(configuration.LlmSettings.BaseUrl)) return;
        availableModels = await LlmTools.GetAvailableModels(new Uri(configuration.LlmSettings.BaseUrl));
    }
}

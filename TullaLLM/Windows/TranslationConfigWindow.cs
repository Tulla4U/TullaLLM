using System;
using Dalamud.Bindings.ImGui;
using Dalamud.Interface.Windowing;
using Translate;

namespace TullaLLM.Windows;

public class TranslationConfigWindow : Window, IDisposable
{
    private readonly Configuration configuration;

    public TranslationConfigWindow(Configuration configuration) : base("Translation Config", ImGuiWindowFlags.None)
    {
        this.configuration = configuration;
    }

    public void Dispose()
    {
        // TODO release managed resources here
    }

    public override void Draw()
    {
        var translateConfig = configuration.TranslationConfig;
        var ollamaUrl = translateConfig.OllamaUri;
        if (ImGui.InputText("URL to your Ollama Endpoint", ref ollamaUrl))
        {
            translateConfig.OllamaUri = ollamaUrl;
            configuration.Save();
        }
        
        var modelName = translateConfig.ModelName;
        if (ImGui.InputText("Translation model", ref modelName))
        {
            translateConfig.ModelName = modelName;
            configuration.Save();
        }

        var languageOptions = typeof(Language).GetEnumNames();

        var incomingLanguage = languageOptions.IndexOf(translateConfig.IncomingLanguage.ToString());
        if (ImGui.Combo("Translate incoming to", ref incomingLanguage, languageOptions) && Enum.TryParse(languageOptions[incomingLanguage], out Language selectedIncomingLanguage))
        {
            translateConfig.IncomingLanguage = selectedIncomingLanguage;
            configuration.Save();
        }
        
        var outgoingLanguage = languageOptions.IndexOf(translateConfig.OutgoingLanguage.ToString());
        if (ImGui.Combo("Translate outgoing to", ref outgoingLanguage, languageOptions) && Enum.TryParse(languageOptions[outgoingLanguage], out Language selectedOutgoingLanguage))
        {
            translateConfig.OutgoingLanguage = selectedOutgoingLanguage;
            configuration.Save();
        }
    }
}

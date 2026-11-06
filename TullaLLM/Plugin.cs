using Dalamud.Game.Command;
using Dalamud.Interface.Windowing;
using Dalamud.IoC;
using Dalamud.Plugin;
using Dalamud.Plugin.Services;
using Translate;
using TullaLLM.Util;
using TullaLLM.Windows;
using TullaLLMLib;

namespace TullaLLM;

public sealed class Plugin : IDalamudPlugin
{
    [PluginService] internal static IDalamudPluginInterface PluginInterface { get; private set; } = null!;
    [PluginService] internal static ICommandManager CommandManager { get; private set; } = null!;
    [PluginService] internal static IPluginLog Log { get; private set; } = null!;    
    [PluginService] internal static IChatGui ChatGui { get; private set; } = null!;

    [PluginService] internal static IObjectTable ObjectTable { get; private set; } = null!;

    private const string QueryCommand = "/llm";
    private const string ToggleConfigCommand = "/llm-cfg";
    private const string TranslateCommand = "/tl";
    private const string TranslateConfigCommand = "/tl-cfg";

    public Configuration Configuration { get; init; }

    public readonly WindowSystem WindowSystem = new("Tulla LLM");
    private ConfigWindow ConfigWindow { get; init; }
    private TranslationConfigWindow TranslationConfigWindow { get; init; }
    
    private AnswerBot AnswerBot { get; init; }
    private TranslationHook Translator { get; init; }

    public Plugin()
    {
        Configuration = PluginInterface.GetPluginConfig() as Configuration ?? new Configuration();
        var chatInteractor = new ChatInteractor();
        
        AnswerBot = new AnswerBot(Configuration.LlmSettings, chatInteractor);

        ConfigWindow = new ConfigWindow(this, AnswerBot);
        TranslationConfigWindow = new TranslationConfigWindow(Configuration);

        Translator = new TranslationHook(Configuration.TranslationConfig, chatInteractor, "Undefined");

        WindowSystem.AddWindow(ConfigWindow);
        WindowSystem.AddWindow(TranslationConfigWindow);

        CommandManager.AddHandler(QueryCommand, new CommandInfo(OnCommand)
        {
            HelpMessage = "Send queries to your LLM"
        });
        CommandManager.AddHandler(ToggleConfigCommand, new CommandInfo(OnCommand)
        {
            HelpMessage = "Open llm config"
        });
        CommandManager.AddHandler(TranslateCommand, new CommandInfo(OnCommand)
        {
            HelpMessage = "Translate text into the configured 'outgoing' Language and sends it via /say"
        });
        CommandManager.AddHandler(TranslateConfigCommand, new CommandInfo(OnCommand)
        {
            HelpMessage = "Translation Configuration"
        });

        // Tell the UI system that we want our windows to be drawn through the window system
        PluginInterface.UiBuilder.Draw += WindowSystem.Draw;

        // This adds a button to the plugin installer entry of this plugin which allows
        // toggling the display status of the configuration ui
        PluginInterface.UiBuilder.OpenConfigUi += ToggleConfigUi;
    }

    public void Dispose()
    {
        // Unregister all actions to not leak anything during disposal of plugin
        PluginInterface.UiBuilder.Draw -= WindowSystem.Draw;
        PluginInterface.UiBuilder.OpenConfigUi -= ToggleConfigUi;
        
        WindowSystem.RemoveAllWindows();

        ConfigWindow.Dispose();
        TranslationConfigWindow.Dispose();
        AnswerBot.Dispose();

        CommandManager.RemoveHandler(QueryCommand);
        CommandManager.RemoveHandler(ToggleConfigCommand);
        CommandManager.RemoveHandler(TranslateCommand);
        CommandManager.RemoveHandler(TranslateConfigCommand);
    }

    private void OnCommand(string command, string args)
    {
        switch (command)
        {
            case QueryCommand: 
                AnswerBot.Query(args);
                break;
            case ToggleConfigCommand:
                ToggleConfigUi();
                break;
            case TranslateCommand:
                if (args.Length == 0)
                {
                    Translator.OnMessageReceived();
                }
                else
                {
                    Translator.OnMessageSent(args);
                }
                break;
            case TranslateConfigCommand:
                ToggleTranslateConfigUi();
                break;
        }
    }
    
    public void ToggleConfigUi() => ConfigWindow.Toggle();
    public void ToggleTranslateConfigUi() => TranslationConfigWindow.Toggle();
}

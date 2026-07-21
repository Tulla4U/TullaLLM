using Dalamud.Game.Command;
using Dalamud.Interface.Windowing;
using Dalamud.IoC;
using Dalamud.Plugin;
using Dalamud.Plugin.Services;
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

    private const string QueryCommand = "/llm";
    private const string ToggleConfigCommand = "/llm-cfg";

    public Configuration Configuration { get; init; }

    public readonly WindowSystem WindowSystem = new("Tulla LLM");
    private ConfigWindow ConfigWindow { get; init; }
    
    private AnswerBot AnswerBot { get; init; }

    public Plugin()
    {
        Configuration = PluginInterface.GetPluginConfig() as Configuration ?? new Configuration();
        
        AnswerBot = new AnswerBot(Configuration.LlmSettings, new ChatInteractor());

        ConfigWindow = new ConfigWindow(this, AnswerBot);

        WindowSystem.AddWindow(ConfigWindow);

        CommandManager.AddHandler(QueryCommand, new CommandInfo(OnCommand)
        {
            HelpMessage = "Send queries to your LLM"
        });
        CommandManager.AddHandler(ToggleConfigCommand, new CommandInfo(OnCommand)
        {
            HelpMessage = "Open llm config"
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

        CommandManager.RemoveHandler(QueryCommand);
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
        }
    }
    
    public void ToggleConfigUi() => ConfigWindow.Toggle();
}

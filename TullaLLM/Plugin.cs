using System.IO;
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

    private const string CommandName = "/llm";

    public Configuration Configuration { get; init; }

    public readonly WindowSystem WindowSystem = new("Tulla LLM");
    private ConfigWindow ConfigWindow { get; init; }
    
    private AnswerBot AnswerBot { get; init; }

    public Plugin()
    {
        Configuration = PluginInterface.GetPluginConfig() as Configuration ?? new Configuration();

        ConfigWindow = new ConfigWindow(this);
        
        AnswerBot = new AnswerBot(Configuration.LlmSettings, new ChatInteractor());

        WindowSystem.AddWindow(ConfigWindow);

        CommandManager.AddHandler(CommandName, new CommandInfo(OnCommand)
        {
            HelpMessage = "Send queries to your LLM using /llm"
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

        CommandManager.RemoveHandler(CommandName);
    }

    private void OnCommand(string command, string args)
    {
        switch (command)
        {
            case CommandName: 
                AnswerBot.Query(args);
                break;
        }
    }
    
    public void ToggleConfigUi() => ConfigWindow.Toggle();
}

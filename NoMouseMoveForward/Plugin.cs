using Dalamud.Game.Command;
using Dalamud.Interface.Windowing;
using Dalamud.IoC;
using Dalamud.Plugin;
using Dalamud.Plugin.Services;
using NoMouseMoveForward.Hooks;
using NoMouseMoveForward.Windows;

namespace NoMouseMoveForward;

public sealed class Plugin : IDalamudPlugin
{
    [PluginService] internal static IDalamudPluginInterface PluginInterface { get; private set; } = null!;
    [PluginService] internal static ICommandManager CommandManager { get; private set; } = null!;
    [PluginService] internal static IGameInteropProvider GameInteropProvider { get; private set; } = null!;

    internal Configuration Configuration { get; init; }
    
    public readonly WindowSystem WindowSystem = new("NoMouseMoveForward");
    private ConfigWindow ConfigWindow { get; init; }

    private const string ConfigCommand = "/nomousemoveforward";
    
    internal readonly NoLmbRmbWalking NoLmbRmbWalking = new();
    internal readonly NoLmbRmbFlying NoLmbRmbFlying = new();

    public Plugin()
    {
        Configuration = PluginInterface.GetPluginConfig() as Configuration ?? new Configuration();
        
        ConfigWindow = new ConfigWindow(this);
        WindowSystem.AddWindow(ConfigWindow);

        CommandManager.AddHandler(ConfigCommand, new CommandInfo((_, _) => ConfigWindow.Toggle())
        {
            HelpMessage = "Open the settings window."
        });
        
        // Tell the UI system that we want our windows to be drawn through the window system
        PluginInterface.UiBuilder.Draw += WindowSystem.Draw;

        // This adds a button to the plugin installer entry of this plugin which allows
        // toggling the display status of the configuration ui
        PluginInterface.UiBuilder.OpenConfigUi += ToggleConfigUi;

        NoLmbRmbWalking.Toggle(Configuration.DisableForWalking);
        NoLmbRmbFlying.Toggle(Configuration.DisableForFlying);
    }

    public void Dispose()
    {   
        PluginInterface.UiBuilder.OpenConfigUi -= ToggleConfigUi;
        PluginInterface.UiBuilder.Draw -= WindowSystem.Draw;
        
        WindowSystem.RemoveAllWindows();
        ConfigWindow.Dispose();

        CommandManager.RemoveHandler(ConfigCommand);
        
        NoLmbRmbFlying.Dispose();
        NoLmbRmbWalking.Dispose();
    }
    
    public void ToggleConfigUi() => ConfigWindow.Toggle();
}

using System;
using System.Numerics;
using Dalamud.Bindings.ImGui;
using Dalamud.Interface.Windowing;

namespace NoMouseMoveForward.Windows;

public class ConfigWindow : Window, IDisposable
{
    private readonly Plugin plugin;
    
    public ConfigWindow(Plugin plugin) : base("NoMouseMoveForward Configuration")
    {
        this.plugin = plugin;

        SizeConstraints = new WindowSizeConstraints
        {
            MinimumSize = new Vector2(375, 100),
            MaximumSize = new Vector2(float.MaxValue, float.MaxValue)
        };
        SizeCondition = ImGuiCond.FirstUseEver;
    }

    public void Dispose()
    {
        GC.SuppressFinalize(this);
    }

    public override void Draw()
    {
        var disableForWalking = plugin.Configuration.DisableForWalking;

        if (ImGui.Checkbox("Disable LMB+RMB to move forward when walking", ref disableForWalking))
        {
            plugin.Configuration.DisableForWalking = disableForWalking;
            plugin.Configuration.Save();

            plugin.NoLmbRmbWalking.Toggle(disableForWalking);
        }

        var disableForFlying = plugin.Configuration.DisableForFlying;

        if (ImGui.Checkbox("Disable LMB+RMB to move forward when flying", ref disableForFlying))
        {
            plugin.Configuration.DisableForFlying = disableForFlying;
            plugin.Configuration.Save();
            
            plugin.NoLmbRmbFlying.Toggle(disableForFlying);
        }
    }
}

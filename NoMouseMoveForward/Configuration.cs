using System;
using Dalamud.Configuration;

namespace NoMouseMoveForward;

[Serializable]
public class Configuration : IPluginConfiguration
{
    public int Version { get; set; } = 0;

    public bool DisableForWalking { get; set; } = true;
    public bool DisableForFlying { get; set; } = false;
    
    public void Save()
    {
        Plugin.PluginInterface.SavePluginConfig(this);
    }
}

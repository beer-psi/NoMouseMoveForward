using System;
using System.Collections.Generic;
using System.Linq;
using Dalamud.Hooking;

namespace NoMouseMoveForward.Hooks;

public class AsmHookBase : IDisposable
{
    protected static readonly byte[] TestRspRsp = [0x48, 0x85, 0xE4];
    protected readonly List<AsmHook> Hooks = [];

    protected AsmHook AddHook(nint address, byte[] assembly, string hookName, AsmHookBehaviour hookBehaviour = AsmHookBehaviour.ExecuteFirst)
    {
        var hook = new AsmHook(address, assembly, hookName, hookBehaviour);
        
        Hooks.Add(hook);
        return hook;
    }

    public void Enable()
    {
        foreach (var hook in Hooks)
            hook.Enable();
    }

    public bool Enabled => Hooks.All(h => h.IsEnabled);

    public void Disable()
    {
        foreach (var hook in Hooks)
            hook.Disable();
    }

    public void Toggle(bool enabled)
    {
        var wasEnabled = Enabled;

        if (enabled && !wasEnabled)
            Enable();
        else if (!enabled && wasEnabled)
            Disable();
    }
    
    public void Dispose()
    {
        foreach (var hook in Hooks)
        {
            hook.Disable();
            hook.Dispose();
        }
        
        GC.SuppressFinalize(this);
    }
}

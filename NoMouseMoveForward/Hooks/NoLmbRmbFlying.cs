using System;
using Dalamud.Hooking;
using Dalamud.Utility.Signatures;
using FFXIVClientStructs.FFXIV.Client.Game.Control;

namespace NoMouseMoveForward.Hooks;

public unsafe class NoLmbRmbFlying : HookBase
{
    [Signature("E8 ?? ?? ?? ?? 0F B6 0D ?? ?? ?? ?? B8", DetourName = nameof(RmiFlyDetour))]
    private readonly Hook<RmiFlyDelegate> rmiFlyHook = null!;
    private delegate void RmiFlyDelegate(void* self, void* result);
    
    public NoLmbRmbFlying()
    {
        Plugin.GameInteropProvider.InitializeFromAttributes(this);
    }

    public override void Dispose()
    {
        rmiFlyHook.Disable();
        rmiFlyHook.Dispose();
        GC.SuppressFinalize(this);
    }

    protected override void Enable()
    {
        rmiFlyHook.Enable();
    }

    protected override void Disable()
    {
        rmiFlyHook.Disable();
    }

    protected override bool Enabled => rmiFlyHook.IsEnabled;

    private void RmiFlyDetour(void* self, void* result)
    {
        var im = InputManager.Instance();
        var previousMouseState = im->MouseButtonHoldStateRaw;

        if (previousMouseState == 3)  // LMB+RMB
            im->MouseButtonHoldStateRaw = 2;
        
        rmiFlyHook.Original(self, result);
        im->MouseButtonHoldStateRaw = previousMouseState;
    }
}

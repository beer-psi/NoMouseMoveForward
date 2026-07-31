using System;
using Dalamud.Hooking;
using Dalamud.Utility.Signatures;
using FFXIVClientStructs.FFXIV.Client.Game.Control;

namespace NoMouseMoveForward.Hooks;

public unsafe class NoLmbRmbWalking : HookBase
{
    [Signature("E8 ?? ?? ?? ?? 80 7B 3E 00 48 8D 3D", DetourName = nameof(RmiWalkDetour))]
    private readonly Hook<RmiWalkDelegate> rmiWalkHook = null!;
    private delegate void RmiWalkDelegate(void* self, float* sumLeft, float* sumForward, float* sumTurnLeft, byte* haveBackwardOrStrafe, byte* a6, byte bAdditiveUnk);
    
    public NoLmbRmbWalking()
    {
        Plugin.GameInteropProvider.InitializeFromAttributes(this);
    }
    
    public override void Dispose()
    {
        rmiWalkHook.Disable();
        rmiWalkHook.Dispose();
        GC.SuppressFinalize(this);
    }

    protected override void Enable()
    {
        rmiWalkHook.Enable();
    }

    protected override void Disable()
    {
        rmiWalkHook.Disable();
    }

    protected override bool Enabled => rmiWalkHook.IsEnabled;

    private void RmiWalkDetour(void* self, float* sumLeft, float* sumForward, float* sumTurnLeft, byte* haveBackwardOrStrafe, byte* a6, byte bAdditiveUnk)
    {
        var im = InputManager.Instance();
        var previousMouseState = im->MouseButtonHoldStateRaw;

        if (previousMouseState == 3)
            im->MouseButtonHoldStateRaw = 2;
        
        rmiWalkHook.Original(self, sumLeft, sumForward, sumTurnLeft, haveBackwardOrStrafe, a6, bAdditiveUnk);
        im->MouseButtonHoldStateRaw = previousMouseState;
    }
}

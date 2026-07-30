namespace NoMouseMoveForward.Hooks;

public class NoLmbRmbWalking : AsmHookBase
{
    public NoLmbRmbWalking()
    {
        // https://github.com/awgil/ffxiv_navmesh/blob/2bafac676b9fc5abbd0799add526ad7f0cfbba1d/vnavmesh/Movement/OverrideMovement.cs#L57-L59
        // Inside this function there is a check for whether both mouse buttons are held down.
        // This signature is for that.
        //     83 3D ?? ?? ?? ?? 03        CMP dword ptr [g_Client::Game::Control::InputManager_MouseButtonHoldState], 0x03
        //     75 ??                       JNZ ??
        //     F3 0F 10 0F                 MOVSS XMM1, dword ptr [RDI]
        // Before the JNZ instruction, we execute:
        //     48 85 E4                    TEST RSP, RSP
        // Since the stack pointer should always be non-zero, this clears the ZF flag and makes the JMP always taken.
        var address = Plugin.SigScanner.ScanText("83 3D ?? ?? ?? ?? 03 75 ?? F3 0F 10 0F");
        AddHook(address + 7, TestRspRsp, "NoLmbRmbWalking1");
        
        address = Plugin.SigScanner.ScanText("83 3D ?? ?? ?? ?? 03 0F 85 ?? ?? ?? ?? F3 0F 10 0F");
        AddHook(address + 7, TestRspRsp, "NoLmbRmbWalking2");
    }
}

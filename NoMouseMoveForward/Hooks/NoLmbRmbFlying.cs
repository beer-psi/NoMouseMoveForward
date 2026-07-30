namespace NoMouseMoveForward.Hooks;

public class NoLmbRmbFlying : AsmHookBase
{
    public NoLmbRmbFlying()
    {
        // https://github.com/awgil/ffxiv_navmesh/blob/2bafac676b9fc5abbd0799add526ad7f0cfbba1d/vnavmesh/Movement/OverrideMovement.cs#L61-L63
        // Check NoLmbRmbWalking for more information.
        //     83 3D ?? ?? ?? ?? 03        CMP dword ptr [g_Client::Game::Control::InputManager_MouseButtonHoldState], 0x03
        //     75 ??                       JNZ ??
        //     0F 28 C6                    MOVAPS XMM0, XMM6
        //     B3 ??                       MOV BL, ??
        //     F3 41 0F 58 C1              ADDSS XMM0, XMM9
        //     88 1D ....
        var address = Plugin.SigScanner.ScanText("83 3D ?? ?? ?? ?? 03 75 ?? 0F 28 C6 B3 ?? F3 41 0F 58 C1 88 1D");
        AddHook(address + 7, TestRspRsp, "NoLmbRmbFlying1");
        
        //     8B 0D ?? ?? ?? ??           MOV ECX, dword ptr [g_Client::Game::Control::InputManager_MouseButtonHoldState]
        //     ; some code in between
        //     83 F9 03                    CMP ECX, 0x03
        //     75 ??                       JNZ ??
        //     40 B5 ??                    MOV BPL, ??
        address = Plugin.SigScanner.ScanText("83 F9 03 75 ?? 40 B5");
        AddHook(address + 3, TestRspRsp, "NoLmbRmbFlying2");
        
        //     8B 05 ?? ?? ?? ??           MOV EAX, dword ptr [g_Client::Game::Control::InputManager_MouseButtonHoldState]
        //     ; some code in between
        //     83 F8 03                    CMP EAX, 0x03
        //     41 0F 94 C6                 SETZ R14B
        //     E9 ?? ?? ?? ??              JMP ??
        address = Plugin.SigScanner.ScanText("83 F8 03 41 0F 94 C6 E9");
        AddHook(address + 3, TestRspRsp, "NoLmbRmbFlying3");

        //     83 3D ?? ?? ?? ?? 03        CMP dword ptr [g_Client::Game::Control::InputManager_MouseButtonHoldState], 0x03
        //     75 ??                       JNZ ??
        //     0F 28 C6                    MOVAPS XMM0, XMM6
        //     B3 ??                       MOV BL, ??
        //     F3 41 0F 58 C1              ADDSS XMM0, XMM9
        //     41 0F 28 F1                 MOVAPS XMM6, XMM9
        address = Plugin.SigScanner.ScanText("83 3D ?? ?? ?? ?? 03 75 ?? 0F 28 C6 B3 ?? F3 41 0F 58 C1 41 0F 28 F1");
        AddHook(address + 7, TestRspRsp, "NoLmbRmbFlying4");
    }
}

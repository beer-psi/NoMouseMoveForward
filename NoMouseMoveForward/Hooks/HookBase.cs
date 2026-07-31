using System;

namespace NoMouseMoveForward.Hooks;

public abstract class HookBase : IDisposable
{
    public virtual void Dispose()
    {
        GC.SuppressFinalize(this);
    }

    protected abstract void Enable();
    protected abstract void Disable();
    protected abstract bool Enabled { get; }

    public void Toggle(bool enabled)
    {
        var wasEnabled = Enabled;

        if (enabled && !wasEnabled)
            Enable();
        else if (!enabled && wasEnabled)
            Disable();
    }
}

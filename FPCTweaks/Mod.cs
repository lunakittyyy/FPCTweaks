using FPCTweaks.Patches;
using GDWeave;

namespace FPCTweaks;

public class Mod : IMod {
    public Config Config;
    public static Mod? Instance;

    public Mod(IModInterface modInterface) {
        this.Config = modInterface.ReadConfig<Config>();
        Instance = this;
        modInterface.RegisterScriptMod(new AutoLook());
        modInterface.RegisterScriptMod(new SoftLockPatch());
    }

    public void Dispose() {
        // Cleanup anything you do here
    }
}

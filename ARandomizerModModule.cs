using System;
using System.Collections;
using Microsoft.Xna.Framework;
using MonoMod.ModInterop;

namespace Celeste.Mod.ARandomizerMod {
    public class ARandomizerModModule : EverestModule {
        public static ARandomizerModModule Instance { get; private set; }

        public override Type SettingsType => typeof(ARandomizerModModuleSettings);
        public static ARandomizerModModuleSettings Settings => (ARandomizerModModuleSettings) Instance._Settings;

        public override Type SessionType => typeof(ARandomizerModModuleSession);
        public static ARandomizerModModuleSession Session => (ARandomizerModModuleSession) Instance._Session;

        public ARandomizerModModule()
        {
            Instance = this;
#if DEBUG
            // debug builds use verbose logging
            Logger.SetLogLevel(nameof(ARandomizerModModule), LogLevel.Info);
#else
            // release builds use info logging to reduce spam in log files
            Logger.SetLogLevel(nameof(ARandomizerModModule), LogLevel.Info);
#endif
        }

        public override void Load() {
            On.Celeste.Level.TransitionRoutine += TestLoadVariant;
            On.Celeste.Player.WallJumpCheck += NoWallJump;
            typeof(ExtendedVariantImports).ModInterop();
        }

        private bool NoWallJump(On.Celeste.Player.orig_WallJumpCheck orig, Player self, int dir)
        {
            return false;
        }

        private IEnumerator TestLoadVariant(On.Celeste.Level.orig_TransitionRoutine orig, Level self, LevelData next, Vector2 direction)
        {
            ExtendedVariantImports.TriggerBooleanVariant.Invoke("AlwaysInvisible", true, false);
            return orig(self, next, direction);
        }


        public override void Unload() {
            // TODO: unapply any hooks applied in Load()
        }
    }
}
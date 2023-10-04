using System;
using System.Collections;
using Microsoft.Xna.Framework;
using Monocle;
using MonoMod.ModInterop;

namespace Celeste.Mod.ARandomizerMod {
    public class ARandomizerModModule : EverestModule {
        public static ARandomizerModModule Instance { get; private set; }

        public override Type SettingsType => typeof(ARandomizerModModuleSettings);
        public static ARandomizerModModuleSettings Settings => (ARandomizerModModuleSettings)Instance._Settings;

        public override Type SessionType => typeof(ARandomizerModModuleSession);
        public static ARandomizerModModuleSession Session => (ARandomizerModModuleSession)Instance._Session;

        UI ui;

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
            On.Celeste.Level.Render += LevelRender;
            typeof(ExtendedVariantImports).ModInterop();

        }

        private IEnumerator TestLoadVariant(On.Celeste.Level.orig_TransitionRoutine orig, Level self, LevelData next, Vector2 direction)
        {
            //ExtendedVariantImports.TriggerBooleanVariant.Invoke("AlwaysInvisible", true, false);
            //ui.Active = true;
            return orig(self, next, direction);
        }

        private void LevelRender(On.Celeste.Level.orig_Render orig, Level self)
        {
            orig(self);
            if (Engine.Scene is not Level level)
                return;
            if (ui == null)
            {
                ui = new UI
                {
                    Active = true
                };
            }

            level.Add(ui);
        }

        public override void Unload()
        {

        }
    }
}
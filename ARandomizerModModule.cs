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

        VaraintsUI ui;

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

        public override void Load() {;
            typeof(ExtendedVariantImports).ModInterop();
            On.Celeste.Level.LoadLevel += LevelLoad;
            On.Celeste.Level.Update += LevelUpdate;
            //On.Celeste.Player.Update += PlayerUpdate;

        }

        private void LevelLoad(On.Celeste.Level.orig_LoadLevel orig, Level self, Player.IntroTypes playerIntro, bool isFromLoader)
        {
            if (ui == null)
            {
                ui = new VaraintsUI
                {
                    Active = false
                };
            }
        }

        private void LevelUpdate(On.Celeste.Level.orig_Update orig, Level self)
        {
            if (ARandomizerModModule.Settings.OpenVariantsMenu.Check)
            {
                ui.render = true;
            }
            else
            {
                ui.render = false;
                orig(self);
            }
        }

        private void PlayerUpdate(On.Celeste.Player.orig_Update orig, Player self)
        {
            if (ARandomizerModModule.Settings.OpenVariantsMenu.Check)
                orig(self);
        }

        public override void Unload()
        {
        
        }
    }
}
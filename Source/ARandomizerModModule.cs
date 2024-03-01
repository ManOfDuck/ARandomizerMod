using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Monocle;
using MonoMod.ModInterop;
using DifficultyOptions = Celeste.Mod.ARandomizerMod.ARandomizerModModuleSettings.DifficultyOptions;
using Lists = Celeste.Mod.ARandomizerMod.VariantLists;

namespace Celeste.Mod.ARandomizerMod {
    public class ARandomizerModModule : EverestModule {
        public static ARandomizerModModule Instance { get; private set; }

        public override Type SettingsType => typeof(ARandomizerModModuleSettings);
        public static ARandomizerModModuleSettings Settings => (ARandomizerModModuleSettings)Instance._Settings;

        public override Type SessionType => typeof(ARandomizerModModuleSession);
        public static ARandomizerModModuleSession Session => (ARandomizerModModuleSession)Instance._Session;

        public VaraintsUI ui;
        public VariantManager variantManager;
        public EconomyManager economyManager;

        public ARandomizerModModule()
        {
            Instance = this;

//#if DEBUG   
            // debug builds use verbose logging
            Logger.SetLogLevel(nameof(ARandomizerModModule), LogLevel.Debug);
//#else
            // release builds use info logging to reduce spam in log files
            //Logger.SetLogLevel(nameof(ARandomizerModModule), LogLevel.Info);
//#endif
        }

        public override void Load() {
            typeof(ExtendedVariantImports).ModInterop();

            variantManager = new();
            economyManager = new(variantManager);

            On.Celeste.Level.LoadLevel += LevelLoad;
            On.Celeste.Level.TransitionRoutine += RoomTransition;
            On.Celeste.LevelLoader.StartLevel += LevelStarted;
        }

        // TODO: this is activated on debug teleport too, maybe something to fix?
        private void LevelStarted(On.Celeste.LevelLoader.orig_StartLevel orig, LevelLoader self)
        {
            orig(self);
            variantManager.ResetAllVariants();
        }

        private void LevelLoad(On.Celeste.Level.orig_LoadLevel orig, Level self, Player.IntroTypes playerIntro, bool isFromLoader)
        {
            orig(self, playerIntro, isFromLoader);

            Logger.Log(LogLevel.Error, "ARandomizerMod", self.ToString());

            ui??= new VaraintsUI(variantManager, economyManager)
            {
                Active = true
            };
            self.Add(ui);
        }

        private IEnumerator RoomTransition(On.Celeste.Level.orig_TransitionRoutine orig, Level self, LevelData next, Vector2 direction)
        {
            economyManager.RoomCleared();

            variantManager.RoomLoaded(next);

            self.Remove(ui);
            ui = new VaraintsUI(variantManager, economyManager)
            {
                Active = true
            };
            self.Add(ui);

            return orig(self, next, direction);
        }

        private void TestAllVariants()
        {
            TestVariantList(VariantLists.FUCKED_UP);
            TestVariantList(VariantLists.nasty);
            TestVariantList(VariantLists.tame);
            TestVariantList(VariantLists.dubious);
            TestVariantList(VariantLists.silly);
            TestVariantList(VariantLists.nice);
            TestVariantList(VariantLists.good);
            TestVariantList(VariantLists.great);
        }

        private void TestVariantList(Variant[] list)
        {
            foreach (Variant v in list)
            {
                Logger.Log(LogLevel.Warn, "ARandomizerMod", v.name);
                variantManager.TriggerVariant(v);
            }
        }

        public override void Unload()
        {
            On.Celeste.Level.LoadLevel -= LevelLoad;
            On.Celeste.Level.TransitionRoutine -= RoomTransition;
        }
    }
}
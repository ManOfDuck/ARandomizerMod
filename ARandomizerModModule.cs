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

        VaraintsUI ui;

        Dictionary<DifficultyOptions, int[]> variantRolls = new Dictionary<DifficultyOptions, int[]>();
        int[] easyRolls = { 1, 2 };

        Dictionary<DifficultyOptions, float[]> variantRanges = new Dictionary<DifficultyOptions, float[]>();
        float[] easyRanges = { 0.05f, 0.15f, 0.35f, 0.5f, .65f, 0.75f, 0.85f, 0.9f,  0.95f };

        public ARandomizerModModule()
        {
            Instance = this;
            variantRolls.Add(DifficultyOptions.EASY, easyRolls);

            variantRanges.Add(DifficultyOptions.EASY, easyRanges);

#if DEBUG   
            // debug builds use verbose logging
            Logger.SetLogLevel(nameof(ARandomizerModModule), LogLevel.Warn);
#else
            // release builds use info logging to reduce spam in log files
            Logger.SetLogLevel(nameof(ARandomizerModModule), LogLevel.Info);
#endif
        }

        public override void Load() {
            typeof(ExtendedVariantImports).ModInterop();
            
            On.Celeste.Level.LoadLevel += LevelLoad;
            On.Celeste.Level.Update += LevelUpdate;
            On.Celeste.Level.TransitionRoutine += RoomTransition;
        }

        private void LevelUpdate(On.Celeste.Level.orig_Update orig, Level self)
        {
            ui ??= new VaraintsUI
            {
                Active = true
            };
            self.Add(ui);
            ui.Update();
            ui.Active = true;
            orig(self);
        }

        private void LevelLoad(On.Celeste.Level.orig_LoadLevel orig, Level self, Player.IntroTypes playerIntro, bool isFromLoader)
        {
            orig(self, playerIntro, isFromLoader);

            ui ??= new VaraintsUI
                {
                    Active = true
                };

            self.Add(ui);
        }

        private IEnumerator RoomTransition(On.Celeste.Level.orig_TransitionRoutine orig, Level self, LevelData next, Vector2 direction)
        {
            ui.RoomCleared();

            DifficultyOptions difficulty = ARandomizerModModule.Settings.Difficulty;

            int[] rollsRange = variantRolls[difficulty];
            float[] ranges = variantRanges[difficulty];

            List<Variant> variantsToActivate = GetRandomVariants(rollsRange, ranges);

            foreach (Variant v in variantsToActivate)
            {
                //Logger.Log(LogLevel.Info, "ARandomizerMod", "Passing: " + v.name);
                if (v is null)
                    Logger.Log(LogLevel.Error, "ARandomizerMod", "Passing null hehe");

                ui.TriggerVariant(v);
            }
   
            ui.Active = true;

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
                ui.TriggerVariant(v);
            }
        }

        private List<Variant> GetRandomVariants(int[] rollsRange, float[] ranges)
        {
            Random random = new Random();
            int rolls = random.Next(rollsRange[0], rollsRange[1]);

            List<Variant> variants = new List<Variant>();
            for (int i = 0; i < rolls; i++)
            {
                float roll = random.NextFloat(1);

                if (roll < ranges[0])
                {
                    variants.Add(GetRandomVariant(VariantLists.FUCKED_UP));
                }
                else if (roll < ranges[1])
                {
                    variants.Add(GetRandomVariant(VariantLists.nasty));
                }
                else if (roll < ranges[2])
                {
                    variants.Add(GetRandomVariant(VariantLists.tame));
                }
                else if (roll < ranges[3])
                {
                    variants.Add(GetRandomVariant(VariantLists.dubious));
                }
                else if (roll < ranges[4])
                {
                    variants.Add(GetRandomVariant(VariantLists.silly));
                }
                else if (roll < ranges[5])
                {
                    variants.Add(GetRandomVariant(VariantLists.nice));
                }
                else if (roll < ranges[6])
                {
                    variants.Add(GetRandomVariant(VariantLists.good));
                }
                else if (roll < ranges[7])
                {
                    variants.Add(GetRandomVariant(VariantLists.great));
                }
                else if (roll < ranges[8])
                {
                    ui.ResetRandomVariant();
                }
            }

            return variants;
        } 

        private Variant GetRandomVariant(Variant[] variantList)
        {
            int variantIndex = new Random().Next(variantList.Length);
            return variantList[variantIndex];
        }

        public override void Unload()
        {
            On.Celeste.Level.LoadLevel -= LevelLoad;
            On.Celeste.Level.Update -= LevelUpdate;
            On.Celeste.Level.TransitionRoutine -= RoomTransition;
        }
    }
}
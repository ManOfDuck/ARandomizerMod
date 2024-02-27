using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Monocle;
using MonoMod.ModInterop;
using DifficultyOptions = Celeste.Mod.ARandomizerMod.ARandomizerModModuleSettings.DifficultyOptions;
using Lists = Celeste.Mod.ARandomizerMod.VariantLists;
using Celeste.Mod.CelesteNet.DataTypes;

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

        Dictionary<DifficultyOptions, int[]> variantRolls = new Dictionary<DifficultyOptions, int[]>();
        readonly int[] easyRolls = { 1, 2 };

        Dictionary<DifficultyOptions, float[]> variantRanges = new Dictionary<DifficultyOptions, float[]>();
        readonly float[] easyRanges = { 0.05f, 0.15f, 0.35f, 0.5f, .65f, 0.75f, 0.85f, 0.9f,  0.95f };


        public ARandomizerModModule()
        {
            Instance = this;
            variantRolls.Add(DifficultyOptions.EASY, easyRolls);
            variantRanges.Add(DifficultyOptions.EASY, easyRanges);

//#if DEBUG   
            // debug builds use verbose logging
            Logger.SetLogLevel(nameof(ARandomizerModModule), LogLevel.Debug);
//#else
            // release builds use info logging to reduce spam in log files
            //Logger.SetLogLevel(nameof(ARandomizerModModule), LogLevel.Info);
//#endif
        }

        public class TestType : DataType
        {

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

            ui??= new VaraintsUI(variantManager, economyManager)
            {
                Active = true
            };
            self.Add(ui);
        }

        private IEnumerator RoomTransition(On.Celeste.Level.orig_TransitionRoutine orig, Level self, LevelData next, Vector2 direction)
        {
            economyManager.RoomCleared();

            DifficultyOptions difficulty = ARandomizerModModule.Settings.Difficulty;

            int[] rollsRange = variantRolls[difficulty];
            float[] ranges = variantRanges[difficulty];

            List<Variant> variantsToActivate = GetRandomVariants(rollsRange, ranges);

            foreach (Variant v in variantsToActivate)
            {
                //Logger.Log(LogLevel.Info, "ARandomizerMod", "Passing: " + v.name);
                if (v is null)
                    Logger.Log(LogLevel.Error, "ARandomizerMod", "Passing null hehe");

                variantManager.TriggerVariant(v);
            }

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

        private List<Variant> GetRandomVariants(int[] rollsRange, float[] ranges)
        {
            Random random = new();
            int rolls = random.Next(rollsRange[0], rollsRange[1]);

            List<Variant> variants = new();
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
                    variantManager.ResetRandomVariant();
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
            On.Celeste.Level.TransitionRoutine -= RoomTransition;
        }
    }
}
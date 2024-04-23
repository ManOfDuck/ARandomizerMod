using System;
using System.Collections;
using Celeste.Mod.ARandomizerMod.CelesteNet;
using Celeste.Mod.ARandomizerMod.CelesteNet.Data;
using Celeste.Mod.ARandomizerMod.Data;
using Microsoft.Xna.Framework;
using Monocle;
using MonoMod.ModInterop;

namespace Celeste.Mod.ARandomizerMod
{
    public class ARandomizerModModule : EverestModule {
        public static readonly string ProtocolVersion = "1_0_2";

        public static ARandomizerModModule Instance { get; private set; }

        public override Type SettingsType => typeof(ARandomizerModModuleSettings);
        public static ARandomizerModModuleSettings Settings => (ARandomizerModModuleSettings)Instance._Settings;

        private static VaraintsUI ui;

        public ARandomizerModModule()
        {
            Instance = this;

#if DEBUG   
            // debug builds use verbose logging
            Logger.SetLogLevel(nameof(ARandomizerModModule), LogLevel.Debug);
#else
            // release builds use info logging to reduce spam in log files
            Logger.SetLogLevel(nameof(ARandomizerModModule), LogLevel.Info);
#endif
        }

        public override void Load() {
            typeof(ExtendedVariantImports).ModInterop();

            //Everest Events
            Everest.Events.LevelLoader.OnLoadingThread += OnLoadingThread;

            // Celeste Hooks
            On.Celeste.Level.TransitionRoutine += RoomTransition;
            On.Celeste.LevelLoader.StartLevel += LevelStarted;
            On.Celeste.StrawberryPoints.Added += StrawberryCollected;
            On.Celeste.HeartGem.Collect += HeartCollected;
            On.Celeste.Cassette.CollectRoutine += CassetteCollected;

            // Multiplayer Events
            CNetComm.OnReceiveVariantUpdate += OnVariantUpdate;
            CNetComm.OnReceiveTest += OnReceiveTest;

            // Add CNet game object
            Celeste.Instance.Components.Add(new CNetComm(Celeste.Instance));
        }

        public override void Unload()
        {
            On.Celeste.Level.TransitionRoutine -= RoomTransition;
            On.Celeste.LevelLoader.StartLevel -= LevelStarted;
            On.Celeste.StrawberryPoints.Added -= StrawberryCollected;
            On.Celeste.HeartGem.Collect -= HeartCollected;
            On.Celeste.Cassette.CollectRoutine -= CassetteCollected;

            CNetComm.OnReceiveVariantUpdate -= OnVariantUpdate;
            CNetComm.OnReceiveTest -= OnReceiveTest;

            if (Celeste.Instance.Components.Contains(CNetComm.Instance))
                Celeste.Instance.Components.Remove(CNetComm.Instance);
        }

        private static void OnReceiveTest(TestData data)
        {
            Logger.Log(LogLevel.Info, nameof(ARandomizerModModule), data.Message);
        }

        private void OnLoadingThread(Level level)
        {
            ui ??= new VaraintsUI()
            {
                Active = true
            };
            level.Add(ui);
        }

        // TODO: this is activated on debug teleport too, maybe something to fix?
        private static void LevelStarted(On.Celeste.LevelLoader.orig_StartLevel orig, LevelLoader self)
        {
            orig(self);
            VariantManager.ResetAllVariants();
        }

        private static IEnumerator RoomTransition(On.Celeste.Level.orig_TransitionRoutine orig, Level self, LevelData next, Vector2 direction)
        {
            EconomyManager.RoomCleared();
            VariantManager.RoomLoaded(next);

            yield return new SwapImmediately(orig(self, next, direction));
        }

        private static void StrawberryCollected(On.Celeste.StrawberryPoints.orig_Added orig, StrawberryPoints self, Scene scene)
        {
            EconomyManager.StrawberryCollected();

            orig(self, scene);
        }

        private static void HeartCollected(On.Celeste.HeartGem.orig_Collect orig, HeartGem self, Player player)
        {
            EconomyManager.HeartCollected();

            orig(self, player);
        }

        private static IEnumerator CassetteCollected(On.Celeste.Cassette.orig_CollectRoutine orig, Cassette self, Player player)
        {
            EconomyManager.CassetteCollected();

            yield return new SwapImmediately(orig(self, player));
        }

        private static void OnVariantUpdate(VariantUpdateData data)
        {
            VariantManager.ProcessVariantUpdate(data);
        }
    }
}
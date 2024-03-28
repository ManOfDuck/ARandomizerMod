using Celeste.Mod.ARandomizerMod.CelesteNet;
using Monocle;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Celeste.Mod.ARandomizerMod
{
    public class EconomyManager
    {
        readonly VariantManager variantManager;

        public int money = 0;
        readonly int moneyGainedPerRoom = 50;
        readonly int strawberryMoney = 150;
        readonly int cassetteMoney = 500;
        readonly int heartMoney = 1000;
        readonly float variantCostToGainRatio = 0f;

        public int score = 0;
        readonly int scoreGainedPerRoom = 100;
        readonly int strawberryScore = 1000;
        readonly int cassetteScore = 50000;
        readonly int heartScore = 10000;
        readonly float variantCostToScoreRatio = 0.1f;

        public EconomyManager(VariantManager variantManager)
        {
            On.Celeste.StrawberryPoints.Added += StrawberryCollected;
            On.Celeste.HeartGem.Collect += HeartCollected;
            On.Celeste.Cassette.OnPlayer += CassetteCollected;
            this.variantManager = variantManager;
        }

        private void CassetteCollected(On.Celeste.Cassette.orig_OnPlayer orig, Cassette self, Player player)
        {
            money += cassetteMoney;
            score += cassetteScore;

            orig(self, player);
        }

        private void StrawberryCollected(On.Celeste.StrawberryPoints.orig_Added orig, StrawberryPoints self, Scene scene)
        {
            money += strawberryMoney;
            score += strawberryScore;

            orig(self, scene);
        }
        private void HeartCollected(On.Celeste.HeartGem.orig_Collect orig, HeartGem self, Player player)
        {
            money += heartMoney;
            score += heartScore;

            orig(self, player);
        }

        public void RoomCleared()
        {
            money += moneyGainedPerRoom;
            score += scoreGainedPerRoom;
            foreach (Variant variant in variantManager.activeVariants)
            {
                money += (int)(variant.cost * variantCostToGainRatio);
                score += (int)(variant.cost * variantCostToScoreRatio);
            }
        }

        public void PurchaseVariantRemoval(Variant variant)
        {
            money -= variant.cost;
            CNetComm.Instance.SendVariantUpdate(VariantManager.AllRoomsIdentifier, variant, CelesteNet.Data.VariantUpdateData.Operation.REMOVE);
        }
    }
}

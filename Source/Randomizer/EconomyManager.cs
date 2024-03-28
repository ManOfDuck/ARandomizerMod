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
    public static class EconomyManager
    {
        public static int money = 0;
        readonly static int moneyGainedPerRoom = 50;
        readonly static int strawberryMoney = 150;
        readonly static int cassetteMoney = 500;
        readonly static int heartMoney = 1000;
        readonly static float variantCostToGainRatio = 0f;

        public static int score = 0;
        readonly static int scoreGainedPerRoom = 100;
        readonly static int strawberryScore = 1000;
        readonly static int cassetteScore = 50000;
        readonly static int heartScore = 10000;
        readonly static float variantCostToScoreRatio = 0.1f;

        public static void CassetteCollected()
        {
            money += cassetteMoney;
            score += cassetteScore;
        }

        public static void StrawberryCollected()
        {
            money += strawberryMoney;
            score += strawberryScore;
        }
        public static void HeartCollected()
        {
            money += heartMoney;
            score += heartScore;
        }

        public static void RoomCleared()
        {
            money += moneyGainedPerRoom;
            score += scoreGainedPerRoom;
            foreach (Variant variant in VariantManager.activeVariants)
            {
                money += (int)(variant.cost * variantCostToGainRatio);
                score += (int)(variant.cost * variantCostToScoreRatio);
            }
        }

        public static void PurchaseVariantRemoval(Variant variant)
        {
            money -= variant.cost;
            CNetComm.Instance.SendVariantUpdate(VariantManager.AllRoomsIdentifier, variant, CelesteNet.Data.VariantUpdateData.Operation.REMOVE);
        }
    }
}

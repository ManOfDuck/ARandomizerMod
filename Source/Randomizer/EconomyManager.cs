using Celeste.Mod.ARandomizerMod.CelesteNet;

namespace Celeste.Mod.ARandomizerMod
{
    public static class EconomyManager
    {
        public static int Money { get; private set; } = 0;
        private static readonly int moneyGainedPerRoom = 50;
        private static readonly int strawberryMoney = 150;
        private static readonly int cassetteMoney = 500;
        private static readonly int heartMoney = 1000;
        private static readonly float variantCostToGainRatio = 0f;

        public static int Score { get; private set; } = 0;
        private static readonly int scoreGainedPerRoom = 100;
        private static readonly int strawberryScore = 1000;
        private static readonly int cassetteScore = 50000;
        private static readonly int heartScore = 10000;
        private static readonly float variantCostToScoreRatio = 0.1f;

        public static void CassetteCollected()
        {
            Money += cassetteMoney;
            Score += cassetteScore;
        }

        public static void StrawberryCollected()
        {
            Money += strawberryMoney;
            Score += strawberryScore;
        }
        public static void HeartCollected()
        {
            Money += heartMoney;
            Score += heartScore;
        }

        public static void RoomCleared()
        {
            Money += moneyGainedPerRoom;
            Score += scoreGainedPerRoom;
            foreach (Variant variant in VariantManager.ActiveVariants)
            {
                Money += (int)(variant.Cost * variantCostToGainRatio);
                Score += (int)(variant.Cost * variantCostToScoreRatio);
            }
        }

        public static void PurchaseVariantRemoval(Variant variant)
        {
            Money -= variant.Cost;
            Logger.Log(LogLevel.Debug, nameof(ARandomizerModModule), "Purchased variant removal of variant " + variant);
            VariantManager.SendVariantRemovalInAllRooms(variant);
        }
    }
}

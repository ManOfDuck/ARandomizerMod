using System;
using static Celeste.Mod.ARandomizerMod.ARandomizerModModuleSettings;
using System.Collections.Generic;
using Monocle;

namespace Celeste.Mod.ARandomizerMod
{
	public class VariantRandomizer
	{
        private static readonly Dictionary<DifficultyOptions, int[]> variantRolls = new Dictionary<DifficultyOptions, int[]>
        {
            // { min rolls, max rolls}
            {DifficultyOptions.EASY, new int[] { 1, 2 } }
        };

        private static readonly Dictionary<DifficultyOptions, float[]> variantRanges = new Dictionary<DifficultyOptions, float[]>
        {
            // { fucked up, nasty, tame, dubious, silly, nice, good, great, remove variant }
            {DifficultyOptions.EASY, new float[] { 0.05f, 0.15f, 0.35f, 0.5f, .65f, 0.75f, 0.85f, 0.9f, 0.95f } }
        };

        public static LinkedList<Variant> RollNewVariants()
        {
            DifficultyOptions difficulty = ARandomizerModModule.Settings.Difficulty;
            int[] rollsRange = variantRolls[difficulty];
            float[] ranges = variantRanges[difficulty];

            Random random = new();
            int rolls = random.Next(rollsRange[0], rollsRange[1]);

            LinkedList<Variant> variants = new();
            for (int i = 0; i < rolls; i++)
            {
                float roll = random.NextFloat(1);

                if (roll < ranges[0])
                {
                    variants.AddLast(GetRandomVariant(VariantLists.FUCKED_UP));
                }
                else if (roll < ranges[1])
                {
                    variants.AddLast(GetRandomVariant(VariantLists.nasty));
                }
                else if (roll < ranges[2])
                {
                    variants.AddLast(GetRandomVariant(VariantLists.tame));
                }
                else if (roll < ranges[3])
                {
                    variants.AddLast(GetRandomVariant(VariantLists.dubious));
                }
                else if (roll < ranges[4])
                {
                    variants.AddLast(GetRandomVariant(VariantLists.silly));
                }
                else if (roll < ranges[5])
                {
                    variants.AddLast(GetRandomVariant(VariantLists.nice));
                }
                else if (roll < ranges[6])
                {
                    variants.AddLast(GetRandomVariant(VariantLists.good));
                }
                else if (roll < ranges[7])
                {
                    variants.AddLast(GetRandomVariant(VariantLists.great));
                }
            }

            return variants;
        }

        private static Variant GetRandomVariant(Variant[] variantList)
        {
            int variantIndex = new Random().Next(variantList.Length);
            return variantList[variantIndex];
        }

        public static int RollRemovedVariants()
        {
            DifficultyOptions difficulty = ARandomizerModModule.Settings.Difficulty;
            int[] rollsRange = variantRolls[difficulty];
            float[] ranges = variantRanges[difficulty];

            Random random = new();
            int rolls = random.Next(rollsRange[0], rollsRange[1]);

            int numVariantsToRemove = 0;

            for (int i = 0; i < rolls; i++)
            {
                float roll = random.NextFloat(1);

                if (roll >= ranges[7] && roll < ranges[8])
                {
                    numVariantsToRemove++;
                }
            }

            return numVariantsToRemove;
        }
    }
}


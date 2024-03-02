using System;
using Celeste.Mod.CelesteNet;

namespace Celeste.Mod.ARandomizerMod
{
    public class IntegerVariant : Variant
    {
        public int minInt;
        public int maxInt;
        public int defaultInt;

        public IntegerVariant(String name, int minInt, int maxInt, int defaultInt, Level level)
            : base(name, level)
        {
            this.minInt = minInt;
            this.maxInt = maxInt;
            this.defaultInt = defaultInt;
        }
    }

    public static class IntegerVariantExt
    {
        public static IntegerVariant ReadIntegerVariant(this CelesteNetBinaryReader reader)
        {
            // Read base data
            IntegerVariant integerVariant = (IntegerVariant) VariantExt.ReadVariantBase(reader);

            // Read class-specific data
            integerVariant.minInt = reader.ReadInt32();
            integerVariant.maxInt = reader.ReadInt32();
            integerVariant.defaultInt = reader.ReadInt32();

            return integerVariant;
        }

        public static void Write(this CelesteNetBinaryWriter writer, IntegerVariant integerVariant)
        {
            // Write base data
            VariantExt.WriteVariantBase(writer, integerVariant);

            // Write class-specific data
            writer.Write(integerVariant.minInt);
            writer.Write(integerVariant.maxInt);
            writer.Write(integerVariant.defaultInt);
        }
    }
}


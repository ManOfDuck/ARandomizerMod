using System;
using System.Runtime.InteropServices;
using Celeste.Mod.ARandomizerMod;
using Celeste.Mod.CelesteNet;
using Monocle;

namespace Celeste.Mod.ARandomizerMod
{
    public class IntegerVariant : Variant
    {
        public int minInt;
        public int maxInt;
        public int defaultInt;
        public int intValue;

        public IntegerVariant(String name, int minInt, int maxInt, int defaultInt, Level level)
            : base(name, level)
        {
            this.minInt = minInt;
            this.maxInt = maxInt;
            this.defaultInt = defaultInt;
        }

        public void SetValue(int value)
        {
            intValue = value;
            valueString = value.ToString();
        }

        new public void Trigger()
        {
            // If we've never set this string, we haven't initilaized this variant yet
            if (valueString is null)
            {
                Random random = new();
                SetValue(random.Next(minInt, maxInt));
            }
            ExtendedVariantImports.TriggerFloatVariant?.Invoke(name, intValue, false);
        }

        new public void Reset()
        {
            SetValue(defaultInt);
            ExtendedVariantImports.TriggerFloatVariant?.Invoke(name, defaultInt, false);
        }
    }

    public static class IntegerVariantExt
    {
        public static IntegerVariant ReadIntegerVariant(this CelesteNetBinaryReader reader)
        {
            // Read base data
            string name = reader.ReadString();
            Variant.Level level = (Variant.Level)reader.ReadInt32();

            // Read class-specific data
            int minInt = reader.ReadInt32();
            int maxInt = reader.ReadInt32();
            int defaultInt = reader.ReadInt32();
            int intValue = reader.ReadInt32();

            // Deserialize variant
            IntegerVariant integerVariant = new(name, minInt, maxInt, defaultInt, level);
            integerVariant.SetValue(intValue);

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
            writer.Write(integerVariant.intValue);
        }
    }
}


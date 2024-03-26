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

        override public void SetValue()
        {
            Random random = new();
            intValue = random.Next(minInt, maxInt);
            valueString = intValue.ToString();
        }

        override public void Trigger()
        {

            ExtendedVariantImports.TriggerIntegerVariant?.Invoke(name, intValue, false);
        }

        override public void Reset()
        {
            intValue = defaultInt;
            valueString = defaultInt.ToString();
            ExtendedVariantImports.TriggerIntegerVariant?.Invoke(name, defaultInt, false);
        }
    }

    public static class IntegerVariantExt
    {
        public static IntegerVariant ReadIntegerVariant(this CelesteNetBinaryReader reader)
        {
            // Read base data
            string name = reader.ReadString();
            Variant.Level level = (Variant.Level)reader.ReadInt32();
            string valueString = reader.ReadString();

            // Read class-specific data
            int minInt = reader.ReadInt32();
            int maxInt = reader.ReadInt32();
            int defaultInt = reader.ReadInt32();
            int intValue = reader.ReadInt32();
            Logger.Log(LogLevel.Error, "ARandomizerMod", "Reading Int variant: name = " + name + " level = " + level + " valueString = " + valueString + " min = " + minInt + " max = " + maxInt + " default = " + defaultInt + " value = " + intValue);

            // Deserialize variant
            IntegerVariant integerVariant = new(name, minInt, maxInt, defaultInt, level)
            {
                valueString = valueString,
                intValue = intValue
            };

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


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
            intValue = VariantRandomizer.randomGenerator.Next(minInt, maxInt);
            valueString = intValue.ToString();
        }

        override public bool Trigger()
        {
            try
            {
                ExtendedVariantImports.TriggerIntegerVariant?.Invoke(name, intValue, false);
                return true;
            }
            catch (Exception e)
            {
                Logger.Log(LogLevel.Error, nameof(ARandomizerModModule), "Triggering variant " + name + " with value " + intValue + " failed! " +
                    "Exception follows:\n" + e.Message);
                return false;
            }
        }

        override public bool Reset()
        {
            try
            {
                ExtendedVariantImports.TriggerIntegerVariant?.Invoke(name, defaultInt, false);
                return true;
            }
            catch (Exception e)
            {
                Logger.Log(LogLevel.Error, nameof(ARandomizerModModule), "Resetting variant " + name + "failed! " +
                    "Exception follows:\n" + e.Message);
                return false;
            }
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


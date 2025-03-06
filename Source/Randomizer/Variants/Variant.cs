using System;
using System.Collections.Generic;
using Celeste.Mod.CelesteNet;

namespace Celeste.Mod.ARandomizerMod
{
    public class Variant
    {
        readonly static Dictionary<Level, int> costPerLevel = new()
        {
            { Level.GREAT, -100 },
            { Level.GOOD, -50 },
            { Level.NICE, -10 },
            { Level.SILLY, 10 },
            { Level.DUBIOUS, 100 },
            { Level.TAME, 200 },
            { Level.NASTY, 300 },
            { Level.FUCKED_UP, 500 },
            { Level.SUB, 0 }
        };

        public string name;

        public int Cost { get; set; }

        public enum Level { GREAT, GOOD, NICE, SILLY, DUBIOUS, TAME, NASTY, FUCKED_UP, SUB };
        public Level level;

        public string valueString = "";

        public Variant(String name, Level level)
        {
            this.name = name;
            this.level = level;
            DoCost();
        }

        override public string ToString()
        {
            return (name + "," + valueString);
        }

        public override bool Equals(object obj)
        {
            if (obj is not Variant otherVariant) return false;

            return (otherVariant.name == name && otherVariant.level == level && otherVariant.valueString == valueString);
        }

        virtual protected void DoCost()
        {
            Cost = costPerLevel[level];
        }

        virtual public void SetValue()
        {
            Logger.Log(LogLevel.Error, nameof(ARandomizerModModule), "Unimplemented SetValue() method");
        }

        virtual public bool Trigger()
        {
            Logger.Log(LogLevel.Error, nameof(ARandomizerModModule), "Unimplemented Trigger() method");
            return false;
        }

        virtual public bool Reset()
        {
            Logger.Log(LogLevel.Error, nameof(ARandomizerModModule), "Unimplemented Reset() method");
            return false;
        }
    }

    // Extensions for CelesteNet Binary Reader/Writer
    public static class VariantExt
    {
        private enum VariantType { INT, FLOAT, BOOL };

        public static Variant ReadVariant(this CelesteNetBinaryReader reader)
        {
            //Read the variant's type
            VariantType type = (VariantType)reader.ReadInt32();

            // Read the variant's data
            switch (type)
            {
                case VariantType.INT:
                    // Read as integer variant
                    return reader.ReadIntegerVariant();
                case VariantType.FLOAT:
                    // Read as float variant
                    return reader.ReadFloatVariant();
                case VariantType.BOOL:
                    // Read as boolean variant
                    return reader.ReadBooleanVariant();
                default:
                    // Variant type is unimplemented, error out
                    Logger.Log(LogLevel.Error, nameof(ARandomizerModModule), "Unrecognized Variant Type");
                    return null;
            }
        }

        public static void Write(this CelesteNetBinaryWriter writer, Variant variant)
        {
            // Get the variant's type
            switch (variant)
            {
                case IntegerVariant integerVariant:
                    // Write the variant as an integer variant
                    writer.Write((int)VariantType.INT);
                    writer.Write(integerVariant);
                    break;
                case FloatVariant floatVariant:
                    // Write the variant as an float variant
                    writer.Write((int)VariantType.FLOAT);
                    writer.Write(floatVariant);
                    break;
                case BooleanVariant booleanVariant:
                    // Write the variant as an boolean variant
                    writer.Write((int)VariantType.BOOL);
                    writer.Write(booleanVariant);
                    break;
            }
        }

        // Called by variant classes to write shared members
        public static void WriteVariantBase(CelesteNetBinaryWriter writer, Variant variant)
        {
            writer.Write(variant.name);
            writer.Write((int)variant.level);
            writer.Write(variant.valueString);
        }
    }
}


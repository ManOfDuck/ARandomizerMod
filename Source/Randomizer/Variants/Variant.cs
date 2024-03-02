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

        public int cost { get; set; }

        public enum Level { GREAT, GOOD, NICE, SILLY, DUBIOUS, TAME, NASTY, FUCKED_UP, SUB };
        public Level level;

        public string value;

        public Variant(String name, Level level)
        {
            this.name = name;
            this.level = level;
            DoCost();
        }

        virtual protected void DoCost()
        {
            cost = costPerLevel[level];
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
                    Logger.Log(LogLevel.Error, "ARandomizerMod", "Unrecognized Variant Type");
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


        // Called by variant classes to read shared members
        public static Variant ReadVariantBase(CelesteNetBinaryReader reader)
        {
            string name = reader.ReadString();
            Variant.Level level = (Variant.Level)reader.ReadInt32();
            string value = reader.ReadString();

            Variant variant = new(name, level)
            {
                value = value
            };

            return variant;
        }

        // Called by variant classes to write shared members
        public static void WriteVariantBase(CelesteNetBinaryWriter writer, Variant variant)
        {
            writer.Write(variant.name);
            writer.Write((int)variant.level);
            writer.Write(variant.value);
        }
    }
}


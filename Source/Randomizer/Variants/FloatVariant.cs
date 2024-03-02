using System;
using Celeste.Mod.CelesteNet;

namespace Celeste.Mod.ARandomizerMod
{
	public class FloatVariant : Variant
	{
        public float minFloat;
        public float maxFloat;
        public float defaultFloat;

        public FloatVariant(String name, float minFloat, float maxFloat, float defaultFloat, Level level)
            : base(name, level)
        {
            this.minFloat = minFloat;
            this.maxFloat = maxFloat;
            this.defaultFloat = defaultFloat;
        }
    }

    public static class FloatVariantExt
    {
        public static FloatVariant ReadFloatVariant(this CelesteNetBinaryReader reader)
        {
            // Read base data
            FloatVariant floatVariant = (FloatVariant)VariantExt.ReadVariantBase(reader);

            // Read class-specific data
            floatVariant.minFloat = reader.ReadSingle();
            floatVariant.maxFloat = reader.ReadSingle();
            floatVariant.defaultFloat = reader.ReadSingle();

            return floatVariant;
        }

        public static void Write(this CelesteNetBinaryWriter writer, FloatVariant floatVariant)
        {
            // Write base data
            VariantExt.WriteVariantBase(writer, floatVariant);

            // Write class-specific data
            writer.Write(floatVariant.minFloat);
            writer.Write(floatVariant.maxFloat);
            writer.Write(floatVariant.defaultFloat);
        }
    }
}


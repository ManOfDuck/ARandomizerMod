using System;
using System.Runtime.InteropServices;
using Celeste.Mod.CelesteNet;
using Monocle;

namespace Celeste.Mod.ARandomizerMod
{
	public class FloatVariant : Variant
	{
        public float minFloat;
        public float maxFloat;
        public float defaultFloat;
        public float floatValue;

        public FloatVariant(String name, float minFloat, float maxFloat, float defaultFloat, Level level)
            : base(name, level)
        {
            this.minFloat = minFloat;
            this.maxFloat = maxFloat;
            this.defaultFloat = defaultFloat;
        }

        public void SetValue(float value)
        {
            floatValue = value;
            valueString = value.ToString();
        }

        new public void Trigger()
        {
            if (valueString is null)
            {
                Random random = new();
                SetValue(random.NextFloat(maxFloat - minFloat) + minFloat);
            }
            ExtendedVariantImports.TriggerFloatVariant?.Invoke(name, floatValue, false);
        }

        new public void Reset()
        {
            SetValue(defaultFloat);
            ExtendedVariantImports.TriggerFloatVariant?.Invoke(name, defaultFloat, false);
        }
    }

    public static class FloatVariantExt
    {
        public static FloatVariant ReadFloatVariant(this CelesteNetBinaryReader reader)
        {
            // Read base data
            string name = reader.ReadString();
            Variant.Level level = (Variant.Level)reader.ReadInt32();
            string valueString = reader.ReadString();

            // Read class-specific data
            float minFloat = reader.ReadSingle();
            float maxFloat = reader.ReadSingle();
            float defaultFloat = reader.ReadSingle();
            float floatValue = reader.ReadSingle();

            // Deserialize variant
            FloatVariant floatVariant = new(name, minFloat, maxFloat, defaultFloat, level)
            {
                valueString = valueString,
                floatValue = floatValue
            };

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
            writer.Write(floatVariant.floatValue);
        }
    }
}


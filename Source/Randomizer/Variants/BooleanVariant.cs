using System;
using Celeste.Mod.CelesteNet;

namespace Celeste.Mod.ARandomizerMod
{
    public class BooleanVariant : Variant
    {
        public bool status;

        public Variant[] subVariants = Array.Empty<Variant>();

        public BooleanVariant(String name, bool status, Level level, Variant[] subVariants = null)
            : base(name, level)
        {
            this.status = status;
            this.subVariants = subVariants ?? Array.Empty<Variant>();
        }

        protected override void DoCost()
        {
            if (this.cost != 0) return;

            base.DoCost();

            foreach (Variant subVariant in subVariants)
            {
                subVariant.cost = (int)(this.cost / (float)subVariants.Length);
            }
        }

        override public void SetValue()
        {
            valueString = status.ToString();
        }

        override public void Trigger()
        {
            ExtendedVariantImports.TriggerBooleanVariant?.Invoke(name, status, false);
            foreach (Variant subVariant in subVariants)
                subVariant.Trigger();
        }

        override public void Reset()
        {
            ExtendedVariantImports.TriggerBooleanVariant?.Invoke(name, !status, false);
            foreach (Variant subVariant in subVariants)
                subVariant.Reset();
        }
    }

    public static class BooleanVariantExt
    {
        public static BooleanVariant ReadBooleanVariant(this CelesteNetBinaryReader reader)
        {
            // Read base data
            string name = reader.ReadString();
            Variant.Level level = (Variant.Level)reader.ReadInt32();
            string valueString = reader.ReadString();

            // Read class-specific data
            bool status = reader.ReadBoolean();

            // Read length of sub-variant array
            int numSubVariants = reader.ReadInt32();
            Variant[] subVariants = new Variant[numSubVariants];

            // Read variants to sub-variant array
            for (int i = 0; i < numSubVariants; i++)
            {
                subVariants[i] = reader.ReadVariant();
            }

            // Deserialize variant
            BooleanVariant booleanVariant = new(name, status, level, subVariants)
            {
                valueString = valueString
            };
            return booleanVariant;
        }

        public static void Write(this CelesteNetBinaryWriter writer, BooleanVariant booleanVariant)
        {
            // Write base data
            VariantExt.WriteVariantBase(writer, booleanVariant);

            // Write class-specific data
            writer.Write(booleanVariant.status);

            // Get length of sub-variant array
            writer.Write(booleanVariant.subVariants.Length);

            // Write variants to sub-variant array
            foreach (Variant subVariant in booleanVariant.subVariants)
            {
                writer.Write(subVariant);
            }
        }
    }
}


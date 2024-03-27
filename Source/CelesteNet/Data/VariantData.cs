using System;
using Celeste.Mod.ARandomizerMod.Data;
using Celeste.Mod.CelesteNet;

namespace Celeste.Mod.ARandomizerMod.CelesteNet.Data
{
	public class VariantData : BaseARMData<VariantData>
	{
        static VariantData()
        {
            DataID = "ARandomizerMod_VariantData_" + ARandomizerModModule.ProtocolVersion;
        }

        public Variant variant;

        protected override void Read(CelesteNetBinaryReader reader)
        {
            base.Read(reader);
            variant = reader.ReadVariant();
        }

        protected override void Write(CelesteNetBinaryWriter writer)
        {
            base.Write(writer);
            writer.Write(variant);
        }
    }
}


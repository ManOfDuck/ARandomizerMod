using System;
using Celeste.Mod.ARandomizerMod.Data;
using Celeste.Mod.CelesteNet;
using static MonoMod.InlineRT.MonoModRule;

namespace Celeste.Mod.ARandomizerMod.CelesteNet.Data
{
	public class VariantUpdateData : BaseARMData<VariantUpdateData>
    {
        static VariantUpdateData()
        {
            DataID = "ARandomizerMod_VariantModificationData_" + ARandomizerModModule.ProtocolVersion;
        }

        public enum Operation { ADD, REMOVE }

        public string roomName; 
        public Variant variant;
        public Operation operation;

        protected override void Read(CelesteNetBinaryReader reader)
        {
            base.Read(reader);
            roomName = reader.ReadString();
            variant = reader.ReadVariant();
            operation = (Operation)reader.ReadInt32();
        }

        protected override void Write(CelesteNetBinaryWriter writer)
        {
            base.Write(writer);
            writer.Write(roomName);
            writer.Write(variant);
            writer.Write((int)operation);
        }
    }
}


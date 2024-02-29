using System;
using Celeste.Mod.CelesteNet;

namespace Celeste.Mod.ARandomizerMod.Data
{
	public class TestData : BaseARMData<TestData>
	{
		static TestData()
		{
            DataID = "ARandomizerMod_TestData_" + ARandomizerModModule.ProtocolVersion;
        }

		public string Message;

        protected override void Read(CelesteNetBinaryReader reader)
        {
            base.Read(reader);
            Message = reader.ReadString();
        }

        protected override void Write(CelesteNetBinaryWriter writer)
        {
            base.Write(writer);
            writer.Write(Message);
        }
    }
}


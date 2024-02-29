using System;
using Celeste.Mod.CelesteNet;
using Celeste.Mod.CelesteNet.DataTypes;

namespace Celeste.Mod.ARandomizerMod.Data
{
	abstract public class BaseARMData<T> : DataType<T> where T : BaseARMData<T>
	{
        public DataPlayerInfo player;

        public override MetaType[] GenerateMeta(DataContext ctx)
        {
            return new MetaType[] { new MetaPlayerPrivateState(player) };
        }

        public override void FixupMeta(DataContext ctx)
        {
            player = Get<MetaPlayerPrivateState>(ctx);
        }
    }
}


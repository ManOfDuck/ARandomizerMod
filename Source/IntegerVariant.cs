using System;
namespace Celeste.Mod.ARandomizerMod
{
    public class IntegerVariant : Variant
    {
        public int minInt;
        public int maxInt;
        public int defaultInt;

        public IntegerVariant(String name, int minInt, int maxInt, int defaultInt, Level level)
            : base(name, level)
        {
            this.minInt = minInt;
            this.maxInt = maxInt;
            this.defaultInt = defaultInt;
        }
    }
}


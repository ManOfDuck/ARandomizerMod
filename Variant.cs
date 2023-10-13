using System;
namespace Celeste.Mod.ARandomizerMod
{
    public class Variant
    {
        readonly int greatPrice = -100;
        readonly int goodPrice = -50;
        readonly int nicePrice = -10;
        readonly int sillyPrice = 10;
        readonly int dubiousPrice = 100;
        readonly int tamePrice = 200;
        readonly int nastyPrice = 300;
        readonly int fuckedUpPrice = 500;

        public string name;

        public int? minInt;
        public int? maxInt;
        public int? defaultInt;

        public float? minFloat;
        public float? maxFloat;
        public float? defaultFloat;

        public bool? boolValue;

        public Variant subVariant = null;
        public Variant variant1 = null;
        public Variant variant2 = null;

        public int cost = 0;

        public enum Level { GREAT, GOOD, NICE, SILLY, DUBIOUS, TAME, NASTY, FUCKED_UP, SUB };
        public Level level;

        public string value;

        public Variant(String name, int minInt, int maxInt, int defaultInt, Level level)
		{
			this.name = name;
            this.minInt = minInt;
			this.maxInt = maxInt;
            this.defaultInt = defaultInt;

            this.level = level;

            doCost();
        }

        public Variant(String name, float minFloat, float maxFloat, float defaultFloat, Level level)
        {
            this.name = name;
            this.minFloat = minFloat;
            this.maxFloat = maxFloat;
            this.defaultFloat = defaultFloat;

            this.level = level;

            doCost();
        }

        public Variant(String name, bool value, Level level)
        {
            this.name = name;
            this.boolValue = value;

            this.level = level;

            doCost();
        }

        public Variant(String name, bool value, Variant subVariant, Level level)
        {
            this.name = name;
            this.boolValue = value;
            this.subVariant = subVariant;

            this.level = level;

            doCost();
            this.subVariant.cost = this.cost;
        }

        public Variant (Variant variant1, Variant variant2)
        {
            this.variant1 = variant1;
            this.variant2 = variant2;

            if (this.cost != 0)
            {
                variant1.cost = this.cost / 2;
                variant2.cost = this.cost / 2;
            }
        }

        public void doCost()
        {
            if (cost != 0) return;

            switch (level)
            {
                case Level.GREAT:
                    cost = greatPrice;
                    break;
                case Level.GOOD:
                    cost = goodPrice;
                    break;
                case Level.NICE:
                    cost = nicePrice;
                    break;
                case Level.SILLY:
                    cost = sillyPrice;
                    break;
                case Level.DUBIOUS:
                    cost = dubiousPrice;
                    break;
                case Level.TAME:
                    cost = tamePrice;
                    break;
                case Level.NASTY:
                    cost = nastyPrice;
                    break;
                case Level.FUCKED_UP:
                    cost = fuckedUpPrice;
                    break;

            }
        }
    }
}


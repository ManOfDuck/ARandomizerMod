using System;
namespace Celeste.Mod.ARandomizerMod
{
    public class Variant
    {
        readonly static int greatPrice = -100;
        readonly static int goodPrice = -50;
        readonly static int nicePrice = -10;
        readonly static int sillyPrice = 10;
        readonly static int dubiousPrice = 100;
        readonly static int tamePrice = 200;
        readonly static int nastyPrice = 300;
        readonly static int fuckedUpPrice = 500;

        public string name;

        public int cost;

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


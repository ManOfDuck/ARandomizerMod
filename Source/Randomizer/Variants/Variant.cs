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

        public int cost { get => DoCost(); } 

        public enum Level { GREAT, GOOD, NICE, SILLY, DUBIOUS, TAME, NASTY, FUCKED_UP, SUB };
        public Level level;

        public string value;

        public Variant(String name, Level level)
        {
            this.name = name;
            this.level = level;
            DoCost();
        }

        virtual protected int DoCost()
        {
            int retCost = 0;

            switch (level)
            {
                case Level.GREAT:
                    retCost = greatPrice;
                    break;
                case Level.GOOD:
                    retCost = goodPrice;
                    break;
                case Level.NICE:
                    retCost = nicePrice;
                    break;
                case Level.SILLY:
                    retCost = sillyPrice;
                    break;
                case Level.DUBIOUS:
                    retCost = dubiousPrice;
                    break;
                case Level.TAME:
                    retCost = tamePrice;
                    break;
                case Level.NASTY:
                    retCost = nastyPrice;
                    break;
                case Level.FUCKED_UP:
                    retCost = fuckedUpPrice;
                    break;
            }

            return retCost;
        }
    }
}


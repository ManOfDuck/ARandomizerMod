using System;
namespace Celeste.Mod.ARandomizerMod
{
    public class Variant
    {
        public string name;
        public int minInt;
        public int maxInt;
        public float minFloat;
        public float maxFloat;
        public bool value;

        public Variant(String name, int minInt, int maxInt)
		{
			this.name = name;
                
			this.minInt = minInt;
			this.maxInt = maxInt;
		}

        public Variant(String name, float minFloat, float maxFloat)
        {
            this.name = name;
            this.minFloat = minFloat;
            this.maxFloat = maxFloat;
        }

        public Variant(String name, bool value)
        {
            this.name = name;
            this.value = value;
        }
    }
}


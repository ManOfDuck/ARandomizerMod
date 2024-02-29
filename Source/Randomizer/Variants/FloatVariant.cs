using System;
namespace Celeste.Mod.ARandomizerMod
{
	public class FloatVariant : Variant
	{
        public float minFloat;
        public float maxFloat;
        public float defaultFloat;

        public FloatVariant(String name, float minFloat, float maxFloat, float defaultFloat, Level level)
            : base(name, level)
        {
            this.minFloat = minFloat;
            this.maxFloat = maxFloat;
            this.defaultFloat = defaultFloat;
        }
    }
}


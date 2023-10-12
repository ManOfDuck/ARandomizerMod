﻿using System;
namespace Celeste.Mod.ARandomizerMod
{
    public class Variant
    {
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
        }

        public Variant(String name, float minFloat, float maxFloat, float defaultFloat, Level level)
        {
            this.name = name;
            this.minFloat = minFloat;
            this.maxFloat = maxFloat;
            this.defaultFloat = defaultFloat;

            this.level = level;
        }

        public Variant(String name, bool value, Level level)
        {
            this.name = name;
            this.boolValue = value;

            this.level = level;
        }

        public Variant(String name, bool value, Variant subVariant, Level level)
        {
            this.name = name;
            this.boolValue = value;
            this.subVariant = subVariant;

            this.level = level;
        }

        public Variant (Variant variant1, Variant variant2)
        {
            this.variant1 = variant1;
            this.variant2 = variant2;
        }
    }
}


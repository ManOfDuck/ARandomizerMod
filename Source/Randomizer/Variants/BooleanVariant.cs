using System;
namespace Celeste.Mod.ARandomizerMod
{
	public class BooleanVariant : Variant
	{
        public bool status;

        public Variant[] subVariants = Array.Empty<Variant>();

        public BooleanVariant(String name, bool status, Level level, Variant[] subVariants = null)
            : base(name, level)
        {
            this.status = status;
            this.subVariants = subVariants ?? Array.Empty<Variant>();
        }

        public override void DoCost()
        {
            if (this.cost != 0) return;

            base.DoCost();

            foreach (Variant variant in subVariants)
            {
                variant.cost = (int)((float) this.cost / subVariants.Length);
            }
        }
    }
}


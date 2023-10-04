using System;
using MonoMod.ModInterop;

namespace Celeste.Mod.ARandomizerMod
{
	[ModImportName("ExtendedVariantMode")]
	public static class ExtendedVariantImports
	{
		public static Action<string, bool, bool> TriggerBooleanVariant;
	}
}
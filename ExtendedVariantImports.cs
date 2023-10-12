using System;
using MonoMod.ModInterop;

namespace Celeste.Mod.ARandomizerMod
{
	[ModImportName("ExtendedVariantMode")]
	public static class ExtendedVariantImports
	{
		public static Action<string, int, bool> TriggerIntegerVariant;

		public static Action<string, bool, bool> TriggerBooleanVariant;

		public static Action<string, float, bool> TriggerFloatVariant;

		public static Action<string, object, bool> TriggerVariant;
	}
}
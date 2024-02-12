using Monocle;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Celeste.Mod.ARandomizerMod
{
    public class VariantManager
    {
        public LinkedList<Variant> activeVariants = new();
        
        public void TriggerVariant(Variant variant)
        {
            Logger.Log(LogLevel.Debug, "ARandomizerMod", "Activating variant " + variant.name + "...");
            Random random = new();

            switch (variant)
            {
                case IntegerVariant integerVariant:
                    int intValue = random.Next(integerVariant.minInt, integerVariant.maxInt);
                    ExtendedVariantImports.TriggerIntegerVariant?.Invoke(variant.name, intValue, false);
                    variant.value = intValue.ToString();
                    break;
                case FloatVariant floatVariant:
                    float floatValue = random.NextFloat(floatVariant.maxFloat - floatVariant.minFloat) + floatVariant.minFloat;
                    ExtendedVariantImports.TriggerFloatVariant?.Invoke(variant.name, floatValue, false);
                    variant.value = floatValue.ToString();
                    break;
                case BooleanVariant booleanVariant:
                    bool boolValue = booleanVariant.status;
                    ExtendedVariantImports.TriggerBooleanVariant?.Invoke(variant.name, boolValue, false);
                    variant.value = boolValue.ToString();
                    foreach (Variant subVariant in booleanVariant.subVariants)
                        TriggerVariant(subVariant);
                    break;
                case null:
                    Logger.Log(LogLevel.Error, "ARandomizerMod", "JESSE: Null variant triggered");
                    return;
            }

            foreach (Variant activeVariant in activeVariants)
            {
                if (activeVariant.name.Equals(variant.name))
                {
                    activeVariants.Remove(activeVariant);
                    break;
                }
            }

            activeVariants.AddLast(variant);
            Logger.Log(LogLevel.Debug, "ARandomizerMod", "Activated variant " + variant.name + " with value " + variant.value);
        }

        public void ResetRandomVariant()
        {
            if (activeVariants.Count < 1) return;

            int variantToReset = new Random().Next(activeVariants.Count);
            LinkedListNode<Variant> node = activeVariants.First;
            for (int i = 0; i < activeVariants.Count; i++)
            {
                if (i >= variantToReset && node.Value.cost < 200)
                {
                    ResetVariant(node.Value);
                    return;
                }

                if (node.Next == null) return;
                node = node.Next;
            }
        }

        public void ResetAllVariants()
        {
            //Idk what this does but VS told me it was better than .ToArray()
            Variant[] variantsToReset = [.. activeVariants];

            foreach (Variant variant in variantsToReset)
            {
                ResetVariant(variant);
            }

            activeVariants.Clear();
        }

        public void ResetVariant(Variant variant)
        {
            Logger.Log(LogLevel.Debug, "ARandomizerMod", "Resetting variant " + variant.name + "...");

            switch (variant)
            {
                case IntegerVariant integerVariant:
                    ExtendedVariantImports.TriggerIntegerVariant?.Invoke(integerVariant.name, integerVariant.defaultInt, false);
                    variant.value = integerVariant.value;
                    break;
                case FloatVariant floatVariant:
                    ExtendedVariantImports.TriggerFloatVariant?.Invoke(floatVariant.name, floatVariant.defaultFloat, false);
                    variant.value = floatVariant.value;
                    break;
                case BooleanVariant booleanVariant:
                    ExtendedVariantImports.TriggerBooleanVariant?.Invoke(booleanVariant.name, !booleanVariant.status, false);
                    variant.value = booleanVariant.value;
                    foreach (Variant subVariant in booleanVariant.subVariants)
                        ResetVariant(subVariant);
                    break;
                case null:
                    Logger.Log(LogLevel.Error, "ARandomizerMod", "JESSE: Null variant triggered");
                    return;
            }

            activeVariants.Remove(variant);
            Logger.Log(LogLevel.Debug, "ARandomizerMod", "Reset variant " + variant.name + " to value " + variant.value);
        }
    }
}

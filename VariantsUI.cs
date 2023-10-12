using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Monocle;
using Celeste;

namespace Celeste.Mod.ARandomizerMod
{
	public class VaraintsUI : Entity
	{
        public bool render = false;

        public VaraintsUI()
        {
            AddTag(Tags.HUD);
        }

        public override void Render()
        {
            base.Render();

            if (!render)
                return;

            Draw.Rect(new Vector2(10, 250), 500, 250, new(0, 0, 0, 200));
            ActiveFont.Draw("Test UI Box", new Vector2(10, 325), Color.White);
        }

        public override void Update()
        {
            base.Update();

            if (ARandomizerModModule.Settings.OpenVariantsMenu.Pressed)
            {
                EnableUI();
            }
            if (ARandomizerModModule.Settings.OpenVariantsMenu.Released)
            {
                DisableUI();
            }
        }

        public void EnableUI()
        {
            Player player = Scene.Tracker.GetEntity<Player>();
            if (player is null) return;

            render = true;
            player.StateMachine.State = Player.StDummy;
        }

        public void DisableUI()
        {
            render = false;

            Player player = Scene.Tracker.GetEntity<Player>();
            if (player is null) return;

            player.StateMachine.State = Player.StNormal;
        }

        public void TriggerVariant(Variant variant)
        {
            Random random = new Random();

            if (variant.name != null)
            {
                
            }

            if (variant.minInt.HasValue && variant.maxInt.HasValue && variant.defaultInt.HasValue)
            {
                int value = random.Next(variant.minInt.Value, variant.maxInt.Value);
                Logger.Log(LogLevel.Warn, "ARandomizerMod", "Triggering " + variant.name + " with int " + value);

                ExtendedVariantImports.TriggerIntegerVariant?.Invoke(variant.name, value, false);
            }
            else if (variant.minFloat.HasValue && variant.maxFloat.HasValue && variant.defaultFloat.HasValue)
            {
                float value = random.NextFloat(variant.maxFloat.Value - variant.minFloat.Value) + variant.minFloat.Value;
                value = (float) Math.Round((decimal)value, 2);
                Logger.Log(LogLevel.Warn, "ARandomizerMod", "Triggering " + variant.name + " with float " + value);

                ExtendedVariantImports.TriggerFloatVariant?.Invoke(variant.name, value, false);
            }
            else if (variant.value.HasValue)
            {
                Logger.Log(LogLevel.Warn, "ARandomizerMod", "Triggering " + variant.name + " with bool " + variant.value.Value);
                ExtendedVariantImports.TriggerBooleanVariant?.Invoke(variant.name, variant.value.Value, false);

                if (variant.subVariant != null)
                    TriggerVariant(variant.subVariant);
            }
            else if (variant.variant1 != null && variant.variant2 != null)
            {
                Logger.Log(LogLevel.Warn, "ARandomizerMod", "Triggering " + variant.name + " with subvariants");

                TriggerVariant(variant.variant1);
                TriggerVariant(variant.variant2);
            }
        }

        public void ResetVariant(Variant variant)
        {
            if (variant.minInt.HasValue && variant.maxInt.HasValue && variant.defaultInt.HasValue)
            {
                ExtendedVariantImports.TriggerIntegerVariant?.Invoke(variant.name, variant.defaultInt.Value, false);
            }
            else if (variant.minFloat.HasValue && variant.maxFloat.HasValue && variant.defaultFloat.HasValue)
            {
                ExtendedVariantImports.TriggerFloatVariant?.Invoke(variant.name, variant.defaultFloat.Value, false);
            }
            if (variant.value.HasValue)
            {
                ExtendedVariantImports.TriggerBooleanVariant?.Invoke(variant.name, !variant.value.Value, false);
                if (variant.subVariant != null)
                {
                    ResetVariant(variant.subVariant);
                }
            }
            else if (variant.variant1 != null && variant.variant2 != null)
            {
                ResetVariant(variant.variant1);
                ResetVariant(variant.variant2);
            }
        }
    }
}


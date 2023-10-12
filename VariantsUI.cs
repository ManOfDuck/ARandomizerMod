using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Monocle;
using Celeste;
using System.Collections.Generic;

namespace Celeste.Mod.ARandomizerMod
{
	public class VaraintsUI : Entity
	{
        public bool render = false;
        public LinkedList<Variant> activeVariants = new LinkedList<Variant>();
        public LinkedListNode<Variant> selectedNode;
        
        public int topHeight = 250;
        public int width = 700;
        public int minLines = 4;
        public int verticalPadding = 10;
        public int offset = 0;
        public Color color = new(0, 0, 0, 230);

        public int lineHeight = 45;
        public int textOffset = 15;
        public float textScale = 0.7f;
        public Color textColor = Color.White;

        public int subVariantOffset = 40;
        public Color selectionColor = Color.LightGreen;

        public VaraintsUI()
        {
            AddTag(Tags.HUD);
        }

        public override void Render()
        {
            base.Render();

            if (!render)
                return;

            RenderBackground();

            if (activeVariants.Count < 1)
            {
                RenderText("No Active Variants", textColor, 0, 0);
            }
            else
            {
                RenderActiveVariants();
            }
        }

        private void RenderBackground()
        {
            int height = Math.Max(minLines * lineHeight, (lineHeight * activeVariants.Count) + (verticalPadding * 2));
            Draw.Rect(new Vector2(offset, topHeight), width, height, color);
        }

        private void RenderActiveVariants()
        {
            LinkedListNode<Variant> node = activeVariants.First;
            for (int line = 0; line < activeVariants.Count; line++)
            {
                if (node == null) break; // for safety, should never happen
                RenderVariant(node.Value, line);     
                node = node.Next;
            }
        }

        private void RenderVariant(Variant variant, float line)
        {
            Color color = (selectedNode is not null && selectedNode.Value.name == variant.name) ? selectionColor : textColor;
            int offset = (variant.level == Variant.Level.SUB) ? subVariantOffset : 0;
            string text = variant.name + ": " + ExtendedVariantImports.GetCurrentVariantValue?.Invoke(variant.name);
            
            RenderText(text, color, line, offset);
        }

        private void RenderText(String text, Color color, float line, float lineOffset)
        {
            float xPos = offset + textOffset + lineOffset;
            float yPos = topHeight + verticalPadding + (lineHeight * line);
            ActiveFont.Draw(text, new Vector2(xPos, yPos), Vector2.Zero, new Vector2(textScale, textScale), color);
        }

        public override void Update()
        {
            base.Update();

            if (ARandomizerModModule.Settings.OpenVariantsMenu.Check)
            {
                EnableUI();
            }
            if (ARandomizerModModule.Settings.OpenVariantsMenu.Released)
            {
                DisableUI();
                selectedNode = null;
            }

            if (render && activeVariants.Count > 0)
            {
                NavigateUI();
            }   
        }

        public void NavigateUI()
        {
            if (ARandomizerModModule.Settings.NavigateUp.Pressed)
            {
                ARandomizerModModule.Settings.NavigateUp.ConsumePress();
                if (selectedNode == null)
                {
                    selectedNode = activeVariants.First;
                }
                else if (selectedNode.Previous != null)
                {
                    selectedNode = selectedNode.Previous;
                }
                else
                {
                    selectedNode = activeVariants.Last;
                }
            }
            else if (ARandomizerModModule.Settings.NavigateDown.Pressed)
            {
                ARandomizerModModule.Settings.NavigateDown.ConsumePress();
                if (selectedNode == null)
                {
                    selectedNode = activeVariants.First;
                }
                else if (selectedNode.Next != null)
                {
                    selectedNode = selectedNode.Next;
                }
                else
                {
                    selectedNode = activeVariants.First;
                }
            }

            if (ARandomizerModModule.Settings.Select.Pressed && selectedNode != null && selectedNode.Value != null)
            {
                ResetVariant(selectedNode.Value);
            }
        }

        public void EnableUI()
        {
            Player player = Scene?.Tracker?.GetEntity<Player>();
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

            if (variant is null)
            {
                Logger.Log(LogLevel.Error, "ARandomizerMod", "UH OH!");
                return;
            }

            if (variant.name != null)
            {
                ResetVariantsWithName(variant.name);
            }

            if (variant.minInt.HasValue && variant.maxInt.HasValue && variant.defaultInt.HasValue)
            {
                int value = random.Next(variant.minInt.Value, variant.maxInt.Value);
                Logger.Log(LogLevel.Warn, "ARandomizerMod", "Triggering " + variant.name + " with int " + value);

                ExtendedVariantImports.TriggerIntegerVariant?.Invoke(variant.name, value, false);
                variant.value = value.ToString();
                activeVariants.AddLast(variant);
            }
            else if (variant.minFloat.HasValue && variant.maxFloat.HasValue && variant.defaultFloat.HasValue)
            {
                float value = random.NextFloat(variant.maxFloat.Value - variant.minFloat.Value) + variant.minFloat.Value;
                value = (float) Math.Round((decimal)value, 2);
                Logger.Log(LogLevel.Warn, "ARandomizerMod", "Triggering " + variant.name + " with float " + value);

                ExtendedVariantImports.TriggerFloatVariant?.Invoke(variant.name, value, false);
                variant.value = value.ToString();
                activeVariants.AddLast(variant);
            }
            else if (variant.boolValue.HasValue)
            {
                Logger.Log(LogLevel.Warn, "ARandomizerMod", "Triggering " + variant.name + " with bool " + variant.boolValue.Value);
                ExtendedVariantImports.TriggerBooleanVariant?.Invoke(variant.name, variant.boolValue.Value, false);

                variant.value = variant.boolValue.Value.ToString();
                activeVariants.AddLast(variant);

                if (variant.subVariant != null)
                {
                    Logger.Log(LogLevel.Warn, "ARandomizerMod", variant.name + " has subVariant, triggering subVariant:");
                    TriggerVariant(variant.subVariant);
                }
            }
            else if (variant.variant1 != null && variant.variant2 != null)
            {
                Logger.Log(LogLevel.Warn, "ARandomizerMod", "Triggering variant with subvariants:");

                TriggerVariant(variant.variant1);
                TriggerVariant(variant.variant2);
            }
        }

        public void ResetVariantsWithName(String name)
        {

            LinkedListNode<Variant> node = activeVariants.First;
            for (int i = 0; i < activeVariants.Count; i++)
            {
                if (node.Value != null && node.Value.name == name)
                {
                    ResetVariant(node.Value);
                }
                node = node.Next;
            }
        }

        public void ResetRandomVariant()
        {

        }

        public void ResetVariant(Variant variant)
        {
            if (variant.name is not null)
            {
                Logger.Log(LogLevel.Warn, "ARandomizerMod", "Resetting " + variant.name);
            }
            if (variant.defaultInt.HasValue)
            {
                ExtendedVariantImports.TriggerIntegerVariant?.Invoke(variant.name, variant.defaultInt.Value, false);
                activeVariants.Remove(variant);
            }
            else if (variant.defaultFloat.HasValue)
            {
                ExtendedVariantImports.TriggerFloatVariant?.Invoke(variant.name, variant.defaultFloat.Value, false);
                activeVariants.Remove(variant);
            }
            else if (variant.boolValue.HasValue)
            {
                ExtendedVariantImports.TriggerBooleanVariant?.Invoke(variant.name, !variant.boolValue.Value, false);
                activeVariants.Remove(variant);

                if (variant.subVariant != null)
                {
                    ResetVariant(variant.subVariant);
                }
            }
            if (variant.variant1 != null && variant.variant2 != null)
            {
                ResetVariant(variant.variant1);
                ResetVariant(variant.variant2);
            }
        }
    }
}


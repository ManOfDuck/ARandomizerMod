using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Monocle;
using Celeste;
using System.Collections.Generic;
using System.Collections;
using MonoMod.Utils;

namespace Celeste.Mod.ARandomizerMod
{
	public class VaraintsUI : Entity
	{
        public bool render = false;
        public bool controlsDisabled = false;
        public LinkedListNode<Variant> selectedNode;

        readonly VariantManager variantManager;
        readonly EconomyManager economyManager;

        readonly int topHeight = 250;
        readonly int width = 730;
        readonly int minLines = 4;
        readonly int verticalPadding = 10;
        readonly int offset = 0;
        readonly Color color = new(0, 0, 0, 230);

        readonly int lineHeight = 50;
        readonly int textOffset = 5;
        readonly float textScale = 0.64f;
        readonly Color textColor = Color.White; 

        readonly int moneyHeight = 80;
        readonly int moneyPadding = 20;
        readonly int moneyOffset = 0; 
        readonly float moneyScale = 0.92f;
        readonly Color moneyColor = Color.LightYellow;

        readonly int smallMoneyHeight = 70;
        readonly int smallMoneyPadding = 20;
        readonly int smallMoneyOffset = 0;
        readonly float smallMoneyScale = 0.85f;
        readonly Color smallMoneyColor = Color.Ivory;
         
        readonly int scoreOffset = 730 - 250;
        readonly int scorePadding = 10;
        readonly float scoreScale = 0.64f;
        readonly Color scoreColor = Color.White;
        
        readonly int costOffset = 105;
        readonly Color veryCheapColor = Color.LightGray;
        readonly Color sellColor = Color.PaleGoldenrod;
        readonly Color sellSelectedColor = Color.Goldenrod;
        readonly Color canAffordColor = Color.Green;
        readonly Color canAffordSelectedColor = Color.GreenYellow;
        readonly Color cantAffordColor = Color.DarkRed;
        readonly Color cantAffordSelectedColor = Color.Red;

        readonly int subVariantOffset = 40;
        readonly Color selectionColor = Color.LightGreen;
        readonly Color cantAffordSelectionColor = Color.LightGray;

        public VaraintsUI(VariantManager variantManager, EconomyManager economyManager)
        {
            this.variantManager = variantManager;
            this.economyManager = economyManager;

            AddTag(Tags.HUD);

           //On.Monocle.Binding.Pressed += Pressed;
        }

        private bool Pressed(On.Monocle.Binding.orig_Pressed orig, Binding self, int gamepadIndex, float threshold)
        {
            
            return !render && orig(self, gamepadIndex, threshold);
        }

        #region Rendering
        public override void Render()
        {
            base.Render();

            if (!render)
                return;

            RenderBackground();
            RenderScore();

            if (selectedNode is not null)
            {
                RenderMoneyBig();
            }
            else
            {
                RenderMoneySmall();
            }

            if (variantManager.activeVariants.Count < 1)
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
            int moneySpace = (selectedNode is not null) ? moneyHeight : smallMoneyHeight;
            int height = Math.Max(minLines * lineHeight, (lineHeight * variantManager.activeVariants.Count) + (verticalPadding * 2)) + moneySpace;
            Draw.Rect(new Vector2(offset, topHeight - moneySpace), width, height, color);
        }

        private void RenderScore()
        {
            float xPos = offset + textOffset + scoreOffset;
            int moneySpace = (selectedNode is not null) ? moneyHeight : smallMoneyHeight;
            float yPos = topHeight - moneySpace + scorePadding;
            ActiveFont.Draw("Score: " + economyManager.score, new Vector2(xPos, yPos), Vector2.Zero, new Vector2(scoreScale, scoreScale), scoreColor);
        }

        private void RenderMoneyBig()
        {
            float xPos = offset + textOffset + moneyOffset;
            float yPos = topHeight - moneyHeight + moneyPadding;
            ActiveFont.Draw("$" + economyManager.money, new Vector2(xPos, yPos), Vector2.Zero, new Vector2(moneyScale, moneyScale), moneyColor);
        }

        private void RenderMoneySmall()
        {
            float xPos = offset + textOffset + smallMoneyOffset;
            float yPos = topHeight - smallMoneyHeight + smallMoneyPadding;
            ActiveFont.Draw("$" + economyManager.money, new Vector2(xPos, yPos), Vector2.Zero, new Vector2(smallMoneyScale, smallMoneyScale), smallMoneyColor);
        }

        private void RenderActiveVariants()
        {
            LinkedListNode<Variant> node = variantManager.activeVariants.First;
            for (int line = 0; line < variantManager.activeVariants.Count; line++)
            {
                if (node == null) break; // for safety, should never happen
                RenderVariant(node.Value, line);
                if (selectedNode is not null)
                {
                    RenderCost(node.Value, line);
                }
                node = node.Next;
            }
        }

        private void RenderVariant(Variant variant, float line)
        {
            Color color;
            if (selectedNode is not null && selectedNode.Value.name == variant.name)
            {
                color = (variant.cost <= economyManager.money) ? selectionColor : cantAffordSelectionColor;
            }
            else
            {
                color = textColor;
            }
            int offset = (variant.level == Variant.Level.SUB) ? subVariantOffset : 0;
            offset += (selectedNode is not null) ? costOffset : textOffset;
            string text = variant.name + ": " + variant.value;
            
            RenderText(text, color, line, offset);
        }

        private void RenderCost(Variant variant, float line)
        {
            Color color;
            if (variant.cost < 0)
            {
                color = (selectedNode.Value.name.Equals(variant.name)) ? sellColor : sellSelectedColor;
            }
            else if (variant.cost <= economyManager.money)
            {
                color = (selectedNode.Value.name.Equals(variant.name)) ? canAffordSelectedColor : canAffordColor;
            }
            else
            {
                color = (selectedNode.Value.name.Equals(variant.name)) ? cantAffordSelectedColor : cantAffordColor;
            }
            int offset = textOffset;
            string prefix = (variant.cost < 0) ? "+$" : "$";
            string text = prefix + Math.Abs(variant.cost);

            RenderText(text, color, line, offset);
        }

        private void RenderText(String text, Color color, float line, float lineOffset)
        {
            float xPos = offset + textOffset + lineOffset;
            float yPos = topHeight + verticalPadding + (lineHeight * line);
            ActiveFont.Draw(text, new Vector2(xPos, yPos), Vector2.Zero, new Vector2(textScale, textScale), color);
        }
        #endregion

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

            if (controlsDisabled)
            {
                Input.MoveX.Value = 0;
                Input.MoveY.Value = 0;
            }

            if (render && variantManager.activeVariants.Count > 0)
            {
                NavigateUI();
            }
        }

        public void EnableUI()
        {
            render = true;
            DisableControls();
        }

        public void DisableUI()
        {
            render = false;
            EnableControls();
        }

        // Hack - Trying to do any of this through input hooks bore no fruit after <6 hours, using Extended Variants instead. Works perfectly but pretty jank
        private void DisableControls()
        {
            controlsDisabled = true;

            ExtendedVariantImports.TriggerIntegerVariant?.Invoke("JumpCount", 0, false);
            ExtendedVariantImports.TriggerIntegerVariant?.Invoke("DashRestriction", 2, false);
            ExtendedVariantImports.TriggerBooleanVariant?.Invoke("DisableWallJumping", true, false);
            ExtendedVariantImports.TriggerBooleanVariant?.Invoke("NoGrabbing", true, false);
        }

        private void EnableControls()
        {
            controlsDisabled = false;

            int jumpCount = Int32.Parse(variantManager.GetVariantWithName("JumpCount")?.value ?? "1");
            int dashRestriction = Int32.Parse(variantManager.GetVariantWithName("DashRestriction")?.value ?? "0");
            bool disableWallJumping = Boolean.Parse(variantManager.GetVariantWithName("DisableWallJumping")?.value ?? "False");
            bool noGrabbing = Boolean.Parse(variantManager.GetVariantWithName("NoGrabbing")?.value ?? "False");

            ExtendedVariantImports.TriggerIntegerVariant?.Invoke("JumpCount", jumpCount, false);
            ExtendedVariantImports.TriggerIntegerVariant?.Invoke("DashRestriction", dashRestriction, false);
            ExtendedVariantImports.TriggerBooleanVariant?.Invoke("DisableWallJumping", disableWallJumping, false);
            ExtendedVariantImports.TriggerBooleanVariant?.Invoke("NoGrabbing", noGrabbing, false);
        }

        #region Navigation
        public void NavigateUI()
        {
            if (ARandomizerModModule.Settings.NavigateUp.Pressed)
            {
                ARandomizerModModule.Settings.NavigateUp.ConsumePress();
                NavigateUp();
            }
            else if (ARandomizerModModule.Settings.NavigateDown.Pressed)
            {
                ARandomizerModModule.Settings.NavigateDown.ConsumePress();
                NavigateDown();
            }

            if (ARandomizerModModule.Settings.Select.Pressed && selectedNode != null && selectedNode.Value != null)
            {
                ARandomizerModModule.Settings.Select.ConsumePress();
                if (selectedNode.Value.cost <= economyManager.money)
                {
                    economyManager.PurchaseVariantRemoval(selectedNode.Value);
                    NavigateDown();
                }
            }
        }

        public void NavigateUp()
        {
            if (selectedNode == null)
            {
                selectedNode = variantManager.activeVariants.Last;
            }
            else if (selectedNode.Previous != null)
            {
                selectedNode = selectedNode.Previous;
            }
            else
            {
                selectedNode = variantManager.activeVariants.Last;
            }
        }

        public void NavigateDown()
        {
            if (selectedNode == null)
            {
                selectedNode = variantManager.activeVariants.First;
            }
            else if (selectedNode.Next != null)
            {
                selectedNode = selectedNode.Next;
            }
            else
            {
                selectedNode = variantManager.activeVariants.First;
            }
        }
        #endregion
    }
}


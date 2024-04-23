using System;
using Microsoft.Xna.Framework;
using Monocle;
using System.Collections.Generic;

namespace Celeste.Mod.ARandomizerMod
{
    public class VaraintsUI : Entity
	{
        public bool render = false;
        public static bool ControlsDisabled { get; private set; } = false;
        public LinkedListNode<Variant> selectedNode;
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
        readonly Color sellColor = Color.PaleGoldenrod;
        readonly Color sellSelectedColor = Color.Goldenrod;
        readonly Color canAffordColor = Color.Green;
        readonly Color canAffordSelectedColor = Color.GreenYellow;
        readonly Color cantAffordColor = Color.DarkRed;
        readonly Color cantAffordSelectedColor = Color.Red;

        readonly int subVariantOffset = 40;
        readonly Color selectionColor = Color.LightGreen;
        readonly Color cantAffordSelectionColor = Color.LightGray;

        public VaraintsUI()
        {
            Tag = Tags.HUD | Tags.Global;

            On.Monocle.Binding.Pressed += Pressed;
        }

        private static bool Pressed(On.Monocle.Binding.orig_Pressed orig, Binding self, int gamepadIndex, float threshold)
        {
            if (ControlsDisabled)
            {
                Input.MoveX.Value = 0;
                Input.MoveY.Value = 0;
            }
            return orig(self, gamepadIndex, threshold);
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

            if (VariantManager.ActiveVariants.Count < 1)
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
            int height = Math.Max(minLines * lineHeight, (lineHeight * VariantManager.ActiveVariants.Count) + (verticalPadding * 2)) + moneySpace;
            Draw.Rect(new Vector2(offset, topHeight - moneySpace), width, height, color);
        }

        private void RenderScore()
        {
            float xPos = offset + textOffset + scoreOffset;
            int moneySpace = (selectedNode is not null) ? moneyHeight : smallMoneyHeight;
            float yPos = topHeight - moneySpace + scorePadding;
            ActiveFont.Draw("Score: " + EconomyManager.Score, new Vector2(xPos, yPos), Vector2.Zero, new Vector2(scoreScale, scoreScale), scoreColor);
        }

        private void RenderMoneyBig()
        {
            float xPos = offset + textOffset + moneyOffset;
            float yPos = topHeight - moneyHeight + moneyPadding;
            ActiveFont.Draw("$" + EconomyManager.Money, new Vector2(xPos, yPos), Vector2.Zero, new Vector2(moneyScale, moneyScale), moneyColor);
        }

        private void RenderMoneySmall()
        {
            float xPos = offset + textOffset + smallMoneyOffset;
            float yPos = topHeight - smallMoneyHeight + smallMoneyPadding;
            ActiveFont.Draw("$" + EconomyManager.Money, new Vector2(xPos, yPos), Vector2.Zero, new Vector2(smallMoneyScale, smallMoneyScale), smallMoneyColor);
        }

        private void RenderActiveVariants()
        {
            LinkedListNode<Variant> node = VariantManager.ActiveVariants.First;
            int line = 0;
            while (node != null)
            {
                if (selectedNode is not null)
                {
                    RenderCost(node.Value, line);
                }
                RenderVariant(node.Value, ref line);
                node = node.Next;
            }
        }

        private void RenderVariant(Variant variant, ref int line)
        {
            Color color;
            if (selectedNode is not null && selectedNode.Value.name == variant.name)
            {
                color = (variant.Cost <= EconomyManager.Money) ? selectionColor : cantAffordSelectionColor;
            }
            else
            {
                color = textColor;
            }
            int offset = (variant.level == Variant.Level.SUB) ? subVariantOffset : 0;
            offset += (selectedNode is not null) ? costOffset : textOffset;
            string text = variant is BooleanVariant ? variant.name : variant.name + ": " + variant.valueString;
            
            RenderText(text, color, line, offset);
            line++;

            if (variant is BooleanVariant booleanVariant)
            {
                foreach(Variant subVariant in booleanVariant.subVariants)
                {
                    RenderVariant(subVariant, ref line);
                }
            }
        }

        private void RenderCost(Variant variant, float line)
        {
            Color color;
            if (variant.Cost < 0)
            {
                color = selectedNode.Value.name.Equals(variant.name) ? sellColor : sellSelectedColor;
            }
            else if (variant.Cost <= EconomyManager.Money)
            {
                color = selectedNode.Value.name.Equals(variant.name) ? canAffordSelectedColor : canAffordColor;
            }
            else
            {
                color = selectedNode.Value.name.Equals(variant.name) ? cantAffordSelectedColor : cantAffordColor;
            }
            int offset = textOffset;
            string prefix = (variant.Cost < 0) ? "+$" : "$";
            string text = prefix + Math.Abs(variant.Cost);

            RenderText(text, color, line, offset);
        }

        private void RenderText(string text, Color color, float line, float lineOffset)
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
            if (ControlsDisabled)
            {
                Input.MoveX.Value = 0;
                Input.MoveY.Value = 0;
            }
            if (render && VariantManager.ActiveVariants.Count > 0)
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
        private static void DisableControls()
        {
            ControlsDisabled = true;

            ExtendedVariantImports.TriggerIntegerVariant?.Invoke("JumpCount", 0, false);
            ExtendedVariantImports.TriggerIntegerVariant?.Invoke("DashRestriction", 2, false);
            ExtendedVariantImports.TriggerBooleanVariant?.Invoke("DisableWallJumping", true, false);
            ExtendedVariantImports.TriggerBooleanVariant?.Invoke("NoGrabbing", true, false);
        }

        private static void EnableControls()
        {
            ControlsDisabled = false;

            int jumpCount = int.Parse(VariantManager.GetVariantWithName("JumpCount")?.valueString ?? "1");
            int dashRestriction = int.Parse(VariantManager.GetVariantWithName("DashRestriction")?.valueString ?? "0");
            bool disableWallJumping = bool.Parse(VariantManager.GetVariantWithName("DisableWallJumping")?.valueString ?? "False");
            bool noGrabbing = bool.Parse(VariantManager.GetVariantWithName("NoGrabbing")?.valueString ?? "False");

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
                if (selectedNode.Value.Cost <= EconomyManager.Money)
                {
                    EconomyManager.PurchaseVariantRemoval(selectedNode.Value);
                    NavigateDown();
                }
            }
        }

        public void NavigateUp()
        {
            if (selectedNode == null)
            {
                selectedNode = VariantManager.ActiveVariants.Last;
            }
            else if (selectedNode.Previous != null)
            {
                selectedNode = selectedNode.Previous;
            }
            else
            {
                selectedNode = VariantManager.ActiveVariants.Last;
            }
        }

        public void NavigateDown()
        {
            if (selectedNode == null)
            {
                selectedNode = VariantManager.ActiveVariants.First;
            }
            else if (selectedNode.Next != null)
            {
                selectedNode = selectedNode.Next;
            }
            else
            {
                selectedNode = VariantManager.ActiveVariants.First;
            }
        }
        #endregion
    }
}


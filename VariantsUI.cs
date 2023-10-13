using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Monocle;
using Celeste;
using System.Collections.Generic;
using System.Collections;

namespace Celeste.Mod.ARandomizerMod
{
	public class VaraintsUI : Entity
	{
        public bool render = false;
        public LinkedList<Variant> activeVariants = new LinkedList<Variant>();
        public LinkedListNode<Variant> selectedNode;

        public int money = 0;
        readonly int moneyGainedPerRoom = 50;
        readonly int strawberryMoney = 100;
        readonly int heartMoney = 1000;
        readonly float variantCostToGainRatio = 0.01f;

        public int score = 0;
        readonly int scoreGainedPerRoom = 100;
        readonly int strawberryScore = 1000;
        readonly int heartScore = 10000;
        readonly float variantCostToScoreRatio = 0.5f;

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
         
        readonly int scoreOffset = 730 - 150;
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

        public VaraintsUI()
        {
            AddTag(Tags.HUD);

            On.Celeste.StrawberryPoints.Added += StrawberryCollected;
            On.Celeste.HeartGem.Collect += HeartCollected;
        }

        private void HeartCollected(On.Celeste.HeartGem.orig_Collect orig, HeartGem self, Player player)
        {
            money += heartMoney;
            score += heartScore;

            orig(self, player);
        }

        private void StrawberryCollected(On.Celeste.StrawberryPoints.orig_Added orig, StrawberryPoints self, Scene scene)
        {
            money += strawberryMoney;
            score += strawberryScore;

            orig(self, scene);
        }

        public void RoomCleared()
        {
            money += moneyGainedPerRoom;
            score += scoreGainedPerRoom;
            foreach (Variant variant in activeVariants)
            {
                money += (int) (variant.cost * variantCostToGainRatio);
                score += (int)(variant.cost * variantCostToScoreRatio);
            }
        }

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
            int moneySpace = (selectedNode is not null) ? moneyHeight : smallMoneyHeight;
            int height = Math.Max(minLines * lineHeight, (lineHeight * activeVariants.Count) + (verticalPadding * 2)) + moneySpace;
            Draw.Rect(new Vector2(offset, topHeight - moneySpace), width, height, color);
        }

        private void RenderScore()
        {
            float xPos = offset + textOffset + scoreOffset;
            int moneySpace = (selectedNode is not null) ? moneyHeight : smallMoneyHeight;
            float yPos = topHeight - moneySpace + scorePadding;
            ActiveFont.Draw("Score: " + score, new Vector2(xPos, yPos), Vector2.Zero, new Vector2(scoreScale, scoreScale), scoreColor);
        }

        private void RenderMoneyBig()
        {
            float xPos = offset + textOffset + moneyOffset;
            float yPos = topHeight - moneyHeight + moneyPadding;
            ActiveFont.Draw("$" + money, new Vector2(xPos, yPos), Vector2.Zero, new Vector2(moneyScale, moneyScale), moneyColor);
        }

        private void RenderMoneySmall()
        {
            float xPos = offset + textOffset + smallMoneyOffset;
            float yPos = topHeight - smallMoneyHeight + smallMoneyPadding;
            ActiveFont.Draw("$" + money, new Vector2(xPos, yPos), Vector2.Zero, new Vector2(smallMoneyScale, smallMoneyScale), smallMoneyColor);
        }

        private void RenderActiveVariants()
        {
            LinkedListNode<Variant> node = activeVariants.First;
            for (int line = 0; line < activeVariants.Count; line++)
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
                color = (variant.cost <= money) ? selectionColor : cantAffordSelectionColor;
            }
            else
            {
                color = textColor;
            }
            int offset = (variant.level == Variant.Level.SUB) ? subVariantOffset : 0;
            offset += (selectedNode is not null) ? costOffset : textOffset;
            string text = variant.name + ": " + ExtendedVariantImports.GetCurrentVariantValue?.Invoke(variant.name);
            
            RenderText(text, color, line, offset);
        }

        private void RenderCost(Variant variant, float line)
        {
            Color color;
            if (variant.cost < 0)
            {
                color = (selectedNode.Value.name.Equals(variant.name)) ? sellColor : sellSelectedColor;
            }
            else if (variant.cost <= money)
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
                if (selectedNode.Value.cost <= money)
                {
                    money -= selectedNode.Value.cost;
                    ResetVariant(selectedNode.Value);
                    NavigateDown();
                }
            }
        }

        public void NavigateUp()
        {
            if (selectedNode == null)
            {
                selectedNode = activeVariants.Last;
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

        public void NavigateDown()
        {
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

        //StDummy breaks things like Badeline and chasers
        public void EnableUI()
        {
            //Player player = Scene?.Tracker?.GetEntity<Player>();
            //if (player is null) return;

            render = true;
            //player.StateMachine.State = Player.StDummy;
        }

        public void DisableUI()
        {
            render = false;

           // Player player = Scene.Tracker.GetEntity<Player>();
            //if (player is null) return;

            //player.StateMachine.State = Player.StNormal;
        }

        public void TriggerVariant(Variant variant)
        {
            Random random = new Random();

            if (variant == null)
            {
                Logger.Log(LogLevel.Error, "ARandomizerMod", "UH OH!");
                return;
            }

            if (variant.name != null)
            {
                ResetVariantsWithName(variant.name);
            }else if (variant.variant1 != null && variant.variant2 != null)
            {
                Logger.Log(LogLevel.Warn, "ARandomizerMod", "Triggering variant with subvariants:");

                TriggerVariant(variant.variant1);
                TriggerVariant(variant.variant2);
            }
            else if (variant.variant1 != null && variant.variant2 != null)
            {
                Logger.Log(LogLevel.Warn, "ARandomizerMod", "Triggering variant with subvariants:");

                TriggerVariant(variant.variant1);
                TriggerVariant(variant.variant2);
                return;
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
        }

        public void ResetVariantsWithName(String name)
        {
            foreach (Variant variant in activeVariants)
            {
                if (variant.name is not null && variant.name == name)
                {
                    ResetVariant(variant);
                    return;
                }
            }
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


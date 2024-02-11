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
        readonly int moneyGainedPerRoom = 100;
        readonly int strawberryMoney = 300;
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

        //StDummy breaks things like Badeline and chasers, oh well
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

            activeVariants.AddLast(variant);
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
            switch (variant)
            {
                case IntegerVariant integerVariant:
                    ExtendedVariantImports.TriggerIntegerVariant?.Invoke(integerVariant.name, integerVariant.defaultInt, false);
                    break;
                case FloatVariant floatVariant:
                    ExtendedVariantImports.TriggerFloatVariant?.Invoke(floatVariant.name, floatVariant.defaultFloat, false);
                    break;
                case BooleanVariant booleanVariant:
                    ExtendedVariantImports.TriggerBooleanVariant?.Invoke(booleanVariant.name, !booleanVariant.status, false);
                    foreach (Variant subVariant in booleanVariant.subVariants)
                        ResetVariant(subVariant);
                    break;
                case null:
                    Logger.Log(LogLevel.Error, "ARandomizerMod", "JESSE: Null variant triggered");
                    return;
            }

            activeVariants.Remove(variant);
        }
    }
}


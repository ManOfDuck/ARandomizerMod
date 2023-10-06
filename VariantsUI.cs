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
    }
}


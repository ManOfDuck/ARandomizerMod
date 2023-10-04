using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Monocle;
using Celeste;

namespace Celeste.Mod.ARandomizerMod
{
	public class UI : Entity
	{
        public UI()
        {
            AddTag(Tags.HUD);
        }

        public override void Render()
        {
            base.Render();

            Draw.Rect(new Vector2(10, 250), 500, 250, new(0, 0, 0, 200));
            ActiveFont.Draw("Test UI Box", new Vector2(10, 325), Color.White);
        }
    }
}


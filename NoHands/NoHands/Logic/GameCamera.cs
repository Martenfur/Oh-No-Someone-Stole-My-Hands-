using System;
using Monofoxe.Engine;
using Monofoxe.Engine.Drawing;
using Microsoft.Xna.Framework;
using Monofoxe.Utils;

namespace NoHands.Logic
{
	public class GameCamera : GameObj
	{
		public Vector2 Position;
		public Character Viewer;
		
		
		public override void Update()
		{
			Position += (Viewer.Position - Position) / 20f;

			DrawCntrl.Cameras[0].Pos = new Vector2((float)Math.Round(Position.X), (float)Math.Round(Position.Y));
		}

		public override void Draw()
		{
			DrawCntrl.CurrentColor = Color.Black;
			DrawCntrl.DrawCircle(Position, 8, false);
			DrawCntrl.CurrentColor = Color.White;
		}

	}
}

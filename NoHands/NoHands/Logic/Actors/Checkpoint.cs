using System;
using Monofoxe.Engine;
using Monofoxe.Engine.Drawing;
using Resources;
using Resources.Sprites;
using Microsoft.Xna.Framework;
using Monofoxe.Utils;

namespace NoHands.Logic
{
	public class Checkpoint : Actor
	{
		public Vector2 SecondPosition;
		public Vector2 Mask;

		public const int Width = 8;

		public const float Speed = 70;
		public Vector2 Dir;


		public Checkpoint(Vector2 pos)
		{
			Position = pos;
			Depth = -(int)Position.Y;
		}
		
		public override void Draw()
		{
			DrawCntrl.CurrentColor = Color.White;
			DrawCntrl.DrawCircle(Position, 16, false);
		}
	}
}
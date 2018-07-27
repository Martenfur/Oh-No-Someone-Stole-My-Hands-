using System;
using Monofoxe.Engine;
using Monofoxe.Engine.Drawing;
using Resources;
using Resources.Sprites;
using Microsoft.Xna.Framework;
using Monofoxe.Utils;

namespace NoHands.Logic
{
	public class Solid : GameObj
	{
		public Vector2 Position;
		public Vector2 Size;
		

		public Solid(Vector2 pos, Vector2 size)
		{
			Position = pos;
			Size = size;
			
			Depth = -(int)Position.Y;
		}

		public override void Update()
		{
			
		}

		
		public override void Draw()
		{
			DrawCntrl.CurrentColor = Color.Black;
			DrawCntrl.DrawRectangle(Position, Position + Size, false);
			DrawCntrl.CurrentColor = Color.White;
		}
		
		

	}
}
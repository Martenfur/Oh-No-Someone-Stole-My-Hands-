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
		public Sprite Sprite;

		public Solid(Vector2 pos, Vector2 size, Sprite sprite = null)
		{
			Position = pos;
			Size = size;
			Sprite = sprite;

			Depth = -(int)Position.Y;
		}
		
		public override void Draw()
		{
			if (Sprite != null)
			{
				DrawCntrl.CurrentColor = Color.Black;
				DrawCntrl.DrawRectangle(Position, Position + Size, false);
				DrawCntrl.CurrentColor = Color.White;
			}
		}
	}
}
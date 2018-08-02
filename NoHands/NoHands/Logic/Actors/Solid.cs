using System;
using Monofoxe.Engine;
using Monofoxe.Engine.Drawing;
using Resources;
using Resources.Sprites;
using Microsoft.Xna.Framework;
using Monofoxe.Utils;

namespace NoHands.Logic
{
	public class Solid : Actor
	{
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
				DrawCntrl.CurrentColor = Color.White;
				DrawCntrl.DrawSprite(Sprite, Position + Vector2.One * Scene.CellSize / 2);
			}
		}
	}
}
using System;
using Monofoxe.Engine;
using Monofoxe.Engine.Drawing;
using Resources;
using Resources.Sprites;
using Microsoft.Xna.Framework;
using Monofoxe.Utils;

namespace NoHands.Logic
{
	public class Coin : Actor
	{
		
		public Coin(Vector2 pos)
		{
			Position = pos;
			
			Depth = -(int)Position.Y;
		}
		
		public override void Draw()
		{	
			DrawCntrl.CurrentColor = Color.White;
			DrawCntrl.DrawSprite(SpritesDefault.Coin, Position);
		}
	}
}
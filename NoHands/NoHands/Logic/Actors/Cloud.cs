using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Monofoxe.Engine;
using Monofoxe.Engine.Drawing;
using Microsoft.Xna.Framework;
using Monofoxe.Utils;
using Resources.Sprites;

namespace NoHands.Logic
{
	public class Cloud : Actor
	{
		
		public float Parallax;
		public int Id;

		public Cloud(Vector2 pos, int id, float parallax)
		{
			Position = pos;
			Id = id;
			Parallax = parallax;
			Depth = 100000;
		}

		public override void DrawBegin()
		{
			DrawCntrl.CurrentColor = Color.White;
			DrawCntrl.DrawSprite(SpritesDefault.Clouds, Id, Position + DrawCntrl.CurrentCamera.Pos * Parallax);
		}

	}
}

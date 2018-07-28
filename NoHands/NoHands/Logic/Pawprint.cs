using System;
using Monofoxe.Engine;
using Monofoxe.Engine.Drawing;
using Resources;
using Resources.Sprites;
using Microsoft.Xna.Framework;
using Monofoxe.Utils;

namespace NoHands.Logic
{
	public class Pawprint : GameObj
	{
		public Vector2 Position;
		
		Alarm _life = new Alarm();
		float _lifetime = 5;

		public Pawprint(Vector2 pos)
		{
			Position = pos;
			_life.Set(_lifetime);
		}

		public override void Update()
		{
			if (_life.Update())
			{
				Objects.Destroy(this);
			}
		}

		
		public override void Draw()
		{
			//DrawCntrl.DrawSprite(SpritesDefault.FoxPaw, Test.RoundVector2(Position));
			DrawCntrl.CurrentColor = new Color(Color.White, (float)_life.Counter / _lifetime);
			DrawCntrl.DrawCircle(Test.RoundVector2(Position), 4, false);
			DrawCntrl.CurrentColor = Color.White;
		}
	}
}
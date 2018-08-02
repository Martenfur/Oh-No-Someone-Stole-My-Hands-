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
	public class NPC : Character
	{
		public NPC(Vector2 pos, byte arg) : base(pos, SpritesDefault.VillainBody, SpritesDefault.VillainFace)
		{
			new Solid(pos - Size / 2, Size);

			if (arg == 0)
			{
				LeftPaw.Sprite = SpritesDefault.VillainPaw;
				RightPaw.Sprite = SpritesDefault.VillainPaw;
			}
			if (arg == 1)
			{
				BodySprite = SpritesDefault.MonkeyBody;
				FaceSprite = SpritesDefault.MonkeyFace;
				LeftPaw.Sprite = SpritesDefault.MonkeyPaw;
				RightPaw.Sprite = SpritesDefault.MonkeyPaw;
				FaceOffsetMax = 2;
			}
		}

		public override void Update()
		{
			var player = Objects.ObjFind<Player>(0);
			LookAtPoint(player.Position);
			base.Update();
		}
	}
}

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
		public NPC(Vector2 pos, byte arg) : base(pos, SpritesDefault.FoxBody, SpritesDefault.FoxFace)
		{
			new Solid(pos - Size / 2, Size);
		}

		public override void Update()
		{
			var player = Objects.ObjFind<Player>(0);
			LookAtPoint(player.Position);
			base.Update();
		}
	}
}

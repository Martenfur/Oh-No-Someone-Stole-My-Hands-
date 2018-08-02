using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Monofoxe.Engine.Drawing;
using Resources.Sprites;

namespace NoHands.Logic
{
	public class PawTrail
	{
		public List<Pawprint> Pawprints = new List<Pawprint>();
		
		public void Update()
		{
			if (Pawprints.Count > 0 && Pawprints.Last().Destroyed)
			{
				Pawprints.Remove(Pawprints.Last());
			}
		}

		public void AddPawprint(Vector2 pos, float dir, Sprite spr) =>
			Pawprints.Insert(0, new Pawprint(pos, dir, spr));

	}
}

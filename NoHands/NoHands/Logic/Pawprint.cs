﻿using System;
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

		public float Dir;

		public Sprite Sprite;

		public Pawprint(Vector2 pos, float dir, Sprite spr)
		{
			Position = pos;
			Dir = dir;
			Sprite = spr;
			_life.Set(_lifetime);
		}

		public override void Update()
		{
			if (_life.Update())
			{
				Objects.Destroy(this);
			}

			int x = (int)Position.X / Scene.CellSize;
			int y = (int)Position.Y / Scene.CellSize;

			if (Test.CurrentScene.TileMap[x, y] == 2)
			{
				Objects.Destroy(this);
			}
		}

		
		public override void DrawBegin()
		{		
			if (!Destroyed)
			{
				var c = new Color(Color.White, (float)_life.Counter / _lifetime);
				DrawCntrl.DrawSprite(
					Sprite, 
					0, 
					Test.RoundVector2(Position + (Vector2.UnitY * Test.CurrentScene.GetLift(Position))), 
					Vector2.One, 
					-Dir, 
					c
				);
			}
		}
	}
}
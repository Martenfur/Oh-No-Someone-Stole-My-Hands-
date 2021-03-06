﻿using System;
using Monofoxe.Engine;
using Monofoxe.Engine.Drawing;
using Resources;
using Resources.Sprites;
using Microsoft.Xna.Framework;
using Monofoxe.Utils;

namespace NoHands.Logic
{
	public class Laser : Actor
	{
		public Vector2 SecondPosition;
		public Vector2 Mask;

		public const int Width = 8;

		public const float Speed = 70;
		public Vector2 Dir;

		double _ang;
		float amp = 2;

		public Laser(Vector2 pos, Vector2 secondPos)
		{
			Position = pos;
			SecondPosition = secondPos - pos;
			
			Mask = SecondPosition;
			if (Mask.X == 0)
			{
				Position.Y -= Scene.CellSize / 2 * Math.Sign(Mask.Y);
				SecondPosition.Y += Scene.CellSize * Math.Sign(Mask.Y);
				Mask = SecondPosition;

				Mask.X = Width;
				Mask.Y = Math.Abs(Mask.Y);
				Dir = Vector2.UnitX;
			}
			else
			{
				Position.X -= Scene.CellSize / 2 * Math.Sign(Mask.X);
				SecondPosition.X += Scene.CellSize * Math.Sign(Mask.X);
				Mask = SecondPosition;

				Mask.Y = Width;
				Mask.X = Math.Abs(Mask.X);
				Dir = Vector2.UnitY;
			}

			Depth = -(int)Position.Y;
		}

		
		public override void Update()
		{
			Position += Dir * (float)GameCntrl.Time(Speed);
			if (CheckCollision(Position + SecondPosition / 2))
			{
				Position -= Dir * (float)GameCntrl.Time(Speed);
				Dir *= -1;
			}

			var player = Objects.ObjFind<Player>(0);

			if (player != null)
			{
				var center = Position + SecondPosition / 2;
			
				if (
					player.LeftPaw.CurrentState != Paw.State.Jumping 
					&& GameMath.RectangleInRectangle(center - Mask / 2, center + Mask / 2, player.LeftPaw.Position, player.LeftPaw.Position)
					|| player.RightPaw.CurrentState != Paw.State.Jumping
					&& GameMath.RectangleInRectangle(center - Mask / 2, center + Mask / 2, player.RightPaw.Position, player.RightPaw.Position)
				)
				{
					player.Die();
					//Console.WriteLine("KILLED" + GameCntrl.ElapsedTimeTotal);
				}
			}

			_ang += GameCntrl.Time(3);
			if (_ang > Math.PI * 2)
			{
				_ang -= Math.PI * 2;
			}

			Depth = -(int)Position.Y + 1000;
		}
		
		public override void Draw()
		{
			var center = Position + SecondPosition / 2;
			DrawCntrl.CurrentColor = Color.White;
			
			var v = Vector2.UnitY * amp * (float)Math.Sin(_ang); 

			DrawCntrl.DrawRectangle(center - Mask / 2, center + Mask / 2, false);
			
			DrawCntrl.DrawSprite(SpritesDefault.LaserOrb, Test.RoundVector2(Position + v));
			DrawCntrl.DrawSprite(SpritesDefault.LaserOrb, Test.RoundVector2(Position + SecondPosition + v));
		}


		bool CheckCollision(Vector2 pos)
		{
			foreach(var solid in Objects.GetList<Solid>())
			{
				if (GameMath.RectangleInRectangle(pos - Mask / 2, pos + Mask / 2, solid.Position, solid.Position + solid.Size))
				{
					return true;
				}
			}

			return false;
		}
		
	}
}
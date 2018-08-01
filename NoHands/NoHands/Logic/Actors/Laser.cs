using System;
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


		public Laser(Vector2 pos, Vector2 secondPos)
		{
			Position = pos;
			SecondPosition = secondPos - pos;
			
			Mask = SecondPosition;
			if (Mask.X == 0)
			{
				Mask.X = Width;
				Mask.Y = Math.Abs(Mask.Y);
				Dir = Vector2.UnitX;
			}
			else
			{
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

			Depth = -(int)Position.Y + 1000;
		}
		
		public override void Draw()
		{
			var center = Position + SecondPosition / 2;
			DrawCntrl.CurrentColor = Color.White;
			
			DrawCntrl.DrawRectangle(center - Mask / 2, center + Mask / 2, false);
			
			DrawCntrl.CurrentColor = Color.Black;
			DrawCntrl.DrawCircle(Position, 8, false);
			DrawCntrl.DrawCircle(Position + SecondPosition, 8, false);

			DrawCntrl.CurrentColor = Color.White;
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
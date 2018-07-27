using System;
using Monofoxe.Engine;
using Monofoxe.Engine.Drawing;
using Resources;
using Resources.Sprites;
using Microsoft.Xna.Framework;
using Monofoxe.Utils;
using System.Collections.Generic;

namespace NoHands.Logic
{
	public class Character : GameObj
	{
		public Vector2 Position;
		public Vector2 Size = new Vector2(32, 32);

		Buttons _leftPawButton = Buttons.A;
		Buttons _rightPawButton = Buttons.D;
		Buttons _backButton = Buttons.S;

		public Paw LeftPaw, RightPaw;

		private int _pawRadius = 12;

		public double FacingDirection;

		Vector2 _facePos = new Vector2(0, -40);
		Vector2 _bodyOffset = new Vector2(0, -12);

		float _faceOffsetMax = 10;

		public Character(Vector2 pos)
		{
			Position = pos;
			LeftPaw = new Paw(Position - Vector2.UnitX * _pawRadius);
			RightPaw = new Paw(Position + Vector2.UnitX * _pawRadius);
			LeftPaw.Pair = RightPaw;
			RightPaw.Pair = LeftPaw;
			RightPaw.Inversion = -1;
		}

		public override void Update()
		{
			if (Input.CheckButton(_leftPawButton))
			{
				LeftPaw.StartStep();
			}
			if (Input.CheckButtonRelease(_leftPawButton))
			{
				LeftPaw.StopStep();
			}
			if (Input.CheckButton(_rightPawButton))
			{
				RightPaw.StartStep();
			}
			if (Input.CheckButtonRelease(_rightPawButton))
			{
				RightPaw.StopStep();
			}

			if (Input.CheckButton(_backButton))
			{
				LeftPaw.Inversion = -1;
				RightPaw.Inversion = 1;
			}
			else
			{
				LeftPaw.Inversion = 1;
				RightPaw.Inversion = -1;
			}

			FacingDirection = GameMath.Direction(RightPaw.Position, LeftPaw.Position) + 90;
			if (FacingDirection >= 360)
			{
				FacingDirection -= 360;
			}

			Move((LeftPaw.Position + RightPaw.Position) / 2f);
			Depth = -(int)Position.Y;
		}

		
		public override void Draw()
		{
			var frame = 0;
			if (FacingDirection > 90 - 90 && FacingDirection < 90 + 90)
			{
				frame = 1;
			}
			DrawCntrl.DrawSprite(SpritesDefault.FoxBody, frame, Position + _bodyOffset);

			if (frame == 0)
			{
				float ratio = ((float)FacingDirection - 270) / 90f;
				
				if (ratio > 1)
				{
					ratio = 1;
				}
				if (ratio < -1)
				{
					ratio = -1;
				}

				DrawCntrl.DrawSprite(SpritesDefault.FoxFace, Position + _bodyOffset + _facePos + Vector2.UnitX * _faceOffsetMax * ratio);
			}

			
			DrawCntrl.CurrentFont = Fonts.Arial;
			//DrawCntrl.DrawText(ratio.ToString(), 32, 32);
		}

		void Move(Vector2 newPos)
		{
			var delta = newPos - Position;

			var solids = Objects.GetList<Solid>();

			var resDelta = Vector2.Zero;

			if (CheckCollision(Position + Vector2.UnitX * delta.X, solids))
			{
				var sign = Math.Sign(delta.X);
				for(var i = 0; i < delta.X; i += 1)
				{
					if (!CheckCollision(Position + Vector2.UnitX * i * sign, solids))
					{
						resDelta.X = i * sign;
						break;
					}
				}
			}
			else
			{
				resDelta.X = delta.X;
			}


			if (CheckCollision(Position + Vector2.UnitY * delta.Y, solids))
			{
				var sign = Math.Sign(delta.Y);
				for(var i = 0; i < delta.Y; i += 1)
				{
					if (!CheckCollision(Position + Vector2.UnitY * i * sign, solids))
					{
						resDelta.Y = i * sign;
						break;
					}
				}
			}
			else
			{
				resDelta.Y = delta.Y;
			}
			
			Position += resDelta;
			LeftPaw.Position += -(delta - resDelta);
			RightPaw.Position += -(delta - resDelta);


		}

		bool CheckCollision(Vector2 pos, List<Solid> solids)
		{
			foreach(var solid in solids)
			{
				if (GameMath.RectangleInRectangle(pos - Size / 2, pos + Size / 2, solid.Position, solid.Position + solid.Size))
				{
					return true;
				}
			}

			return false;
		}
		
		
	}
}

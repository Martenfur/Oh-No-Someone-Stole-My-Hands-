using System;
using Monofoxe.Engine;
using Monofoxe.Engine.Drawing;
using Resources;
using Resources.Sprites;
using Microsoft.Xna.Framework;
using Monofoxe.Utils;

namespace NoHands.Logic
{
	public class Character : GameObj
	{
		public Vector2 Position;

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

			FacingDirection = GameMath.Direction(RightPaw.Position, LeftPaw.Position) + 90;
			if (FacingDirection >= 360)
			{
				FacingDirection -= 360;
			}

			Position = (LeftPaw.Position + RightPaw.Position) / 2f;
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
		
	}
}

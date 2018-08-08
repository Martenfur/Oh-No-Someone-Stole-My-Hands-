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
	public class Character : Actor
	{
		public Vector2 Size = new Vector2(32, 32);

		public Paw LeftPaw, RightPaw;

		public Sprite BodySprite, FaceSprite;

		public int PawRadius = 14;

		public double FacingDirection;

		Vector2 _facePos = new Vector2(0, -40);
		Vector2 _bodyOffset = new Vector2(0, -12);

		public float FaceOffsetMax = 10;

		Alarm _jumpCharge = new Alarm();
		float _jumpChargeTime = 0.25f;
		protected bool _jumpChargeReady = false;
		float _jumpChargeZ;
		float _jumpChargeZMax = 4;

		public float Z;
		public float Zspd;
		public float Gravity = 800;

		public PawTrail PawTrail = new PawTrail();

		protected bool _movingLeftPaw, _movingRightPaw, _chargingJump;

		public Character(Vector2 pos, Sprite bodySprite,Sprite faceSprite)
		{
			Position = pos;
			BodySprite = bodySprite;
			FaceSprite = faceSprite;

			LeftPaw = new Paw(Position - Vector2.UnitX * PawRadius, this, SpritesDefault.FoxPaw);
			LeftPaw.ZRubberBand = 2;
			RightPaw = new Paw(Position + Vector2.UnitX * PawRadius, this, SpritesDefault.FoxPaw);
			LeftPaw.Pair = RightPaw;
			RightPaw.Pair = LeftPaw;
			RightPaw.Inversion = -1;
		}

		public override void Update()
		{
			// Jump.
			if (_chargingJump)
			{
				LeftPaw.StopStep();
				RightPaw.StopStep();

				if (!_jumpChargeReady && !_jumpCharge.Active)
				{
					_jumpCharge.Set(_jumpChargeTime);
				}

				if (_jumpCharge.Update())
				{
					_jumpChargeReady = true;
				}
			}
			else
			{
				if (_jumpChargeReady)
				{
					Zspd = 320;
				}
				_jumpChargeReady = false;
				_jumpCharge.Reset();
			}
			// Jump.

			
			PawTrail.Update();


			FacingDirection = GameMath.Direction(RightPaw.Position, LeftPaw.Position) + 90;
			if (FacingDirection >= 360)
			{
				FacingDirection -= 360;
			}

			Move((LeftPaw.Position + RightPaw.Position) / 2f);
			Depth = -(int)Position.Y;


			Zspd -= (float)GameCntrl.Time(Gravity);
			Z += (float)GameCntrl.Time(Zspd);
			if (Z < 0)
			{
				Z = 0;
				Zspd = 0;
			}

			if (Z > 0)
			{
				LeftPaw.CurrentState = Paw.State.Jumping;
				RightPaw.CurrentState = Paw.State.Jumping;
			}
			else
			{
				if (LeftPaw.CurrentState == Paw.State.Jumping)
				{
					LeftPaw.CurrentState = Paw.State.Resting;
				}
				if (RightPaw.CurrentState == Paw.State.Jumping)
				{
					RightPaw.CurrentState = Paw.State.Resting;
				}
			}

			if (_jumpCharge.Active || _jumpChargeReady)
			{
				_jumpChargeZ = -(float)Math.Sqrt(1 - _jumpCharge.Counter / _jumpChargeTime) * _jumpChargeZMax;
			}
			else
			{
				_jumpChargeZ = _jumpChargeZ / 2f;
			}

		}


		
		public override void Draw()
		{
			var resPos = Position + _bodyOffset - Vector2.UnitY * (Z + _jumpChargeZ);
			
			var frame = 0;
			if (FacingDirection > 0 && FacingDirection < 180)
			{
				frame = 1;
			}
			DrawCntrl.DrawSprite(BodySprite, frame, Test.RoundVector2(resPos + (Vector2.UnitY * Test.CurrentScene.GetLift(Position))));

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

				DrawCntrl.DrawSprite(
					FaceSprite, 
					Test.RoundVector2(
						resPos + _facePos + 
						(Vector2.UnitX * FaceOffsetMax * ratio) + 
						(Vector2.UnitY * Test.CurrentScene.GetLift(Position))
					)
				);
			}

		}

		public void LookAtPoint(Vector2 point)
		{
			var v = point - Position;
			var vRot = new Vector2(-v.Y, v.X);
			vRot.Normalize();

			//LeftPaw.Position = Position + vRot * PawRadius;
			//RightPaw.Position = Position - vRot * PawRadius;
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

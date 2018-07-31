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
		Buttons _jumpButton = Buttons.Space;


		public Paw LeftPaw, RightPaw;

		private int _pawRadius = 14;

		public double FacingDirection;

		Vector2 _facePos = new Vector2(0, -40);
		Vector2 _bodyOffset = new Vector2(0, -12);

		float _faceOffsetMax = 10;

		Alarm _jumpCharge = new Alarm();
		float _jumpChargeTime = 0.25f;
		bool _jumpChargeReady = false;
		float _jumpChargeZ;
		float _jumpChargeZMax = 4;

		public float Z;
		public float Zspd;
		public float Gravity = 1000;

		public PawTrail PawTrail = new PawTrail();

		public Character(Vector2 pos)
		{

			Position = pos;
			LeftPaw = new Paw(Position - Vector2.UnitX * _pawRadius, this);
			LeftPaw.ZRubberBand = 2;
			RightPaw = new Paw(Position + Vector2.UnitX * _pawRadius, this);
			LeftPaw.Pair = RightPaw;
			RightPaw.Pair = LeftPaw;
			RightPaw.Inversion = -1;

			var cam = new GameCamera();
			cam.Viewer = this;
		}

		public override void Update()
		{
			#region Movement controls.
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
			#endregion Movement controls.

			// Jump.
			if (Input.CheckButton(_jumpButton))
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
					Zspd = 300;
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
			DrawCntrl.DrawSprite(SpritesDefault.FoxBody, frame, Test.RoundVector2(resPos));

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

				DrawCntrl.DrawSprite(SpritesDefault.FoxFace, Test.RoundVector2(resPos + _facePos + Vector2.UnitX * _faceOffsetMax * ratio));
			}

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

using System;
using Monofoxe.Engine;
using Monofoxe.Engine.Drawing;
using Resources;
using Resources.Sprites;
using Microsoft.Xna.Framework;
using Monofoxe.Utils;
using Monofoxe.Engine.Audio;

namespace NoHands.Logic
{
	public class Paw : GameObj
	{
		public Vector2 Position;
		public float Z;

		public Sprite Sprite;

		public Paw Pair;
		public Character Owner;

		public enum State
		{
			Resting,
			Stepping,
			Jumping,
		}

		public State CurrentState;

		double _startingAngle;
		float _startingDistance;
		double _stepAngle;
		float _stepSpd = 315; // deg/s

		public int Inversion = 1; 

		public float ZRubberBand = 3f;
		
		public Paw(Vector2 pos, Character owner, Sprite sprite)
		{
			Position = pos;
			Owner = owner;
			Sprite = sprite;
		}

		public override void Update()
		{
			if (CurrentState == State.Stepping)
			{
				var stepAnglePrev = _stepAngle;
				var posPrev = Position;

				_stepAngle += GameCntrl.Time(_stepSpd) * Inversion;
				var deg = MathHelper.ToRadians((float)(_startingAngle + _stepAngle));
				Position = Pair.Position + new Vector2((float)Math.Cos(deg), -(float)Math.Sin(deg)) * _startingDistance;
			}
	
			if (CurrentState == State.Jumping && Z == 0)
			{
				Owner.PawTrail.AddPawprint(Position, (float)Owner.FacingDirection, SpritesDefault.Pawprint);
			}


			Z += (Owner.Z - Z) / ZRubberBand;
			if (CurrentState != State.Jumping && Math.Abs(Owner.Z - Z) < 1)
			{
				Z = 0;
			}

			Depth = -(int)Position.Y;
		}

		
		public override void Draw()
		{
			DrawCntrl.DrawSprite(
				Sprite, 
				Test.RoundVector2(Position - (Vector2.UnitY * Z) + (Vector2.UnitY * Test.CurrentScene.GetLift(Position)))
			);
		}

		public void StartStep()
		{
			if (CurrentState == State.Resting && Pair.CurrentState == State.Resting)
			{
				CurrentState = State.Stepping;
				_startingAngle = GameMath.Direction(Pair.Position, Position);
				_startingDistance = GameMath.Distance(Pair.Position, Position);
				Owner.PawTrail.AddPawprint(Position, (float)Owner.FacingDirection + 45 * Inversion, SpritesDefault.Pawprint);
			}
		}
		
		public void StopStep()
		{
			CurrentState = State.Resting;
			_stepAngle = 0;
			//var snd = AudioMgr.PlaySound(Sounds.Step);
			//snd.Loops = 0;
		}

	}
}
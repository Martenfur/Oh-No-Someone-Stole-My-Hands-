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
		double _maxStepAngle = 45;
		float _stepSpd = 270; // deg/s

		public int Inversion = 1; 

		public float ZRubberBand = 3f;
		
		public Paw(Vector2 pos, Character owner)
		{
			Position = pos;
			Owner = owner;
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
				Owner.PawTrail.AddPawprint(Position);
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
			DrawCntrl.DrawSprite(SpritesDefault.FoxPaw, Test.RoundVector2(Position - Vector2.UnitY * Z));
		}

		public void StartStep()
		{
			if (CurrentState == State.Resting && Pair.CurrentState == State.Resting)
			{
				CurrentState = State.Stepping;
				_startingAngle = GameMath.Direction(Pair.Position, Position);
				_startingDistance = GameMath.Distance(Pair.Position, Position);
				Owner.PawTrail.AddPawprint(Position);
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
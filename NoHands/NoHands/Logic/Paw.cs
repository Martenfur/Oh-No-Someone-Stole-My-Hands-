using System;
using Monofoxe.Engine;
using Monofoxe.Engine.Drawing;
using Resources;
using Resources.Sprites;
using Microsoft.Xna.Framework;
using Monofoxe.Utils;

namespace NoHands.Logic
{
	public class Paw : GameObj
	{
		public Vector2 Position;

		public Paw Pair;

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

		public int Inversion = 1; 

		
		public Paw(Vector2 pos)
		{
			Position = pos;
		}

		public override void Update()
		{
			if (CurrentState == State.Stepping)
			{
				_stepAngle += GameCntrl.Time(270) * Inversion;
				var deg = MathHelper.ToRadians((float)(_startingAngle + _stepAngle));
				Position = Pair.Position + new Vector2((float)Math.Cos(deg), -(float)Math.Sin(deg)) * _startingDistance;
				
			}


			Depth = -(int)Position.Y;
		}

		
		public override void Draw()
		{
			DrawCntrl.DrawSprite(SpritesDefault.FoxPaw, Position);
		}

		public void StartStep()
		{
			if (CurrentState == State.Resting && Pair.CurrentState == State.Resting)
			{
				CurrentState = State.Stepping;
				_startingAngle = GameMath.Direction(Pair.Position, Position);
				_startingDistance = GameMath.Distance(Pair.Position, Position);
			}
		}
		
		public void StopStep()
		{
			CurrentState = State.Resting;
			_stepAngle = 0;
		}
		

	}
}
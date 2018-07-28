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
	public class Enemy : GameObj
	{
		public Vector2 Position;
		
		List<Vector2> _patrolPoints = new List<Vector2>();
		int _currentPointIndex = 0;

		float _speed = 32;

		AutoAlarm _pawprint = new AutoAlarm(0.5);
		int _inverse = 1;

		float _stepWidth = 10;

		public Enemy(Vector2 pos)
		{
			Position = pos;
		
			// Debug.
			_patrolPoints.Add(Position + new Vector2(100, 100));
			_patrolPoints.Add(Position + new Vector2(100, -100));
			_patrolPoints.Add(Position + new Vector2(-100, -100));
			_patrolPoints.Add(Position + new Vector2(-100, 100));
		}

		public override void Update()
		{
			if (GameMath.Distance(Position, _patrolPoints[_currentPointIndex]) < 8)
			{
				_currentPointIndex += 1;
				if (_currentPointIndex == _patrolPoints.Count)
				{
					_currentPointIndex = 0;
				}
			}

			var vec = _patrolPoints[_currentPointIndex] - Position;
			vec.Normalize();

			Position += vec * (float)GameCntrl.Time(_speed);

			if (_pawprint.Update())
			{
				var normVec = new Vector2(vec.Y * _inverse, -vec.X * _inverse) * _stepWidth;
				new Pawprint(Position + normVec);
				_inverse *= -1;
			}

		}

		/*
		public override void Draw()
		{
			DrawCntrl.CurrentColor = Color.Red;
			DrawCntrl.DrawCircle(Test.RoundVector2(Position), 8, true);
			
			foreach(Vector2 pt in _patrolPoints)
			{
				DrawCntrl.DrawCircle(pt, 2, false);
			}
			
			DrawCntrl.CurrentColor = Color.White;


		}
		*/
	}
}

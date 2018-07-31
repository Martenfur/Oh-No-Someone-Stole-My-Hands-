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
		public enum State
		{
			Patroling,
			Alarmed,
			Pursuing,
		}
		public State CurrentState = State.Patroling;

		public Vector2 Position;
		
		List<Vector2> _patrolPoints = new List<Vector2>();
		int _currentPointIndex = 0;

		float _speed = 32;

		AutoAlarm _pawprint = new AutoAlarm(0.5);
		int _inverse = 1;

		float _stepWidth = 10;

		public PawTrail PawTrail = new PawTrail();

		float _detectionRadius = 16;

		Pawprint _tracedPawprint = null;

		
		AutoAlarm _pursuingAlarm = new AutoAlarm(0.1);
		Alarm _pursuingDelayAlarm = new Alarm();
		float _pursuingDelay = 1;


		public Enemy(Vector2 pos, List<Vector2> path)
		{
			Position = pos;
			_patrolPoints = path;

			// Debug.
			/*
			_patrolPoints.Add(Position + new Vector2(100, 100));
			_patrolPoints.Add(Position + new Vector2(100, -100));
			_patrolPoints.Add(Position + new Vector2(-100, -100));
			_patrolPoints.Add(Position + new Vector2(-100, 100));
			*/
			Console.WriteLine(path[0]);

		}

		public override void Update()
		{
			PawTrail.Update();

			if (CurrentState == State.Patroling)
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
					PawTrail.AddPawprint(Position + normVec);
					_inverse *= -1;
				}

				// Checking for player's trail.
				var player = Objects.ObjFind<Character>(0);

				if (player != null)
				{
					foreach(Pawprint myPawprint in PawTrail.Pawprints)
					{
						foreach(Pawprint playersPawprint in player.PawTrail.Pawprints)
						{
							if (!playersPawprint.Destroyed && GameMath.Distance(myPawprint.Position, playersPawprint.Position) < _detectionRadius)
							{
								_tracedPawprint = playersPawprint;
								CurrentState = State.Alarmed;
								_pursuingDelayAlarm.Set(_pursuingDelay);
								break;
							}
						}
						if (_tracedPawprint != null)
						{
							break;
						}
					}
				}
				// Checking for player's trail.
			}

			if (CurrentState == State.Alarmed)
			{
				if (_pursuingDelayAlarm.Update())
				{
					if (!_tracedPawprint.Destroyed)
					{
						CurrentState = State.Pursuing;
						Position = _tracedPawprint.Position;
					}
					else
					{
						_tracedPawprint = null;
						CurrentState = State.Patroling;
					}
				}
			}

			if (CurrentState == State.Pursuing)
			{
				if (_pursuingAlarm.Update())
				{
					var player = Objects.ObjFind<Character>(0);

					var lostTrail = false;

					if (player != null)
					{
						var id = player.PawTrail.Pawprints.IndexOf(_tracedPawprint);
						if (id == -1 || id == 0)
						{
							lostTrail = true;
						}
						else
						{
							id -= 1;
							_tracedPawprint = player.PawTrail.Pawprints[id];
							Position = _tracedPawprint.Position;
							if (_tracedPawprint.Destroyed)
							{
								lostTrail = true;
							}
						}
					}
					else
					{
						lostTrail = true;
					}
					
					if (lostTrail)
					{
						_tracedPawprint = null;
						CurrentState = State.Patroling;
					}
				}
			}
			
		}

		
		public override void Draw()
		{
			
			DrawCntrl.CurrentColor = Color.Red;

			//if (CurrentState == State.Pursuing)
				DrawCntrl.DrawCircle(Test.RoundVector2(Position), 8, false);
			
			foreach(Vector2 pt in _patrolPoints)
			{
				DrawCntrl.DrawCircle(pt, 2, false);
			}
			
			DrawCntrl.CurrentColor = Color.White;


		}
		
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Monofoxe.Engine;
using Monofoxe.Engine.Drawing;
using Microsoft.Xna.Framework;
using Monofoxe.Utils;
using Resources.Sprites;

namespace NoHands.Logic
{
	public class Player : Character
	{
		Buttons _leftPawButton = Buttons.A;
		Buttons _rightPawButton = Buttons.D;
		Buttons _jumpButton = Buttons.Space;

		public Vector2 CheckpointPos;

		public bool ControlsEnabled = true;

		bool _dead = false;
		int _deathStage;
		Alarm _deathAlarm = new Alarm();

		bool _deathScrEnabled = false;

		GameCamera _cam;

		public Player(Vector2 pos) : base(pos, SpritesDefault.FoxBody, SpritesDefault.FoxFace)
		{
			_cam = new GameCamera();
			_cam.Viewer = this;
			_cam.Position = pos;

			CheckpointPos = pos;
		}

		public override void Update()
		{
			_chargingJump = Input.CheckButton(_jumpButton) && ControlsEnabled;

			#region Movement controls.
			if (Input.CheckButton(_leftPawButton))
			{
				LeftPaw.StartStep();
			}
			if (Input.CheckButtonRelease(_leftPawButton) || !ControlsEnabled)
			{
				LeftPaw.StopStep();
			}
			if (Input.CheckButton(_rightPawButton))
			{
				RightPaw.StartStep();
			}
			if (Input.CheckButtonRelease(_rightPawButton) || !ControlsEnabled)
			{
				RightPaw.StopStep();
			}
			#endregion Movement controls.

			base.Update();


			if (_dead)
			{
				_deathAlarm.Update();

				if (_deathAlarm.Triggered)
				{
					if (_deathStage == 0)
					{
						_deathScrEnabled = true;
						
						LeftPaw.Position += CheckpointPos - Position;
						RightPaw.Position += CheckpointPos - Position;
						Position = CheckpointPos;
						_cam.Position = Position;

						_deathAlarm.Set(0.25);
					}
					if (_deathStage == 1)
					{
						_deathScrEnabled = false;
						ControlsEnabled = true;
						_dead = false;
					}

					_deathStage += 1;
				}

			}


			foreach(Checkpoint checkpoint in Objects.GetList<Checkpoint>())
			{
				if (GameMath.Distance(Position, checkpoint.Position) < 32)
				{
					CheckpointPos = Position;
				}
			}

		}

		public override void DrawGUI()
		{
			if (_deathScrEnabled)
			{
				DrawCntrl.CurrentColor = Color.White;
				DrawCntrl.DrawRectangle(Vector2.Zero, GameCntrl.WindowManager.CanvasSize, false);
			}
		}

		public void Die()
		{
			if (!_dead)
			{
				_dead = true;
				_deathStage = 0;
				ControlsEnabled = false;
				_deathAlarm.Set(0.5);
				_jumpChargeReady = false;
			}
		}

	}
}

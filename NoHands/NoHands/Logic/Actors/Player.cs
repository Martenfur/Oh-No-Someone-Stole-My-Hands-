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
		Buttons _previousLineButton = Buttons.P;


		public Vector2 CheckpointPos;

		public bool ControlsEnabled = true;

		bool _dead = false;
		int _deathStage;
		Alarm _deathAlarm = new Alarm();

		bool _deathScrEnabled = false;

		GameCamera _cam;


		public string[] Lines;
		public int LinePtr = -1;
		public bool Speaking = true;

		public SpeechBubble Bubble;

		public Player(Vector2 pos) : base(pos, SpritesDefault.FoxBody, SpritesDefault.FoxFace)
		{
			_cam = new GameCamera();
			_cam.Viewer = this;
			_cam.Position = pos;

			CheckpointPos = pos;

			Lines = new string[]
			{
				"kek",
				"kok", 
				"kek kok maslena",
			};
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

				FaceSprite = SpritesDefault.FoxFaceSad;
			}
			else
			{
				FaceSprite = SpritesDefault.FoxFace;
			}
			


			foreach(Checkpoint checkpoint in Objects.GetList<Checkpoint>())
			{
				if (GameMath.Distance(Position, checkpoint.Position) < 32)
				{
					CheckpointPos = Position;
				}
			}

			if ((Bubble == null || Bubble.Destroyed) && Speaking)
			{
				LinePtr += 1;
				if (LinePtr >= Lines.Length)
				{
					Speaking = false;
				}
				else
				{
					Bubble = new SpeechBubble(this, Lines[LinePtr]);
				}
			}

			if (ControlsEnabled && Input.CheckButtonPress(_previousLineButton))
			{
				// Don't ask.
				LinePtr -= 2;
				Speaking = true;
				if (LinePtr < -1)
				{
					LinePtr = -1;
				}
				if (Bubble != null)
				{
					Objects.Destroy(Bubble);
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

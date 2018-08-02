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
"Ahh, a new day begins.",
"Good morning, grass.",
"Good morning, sky.",
"Good morning, player.",
"...",
"Hm.",
"Something's wrong.",
"Oh.",
"My hands have been stolen.",
"Dang.",
"I guess, I should go" + Environment.NewLine + "and find the thief.",
"Yeah.",
"by the way," + Environment.NewLine + "I like playing videogames.",
"A lot.",
"Maybe even a bit too much.",
"It's fun to imagine the" + Environment.NewLine + "real world as a game.",
"I've even invented a" + Environment.NewLine + "control scheme for myself.",
"A controls my left paw," + Environment.NewLine + "D controls my right paw.",
"The more you hold the key," + Environment.NewLine + "the more the paw turns.",
"Pressing Space and holding" + Environment.NewLine + "it a bit will make me jump.",
"I can also repeat previously" + Environment.NewLine + "said stuff by pressing P.",
"In a way, I am playing myself.",
"Hehe.",
"But at the same time i'm always thinking.",
"Maybe these controls are too simplistic?",
"Like, the real world is so complex.",
"And so real.",
"How can you even make a control" + Environment.NewLine + "scheme for something like real life?",
"Same question, though, this" + Environment.NewLine + "also goes for modern games.",
"I remember playing Tomb Raider" + Environment.NewLine + "when I was young.",
"There were 4 types of jumps," + Environment.NewLine + "and each one was performed differently.",
"But take a look at any recent game.",
"Only one button for all your jumping needs.",
"It's kinda funny.",
"The more realistic games get," + Environment.NewLine + "the more primitive their controls become.",
"But I don't know, tho.",
"Is it even a bad thing?", //--------------
"Like, yeah, sometimes it really" + Environment.NewLine + "feels the game is playing itself.",
"But at the same time your character" + Environment.NewLine + "can do lots of cool stuff on his own.",
"Doing coold stuff requires skill.",
"And not many people have it.",
"But, i dunno.",
"do we want flashy images," + Environment.NewLine + "or do we want an actual game?",
"even i am not sure.",
"...",
"i also kinda like games structure.",
"you're getting progressively harder tasks" + Environment.NewLine + "and an epic boss at the end.",
"i wish something like that" + Environment.NewLine + "would be true in real life too.",
"defeating the tax boss in epic battle.",
"with lava.",
"and laser unicorns.",
"man, i need those unicorns.",
"i also saw an interesting thing in one game.",
"its terrain was a single huge" + Environment.NewLine + "ball of dirt and soil, and water.",
"it's hard for me to imagine" + Environment.NewLine + "anything other than floating isles would even work.",
"you'd probably need to break a lot of laws of physics."
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

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

		public Player(Vector2 pos) : base(pos, SpritesDefault.FoxBody, SpritesDefault.FoxFace)
		{
			var cam = new GameCamera();
			cam.Viewer = this;
			cam.Position = pos;
		}

		public override void Update()
		{
			_chargingJump = Input.CheckButton(_jumpButton);

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
			#endregion Movement controls.

			base.Update();
		}

	}
}

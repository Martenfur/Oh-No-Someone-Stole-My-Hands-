using Monofoxe.Engine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Resources.Sprites;
using NoHands.Logic;
using System;

namespace NoHands
{
	class Test : GameObj 
	{
		Camera cam = new Camera(800, 600);

		public Test()
		{
			GameCntrl.MaxGameSpeed = 60;
			
			cam.BackgroundColor = new Color(64, 32, 32);
			DrawCntrl.BlendState = BlendState.NonPremultiplied;

			GameCntrl.WindowManager.CanvasSize = new Vector2(800, 600);
			GameCntrl.WindowManager.Window.AllowUserResizing = false;
			GameCntrl.WindowManager.ApplyChanges();
			GameCntrl.WindowManager.CenterWindow();
			GameCntrl.WindowManager.CanvasMode = CanvasMode.Fill;
			
			new Character(new Vector2(200, 200));
			
			new Enemy(new Vector2(200, 200));
		}
		
		public override void Update()
		{
			if (Input.CheckButton(Buttons.MouseLeft))
			{
				new Solid(Input.MousePos, new Vector2(48, 48));
			}

		}

		
		public override void Draw()
		{
			DrawCntrl.CurrentColor = new Color(Color.Azure, 0.1f);
			DrawCntrl.DrawCircle(100, 100, 100, false);
			DrawCntrl.DrawCircle(120, 100, 100, false);
			DrawCntrl.CurrentColor = Color.White;
		}

		public static Vector2 RoundVector2(Vector2 vec) =>
			new Vector2((float)Math.Round(vec.X), (float)Math.Round(vec.Y));

	}
}
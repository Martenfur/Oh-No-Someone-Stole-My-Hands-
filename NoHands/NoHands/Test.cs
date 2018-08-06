using Monofoxe.Engine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Resources.Sprites;
using NoHands.Logic;
using System;
using Monofoxe.Engine.Drawing;

namespace NoHands
{
	public class Test : GameObj 
	{
		Camera cam = new Camera(1000, 800);
		public static Scene CurrentScene;

		bool _started = false;

		public Test()
		{
			GameCntrl.MaxGameSpeed = 60;
			DrawCntrl.Sampler = SamplerState.PointClamp;
			
			cam.BackgroundColor = new Color(142, 202, 255);
			DrawCntrl.BlendState = BlendState.NonPremultiplied;


			GameCntrl.WindowManager.CanvasSize = new Vector2(1000, 800);
			GameCntrl.WindowManager.Window.AllowUserResizing = false;
			GameCntrl.WindowManager.ApplyChanges();
			GameCntrl.WindowManager.CenterWindow();
			GameCntrl.WindowManager.CanvasMode = CanvasMode.Fill;
			
			cam.Offset = cam.Size / 2;

			
			Resources.Sounds.Load();

			Resources.Sounds.MusicMain.Play();
			Resources.Sounds.MusicMain.Loops = 1;
		}
		
		public override void Update()
		{
			if (!Resources.Sounds.MusicMain.IsPlaying)
			{	
				Resources.Sounds.MusicMain.Play();
			}

			if (Input.CheckButtonPress(Buttons.Space) && !_started)
			{
				_started = true;
				CurrentScene = new Scene(SpritesDefault.Main);
			}
		}

		
		public override void DrawBegin()
		{
			if (_started)
			{
				CurrentScene.DrawTileMap();
			}
			else
			{
				DrawCntrl.CurrentFont = Resources.Fonts.CartonSix;
				DrawCntrl.HorAlign = TextAlign.Center;
				DrawCntrl.VerAlign = TextAlign.Center;

				DrawCntrl.CurrentColor = Color.Black;
				DrawCntrl.DrawText(
				"oh no, someone stole my hands!" + Environment.NewLine + " " + Environment.NewLine + 
				"by gn.fur, aristokrat952, brodux and cybereye" + Environment.NewLine  + " " +  Environment.NewLine + 
				"monogame jam special" + Environment.NewLine  + " " +  Environment.NewLine + 
				"press space", 
				new Vector2(0, 0)
				);
			}
		}

		public static Vector2 RoundVector2(Vector2 vec) =>
			new Vector2((float)Math.Round(vec.X), (float)Math.Round(vec.Y));

	}
}
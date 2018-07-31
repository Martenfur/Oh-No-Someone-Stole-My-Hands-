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
		Scene scene;

		public Test()
		{
			GameCntrl.MaxGameSpeed = 60;
			DrawCntrl.Sampler = SamplerState.PointClamp;
			
			cam.BackgroundColor = Color.AliceBlue;
			DrawCntrl.BlendState = BlendState.NonPremultiplied;


			GameCntrl.WindowManager.CanvasSize = new Vector2(800, 600);
			GameCntrl.WindowManager.Window.AllowUserResizing = false;
			GameCntrl.WindowManager.ApplyChanges();
			GameCntrl.WindowManager.CenterWindow();
			GameCntrl.WindowManager.CanvasMode = CanvasMode.Fill;
			
			cam.Offset = cam.Size / 2;

			scene = new Scene(SpritesDefault.TestMap);

			Resources.Sounds.Load();


		}
		
		public override void Update()
		{

		}

		
		public override void DrawBegin()
		{
			scene.DrawTileMap();
		}

		public static Vector2 RoundVector2(Vector2 vec) =>
			new Vector2((float)Math.Round(vec.X), (float)Math.Round(vec.Y));

	}
}
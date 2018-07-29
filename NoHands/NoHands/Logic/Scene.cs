using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Monofoxe.Engine.Drawing;


namespace NoHands.Logic
{
	public class Scene
	{
		public static int CellSize = 96 / 2;

		public Sprite map;

		byte[] _wall = {0, 0};
		byte[] _player = {255, 0};


		public Scene(Sprite map)
		{
			var texture = map.Frames[0].Texture;
			var colorData = new Color[texture.Width * texture.Height];
			texture.GetData(colorData); // Map loader won't support sprites on texture atlas!!!
			
			for(var y = 0; y < texture.Height; y += 1)
			{
				for(var x = 0; x < texture.Width; x += 1)
				{
					var color = colorData[x + y * texture.Width];

					if (CheckValue(_wall, color))
					{
						new Solid(new Vector2(x, y) * CellSize, Vector2.One * CellSize);
					}
					if (CheckValue(_player, color))
					{
						new Character(new Vector2(x, y) * CellSize);
					}
				}
			}

		}

		bool CheckValue(byte[] value, Color color) =>
			value[0] == color.R && value[1] == color.G;
	}
}

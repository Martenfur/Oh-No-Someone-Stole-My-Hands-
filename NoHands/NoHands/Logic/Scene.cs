using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Monofoxe.Engine.Drawing;
using Monofoxe.Engine;
using Resources.Sprites;

namespace NoHands.Logic
{
	public class Scene
	{
		public static int CellSize = 64;

		public Sprite map;

		public int[,] TileMap;

		byte[] _wall = {0, 0};
		byte[] _player = {255, 106};
		byte[] _enemy = {255, 0};
		byte[] _enemyPath = {128, 0};
		byte[] _sky = {32, 64};
		byte[] _skyBorder = {32, 0};
		byte[] _npc = {33, 255};
		byte[] _paintedTile = {178, 0};
		

		public Scene(Sprite map)
		{
			var texture = map.Frames[0].Texture;
			var colorData = new Color[texture.Width * texture.Height];
			texture.GetData(colorData); // Map loader won't support sprites on texture atlas!!!
			
			TileMap = new int[texture.Width, texture.Height];

			for(var y = 0; y < texture.Height; y += 1)
			{
				for(var x = 0; x < texture.Width; x += 1)
				{
					TileMap[x, y] = 1;

					var color = colorData[x + y * texture.Width];

					if (CheckValue(_wall, color))
					{
						new Solid(new Vector2(x, y) * CellSize, Vector2.One * CellSize, SpritesDefault.FoxBody);
					}
					if (CheckValue(_player, color))
					{
						new Player(new Vector2(x, y) * CellSize + Vector2.One * CellSize / 2);
					}
					if (CheckValue(_enemy, color))
					{
						new Enemy(new Vector2(x, y) * CellSize + Vector2.One * CellSize / 2, CreatePath(x, y, colorData, texture.Width));
					}
					if (CheckValue(_sky, color))
					{
						TileMap[x, y] = 0;
					}
					if (CheckValue(_paintedTile, color))
					{
						TileMap[x, y] = 2;
					}
					if (CheckValue(_skyBorder, color))
					{
						new Solid(new Vector2(x, y) * CellSize, Vector2.One * CellSize);
						TileMap[x, y] = 1;
					}
					if (CheckValue(_npc, color))
					{
						new NPC(new Vector2(x, y) * CellSize + Vector2.One * CellSize / 2, color.B);
					}
				
				}
			}

		}

		bool CheckValue(byte[] value, Color color) =>
			value[0] == color.R && value[1] == color.G;

		

		Vector2[] _rotation = 
		{
			Vector2.UnitX,
			-Vector2.UnitY,
			-Vector2.UnitX,
			Vector2.UnitY,
		};

		List<Vector2> CreatePath(int x, int y, Color[] colorData, int texW)
		{
			var path = new List<Vector2>();

			var ptrVec = new Vector2(x, y);
			var ptrVecPrev = new Vector2(-x, -y);


			path.Add(ptrVec * CellSize + Vector2.One * CellSize / 2);

			var constructed = false;

			while(!constructed)
			{
				constructed = true;

				foreach(Vector2 side in _rotation)
				{
					var v = ptrVec + side;

					if (v != ptrVecPrev && CheckValue(_enemyPath, colorData[(int)(v.X + v.Y * texW)]))
					{
						path.Add(v * CellSize + Vector2.One * CellSize / 2);
						ptrVecPrev = ptrVec;
						ptrVec = v;
						constructed = false;
						break;
					}
				}

			}

			return path;
		}

		public void DrawTileMap()
		{
			for(var y = 0; y < TileMap.GetLength(1); y += 1)
			{
				for(var x = 0; x < TileMap.GetLength(0); x += 1)
				{
					if (TileMap[x, y] > 0)
					{
						DrawCntrl.DrawSprite(SpritesDefault.Tile, TileMap[x, y] - 1, x * CellSize, y * CellSize);
					}
				}
			}

		}
	}
}

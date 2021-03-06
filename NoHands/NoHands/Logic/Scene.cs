﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Monofoxe.Engine.Drawing;
using Monofoxe.Engine;
using Resources.Sprites;
using Monofoxe.Utils;

namespace NoHands.Logic
{
	public class Scene
	{
		public const int CellSize = 64;

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
		byte[] _laser = {128, 128};
		byte[] _checkpoint = {255, 216};
		byte[] _dialogue = {64, 64};
		byte[] _tree = {33, 128};
		byte[] _coin = {85, 39};
		

		float _tileDeg;
		float _tileAmpl = 2;

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
						new Solid(new Vector2(x, y) * CellSize, Vector2.One * CellSize, SpritesDefault.Block);
					}
					if (CheckValue(_tree, color))
					{
						new Solid(new Vector2(x, y) * CellSize, Vector2.One * CellSize, SpritesDefault.Tree);
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
						TileMap[x, y] = 0;
					}
					if (CheckValue(_npc, color))
					{
						new NPC(new Vector2(x, y) * CellSize + Vector2.One * CellSize / 2, color.B);
					}
					if (CheckValue(_laser, color))
					{
						new Laser(
							new Vector2(x, y) * CellSize + Vector2.One * CellSize / 2, 
							CreateLaserPath(x, y, colorData, texture.Width) * CellSize + Vector2.One * CellSize / 2
						);
					}
					if (CheckValue(_checkpoint, color))
					{
						new Checkpoint(new Vector2(x, y) * CellSize + Vector2.One * CellSize / 2);
					}
					if (CheckValue(_dialogue, color))
					{
						new DialogueTrigger(new Vector2(x, y) * CellSize + Vector2.One * CellSize / 2, color.B);
					}
					if (CheckValue(_coin, color))
					{
						new Coin(new Vector2(x, y) * CellSize);
					}
				}
			}

			var r = new RandomExt(42312342);

			for(var i = 0; i < 50; i += 1)
			{
				new Cloud(
					new Vector2(r.Next(-400, texture.Width * CellSize), r.Next(-400, texture.Height * CellSize)), 
					r.Next(3), 
					(float)r.NextDouble(0.3, 0.7)
				);
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

		Vector2 CreateLaserPath(int x, int y, Color[] colorData, int texW)
		{
			var ptrVec = new Vector2(x, y);
			var dirVec = Vector2.Zero;

			foreach(Vector2 side in _rotation)
			{
				var v = ptrVec + side;

				if (CheckValue(_enemyPath, colorData[(int)(v.X + v.Y * texW)]))
				{
					dirVec = side;
					break;
				}
			}

			if (dirVec == Vector2.Zero)
			{
				return ptrVec;
			}

			while(true)
			{
				var v = ptrVec + dirVec;

				if (CheckValue(_enemyPath, colorData[(int)(v.X + v.Y * texW)]))
				{
					ptrVec = v;
				}
				else
				{
					break;
				}
			}

			return ptrVec;
		}


		public void DrawTileMap()
		{
			_tileDeg += 0.1f;
			if (_tileDeg > Math.PI * 2)
			{
				_tileDeg -= (float)Math.PI * 2;
			}

			DrawCntrl.CurrentColor = Color.White;

			for(var y = 0; y < TileMap.GetLength(1); y += 1)
			{
				for(var x = 0; x < TileMap.GetLength(0); x += 1)
				{
					if (TileMap[x, y] > 0)
					{
						DrawCntrl.DrawSprite(SpritesDefault.Tile, TileMap[x, y] - 1, x * CellSize, y * CellSize + GetLift(new Vector2(x, y) * CellSize));
					}
				}
			}

		}

		public float GetLift(Vector2 pos) =>
			0;//(float)Math.Sin(_tileDeg + (int)(pos.Y + pos.X) / CellSize) * _tileAmpl;
	}
}

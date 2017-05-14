using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;


namespace Dino
{
	public class Map
	{
		public int[,] Layout;
		public Tile[] Tiles;
		public Texture2D tile1;
		public Texture2D tile2;

		public Map(int[,] layout, ContentManager content)
		{
			Layout = layout;
			tile1 = content.Load<Texture2D>("TesTileSided2");
			tile2 = content.Load<Texture2D>("PondTree");
			Tiles = new Tile[Layout.GetLength(0) * Layout.GetLength(1)];
		}

		public Tile this[Point mapCoord] // Indexer - tällä voi poimia kätevästi yksittäisen ruudun kuten CurrentMap[new Point(3, 5)]
		{
			get
			{
				return Tiles[mapCoord.X * 13 + (12 - mapCoord.Y)];
			}
		}

		public void MakeMap()
		{
			int i = 0;
			for (int row = 0; row < Layout.GetLength(0); row++)
			{	for (int col = Layout.GetUpperBound(1); col >= 0; col--)
				{
					Texture2D tex;
					if (Layout[row,col] == 1)
						tex = tile2;
					else tex = tile1;
					Tiles[i] = new Tile(new Point(row, col), tex);
					i++;
				}
			}

			for (int t = 0; t < Tiles.Length; t++)
			{
				Tiles[t].FillNeighborData();
			}
		}

		public static Vector2 MapToScreenCoord(Point mapCoord)
		{
			Vector2 output;
			output.X = (mapCoord.Y * Game1.tileHalfWidth) + (mapCoord.X * Game1.tileHalfWidth);
			output.Y = Game1.mapHalfHeight + (mapCoord.X * Game1.tileHalfHeight) - (mapCoord.Y * Game1.tileHalfHeight);
			return output;			
		}

		//public Vector2 GetTileCenter(Point mapCoord)
		//{
		//    return Tiles[mapCoord.X * 13 + (12 - mapCoord.Y)].Center;
		//}

		public void MapDraw(SpriteBatch sb)
		{
			//int num = 0;
			for (int i = 0; i < Tiles.Length; i++)
			{
				Tiles[i].TileDraw(sb);

				if (Tiles[i].ParentPlayer == Game1.Players[0])
					sb.Draw(Game1.TerritoryBall, Tiles[i].Center + new Vector2(-1, 25), Color.Tomato);
				else if (Tiles[i].ParentPlayer == Game1.Players[1])
					sb.Draw(Game1.TerritoryBall, Tiles[i].Center + new Vector2(-1, 25), Color.Turquoise);

				//sb.DrawString(Game1.font, Tiles[i].MapCoord.X + "," + Tiles[i].MapCoord.Y, Tiles[i].ScreenCoord + new Vector2(50,50), Color.White);
				//sb.DrawString(Game1.font, num.ToString(), Tiles[i].ScreenCoord + new Vector2(50,50), Color.White);
				//num++;
			}
		}
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DinoWin10
{
	public enum Tiletype
	{
		Empty = 0,
		Blueberry = 1,
		Lingonberry = 2
	}

	public class Tile
	{
		public Point MapCoord;
		public Vector2 ScreenCoord;
		public Vector2 Center;
		public Player ParentPlayer;
		public Tile[] Neighbors;
		public Tiletype Type;
		public Texture2D Texture;
		public const int BaseYield = 1;

		public Tile(Point mapCoord, Texture2D texture)
		{
			MapCoord = mapCoord;
			ScreenCoord = Map.MapToScreenCoord(MapCoord);
			Center = ScreenCoord + new Vector2(Game1.tileHalfWidth, Game1.tileHalfHeight);
			Texture = texture;
		}

		public void FillNeighborData()
		{
			int max = Game1.CurrentMap.Layout.GetUpperBound(0);
			Neighbors = new Tile[8];

			if (MapCoord.X > 0 && MapCoord.Y < max)
				Neighbors[0] = Game1.CurrentMap[new Point(MapCoord.X - 1, MapCoord.Y + 1)];
			if (MapCoord.Y < max)
				Neighbors[1] = Game1.CurrentMap[new Point(MapCoord.X, MapCoord.Y + 1)];
			if (MapCoord.X < max && MapCoord.Y < max)
				Neighbors[2] = Game1.CurrentMap[new Point(MapCoord.X + 1, MapCoord.Y + 1)];
			if (MapCoord.X < max)
				Neighbors[3] = Game1.CurrentMap[new Point(MapCoord.X + 1, MapCoord.Y)];
			if (MapCoord.X < max && MapCoord.Y > 0)
				Neighbors[4] = Game1.CurrentMap[new Point(MapCoord.X + 1, MapCoord.Y - 1)];
			if (MapCoord.Y > 0)
				Neighbors[5] = Game1.CurrentMap[new Point(MapCoord.X, MapCoord.Y - 1)];
			if (MapCoord.X > 0 && MapCoord.Y > 0)
				Neighbors[6] = Game1.CurrentMap[new Point(MapCoord.X - 1, MapCoord.Y - 1)];
			if (MapCoord.X > 0)
				Neighbors[7] = Game1.CurrentMap[new Point(MapCoord.X - 1, MapCoord.Y)];
		}

		public void TileDraw(SpriteBatch sb)
		{
			sb.Draw(Texture, ScreenCoord, Color.White);
			//sb.DrawString(Game1.font, MapCoord.X.ToString() + "," + MapCoord.Y.ToString(), Center, Color.White);
		}
	}
}

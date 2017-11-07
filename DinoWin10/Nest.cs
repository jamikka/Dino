using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DinoWin10
{
	public class Nest : PlayerObject
	{
		public static int[] LVLthresholds = { 10, 30, 60, 100 };
		public List<Tile> TerritoryTiles;

		public Nest(Player parent, Point coord)
		{
			ParentPlayer = parent;
			MapCoord = coord;
			ScreenLocation = Game1.CurrentMap[coord].Center;
			SpriteSheet = Game1.NestTex;
			Origin = new Vector2(SpriteSheet.Width * 0.5f, SpriteSheet.Height * 0.5f);
			TerritoryTiles = new List<Tile>();
		}
	}
}

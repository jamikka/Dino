using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DinoWin10
{
	public class SettlerDino : Dino
	{
		public SettlerDino(Player parent, Point pos)
			: base(parent, pos)
		{
			SpriteSheet = Game1.SettlerDinoTex;
			CurrFrameRectangle = new Rectangle(0, 0, SpriteSheet.Width / 2, SpriteSheet.Height / 2);
			Origin = new Vector2(CurrFrameRectangle.Width * 0.5f, CurrFrameRectangle.Height * 0.5f);

			if (ParentPlayer == Game1.Players[0])
				CurrentFrame = 3;
			else
				CurrentFrame = 1;
		}

		public void Nestle()
		{
			Nest newNest = new Nest(ParentPlayer, MapCoord);
			ParentPlayer.Nests.Add(newNest);
			Game1.activePlayerNests.Add(newNest);
			Game1.activePlayerObjs.Add(newNest);
			Game1.activeObj = newNest;
			//Game1.activePlayerNest = newNest;
			Game1.activePlayerDinos.Remove(this);
			Game1.activePlayerObjs.Remove(this);
			ParentPlayer.SettlerDinos.Remove(this);

			Tile currentTile = Game1.CurrentMap[MapCoord];
			newNest.TerritoryTiles.Add(currentTile);
			newNest.TerritoryTiles.Add(Game1.CurrentMap[currentTile.Neighbors[1].MapCoord]);
			newNest.TerritoryTiles.Add(Game1.CurrentMap[currentTile.Neighbors[3].MapCoord]);
			newNest.TerritoryTiles.Add(Game1.CurrentMap[currentTile.Neighbors[5].MapCoord]);
			newNest.TerritoryTiles.Add(Game1.CurrentMap[currentTile.Neighbors[7].MapCoord]);

			currentTile.ParentPlayer = ParentPlayer;
			Game1.CurrentMap[currentTile.Neighbors[1].MapCoord].ParentPlayer = ParentPlayer;
			Game1.CurrentMap[currentTile.Neighbors[3].MapCoord].ParentPlayer = ParentPlayer;
			Game1.CurrentMap[currentTile.Neighbors[5].MapCoord].ParentPlayer = ParentPlayer;
			Game1.CurrentMap[currentTile.Neighbors[7].MapCoord].ParentPlayer = ParentPlayer;
		}
	}
}

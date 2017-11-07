using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DinoWin10
{
	public class ScoutDino : Dino
	{
		public ScoutDino(Player parent, Point mapCoord) 
			: base (parent, mapCoord)
		{
			SpriteSheet = Game1.ScoutDinoTex;
			CurrFrameRectangle = new Rectangle(0, 0, SpriteSheet.Width / 2, SpriteSheet.Height / 2);
			Origin = new Vector2(CurrFrameRectangle.Width * 0.5f, CurrFrameRectangle.Height * 0.5f);

			if (ParentPlayer == Game1.Players[0])
				CurrentFrame = 3;
			else
				CurrentFrame = 1;
		}
	}
}

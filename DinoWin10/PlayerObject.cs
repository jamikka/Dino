using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DinoWin10
{
	public class PlayerObject
	{
		internal Player ParentPlayer;
		internal Vector2 ScreenLocation;
		public Point MapCoord;
		internal Texture2D SpriteSheet;
		internal Rectangle CurrFrameRectangle;
		internal Vector2 Origin;

		public void ObjDraw(SpriteBatch sb)
		{
			sb.Draw(SpriteSheet, ScreenLocation, null, Color.White, 0, Origin, 1, SpriteEffects.None, 0);
		}
	}
}

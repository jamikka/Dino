using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Dino
{
	public static class HUD
	{
		public static Texture2D ActivePlayerObjCircle;
		public static Vector2 ObjCircleOrigin;

		public static void HUDDraw(SpriteBatch sb)
		{
			sb.Draw(ActivePlayerObjCircle, Game1.activeObj.ScreenLocation, null, Color.White, 0, ObjCircleOrigin, 1, SpriteEffects.None, 0);
		}
	}
}

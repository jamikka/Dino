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
			//sb.Draw(ActivePlayerObjCircle, Game1.activeObj.ScreenLocation, null, Color.White, 0, ObjCircleOrigin, 1, SpriteEffects.None, 0);

			sb.DrawString(Game1.font, Game1.activePlayer.MovementPoints.ToString(), new Vector2(1100, 100), Color.Orange);

			Color turnColor;
			if (Game1.activePlayer == Game1.Players[0])
				turnColor = Color.Tomato;
			else
				turnColor = Color.Turquoise;
			sb.DrawString(Game1.font, "Turn " + Game1.TurnCounter, new Vector2(Game1.gD.Viewport.Width * 0.6f, 20), turnColor);
		}
	}
}

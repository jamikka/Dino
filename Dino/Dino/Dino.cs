using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Dino
{
	public class Dino : PlayerObject
	{
		internal int MovementPoints;
		internal int SightRange;
		Vector2 PartialMove;
		int TravelCounter;
		bool IsTravelling;
		Direction Orientation;
		const int SprShRows = 2;
		const int SprShCols = 2;
		public int CurrentFrame 
		{
			get { return SpriteSheet.Width / CurrFrameRectangle.X + (SprShCols * SpriteSheet.Height / CurrFrameRectangle.Y); }
			set { CurrFrameRectangle = new Rectangle((value % SprShCols) * CurrFrameRectangle.Width, (value / SprShRows) * CurrFrameRectangle.Height, CurrFrameRectangle.Width, CurrFrameRectangle.Height); }
		}

		public Dino(Player parent, Point mapCoord)
		{
			ParentPlayer = parent;
			MapCoord = mapCoord;
			//ScreenLocation = Game1.CurrentMap.GetTileCenter(mapCoord);
			ScreenLocation = Game1.CurrentMap[mapCoord].Center;
		}

		public void MoveOneStep(Direction dir)
		{
			Point targetCoord = Point.Zero;
			Vector2 initPos = ScreenLocation;

			switch (dir)
			{
				case Direction.N: targetCoord = new Point(MapCoord.X - 1, MapCoord.Y + 1); CurrentFrame = 1; break;
				case Direction.NE: targetCoord = new Point(MapCoord.X, MapCoord.Y + 1); break;
				case Direction.E: targetCoord = new Point(MapCoord.X + 1, MapCoord.Y + 1); CurrentFrame = 0; break;
				case Direction.SE: targetCoord = new Point(MapCoord.X + 1, MapCoord.Y); break;
				case Direction.S: targetCoord = new Point(MapCoord.X + 1, MapCoord.Y - 1); CurrentFrame = 3; break;
				case Direction.SW: targetCoord = new Point(MapCoord.X, MapCoord.Y - 1); break;
				case Direction.W: targetCoord = new Point(MapCoord.X - 1, MapCoord.Y - 1); CurrentFrame = 2; break;
				case Direction.NW: targetCoord = new Point(MapCoord.X - 1, MapCoord.Y); break;
			}

			if (targetCoord.X >= 0 && targetCoord.X < Game1.CurrentMap.Layout.GetLength(0) && targetCoord.Y >= 0 && targetCoord.Y < Game1.CurrentMap.Layout.GetLength(1))
			{
				if (ParentPlayer.MovementPoints > 0 || MovementPoints > 0)
				{
					MapCoord = targetCoord;
					Vector2 targetPos = Game1.CurrentMap[MapCoord].Center;
					float distance = Vector2.Distance(initPos, targetPos);
					PartialMove = (targetPos - initPos) * 0.03125f; // 1/32
					TravelCounter = 32;
					IsTravelling = true;

					if (MovementPoints > 0)
						MovementPoints--;
					else
						ParentPlayer.MovementPoints--;
				}
			}
		}

		public void Update()
		{
			if (IsTravelling)
			{
				if (TravelCounter <= 0)
					IsTravelling = false;
				else
					ScreenLocation += PartialMove;

				TravelCounter--;
			}
		}

		public void DinoDraw(SpriteBatch sb)
		{
			sb.Draw(SpriteSheet, ScreenLocation, CurrFrameRectangle, Color.White, 0, Origin, 1, SpriteEffects.None, 0);

			if (MovementPoints > 0)
				sb.DrawString(Game1.font, MovementPoints.ToString(), ScreenLocation, Color.Orange);
		}
	}
}

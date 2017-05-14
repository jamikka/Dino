using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Dino
{
	public class Player
	{
		int Score;
		public int MovementPoints;
		public ScoutDino Scout;
		public List<SettlerDino> SettlerDinos;
		public List<Nest> Nests;

		public Player()
		{
			Score = 0;
			SettlerDinos = new List<SettlerDino>();
			Nests = new List<Nest>();
		}

		public void Update()
		{
			Scout.Update();

			for (int i = 0; i < SettlerDinos.Count; i++)
			{
				SettlerDinos[i].Update();
			}
		}

		public void PlayerDraw(SpriteBatch sb)
		{
			for (int i = 0; i < Nests.Count; i++)
				Nests[i].ObjDraw(sb);

			for (int i = 0; i < SettlerDinos.Count; i++)
				SettlerDinos[i].DinoDraw(sb);

			Scout.DinoDraw(sb);
		}
	}
}

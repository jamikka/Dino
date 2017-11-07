using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace DinoWin10
{
	public static class InputManager
	{
		static KeyboardState prevKeyboard;
		static MouseState prevMouse;

		public static void Update(MouseState mouse, KeyboardState keyboard)
		{
			Game1.mousePos = new Vector2(mouse.X, mouse.Y);

			Keys justPressedKey = Keys.None;
			if (keyboard.GetPressedKeys().Length > 0)
			{
				if (prevKeyboard.IsKeyUp(keyboard.GetPressedKeys()[0]))
					justPressedKey = keyboard.GetPressedKeys()[0];
			}

			// Click-select
			for (int i = 0; i < Game1.activePlayerObjs.Count; i++)
			{
				if (Vector2.Distance(Game1.mousePos, Game1.activePlayerObjs[i].ScreenLocation) < 45)
				{
					if (mouse.LeftButton == ButtonState.Pressed && prevMouse.LeftButton == ButtonState.Released)
						Game1.activeObj = Game1.activePlayerObjs[i];
				}
			}

			if (justPressedKey != Keys.None) // ------------- KEY PRESSED -----------
			{
				if (justPressedKey == Keys.Escape) // Exit
					Game1.CurrentGame.Exit();

				if (justPressedKey == Keys.Enter) // Switch turn
				{
					if (Game1.activePlayer == Game1.Players[0])
						Game1.SwitchTurn(1);
					else
						Game1.SwitchTurn(0);
				}

				if (justPressedKey == Keys.N && Game1.activeObj is SettlerDino) // Nestle
					((SettlerDino)Game1.activeObj).Nestle();

				if (justPressedKey == Keys.Tab) // Cycle
				{
					if (Game1.activeObj is Dino)
					{
						int i = Game1.activePlayerDinos.IndexOf((Dino)Game1.activeObj);
						if (i == Game1.activePlayerDinos.Count - 1)
							i = 0;
						else
							i++;
						Game1.activeObj = Game1.activePlayerDinos[i];
					}
					else if (Game1.activeObj is Nest)
					{
						int i = Game1.activePlayerNests.IndexOf((Nest)Game1.activeObj);
						if (i == Game1.activePlayerNests.Count - 1)
							i = 0;
						else
							i++;
						Game1.activeObj = Game1.activePlayerNests[i];
					}

					//int i = Game1.activePlayerObjs.IndexOf(Game1.activeObj);
				}

				if (Game1.activeObj is Dino) // Dino movement
				{
					Dino selectedDino = (Dino)Game1.activeObj;
					switch (justPressedKey)
					{
						case Keys.NumPad8: selectedDino.MoveOneStep(Direction.N); break;
						case Keys.NumPad9: selectedDino.MoveOneStep(Direction.NE); break;
						case Keys.NumPad6: selectedDino.MoveOneStep(Direction.E); break;
						case Keys.NumPad3: selectedDino.MoveOneStep(Direction.SE); break;
						case Keys.NumPad2: selectedDino.MoveOneStep(Direction.S); break;
						case Keys.NumPad1: selectedDino.MoveOneStep(Direction.SW); break;
						case Keys.NumPad4: selectedDino.MoveOneStep(Direction.W); break;
						case Keys.NumPad7: selectedDino.MoveOneStep(Direction.NW); break;
					}
				}
				else if (Game1.activeObj is Nest) // Nest commands
				{

				}
			}

			prevKeyboard = Keyboard.GetState();
			prevMouse = Mouse.GetState();
		}
	}
}

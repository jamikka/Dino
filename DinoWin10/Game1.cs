using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace DinoWin10
{
    public enum Direction
	{
		N = 8,
		NE = 9,
		E = 6,
		SE = 3,
		S = 2,
		SW = 1,
		W = 4,
		NW = 7
	}

	/// <summary>
	/// This is the main type for your game
	/// </summary>
	public class Game1 : Microsoft.Xna.Framework.Game
	{
		// ---- DECLARE - NIMETÄÄN GLOBAALIT MUUTTUJAT ----
		
		// Tekstuurit ja fontti
		public Texture2D tile1;
		public Texture2D tile2;
		public static Texture2D TerritoryBall;
		public static Texture2D ScoutDinoTex;
		public static Texture2D SettlerDinoTex;
		public static Texture2D NestTex;
		public static SpriteFont font;

		// Inputmuuttujat
		public static KeyboardState keyboard;
		public static KeyboardState prevKeyboard;
		public static MouseState mouse;
		public static MouseState prevMouse;
		public static Vector2 mousePos;

		// Muut
		public static Game1 CurrentGame;
		public static Player[] Players;
		public static Map CurrentMap;
		public static Player activePlayer;
		public static List<PlayerObject> activePlayerObjs;
		public static PlayerObject activeObj;
		public static List<Dino> activePlayerDinos;
		public static List<Nest> activePlayerNests;

		public static int TurnCounter;
		public static int tileHalfWidth;
		public static int tileHalfHeight;
		public static int mapHalfHeight;

		// Grafiikkamuuttujat piirtofunktioihin
		public static GraphicsDevice gD;
		GraphicsDeviceManager graphics;
		SpriteBatch spriteBatch;

		// Efektimuuttujat
		int fctMode;
		RenderTarget2D mainTarget;
		RenderTarget2D lightsTarget;
		Effect fct;
		Texture2D lightMask;
		Vector2 lightMaskOrigin;

		public Game1()
		{
			// ---- ASETETAAN PELIN ASETUKSET ----
			graphics = new GraphicsDeviceManager(this);
			Content.RootDirectory = "Content";
			graphics.PreferredBackBufferWidth = 1600;
			graphics.PreferredBackBufferHeight = 950;
			IsMouseVisible = true;
		}

		/// <summary>
		/// Allows the game to perform any initialization it needs to before starting to run.
		/// This is where it can query for any required services and load any non-graphic
		/// related content.  Calling base.Initialize will enumerate through any components
		/// and initialize them as well.
		/// </summary>
		protected override void Initialize()
		{
			// ---- SYNNYTETÄÄN NUMEERISET MUUTTUJAT ----
			CurrentMap = new Map(new int[13, 13], Content);
			CurrentMap.Layout[0,5] = 1;
			CurrentMap.Layout[2, 11] = 1;
			CurrentMap.Layout[5, 7] = 1;

			Players = new Player[2];

			base.Initialize();
		}

		/// <summary>
		/// LoadContent will be called once per game and is the place to load
		/// all of your content.
		/// </summary>
		protected override void LoadContent()
		{
			// ---- SYNNYTETÄÄN KAIKKI MUU ----
			CurrentGame = this;
			gD = GraphicsDevice;

			// TEKSTUURIEN LATAUS
			spriteBatch = new SpriteBatch(GraphicsDevice);
			font = Content.Load<SpriteFont>("Testfont");
			ScoutDinoTex = Content.Load<Texture2D>("scoutDinoSprSh");
			SettlerDinoTex = Content.Load<Texture2D>("SettlerDinoTex1");
			HUD.ActivePlayerObjCircle = Content.Load<Texture2D>("SelectRing");
			HUD.ObjCircleOrigin = new Vector2(HUD.ActivePlayerObjCircle.Width * 0.5f, HUD.ActivePlayerObjCircle.Height * 0.5f);
			NestTex = Content.Load<Texture2D>("testNest");
			TerritoryBall = Content.Load<Texture2D>("ball-5x5");

			// SYNNYTETÄÄN MAPPIDIMENSIOT LADATTUJEN TEKSTUURIEN POHJALTA
			tileHalfWidth = CurrentMap.tile1.Width / 2-1;
			tileHalfHeight = CurrentMap.tile1.Height / 2 - 4;
			mapHalfHeight = tileHalfHeight * CurrentMap.Layout.GetUpperBound(0);
			CurrentMap.MakeMap();

			// Pelaajien ja niiden dinojen synnytys
			Players[0] = new Player();
			Players[1] = new Player();
			activePlayer = Players[0];

			Players[0].Scout = new ScoutDino(Players[0], new Point(2, 10));
			Players[0].SettlerDinos.Add(new SettlerDino(Players[0], new Point(0, 10)));
			Players[0].SettlerDinos.Add(new SettlerDino(Players[0], new Point(1, 11)));
			Players[0].SettlerDinos.Add(new SettlerDino(Players[0], new Point(2, 12)));

			Players[1].Scout = new ScoutDino(Players[1], new Point(10, 2));
			Players[1].SettlerDinos.Add(new SettlerDino(Players[1], new Point(10, 0)));
			Players[1].SettlerDinos.Add(new SettlerDino(Players[1], new Point(11, 1)));
			Players[1].SettlerDinos.Add(new SettlerDino(Players[1], new Point(12, 2)));

			// On tässä, jotta activePlayerobj asetetaan pelin alkuun
			SwitchTurn(0);

			// Efektimuuttujien synnytys
			lightMask = Content.Load<Texture2D>("lightMask");
			lightMaskOrigin = new Vector2(lightMask.Width * 0.5f, lightMask.Height * 0.5f);
			fct = Content.Load<Effect>("Shaders");
			PresentationParameters pp = GraphicsDevice.PresentationParameters;
			mainTarget = new RenderTarget2D(GraphicsDevice, pp.BackBufferWidth, pp.BackBufferHeight);
			lightsTarget = new RenderTarget2D(GraphicsDevice, pp.BackBufferWidth, pp.BackBufferHeight);
		}

		public static void SwitchTurn(int playerIndex)
		{
			activePlayer = Players[playerIndex];
			activeObj = Players[playerIndex].Scout;
			activePlayerDinos = new List<Dino>();
			activePlayerObjs = new List<PlayerObject>();
			activePlayerNests = new List<Nest>();

			activePlayerDinos.Add(Players[playerIndex].Scout);
			for (int i = 0; i < Players[playerIndex].SettlerDinos.Count; i++)
				activePlayerDinos.Add(Players[playerIndex].SettlerDinos[i]);
			for (int i = 0; i < Players[playerIndex].Nests.Count; i++)
				activePlayerNests.Add(Players[playerIndex].Nests[i]);
			activePlayerObjs.AddRange(activePlayerDinos);
			activePlayerObjs.AddRange(activePlayerNests);

			activePlayer.MovementPoints = activePlayer.SettlerDinos.Count * 2;
			activePlayer.Scout.MovementPoints = 1;

			if (activePlayer == Players[0])
				TurnCounter++;
		}

		/// <summary>
		/// UnloadContent will be called once per game and is the place to unload
		/// all content.
		/// </summary>
		protected override void UnloadContent()
		{
		}

		/// <summary>
		/// Allows the game to run logic such as updating the world,
		/// checking for collisions, gathering input, and playing audio.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Update(GameTime gameTime)
		{
			InputManager.Update(Mouse.GetState(), Keyboard.GetState()); // Luetaan kaikki framessa tapahtuvat inputit ja lähetetään käskyt eteenpäin
			activePlayer.Update(); // Päivitetään pelaajan objektit
			// vilja: Jospa inputmanager loisi listan inputeista, joka lähetettäisi eteenpäin toteuttajalle? (jami selvittää eventpohjaistuksen vaatimukset)

			#region EFEKTIPÄIVITYKSET
			if (Keyboard.GetState().IsKeyDown(Keys.Q) && prevKeyboard.IsKeyUp(Keys.Q))
			{
				if (fctMode == 1)
					fctMode = 0;
				else
					fctMode++;
			}

			if (Keyboard.GetState().IsKeyDown(Keys.E) && prevKeyboard.IsKeyUp(Keys.E))
			{
				float currValue = fct.Parameters["blurSizeX"].GetValueSingle();
				fct.Parameters["blurSizeX"].SetValue(currValue - 0.0005f);
				fct.Parameters["blurSizeY"].SetValue(currValue - 0.0005f);
			}
			if (Keyboard.GetState().IsKeyDown(Keys.R) && prevKeyboard.IsKeyUp(Keys.R))
			{
				float currValue = fct.Parameters["blurSizeX"].GetValueSingle();
				fct.Parameters["blurSizeX"].SetValue(currValue + 0.0005f);
				fct.Parameters["blurSizeY"].SetValue(currValue + 0.0005f);
			}
			#endregion

			//
			prevKeyboard = Keyboard.GetState(); // siirrä inputmanageriin
			base.Update(gameTime);
		}

		/// <summary>
		/// This is called when the game should draw itself.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Draw(GameTime gameTime)
		{
			if (fctMode == 0)
			{
				GraphicsDevice.Clear(Color.Black); // Taustaväri
				spriteBatch.Begin();

				CurrentMap.MapDraw(spriteBatch);

				HUD.HUDDraw(spriteBatch);

				for (int i = 0; i < Players.Length; i++)
					Players[i].PlayerDraw(spriteBatch);

				spriteBatch.End();
			}
			else if (fctMode == 2)
			{
				GraphicsDevice.SetRenderTarget(lightsTarget);
				GraphicsDevice.Clear(Color.Black);
				spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive);
				spriteBatch.Draw(lightMask, mousePos - lightMaskOrigin, Color.White);
				spriteBatch.Draw(lightMask, activeObj.ScreenLocation - lightMaskOrigin, Color.White);
				//for (int i = 0; i < activePlayerObjs.Count; i++)
				//{
				//    spriteBatch.Draw(lightMask, activePlayerObjs[i].ScreenLocation - lightMaskOrigin, Color.White);
				//}
				spriteBatch.End();


				GraphicsDevice.SetRenderTarget(mainTarget);
				GraphicsDevice.Clear(Color.Black);
				spriteBatch.Begin();

				CurrentMap.MapDraw(spriteBatch);

				//HUD.HUDDraw(spriteBatch);

				for (int i = 0; i < Players.Length; i++)
					Players[i].PlayerDraw(spriteBatch);

				//spriteBatch.DrawString(font, mousePos.ToString(), mousePos, Color.Tan);

				spriteBatch.End();


				GraphicsDevice.SetRenderTarget(null);
				GraphicsDevice.Clear(Color.Black);
				spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive);
				HUD.HUDDraw(spriteBatch);
				fct.CurrentTechnique = fct.Techniques[0];
				fct.Parameters["lightMask"].SetValue(lightsTarget);
				fct.CurrentTechnique.Passes[0].Apply();
				spriteBatch.Draw(mainTarget, Vector2.Zero, Color.White);
				spriteBatch.End();
			}
			else if (fctMode == 1)
			{
				fct.CurrentTechnique = fct.Techniques[1];

				GraphicsDevice.SetRenderTarget(mainTarget);
				GraphicsDevice.Clear(Color.Black);
				spriteBatch.Begin();
				//CurrentMap.MapDraw(spriteBatch);
				//HUD.HUDDraw(spriteBatch);
				if (activeObj is Dino)
				{
					Dino curDin = (Dino)activeObj;
					curDin.DinoDraw(spriteBatch);
				}
				else
					activeObj.ObjDraw(spriteBatch);
				spriteBatch.End();

				GraphicsDevice.SetRenderTarget(lightsTarget);
				GraphicsDevice.Clear(Color.Black);
				spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive);
				fct.CurrentTechnique = fct.Techniques[1];
				fct.CurrentTechnique.Passes[0].Apply();
				spriteBatch.Draw(mainTarget, Vector2.Zero, Color.White);
				spriteBatch.End();

				GraphicsDevice.SetRenderTarget(null);
				GraphicsDevice.Clear(Color.Black);
				spriteBatch.Begin();
				CurrentMap.MapDraw(spriteBatch);
				HUD.HUDDraw(spriteBatch);
				spriteBatch.End();
				spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive);
				fct.CurrentTechnique.Passes[1].Apply();
				spriteBatch.Draw(lightsTarget, Vector2.Zero, Color.White);
				spriteBatch.End();
				spriteBatch.Begin();
				for (int i = 0; i < Players.Length; i++)
					Players[i].PlayerDraw(spriteBatch);
				spriteBatch.End();
				
			}
			//spriteBatch.Begin();
			//GraphicsDevice.Clear(Color.Black);

			//CurrentMap.MapDraw(spriteBatch);

			//HUD.HUDDraw(spriteBatch);

			////fct.CurrentTechnique.Passes[0].Apply();

			//for (int i = 0; i < Players.Length; i++)
			//    Players[i].PlayerDraw(spriteBatch);

			//spriteBatch.DrawString(font, activePlayer.MovementPoints.ToString(), new Vector2(1100, 100), Color.Orange);


			//Color turnColor;
			//if (activePlayer == Players[0])
			//    turnColor = Color.Tomato;
			//else
			//    turnColor = Color.Turquoise;
			//spriteBatch.DrawString(font, "Turn " + TurnCounter, new Vector2(GraphicsDevice.Viewport.Width * 0.6f, 20), turnColor);


			////spriteBatch.DrawString(font, mousePos.ToString(), mousePos, Color.Tan);

			//spriteBatch.End();

			base.Draw(gameTime);
		}
	}
}

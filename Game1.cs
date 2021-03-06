using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;
using TurnBasedStrategy.Classes.Objects.Items;
using TurnBasedStrategy.Classes.Objects.Terrain;

namespace TurnBasedStrategy
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            Metal steel = new Metal(MetalType.Steel);
            Unit uUnit1 = new Unit(80, 15, new Weapon(WeaponType.Sword, steel), new Armor(steel));
            Unit uUnit2 = new Unit(80, 15, new Weapon(WeaponType.Axe, steel), new Armor(steel));
            Terrain tt = new Terrain();

            tt.AddNeighboringTerrain(Directions.North, new Terrain());
            tt.AddNeighboringTerrain(Directions.NorthEast, new Terrain(new List<Directions>{Directions.SouthWest, Directions.South}));

            tt.IsDirectionTravelable(Directions.North);
            tt.IsDirectionTravelable(Directions.NorthWest);
            tt.IsDirectionTravelable(Directions.NorthEast);
            tt.IsDirectionTravelable(Directions.South);
            tt.IsDirectionTravelable(Directions.SouthWest);
            tt.IsDirectionTravelable(Directions.SouthEast);
            tt.IsDirectionTravelable(Directions.East);
            tt.IsDirectionTravelable(Directions.West);

            uUnit1.AddOpponent(uUnit2);
            uUnit2.ChargeBonus = true;
            
            while (true)
            {
            uUnit1.ProcessBattles();
            }


        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}

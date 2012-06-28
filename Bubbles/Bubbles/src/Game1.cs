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

namespace Bubbles
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        SpriteFont mainfont;
        RenderTarget2D renderTarget;
        string message = "And now a very long test message whououuuuuu";
        Tooltip ttip1;
        bool wasPressed = false;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
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
            IsMouseVisible = true;
            mainfont = Content.Load<SpriteFont>("mainfont");
            graphics.PreferMultiSampling = true;
            graphics.SynchronizeWithVerticalRetrace = true;

            Tools.Init(GraphicsDevice, spriteBatch,Content);
            Widget.Init(GraphicsDevice, Content);

            /*
            string strCmdText;
            strCmdText = "/C mkdir har"; // /C to close the terminal immediatly after completion
            System.Diagnostics.Process.Start("CMD.exe", strCmdText);
             * */

            renderTarget = new RenderTarget2D(
                GraphicsDevice,
                8*message.Length,
                mainfont.LineSpacing);

            Vector2 font_size = mainfont.MeasureString("a");
            Console.WriteLine("font format: {0}x{1} ({2})", font_size.X, font_size.Y,mainfont.LineSpacing);

            GraphicsDevice.SetRenderTarget(renderTarget);
            GraphicsDevice.Clear(Color.Transparent);
            spriteBatch.Begin();
            spriteBatch.DrawString(mainfont, message, Vector2.Zero, Color.Red);
            spriteBatch.End();

            GraphicsDevice.SetRenderTarget(null);

            ttip1 = new Tooltip("Hello from tooltip 1", new Vector2(100, 50));
            new Tooltip("I am the child of ttip1 =)", new Vector2(100, 100), ttip1);
            
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

            if (Keyboard.GetState().IsKeyDown(Keys.A) && !wasPressed)
            {
                ttip1.Message += "A";
                wasPressed = true;
            }
            if (Keyboard.GetState().IsKeyUp(Keys.A))
            {
                wasPressed = false;
            }
            // TODO: Add your update logic here

            ttip1.Update(gameTime);

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(new Color(0,0,0.1f));
            
            /*
            spriteBatch.Begin();
            //spriteBatch.DrawString(mainfont, message, Vector2.Zero, Color.White);
            spriteBatch.Draw(renderTarget, new Rectangle(0, 0, 8*message.Length,mainfont.LineSpacing), Color.White);
            spriteBatch.Draw(ttip1.Texture, ttip1.Destination, Color.White);
            // Tools.DrawLine(4, Color.Wheat, new Vector2(0, 0), new Vector2(300, 300));

            
            //using (System.IO.Stream stream = System.IO.File.OpenWrite("string.png"))
            //{
            //    renderTarget.SaveAsPng(stream, 8 * message.Length, mainfont.LineSpacing);
            //}
            
            int i = 0;
            foreach (char c in message)
            {
                spriteBatch.DrawString(mainfont, c.ToString(), new Vector2(8 * i, mainfont.LineSpacing), new Color((float)i / message.Length, 1 - (float)i / message.Length, 0));
                i++;
            }
            spriteBatch.End();*/

            ttip1.Draw();

            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}

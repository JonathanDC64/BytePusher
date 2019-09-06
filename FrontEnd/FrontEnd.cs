using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using System.Collections.Generic;

namespace FrontEnd
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class FrontEnd : Game
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;

        private SpriteBatch targetBatch;
        private RenderTarget2D target;

        private BytePusher.Emulator emulator;

        private DynamicSoundEffectInstance audio;

        private Texture2D frameBuffer;
        private Rectangle drawDestination;

        private Dictionary<Keys, BytePusher.Keyboard.Keys> keyMap = new Dictionary<Keys, BytePusher.Keyboard.Keys>()
        {
            {Keys.D1, BytePusher.Keyboard.Keys.Key1},
            {Keys.D2, BytePusher.Keyboard.Keys.Key2},
            {Keys.D3, BytePusher.Keyboard.Keys.Key3},
            {Keys.D4, BytePusher.Keyboard.Keys.KeyC},

            {Keys.Q,  BytePusher.Keyboard.Keys.Key4},
            {Keys.W,  BytePusher.Keyboard.Keys.Key5},
            {Keys.E,  BytePusher.Keyboard.Keys.Key6},
            {Keys.R,  BytePusher.Keyboard.Keys.KeyD},

            {Keys.A,  BytePusher.Keyboard.Keys.Key7},
            {Keys.S,  BytePusher.Keyboard.Keys.Key8},
            {Keys.D,  BytePusher.Keyboard.Keys.Key9},
            {Keys.F,  BytePusher.Keyboard.Keys.KeyE},

            {Keys.Z,  BytePusher.Keyboard.Keys.KeyA},
            {Keys.X,  BytePusher.Keyboard.Keys.Key0},
            {Keys.C,  BytePusher.Keyboard.Keys.KeyB},
            {Keys.V,  BytePusher.Keyboard.Keys.KeyF},
        };

        public FrontEnd()
        {
            this.graphics = new GraphicsDeviceManager(this);
            this.Window.Title = "BytePusher Emulator";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            this.targetBatch = new SpriteBatch(GraphicsDevice);
            this.target = new RenderTarget2D(GraphicsDevice, (int)BytePusher.Graphics.WIDTH, (int)BytePusher.Graphics.HEIGHT);

            this.emulator = new BytePusher.Emulator();
            this.emulator.LoadRom("../../../../../Roms/Munching_Squares.BytePusher");

            this.audio = new DynamicSoundEffectInstance(15360, AudioChannels.Mono);
            this.audio.Play();

            this.frameBuffer = new Texture2D(GraphicsDevice, BytePusher.Graphics.WIDTH, BytePusher.Graphics.HEIGHT);
            this.drawDestination = new Rectangle(0, 0, Window.ClientBounds.Width, Window.ClientBounds.Height);

            this.IsFixedTimeStep = true;
            this.TargetElapsedTime = System.TimeSpan.FromSeconds(1d / 60d);

            this.IsMouseVisible = true;
            this.Window.AllowUserResizing = true;

            this.graphics.PreferredBackBufferWidth = 512;
            this.graphics.PreferredBackBufferHeight = 512;
            this.graphics.ApplyChanges();



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
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
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
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // Poll Keyboard
            foreach(Keys key in keyMap.Keys)
            {
                bool isDown = Keyboard.GetState().IsKeyDown(key);
                emulator.SetKey(keyMap[key], isDown);
            }

            // Execute Current frame of CPU cycle
            emulator.ExecuteCPU();

            // Send current sound sample to the audio buffer
            audio.SubmitBuffer(emulator.Sound);

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            // draw to buffer
            GraphicsDevice.SetRenderTarget(target);

            //nearest neighboor scaling
            spriteBatch.Begin(samplerState: SamplerState.PointClamp);

            // Clear currently drawn frame
            GraphicsDevice.Clear(Color.CornflowerBlue);
            graphics.GraphicsDevice.Clear(Color.CornflowerBlue);
            
            // Write pixel data to texture
            frameBuffer.SetData<uint>(emulator.PixelData);

            // Draw texture frame to spritebatch
            spriteBatch.Draw(frameBuffer, frameBuffer.Bounds, Color.White);

            spriteBatch.End();

            //set rendering back to the back buffer
            GraphicsDevice.SetRenderTarget(null);

            //nearest neighboor scaling
            targetBatch.Begin(samplerState: SamplerState.PointClamp);

            targetBatch.Draw(target, new Rectangle(0, 0, Window.ClientBounds.Width, Window.ClientBounds.Height), Color.White);

            this.Window.Title = "BytePusher Emulator " + (1 / (float)gameTime.ElapsedGameTime.TotalSeconds);

            targetBatch.End();

            base.Draw(gameTime);
        }
    }
}

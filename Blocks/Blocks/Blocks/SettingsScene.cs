using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Blocks
{
    public class SettingsScene : GameScene
    {
        private SpriteBatch spriteBatch;

        private Rectangle rectangle;

        private Level Level { get; set; }

        private Texture2D btnTexture;

        private Button btnEasy, btnMedium, btnHard;
        private Rectangle easyRect = new(0, 0, BUTTON_WIDTH, BUTTON_HEIGHT);

        const int BUTTON_WIDTH = 200;
        const int BUTTON_HEIGHT = 80;

        public bool isBackToAction { get; set; } = false;


        public SettingsScene(Game game) : base(game)
        {
            Game1 g = (Game1)game;

            Level = FileManager.Instance.LoadSettings();

            btnTexture = g.Content.Load<Texture2D>("btn");

            var regularFont = g.Content.Load<SpriteFont>("RegularFont");
            var highlightFont = g.Content.Load<SpriteFont>("TitleFont");

            g.IsMouseVisible = true;

            btnEasy = new Button(btnTexture, easyRect, new Vector2((g.GraphicsDevice.PresentationParameters.BackBufferWidth - easyRect.Width) / 2, 100), "Easy", regularFont, highlightFont); 
            btnMedium = new Button(btnTexture, easyRect, new Vector2((g.GraphicsDevice.PresentationParameters.BackBufferWidth - easyRect.Width) / 2, 200), "Medium", regularFont, highlightFont);
            btnHard = new Button(btnTexture, easyRect, new Vector2((g.GraphicsDevice.PresentationParameters.BackBufferWidth - easyRect.Width) / 2, 300), "Hard", regularFont, highlightFont);


            this.spriteBatch = g._spriteBatch;

            rectangle = new Rectangle(0, 0, g.GraphicsDevice.Viewport.Width, g.GraphicsDevice.Viewport.Height);
        }


        public override void Update(GameTime gameTime)
        {
            if (btnEasy.isClicked())
            {
                Level = Level.Easy;
                SaveSettings();
            }
            else if (btnMedium.isClicked())
            {
                Level = Level.Medium;
                SaveSettings();
            }
            else if (btnHard.isClicked())
            {
                Level = Level.Hard;
                SaveSettings();
            }

            base.Update(gameTime);
        }

        private void SaveSettings()
        {
            isBackToAction = true;
            FileManager.Instance.SaveSettings(Level);
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();

            spriteBatch.Draw(Game.Content.Load<Texture2D>("Background"), rectangle, Color.White);


            // Add title using TitleFont
            spriteBatch.DrawString(Game.Content.Load<SpriteFont>("TitleFont"), "Settings", new Vector2(50, 50), Color.White);

            btnEasy.Draw(spriteBatch, Level == Level.Easy );
            btnMedium.Draw(spriteBatch, Level == Level.Medium);
            btnHard.Draw(spriteBatch, Level == Level.Hard);


            spriteBatch.End();
            base.Draw(gameTime);
        }
    }


}

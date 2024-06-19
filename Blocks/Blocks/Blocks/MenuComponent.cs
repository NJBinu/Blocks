using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Blocks
{
    public class MenuComponent : DrawableGameComponent
    {
        private SpriteBatch spriteBatch;
        private Texture2D btnTexture;
        private SpriteFont font;

        public int SelectedIndex { get; set; }

        private KeyboardState oldState;

        Button btnStart, btnAbout, btnQuit, btnHelp, btnCredit, btnSettings;

        const int BUTTON_WIDTH = 200;
        const int BUTTON_HEIGHT = 100;


        private Texture2D bgTexture;

        private Rectangle rectangle;

        Rectangle startRect = new(0, 0, BUTTON_WIDTH, BUTTON_HEIGHT);
        Rectangle aboutRect = new(0, BUTTON_HEIGHT + 5, BUTTON_WIDTH, BUTTON_HEIGHT);
        Rectangle helpRect = new(0, BUTTON_HEIGHT * 2 + 10, BUTTON_WIDTH, BUTTON_HEIGHT);
        Rectangle creditRect = new(0, BUTTON_HEIGHT * 3 + 15, BUTTON_WIDTH, BUTTON_HEIGHT);
        Rectangle settingsRect = new(0, BUTTON_HEIGHT * 4 + 20, BUTTON_WIDTH, BUTTON_HEIGHT);
        Rectangle quitReact = new(0, BUTTON_HEIGHT * 5 + 25, BUTTON_WIDTH, BUTTON_HEIGHT);


        public MenuComponent(Game game,
            SpriteBatch spriteBatch,
            Texture2D texture,
            SpriteFont buttonFont) : base(game)
        {
            this.spriteBatch = spriteBatch;
            this.btnTexture = texture;
            this.font = buttonFont;

            game.IsMouseVisible = false;

            int startY = 20;
            var preferredBufferWidth = game.GraphicsDevice.PresentationParameters.BackBufferWidth;


            bgTexture = game.Content.Load<Texture2D>("Background");
            var selectedButtonFont = game.Content.Load<SpriteFont>("RegularFont");

            rectangle = new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);

            btnStart = new Button(btnTexture, startRect, new Vector2((preferredBufferWidth - startRect.Width) / 2, startY), "Start", buttonFont, selectedButtonFont);
            btnAbout = new Button(btnTexture, aboutRect, new Vector2((preferredBufferWidth - aboutRect.Width) / 2, startY + startRect.Height), "About", buttonFont, selectedButtonFont);
            btnHelp = new Button(btnTexture, helpRect, new Vector2((preferredBufferWidth - helpRect.Width) / 2, startY + 2 * (startRect.Height)), "Help", buttonFont, selectedButtonFont);
            btnCredit = new Button(btnTexture, creditRect, new Vector2((preferredBufferWidth - creditRect.Width) / 2, startY + 3 * (startRect.Height)), "Credit", buttonFont, selectedButtonFont);
            btnSettings = new Button(btnTexture, settingsRect, new Vector2((preferredBufferWidth - quitReact.Width) / 2, startY + 4 * (startRect.Height)), "Settings", buttonFont, selectedButtonFont);
            btnQuit = new Button(btnTexture, quitReact, new Vector2((preferredBufferWidth - quitReact.Width) / 2, startY + 5 * (startRect.Height)), "Quit", buttonFont, selectedButtonFont);

        }


        public override void Update(GameTime gameTime)
        {
            KeyboardState ks = Keyboard.GetState();
            if (ks.IsKeyDown(Keys.Down) && oldState.IsKeyUp(Keys.Down))
            {
                SelectedIndex++;
                if (SelectedIndex == 6)
                {
                    SelectedIndex = 0;
                }
            }

            if (ks.IsKeyDown(Keys.Up) && oldState.IsKeyUp(Keys.Up))
            {
                SelectedIndex--;
                if (SelectedIndex == -1)
                {
                    SelectedIndex = 6 - 1;
                }
            }

            oldState = ks;

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            Game.GraphicsDevice.Clear(Color.AntiqueWhite);

            spriteBatch.Begin();


            spriteBatch.Draw(bgTexture, rectangle, Color.White);

            btnStart.Draw(spriteBatch, SelectedIndex == 0);
            btnAbout.Draw(spriteBatch, SelectedIndex == 1);
            btnHelp.Draw(spriteBatch, SelectedIndex == 2);
            btnCredit.Draw(spriteBatch, SelectedIndex == 3);
            btnSettings.Draw(spriteBatch, SelectedIndex == 4);
            btnQuit.Draw(spriteBatch, SelectedIndex == 5);
            string helperText = "Move up and down to select, press enter to confirm";
            spriteBatch.DrawString(font, helperText, new Vector2((Game.GraphicsDevice.Viewport.Width - font.MeasureString(helperText).X) / 2, Game.GraphicsDevice.Viewport.Height - 50), Color.Black);

            spriteBatch.End();


            base.Draw(gameTime);
        }

    }
}

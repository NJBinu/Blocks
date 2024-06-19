using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Blocks
{
    public class CreditScene : GameScene
    {
        private SpriteBatch spriteBatch;
        private Texture2D backTexture;

        private Rectangle rectangle;

        public CreditScene(Game game) : base(game)
        {
            Game1 g = (Game1)game;
            this.spriteBatch = g._spriteBatch;
            backTexture = g.Content.Load<Texture2D>("Credit");

            rectangle = new Rectangle(0, 0, g.GraphicsDevice.Viewport.Width, g.GraphicsDevice.Viewport.Height);
        }


        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(backTexture, rectangle, Color.White);


            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}

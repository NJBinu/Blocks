using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace Blocks
{

    public class ActionScene : GameScene
    {
        private SpriteBatch spriteBatch;
        private Rectangle boardRect;
        private Tetromino fallingTetromino;
        private Vector2 tetrominoPosition;
        private double fallTime;
        private double fallSpeed = 1.0;
        private int blockSize = 60;

        private bool[,] grid;

        private GameState gameState = GameState.Playing;

        private TetrominoFactory tetrominoFactory;

        Game1 g;

        private GameLevel gameLevel;

        private double lateralMoveTime;
        private double lateralMoveSpeed = 0.15; // Adjust this value to slow down the side movement



        public ActionScene(Game game) : base(game)
        {
            g = (Game1)game;
            this.spriteBatch = g._spriteBatch;

            gameLevel = new GameLevel(FileManager.Instance.LoadSettings());
            blockSize = gameLevel.GetBlockSize();
            fallSpeed = gameLevel.GetFallSpeed();

            g.IsMouseVisible = false;



            tetrominoFactory = new TetrominoFactory(g.GraphicsDevice, blockSize);

            fallingTetromino = tetrominoFactory.CreateRandomTetromino();
            tetrominoPosition = new Vector2(g.GraphicsDevice.Viewport.Width / 2 - blockSize * 2, 0); // Adjust the starting position based on the block size

            //landedBlocks = new List<Vector2>();

            // Initialize the falling tetromino
            fallingTetromino = tetrominoFactory.CreateRandomTetromino();
            tetrominoPosition = new Vector2(g.GraphicsDevice.Viewport.Width / 2 - blockSize, 0); // Position adjusted for blockSize of 60


            // fill the screen
            boardRect = new Rectangle(0, 0, g.GraphicsDevice.Viewport.Width, g.GraphicsDevice.Viewport.Height);

            grid = new bool[boardRect.Width / blockSize, boardRect.Height / blockSize];

        }


        public void Reload()
        {
            gameLevel = new GameLevel(FileManager.Instance.LoadSettings());
            blockSize = gameLevel.GetBlockSize();
            fallSpeed = gameLevel.GetFallSpeed();

            grid = new bool[boardRect.Width / blockSize, boardRect.Height / blockSize];



            tetrominoFactory = new TetrominoFactory(g.GraphicsDevice, blockSize);

            fallingTetromino = tetrominoFactory.CreateRandomTetromino();
            tetrominoPosition = new Vector2(g.GraphicsDevice.Viewport.Width / 2 - blockSize * 2, 0); // Adjust the starting position based on the block size

            fallingTetromino = tetrominoFactory.CreateRandomTetromino();
            tetrominoPosition = new Vector2(g.GraphicsDevice.Viewport.Width / 2 - blockSize, 0); // Position adjusted for blockSize of 60
        }

        public override void Update(GameTime gameTime)
        {
            KeyboardState keyboardState = Keyboard.GetState();
            lateralMoveTime += gameTime.ElapsedGameTime.TotalSeconds;

            // Use lateralMoveTime to control the speed of horizontal movement
            if (keyboardState.IsKeyDown(Keys.Left) && lateralMoveTime >= lateralMoveSpeed)
            {
                tetrominoPosition.X -= blockSize; // Move left by one block size.
                lateralMoveTime = 0; // Reset the lateral move timer
            }
            if (keyboardState.IsKeyDown(Keys.Right) && lateralMoveTime >= lateralMoveSpeed)
            {
                tetrominoPosition.X += blockSize; // Move right by one block size.
                lateralMoveTime = 0; // Reset the lateral move timer
            }

            if(gameState == GameState.GameOver && keyboardState.IsKeyDown(Keys.Enter))
            {
                Reload();
                gameState = GameState.Playing;
            }

            if (CheckGameOver())
            {
                gameState = GameState.GameOver;
            }

            var maxRight = boardRect.Width - blockSize;

            switch (fallingTetromino.Shape)
            {
                case TetrominoShape.I:
                    maxRight = boardRect.Width - blockSize * 4;
                    break;
                case TetrominoShape.O:
                    maxRight = boardRect.Width - blockSize * 2;
                    break;
                case TetrominoShape.T:
                    maxRight = boardRect.Width - blockSize * 3;
                    break;
                case TetrominoShape.L:
                    maxRight = boardRect.Width - blockSize * 3;
                    break;
                case TetrominoShape.J:
                    maxRight = boardRect.Width - blockSize * 3;
                    break;
                case TetrominoShape.S:
                    maxRight = boardRect.Width - blockSize * 3;
                    break;
                case TetrominoShape.Z:
                    maxRight = boardRect.Width - blockSize * 3;
                    break;

            }

            tetrominoPosition.X = MathHelper.Clamp(tetrominoPosition.X, 0, maxRight); // Clamp within board width

            bool hasLanded = CheckForLanding();

            if (!hasLanded)
            {
                fallTime += gameTime.ElapsedGameTime.TotalSeconds;

                if (fallTime >= 1.0 / fallSpeed)
                {
                    tetrominoPosition.Y += blockSize; // Move down by one block size.
                    fallTime = 0;
                }
            }
            else
            {
                // Add the blocks of the landed Tetromino to the list of landed blocks
                foreach (var block in fallingTetromino.Blocks)
                {
                    // landedBlocks.Add(tetrominoPosition + block * blockSize);

                    Vector2 worldPosition = tetrominoPosition + block * blockSize;
                    grid[(int)worldPosition.X / blockSize, (int)worldPosition.Y / blockSize] = true;

                }

                // Generate a new Tetromino
                fallingTetromino = tetrominoFactory.CreateRandomTetromino();
                tetrominoPosition = new Vector2(g.GraphicsDevice.Viewport.Width / 2 - blockSize * 2, 0);
            }
            ClearLines();

            base.Update(gameTime);
        }
        private bool CheckGameOver()
        {
            

            for (int x = 0; x < grid.GetLength(0); x++)
            {
                if (grid[x, 0])
                {
                    return true;
                }
            }

            return false;

        }

        private bool CheckForLanding()
        {
            foreach (var block in fallingTetromino.Blocks)
            {
                Vector2 worldPosition = tetrominoPosition + block * blockSize;


                //Check if the Tetromino has collided with landed blocks
                for (int x = 0; x < grid.GetLength(0); x++)
                {
                    for (int y = 0; y < grid.GetLength(1); y++)
                    {

                        if (grid[x, y])
                        {
                            if (worldPosition.X == x * blockSize && worldPosition.Y + blockSize == y * blockSize)
                            {
                                return true;
                            }

                            if (worldPosition.Y == y * blockSize - blockSize && worldPosition.X == x * blockSize)
                            {
                                foreach (var block2 in fallingTetromino.Blocks)
                                {
                                    Vector2 worldPosition2 = tetrominoPosition + block2 * blockSize;
                                    grid[(int)worldPosition2.X / blockSize, (int)worldPosition2.Y / blockSize] = true;

                                }

                                this.gameState = GameState.GameOver;

                            }
                        }

                    }
                }

                // Check if the Tetromino has reached the bottom of the board
                if (worldPosition.Y + blockSize >= boardRect.Height)
                {

                    foreach (var block2 in fallingTetromino.Blocks)
                    {
                        Vector2 worldPosition2 = tetrominoPosition + block2 * blockSize;
                        grid[(int)worldPosition2.X / blockSize, (int)worldPosition2.Y / blockSize] = true;

                    }

                    return true;
                }

            }

            return false; // The Tetromino has not landed
        }

        private void ClearLines()
        {
            for (int y = 0; y < grid.GetLength(1); y++)
            {
                bool isLine = true;
                for (int x = 0; x < grid.GetLength(0); x++)
                {
                    if (!grid[x, y])
                    {
                        isLine = false;
                        break;
                    }
                }

                if (isLine)
                {
                    for (int x = 0; x < grid.GetLength(0); x++)
                    {
                        grid[x, y] = false;
                    }

                    for (int y2 = y; y2 > 0; y2--)
                    {
                        for (int x = 0; x < grid.GetLength(0); x++)
                        {
                            grid[x, y2] = grid[x, y2 - 1];
                        }
                    }

                    for (int y2 = y; y2 > 0; y2--)
                    {
                        for (int x = 0; x < grid.GetLength(0); x++)
                        {
                            grid[x, y2] = grid[x, y2 - 1];
                        }
                    }

                }
            }
        }

        public override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin();

            if(gameState == GameState.GameOver)
            {
                // Draw the string "Game Over" in the center of the screen
                var font = g.Content.Load<SpriteFont>("RegularFont");
                var text = "Game Over";
                text += "\nPress Enter to restart";
                var textSize = font.MeasureString(text);
                var textPosition = new Vector2((boardRect.Width - textSize.X) / 2, (boardRect.Height - textSize.Y) / 2);

                spriteBatch.DrawString(font, text, textPosition, Color.White);
                spriteBatch.End();
                return;
            }

            // Assume each block is 30x30 pixels
            Color blockColor = new Color(40, 40, 40);
            Color alterBlockColor = new Color(50, 50, 50);
            Color landedBlockColor = new Color(255, 144, 60);
            Texture2D blockTexture = new Texture2D(GraphicsDevice, 1, 1);
            blockTexture.SetData(new[] { Color.White }); // Create a 1x1 white texture to color later

            // Draw the game board dynamically
            for (int x = 0; x < boardRect.Width; x += blockSize)
            {
                for (int y = 0; y < boardRect.Height; y += blockSize)
                {
                    var color = (x + y) % (blockSize * 2) == 0 ? blockColor : alterBlockColor;
                    spriteBatch.Draw(blockTexture, new Rectangle(x, y, blockSize, blockSize), color);
                }
            }

            // Draw landed blocks
            //foreach (var block in landedBlocks)
            //{
            //    spriteBatch.Draw(blockTexture, new Rectangle((int)block.X, (int)block.Y, blockSize, blockSize), landedBlockColor);
            //}

            // Draw landed blocks
            for (int x = 0; x < grid.GetLength(0); x++)
            {
                for (int y = 0; y < grid.GetLength(1); y++)
                {

                    if (grid[x, y])
                    {
                        spriteBatch.Draw(blockTexture, new Rectangle(x * blockSize, y * blockSize, blockSize, blockSize), landedBlockColor);
                    }

                }
            }

            // Draw the falling tetromino with the block size
            fallingTetromino.Draw(spriteBatch, tetrominoPosition, blockSize);

            spriteBatch.End();
            base.Draw(gameTime);
        }

    }


}

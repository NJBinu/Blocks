using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Blocks
{
    public class Tetromino
    {
        private Texture2D texture;
        private TetrominoShape shape;
        private Vector2[] blocks; // Each tetromino consists of four blocks.
        private Color color;
        private int blockSize;

        public Vector2[] Blocks { get { return blocks; } }

        public TetrominoShape Shape { get { return shape; } }


        public Tetromino(GraphicsDevice graphicsDevice, TetrominoShape shape, Color color, int blockSize)
        {
            this.shape = shape;
            this.color = color;
            this.blockSize = blockSize;
            InitializeBlocks();

            // Create a single block texture
            texture = new Texture2D(graphicsDevice, blockSize, blockSize);

            Color[] colorData = new Color[blockSize * blockSize];

            int borderThickness = 5; // for example, 5 pixels
            Color borderColor = Color.Black; // Border color

            for (int y = 0; y < blockSize; y++)
            {
                for (int x = 0; x < blockSize; x++)
                {
                    bool isBorder = x < borderThickness || x >= blockSize - borderThickness ||
                                    y < borderThickness || y >= blockSize - borderThickness;

                    if (isBorder)
                    {
                        colorData[y * blockSize + x] = borderColor;
                    }
                    else
                    {
                        // Apply vignette effect
                        float distanceToCenter = Vector2.Distance(new Vector2(x, y), new Vector2(blockSize / 2, blockSize / 2));
                        float maxDistance = blockSize / 2f;
                        float vignetteEffect = MathHelper.Clamp(1 - (distanceToCenter / maxDistance), 0.3f, 1f); // adjust 0.3f to change vignette strength

                        Color originalColor = color; // Your original block color
                        colorData[y * blockSize + x] = new Color(originalColor.R * vignetteEffect,
                                                                 originalColor.G * vignetteEffect,
                                                                 originalColor.B * vignetteEffect);
                    }
                }
            }

            texture.SetData(colorData);

        }

        private void InitializeBlocks()
        {
            blocks = new Vector2[4];
            switch (shape)
            {
                case TetrominoShape.I:
                    blocks[0] = new Vector2(0, 0);
                    blocks[1] = new Vector2(1, 0);
                    blocks[2] = new Vector2(2, 0);
                    blocks[3] = new Vector2(3, 0);
                    break;
                case TetrominoShape.O:
                    blocks[0] = new Vector2(0, 0);
                    blocks[1] = new Vector2(1, 0);
                    blocks[2] = new Vector2(0, 1);
                    blocks[3] = new Vector2(1, 1);
                    break;
                case TetrominoShape.T:
                    blocks[0] = new Vector2(1, 0);
                    blocks[1] = new Vector2(0, 1);
                    blocks[2] = new Vector2(1, 1);
                    blocks[3] = new Vector2(2, 1);
                    break;
                case TetrominoShape.L:
                    blocks[0] = new Vector2(2, 0);
                    blocks[1] = new Vector2(0, 1);
                    blocks[2] = new Vector2(1, 1);
                    blocks[3] = new Vector2(2, 1);
                    break;
                case TetrominoShape.J:
                    blocks[0] = new Vector2(0, 0);
                    blocks[1] = new Vector2(0, 1);
                    blocks[2] = new Vector2(1, 1);
                    blocks[3] = new Vector2(2, 1);
                    break;
                case TetrominoShape.S:
                    blocks[0] = new Vector2(1, 0);
                    blocks[1] = new Vector2(2, 0);
                    blocks[2] = new Vector2(0, 1);
                    blocks[3] = new Vector2(1, 1);
                    break;
                case TetrominoShape.Z:
                    blocks[0] = new Vector2(0, 0);
                    blocks[1] = new Vector2(1, 0);
                    blocks[2] = new Vector2(1, 1);
                    blocks[3] = new Vector2(2, 1);
                    break;
            }
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 position, int blockSize)
        {
            foreach (var block in blocks)
            {
                Vector2 blockPosition = position + block * blockSize;
                Rectangle destinationRectangle = new Rectangle((int)blockPosition.X, (int)blockPosition.Y, blockSize, blockSize);
                spriteBatch.Draw(texture, destinationRectangle, color);
            }
        }
    }

}

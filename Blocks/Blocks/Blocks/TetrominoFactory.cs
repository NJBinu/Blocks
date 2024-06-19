using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Blocks
{
    public class TetrominoFactory
    {
        private Random random;
        private GraphicsDevice graphicsDevice;
        private Color[] colors;
        private int blockSize;

        public TetrominoFactory(GraphicsDevice graphicsDevice, int blockSize)
        {
            this.graphicsDevice = graphicsDevice;
            this.blockSize = blockSize;
            random = new Random();
            colors = new Color[]
            {
            Color.Cyan, Color.Blue, Color.Orange, Color.Yellow,
            Color.Green, Color.Purple, Color.Red
            };
        }

        public Tetromino CreateRandomTetromino()
        {
            TetrominoShape shape = (TetrominoShape)random.Next(Enum.GetNames(typeof(TetrominoShape)).Length);
            Color color = colors[random.Next(colors.Length)];
            return new Tetromino(graphicsDevice, shape, color, blockSize);
        }
    }


}

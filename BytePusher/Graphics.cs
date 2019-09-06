namespace BytePusher
{
    public class Graphics
    {
        private byte[,] graphics;

        public byte[,] GraphicsData
        {
            get => this.graphics;
        }

        public readonly static int WIDTH  = 256;
        public readonly static int HEIGHT = 256;
 

        public Graphics()
        {
            this.graphics = new byte[256, 256];
        }

        public void WriteByte(byte pixelData, int X, int Y)
        {
            this.graphics[Y, X] = pixelData;
        }

        public byte ReadByte(int X, int Y)
        {
            return this.graphics[Y, X];
        }
    }
}

namespace BytePusher
{
    public class Sound
    {
        private byte[] samples;

        public byte[] SamplesData
        {
            get => this.samples;
        }

        public readonly static int SAMPLE_SIZE = 256;

        public Sound()
        {
            this.samples = new byte[SAMPLE_SIZE];
        }

        public void WriteSampleByte(byte sample, int Z)
        {
            this.samples[Z] = sample;
        }
    }
}

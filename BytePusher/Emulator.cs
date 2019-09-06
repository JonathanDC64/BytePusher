using System;
using System.IO;

namespace BytePusher
{
    // Facade for VM components, Interfaces with the Front-End
    public class Emulator
    {

        private readonly Memory memory;
        private readonly CPU cpu;
        private readonly Keyboard keyboard;
        private readonly Graphics graphics;
        private readonly Sound sound;

        public Emulator()
        {
            this.memory = new Memory();
            this.graphics = new Graphics();
            this.keyboard = new Keyboard();
            this.sound = new Sound();
            this.cpu = new CPU(memory, graphics, keyboard, sound);
        }

        public void LoadRom(string filePath)
        {
            byte[] romData = File.ReadAllBytes(filePath);
            this.memory.LoadRom(romData);
        }

        public void ExecuteCPU()
        {
            this.cpu.Execute();
        }

        public void SetKey(Keyboard.Keys key, bool value)
        {
            this.keyboard.SetKey(key, value);
        }

        public UInt32[] PixelData
        {
            get
            {
                UInt32[] pixels = new UInt32[Graphics.WIDTH * Graphics.HEIGHT];

                for (int y = 0; y < BytePusher.Graphics.HEIGHT; ++y)
                {
                    for (int x = 0; x < BytePusher.Graphics.WIDTH; ++x)
                    {
                        byte pixel = this.graphics.GraphicsData[y, x];
                        if (pixel < 0xD8)
                        {

                            byte blue = (byte)((pixel % 6));
                            byte green = (byte)((((pixel - blue) / 6) % 6));
                            byte red = (byte)((((pixel - blue - (6 * green)) / 36) % 6));
                            pixels[y * BytePusher.Graphics.HEIGHT + x] = 
                                (UInt32)(
                                     
                                    ((red << 0) * 0x33) | 
                                    ((green << 8) * 0x33) | 
                                    ((blue << 16) * 0x33) |
                                    (0xFF << 24)
                                );
                            //new Color(red * 0x33, green * 0x33, blue * 0x33);
                        }
                        else
                        {
                            pixels[y * BytePusher.Graphics.HEIGHT + x] = (UInt32)0xFF000000;//new Color(0, 0, 0);
                        }
                    }
                }
                return pixels;
            }
        }

        //public byte[,] Graphics
        //{
        //    get => this.graphics.GraphicsData;
        //}

        public byte[] Sound
        {
            get
            {
                byte[] soundData = this.sound.SamplesData;
                byte[] audioSample = new byte[soundData.Length * 2];
                var index = 0;
                foreach (var data in soundData)
                {
                    audioSample[index++] = 0;
                    audioSample[index++] = data;
                }
                return audioSample;
            }
        }

    }
}

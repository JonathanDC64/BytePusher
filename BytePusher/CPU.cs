using System;

namespace BytePusher
{
    public class CPU
    {
        private Memory memory;
        private Graphics graphics;
        private Keyboard keyboard;
        private Sound sound;
        private UInt32 pc;

        public CPU(Memory memory, Graphics graphics, Keyboard keyboard, Sound sound)
        {
            this.memory = memory;
            this.graphics = graphics;
            this.keyboard = keyboard;
            this.sound = sound;
            this.pc = 0;
        }

        // Executes single instruction
        public void Execute()
        {
            // Outer loop
            // 
            // 1. Wait for the next timer tick(60 ticks are generated per second).
            // 2. Poll the keys and store their states as a 2 - byte value at address 0.
            // 3. Fetch the 3 - byte program counter from address 2, and execute exactly 65536 instructions.
            // 4. Send the 64 - KiB pixeldata block designated by the byte value at address 5 to the display device.Send the 256 - byte sampledata block designated by the 2 - byte value at address 6 to the audio device.
            // 5. Go back to step 1.


            // 1. Wait for the next timer tick(60 ticks are generated per second).
            // Done in front end

            // 2. Poll the keys and store their states as a 2 - byte value at address 0.
            byte[] keyboardStateBytes = BitConverter.GetBytes(keyboard.State);
            memory.WriteBytes(Memory.MemoryMap.KeyboardState, keyboardStateBytes[0], keyboardStateBytes[1]);
            
            // 3. Fetch the 3 - byte program counter from address 2, and execute exactly 65536 instructions.
            pc = memory.Read3Bytes(Memory.MemoryMap.ProgramCounter);

            for(int i = 0; i < 65536; ++i)
            {
                // An instruction consists of 3 big-endian 24-bit addresses A,B,C stored consecutively in memory. 
                UInt32 A = memory.Read3Bytes(pc + 0);
                UInt32 B = memory.Read3Bytes(pc + 3);

                // The operation performed is: Copy 1 byte from A to B, then jump to C.
                memory[B] = memory[A];

                // An instruction should be able to modify its own jump address before jumping. 
                // Thus the copy operation must be completed before the jump address is read. 
                UInt32 C = memory.Read3Bytes(pc + 6);

                // Jump to jump address
                pc = C;
            }

            // 4.Send the 64 - KiB pixeldata block designated by the byte value at address 5 to the display device.
            UInt16 ZZ = memory[Memory.MemoryMap.PixelData];

            for(int YY = 0; YY < Graphics.HEIGHT; ++YY)
            {
                for(int XX = 0; XX < Graphics.WIDTH; ++XX)
                {
                    // A value of ZZ means: pixel(XX, YY) is at address ZZYYXX. 
                    UInt32 pixelAddress = (UInt32)((ZZ << 16) | (YY << 8) | (XX << 0));
                    byte pixelData = memory[pixelAddress];
                    graphics.WriteByte(pixelData, XX, YY);
                }
            }

            //   Send the 256 - byte sampledata block designated by the 2 - byte value at address 6 to the audio device.
            UInt16 XXYY = memory.Read2Bytes(Memory.MemoryMap.SoundData);

            for(ZZ =  0; ZZ < Sound.SAMPLE_SIZE; ++ZZ)
            {
                //  A value of XXYY means: audio sample ZZ is at address XXYYZZ. 
                UInt32 sampleAddress = (UInt32)((XXYY << 8) | (ZZ << 0));
                byte sample = memory[sampleAddress];
                sound.WriteSampleByte(sample, ZZ);
            }


            // 5. Go back to step 1.
        }
    }
}

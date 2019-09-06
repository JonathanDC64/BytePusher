using System;

namespace BytePusher
{
    // Address  Bytes   Description
    // 0 	    2 	    Keyboard state.Key X = bit X.
    // 2 	    3 	    The program counter is fetched from this address at the beginning of each frame.
    // 5 	    1 	    A value of ZZ means: pixel(XX, YY) is at address ZZYYXX.
    // 6 	    2 	    A value of XXYY means: audio sample ZZ is at address XXYYZZ.

    public class Memory
    {
        public enum MemoryMap : uint
        {
            KeyboardState = 0x0,
            ProgramCounter = 0x2,
            PixelData = 0x5,
            SoundData = 0x6
        }

        private byte[] memory;

        public readonly static int TOTAL_MEMORY = (int)Math.Pow(2, 20) * 16;

        public Memory()
        {
            memory = new byte[TOTAL_MEMORY]; // 16 MiB memory (1 MiB = 2^20 bytes)
        }

        public void WriteBytes(UInt32 startAddress, params byte[] bytes)
        {
            for(int i = 0; i < bytes.Length; ++i)
            {
                memory[startAddress + i] = bytes[i];
            }
        }

        public void WriteBytes(MemoryMap startAddress, params byte[] bytes)
        {
            WriteBytes((UInt32)startAddress, bytes);
        }

        public UInt16 Read2Bytes(UInt32 startAddress)
        {
            return (UInt16)((memory[startAddress + 0] << 8) | (memory[startAddress + 1]));
        }

        public UInt16 Read2Bytes(MemoryMap startAddress)
        {
            return Read2Bytes((UInt32)startAddress);
        }

        public UInt32 Read3Bytes(UInt32 startAddress)
        {
            return (UInt32)((memory[startAddress + 0] << 16) | (memory[startAddress + 1] << 8) | (memory[startAddress + 2]));
        }

        public UInt32 Read3Bytes(MemoryMap startAddress)
        {
            return Read3Bytes((UInt32)startAddress);
        }

        public byte this[UInt32 address]
        {
            get { return this.memory[address]; }
            set { this.memory[address] = value; }
        }

        public byte this[MemoryMap address]
        {
            get { return this[(UInt32)address]; }
            set { this[(UInt32)address] = value; }
        }

        public void LoadRom(byte[] romData)
        {
            for(UInt32 address = 0; address < TOTAL_MEMORY; ++address)
            {
                if(address < romData.Length)
                {
                    this[address] = romData[address];
                }
                else
                {
                    this[address] = 0x00;
                }
            }
        }
    }
}

using System;
using System.Collections.Generic;

namespace BytePusher
{

    public class Keyboard
    {

        // Keyboard layout
        // BytePusher's hex keyboard has the same layout as that of CHIP-8:
        // 1   2 	3 	C
        // 4   5 	6 	D
        // 7   8 	9 	E
        // A   0 	B   F

        // Reverse order because of Big Endian
        public enum Keys
        {
            Key8, Key9, KeyA, KeyB,
            KeyC, KeyD, KeyE, KeyF,

            Key0, Key1, Key2, Key3,
            Key4, Key5, Key6, Key7
        }

        public Dictionary<Keys, bool> KeyStates;

        public Keyboard()
        {
            this.KeyStates = new Dictionary<Keys, bool>();
            foreach (Keys key in Enum.GetValues(typeof(Keys)))
            {
                KeyStates[key] = false;
            }
        }

        public void SetKey(Keys key, bool value)
        {
            KeyStates[key] = value;
        }


        public UInt16 State
        {
            get
            {
                UInt16 state = 0x0;
                foreach(var item in KeyStates)
                {
                    Keys key = item.Key;
                    bool value = item.Value;
                    if (value)
                    {
                        // Toggle bits of X-th key, flags key as pressed
                        state |= (UInt16)Math.Pow(2, (UInt32)key);
                    }
                }
                return state;
            }
        }
    }
}

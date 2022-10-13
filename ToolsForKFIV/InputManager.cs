using NeatInput.Windows;
using NeatInput.Windows.Events;
using NeatInput.Windows.Processing.Keyboard.Enums;
using NeatInput.Windows.Processing.Mouse.Enums;

namespace ToolsForKFIV
{
    public class KeyboardDeviceHandler : IKeyboardEventReceiver
    {
        public bool[] keyboardStateCurrent = new bool[256];

        public void Receive(KeyboardEvent @e)
        {
            int keyIndex = (int)e.Key;
            if (keyIndex < 0 || keyIndex > 255)
                return;

            bool keyState = e.State == KeyStates.Down ? true : false;

            keyboardStateCurrent[keyIndex] = keyState;
        }
    }

    public static class InputManager
    {
        public static KeyboardDeviceHandler keyboardDevice = new KeyboardDeviceHandler();
        private static InputSource keyboardHandler = new InputSource(keyboardDevice);

        public static void Initialize()
        {
            keyboardHandler.Listen();
        }

        public static bool KeyIsDown(int key)
        {
            return keyboardDevice.keyboardStateCurrent[key];
        }
    }
}

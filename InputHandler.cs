using Microsoft.Xna.Framework.Input;

namespace lasthope.Input
{
    public class InputHandler
    {
        private KeyboardState _current, _previous;
        public void Update() { _previous = _current; _current = Keyboard.GetState(); }
        public bool IsPressed(Keys key) => _current.IsKeyDown(key);
        public bool IsNewKey(Keys key) => _current.IsKeyDown(key) && _previous.IsKeyUp(key);
    }
}
using Microsoft.Xna.Framework.Input;

namespace lasthope.Entities
{
    // Хранение состояния клавиатуры
    public class InputHandler
    {
        private KeyboardState _current, _previous;
        public void Update() { _previous = _current; _current = Keyboard.GetState(); }
        
        // Удерживается ли клавиша
        public bool IsPressed(Keys key) => _current.IsKeyDown(key);
        
        // Была ли клавиша нажата в этом фрейме (одиночное нажатие)
        public bool IsNewKey(Keys key) => _current.IsKeyDown(key) && _previous.IsKeyUp(key);
    }
}
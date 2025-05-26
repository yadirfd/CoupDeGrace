using Microsoft.Xna.Framework.Input;
using lasthope.Views;
using lasthope.Entities;

namespace lasthope.Controllers
{
    public class PauseController
    {
        private readonly PauseView    _view;
        private readonly InputHandler _input;

        public PauseController(PauseView view, InputHandler input)
        {
            _view  = view;
            _input = input;
        }

        // Пауза при нажатии на кнопку "Escape"
        public bool Update()
        {
            return _input.IsNewKey(Keys.Escape);
        }

        public void Draw() => _view.Draw();
    }
}

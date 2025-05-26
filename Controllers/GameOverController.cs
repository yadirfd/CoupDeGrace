using Microsoft.Xna.Framework.Input;
using lasthope.Views;
using lasthope.Entities;

namespace lasthope.Controllers
{
    public class GameOverController
    {
        private readonly GameOverView _view;
        private readonly InputHandler _input;
        private int _winnerIndex;

        public GameOverController(GameOverView view, InputHandler input)
        {
            _view = view;
            _input = input;
        }
        public void SetWinner(int winnerIndex)
        {
            _winnerIndex = winnerIndex;
        }

        // Возвращает true, когда игрок нажал Escape для выхода
        public bool Update()
        {
            return _input.IsNewKey(Keys.Escape);
        }

        public void Draw()
        {
            _view.Draw(_winnerIndex);
        }
    }
}
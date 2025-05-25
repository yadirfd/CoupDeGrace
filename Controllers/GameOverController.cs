using Microsoft.Xna.Framework.Input;
using lasthope.Views;
using lasthope.Entities;

namespace lasthope.Controllers
{
    public class GameOverController
    {
        private readonly GameOverView _view;
        private readonly InputHandler _input;
        private string _winnerText;

        public GameOverController(GameOverView view, InputHandler input)
        {
            _view = view;
            _input = input;
        }
        public void SetWinner(string winner)
        {
            _winnerText = winner + " Won!";
        }

        // Возвращает true, когда игрок нажал Escape для выхода
        public bool Update()
        {
            return _input.IsNewKey(Keys.Escape);
        }

        public void Draw()
        {
            _view.Draw(_winnerText);
        }
    }
}
using Microsoft.Xna.Framework.Input;
using lasthope.Views;
using lasthope.Entities;

namespace lasthope.Controllers
{
    public class GameOverController(GameOverView view, InputHandler input)
    {
        private string _winnerText;

        public void SetWinner(string winner)
        {
            _winnerText = winner + " Won!";
        }

        // Возвращает true, когда игрок нажал Escape для выхода
        public bool Update()
        {
            input.Update();
            return input.IsNewKey(Keys.Escape);
        }

        public void Draw()
        {
            view.Draw(_winnerText);
        }
    }
}
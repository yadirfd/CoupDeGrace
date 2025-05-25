using Microsoft.Xna.Framework;
using lasthope.Entities;
using lasthope.Views;

namespace lasthope.Controllers
{
    public class PlayController
    {
        private readonly GameView _view;
        private readonly Character _p1;
        private readonly Character _p2;
        private readonly BotAI _bot;
        
        public PlayController(GameView view, Character p1, Character p2, BotAI bot)
        {
            _view = view;
            _p1 = p1;
            _p2 = p2;
            _bot = bot;
        }
        // Обновляет персонажей и бот, возвращает имя победителя или null
        public string Update(GameTime gt)
        {
            _p1.Update(gt);
            _p2.Update(gt);
            _bot.Update(gt);

            _p1.TryDealDamage(_p2, 10);
            _p2.TryDealDamage(_p1, 10);

            if (!_p1.IsAlive)
                return "Player 2";
            if (!_p2.IsAlive)
                return "Player 1";
            return null;
        }

        public void Draw()
        {
            _view.Draw(_p1, _p2);
        }
    }
}
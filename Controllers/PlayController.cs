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
        private readonly BotAi _bot;
        private bool _isPvC;
        
        public PlayController(GameView view, Character p1, Character p2, BotAi bot)
        {
            _view = view;
            _p1 = p1;
            _p2 = p2;
            _bot = bot;
        }
        
        public void SetBotEnabled(bool enabled)
        {
            _bot.Enabled = enabled;
            _isPvC = enabled;
        }
        
        // Обновляет персонажей и бота, возвращает индекс победителя или null
        public int? Update(GameTime gt)
        {
            _p1.Update(gt);
            _p2.Update(gt);
            _bot.Update(gt);

            _p1.TryDealDamage(_p2, 10);
            _p2.TryDealDamage(_p1, 10);

            if (!_p1.IsAlive)
                return 1;
            if (!_p2.IsAlive)
                return 0;
            return null;
        }

        public void Draw()
        {
            _view.Draw(_p1, _p2, _isPvC);
        }
    }
}
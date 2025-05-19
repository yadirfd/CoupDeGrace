using Microsoft.Xna.Framework;
using lasthope.Entities;
using lasthope.Views;

namespace lasthope.Controllers
{
    public class PlayController(GameView view, Character p1, Character p2, BotAI bot)
    {
        // Обновляет персонажей и бот, возвращает имя победителя или null
        public string Update(GameTime gt)
        {
            p1.Update(gt);
            p2.Update(gt);
            bot.Update(gt);

            p1.TryDealDamage(p2, 10);
            p2.TryDealDamage(p1, 10);

            if (!p1.IsAlive)
                return "Player 2";
            if (!p2.IsAlive)
                return "Player 1";
            return null;
        }

        public void Draw()
        {
            view.Draw(p1, p2);
        }
    }
}
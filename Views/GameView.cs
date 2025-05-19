using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using lasthope.Entities;

namespace lasthope.Views
{
    // Отрисовка игрового мира: фон, персонажи, healthbar
    public class GameView(SpriteBatch sb, Texture2D bg)
    {
        public void Draw(Character p1, Character p2)
        {
            sb.Draw(bg, new Rectangle(0, 0, 800, 600), Color.White);
            p1.Draw(sb);
            p2.Draw(sb);
            p1.DrawHealthBar(sb);
            p2.DrawHealthBar(sb);
        }
    }
}
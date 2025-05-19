using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace lasthope.Views
{
    // Отображение текста победы и инструкции
    public class GameOverView(SpriteBatch sb, SpriteFont font)
    {
        public void Draw(string winnerText)
        {
            var center = new Vector2(400f, 300f);
            var size = font.MeasureString(winnerText);
            sb.DrawString(font, winnerText, center - size / 2, Color.Red);
            const string prompt = "Press Enter or Esc to exit";
            var promptSize = font.MeasureString(prompt);
            sb.DrawString(font, prompt, center + new Vector2(-promptSize.X / 2, size.Y), Color.White);
        }
    }
}
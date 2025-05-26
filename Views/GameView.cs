using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using lasthope.Entities;

namespace lasthope.Views
{
    // Отрисовка игрового мира: фон, персонажи, healthbar
    public class GameView
    {
        private readonly SpriteBatch _sb;
        private readonly Texture2D _bg;
        private readonly SpriteFont _font;
        
        private const float HealthBarWidth = 150f;
        private const float LabelScale = 0.75f;
        
        public GameView(SpriteBatch sb, Texture2D bg, SpriteFont font)
        {
            _sb = sb;
            _bg = bg;
            _font = font;
        }
        public void Draw(Character p1, Character p2, bool isPvC)
        {
            _sb.Draw(_bg, new Rectangle(0, 0, 800, 600), Color.White);
            p1.Draw(_sb);
            p2.Draw(_sb);
            p1.DrawHealthBar(_sb);
            p2.DrawHealthBar(_sb);

            DrawLabel(p1.HealthBarPosition, "Player 1", true);
            DrawLabel(p2.HealthBarPosition, isPvC ? "Computer" : "Player 2", false);
        }

        private void DrawLabel(Vector2 hbPos, string text, bool leftAligned)
        {
            var measured = _font.MeasureString(text);
            var size = measured * LabelScale;
            
            var posX = leftAligned ? hbPos.X : hbPos.X + HealthBarWidth - size.X;
            var posY = hbPos.Y - size.Y - 4;

            _sb.DrawString(_font, text, new Vector2(posX, posY), Color.Black, 0f, Vector2.Zero, LabelScale, SpriteEffects.None, 0f); 
        }
    }
}
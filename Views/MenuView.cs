using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace lasthope.Views
{
    // Отрисовка
    public class MenuView(SpriteBatch sb, SpriteFont font, List<string> items)
    {
        public void Draw(int selectedIndex)
        {
            var center = new Vector2(400f, 300f);
            for (var i = 0; i < items.Count; i++)
            {
                var text = items[i];
                var size = font.MeasureString(text);
                var pos = center + new Vector2(-size.X / 2, (i - items.Count / 2) * 40);
                var color = i == selectedIndex ? Color.Yellow : Color.White;
                sb.DrawString(font, text, pos, color);
            }
        }
    }
}
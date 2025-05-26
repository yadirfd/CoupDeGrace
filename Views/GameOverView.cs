using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace lasthope.Views
{
    public class GameOverView
    {
        private readonly SpriteBatch _sb;
        private readonly Texture2D _win1;
        private readonly Texture2D _win2;

        public GameOverView(SpriteBatch sb, Texture2D win1, Texture2D win2)
        {
            _sb   = sb;
            _win1 = win1;
            _win2 = win2;
        }
        
        // Отрисовка экрана окончания игры в зависимости от победителя
        public void Draw(int winnerIndex)
        {
            var image = winnerIndex == 0 ? _win1 : _win2;

            _sb.Draw(image, _sb.GraphicsDevice.Viewport.Bounds, Color.White);
        }
    }
}
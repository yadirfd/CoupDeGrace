    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    namespace lasthope.Views
    {
        // Отрисовка компонентов меню
        public class MenuView
        {
            private readonly SpriteBatch _sb;
            private readonly Texture2D _pvpImage;
            private readonly Texture2D _pvcImage;
            private readonly MuteButtonView _muteIcon;

            public MenuView(SpriteBatch sb, Texture2D pvpImage, Texture2D pvcImage, MuteButtonView muteIcon)
            {
                _sb = sb;
                _pvpImage = pvpImage;
                _pvcImage = pvcImage;
                _muteIcon = muteIcon;
            }

            // Определение режима игры и иконки музыки в главном меню
            public void Draw(int selectedIndex, bool isMuted)
            {
                var image = selectedIndex == 0 ? _pvpImage : _pvcImage;
                
                _sb.Draw(image, destinationRectangle: _sb.GraphicsDevice.Viewport.Bounds, color: Color.White);
                
                _muteIcon.Draw(isMuted);
            }
        }
    }
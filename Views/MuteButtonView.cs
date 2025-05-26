using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace lasthope.Views
{
    // Иконка мута
    public class MuteButtonView
    {
        private readonly SpriteBatch _sb;
        private readonly Texture2D _iconOn;
        private readonly Texture2D _iconOff;
        private readonly Vector2 _position;

        public MuteButtonView(SpriteBatch sb, Texture2D iconOn, Texture2D iconOff, Vector2 position)
        {
            _sb = sb;
            _iconOn = iconOn;
            _iconOff = iconOff;
            _position = position;
        }

        public void Draw(bool isMuted)
        {
            var icon = isMuted ? _iconOff : _iconOn;
            _sb.Draw(icon, _position, Color.White);
        }
    }
}
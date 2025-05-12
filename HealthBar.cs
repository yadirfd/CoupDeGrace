using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace lasthope.Entities
{
    public class HealthBar(Texture2D texture, Vector2 position, int maxHealth)
    {
        private Texture2D _texture = texture;
        private Vector2 _position = position;
        private int _maxHealth = maxHealth;
        public int CurrentHealth { get; private set; } = maxHealth;

        public void TakeDamage(int amount)
        {
            CurrentHealth = MathHelper.Clamp(CurrentHealth - amount, 0, _maxHealth);
        }

        public void Draw(SpriteBatch sb)
        {
            var bg = new Rectangle((int)_position.X, (int)_position.Y, _texture.Width, _texture.Height);

            sb.Draw(_texture, new Rectangle(bg.X - 1, bg.Y - 1, bg.Width + 2, bg.Height + 2), Color.Black);
            sb.Draw(_texture, bg, Color.Gray);

            float ratio = (float)CurrentHealth / _maxHealth;
            var fg = new Rectangle(bg.X, bg.Y, (int)(bg.Width * ratio), bg.Height);
            
            sb.Draw(_texture, fg, Color.Red);
        }
    }
}
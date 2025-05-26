    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    namespace lasthope.Entities
    {
        // Логика ХП-бара персонажа
        public class HealthBar
        {
            private Texture2D _texture;
            private Vector2 _position;
            private int _maxHealth;
            
            public Vector2 Position => _position;
            public int CurrentHealth { get; private set; }

            public HealthBar(Texture2D texture, Vector2 position, int maxHealth)
            {
                _texture = texture;
                _position = position;
                _maxHealth = maxHealth;
                CurrentHealth = maxHealth;
            }
            
            // Получение урона (вычитаем здоровье)
            public void TakeDamage(int amount)
            {
                CurrentHealth = MathHelper.Clamp(CurrentHealth - amount, 0, _maxHealth);
            }

            // Отрисовка ХП-бара
            public void Draw(SpriteBatch sb)
            {
                var background = new Rectangle((int)_position.X, (int)_position.Y, _texture.Width, _texture.Height);

                sb.Draw(_texture, new Rectangle(background.X - 1, background.Y - 1, background.Width + 2, background.Height + 2), Color.Black);
                sb.Draw(_texture, background, Color.Gray);

                // Считаем здоровье и отрисовываем красный прямоугольник, как убавление здоровья
                var ratio = (float)CurrentHealth / _maxHealth;
                var fg = new Rectangle(background.X, background.Y, (int)(background.Width * ratio), background.Height);
                
                sb.Draw(_texture, fg, Color.Red);
            }
        }
    }
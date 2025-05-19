using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace lasthope.Entities
{
    // Проигрывание анимаций
    public class AnimationManager(Animation animation)
    {
        private Animation _animation = animation;
        private float _timer;
        private int _currentFrame;
        private bool _flip;

        public void Play(Animation anim, bool flip = false)
        {
            if (_animation == anim) return;
            _animation = anim;
            _currentFrame = 0;
            _timer = 0;
            _flip = flip;
        }
        public void Update(GameTime gt)
        {
            _timer += (float)gt.ElapsedGameTime.TotalSeconds;

            if (_timer > _animation.FrameTime)
            {
                _timer -= _animation.FrameTime;
                _currentFrame++;

                if (_currentFrame >= _animation.Frames.Count)
                {
                    if (_animation.IsLooping) 
                        _currentFrame = 0;
                    else 
                        _currentFrame--;
                }
            }
        }
        public void Draw(SpriteBatch sb, Texture2D tex, Rectangle dest)
        {
            var src = _animation.Frames[_currentFrame];

            var scale = dest.Height / (float)src.Height;

            Vector2 position = new(dest.X, dest.Y);

            var effects = _flip ? SpriteEffects.FlipHorizontally : SpriteEffects.None;

            sb.Draw(tex, position, src, Color.White, 0f, Vector2.Zero, scale, effects, 0f);
        }
    }
}
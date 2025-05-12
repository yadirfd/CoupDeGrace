// Character.cs
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using lasthope.Input;

namespace lasthope.Entities
{
    public class Character
    {
        private const int MapWidth = 800;
        private const int AttackRange = 50;

        private Vector2 _velocity;
        private bool _facingRight = true;

        // Флаги и таймеры
        private bool _isAttacking;
        private float _attackTimer;
        private bool _damageDealt;      
        private bool _isBlocking;
        private int _blockDuration;

        private Rectangle _attackBox;
        private int _attackCooldown;

        private Keys _left, _right, _jump, _attack, _block;
        private InputHandler _input;
        private HealthBar _hb;
        private Dictionary<string, Animation> _anims;
        private AnimationManager _am;
        private Dictionary<string, Texture2D> _textures;

        public Rectangle Bounds;
        public bool IsAlive => _hb.CurrentHealth > 0;
        public Rectangle AttackBox => _isAttacking ? _attackBox : Rectangle.Empty;

        public Character
        (
            InputHandler input,
            Rectangle spawn,
            Keys left, Keys right, Keys jump,
            Keys attack, Keys block,
            Dictionary<string, Texture2D> textures,
            Dictionary<string, Animation> anims,
            HealthBar hb
        )

        {
            _input = input;
            Bounds = spawn;

            _left    = left;
            _right   = right;
            _jump    = jump;
            _attack  = attack;
            _block   = block;

            _textures = textures;
            _anims    = anims;
            _hb       = hb;

            _am = new AnimationManager(_anims["Idle"]);
        }

        public void SetFacing(bool right) => _facingRight = right;

        public void TriggerAttack()
        {
            _attackCooldown = 60;

            string atkKey = _facingRight ? "Attack" : "AttackRev";
            var anim = _anims[atkKey];
            _attackTimer = anim.Frames.Count * anim.FrameTime;

            _isAttacking = true;
            _damageDealt = false;   

            int dir = _facingRight ? 1 : -1;
            int x = dir > 0 ? Bounds.Right : Bounds.Left - AttackRange;
            _attackBox = new Rectangle(x, Bounds.Y, AttackRange, Bounds.Height);
        }

        public void TriggerBlock()
        {
            _isBlocking = true;
            _blockDuration = 30;
        }

        public void Update(GameTime gt)
        {
            float dt = (float)gt.ElapsedGameTime.TotalSeconds;

            // Таймер атаки
            if (_attackTimer > 0f)
            {
                _attackTimer -= dt;
                if (_attackTimer <= 0f)
                    _isAttacking = false;
            }

            // Блок
            if (_blockDuration > 0)
                _blockDuration--;
            else
                _isBlocking = false;

            // Движение
            if (_input.IsPressed(_left))
            {
                Bounds.X -= (int)(200 * dt);
                _facingRight = false;
            }

            else if (_input.IsPressed(_right))
            {
                Bounds.X += (int)(200 * dt);
                _facingRight = true;
            }

            // Гравитация
            int ground = 500 - Bounds.Height;
            _velocity.Y += 800 * dt;
            Bounds.Y += (int)(_velocity.Y * dt);
            if (Bounds.Y >= ground)
            {
                Bounds.Y = ground;
                _velocity.Y = 0;
                if (_input.IsNewKey(_jump))
                    _velocity.Y = -400;
            }

            // Атака (cooldown)
            if (_attackCooldown > 0)
                _attackCooldown--;

            if (_input.IsNewKey(_attack) && _attackCooldown == 0)
                TriggerAttack();

            // Блок 
            if (_input.IsNewKey(_block))
                TriggerBlock();

            Bounds.X = MathHelper.Clamp(Bounds.X, 0, MapWidth - Bounds.Width);

            // Анимация
            string key = _isAttacking ? (_facingRight ? "Attack" : "AttackRev") : _isBlocking ? (_facingRight ? "Block" : "BlockRev") : (_facingRight ? "Idle" : "IdleRev");

            _am.Play(_anims[key], false);
            _am.Update(gt);
        }

        public void Draw(SpriteBatch sb)
        {
            string key = _isAttacking ? (_facingRight ? "Attack" : "AttackRev") : _isBlocking ? (_facingRight ? "Block" : "BlockRev") : (_facingRight ? "Idle" : "IdleRev");
            var tex = _textures[key];
            _am.Draw(sb, tex, Bounds);
        }

        public void ReceiveDamage(int dmg)
        {
            if (!_isBlocking)
                _hb.TakeDamage(dmg);
        }


        public void TryDealDamage(Character target, int dmg)
        {
            if (_isAttacking && !_damageDealt && AttackBox.Intersects(target.Bounds))
            {
                target.ReceiveDamage(dmg);
                _damageDealt = true;
            }
        }

        public void DrawHealthBar(SpriteBatch sb) => _hb.Draw(sb);
    }
}

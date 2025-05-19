using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

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

        private readonly Keys _left, _right, _jump, _attack, _block;
        private readonly InputHandler _input;
        private readonly HealthBar _hb;
        private readonly Dictionary<string, Animation> _anims;
        private readonly AnimationManager _am;
        private readonly Dictionary<string, Texture2D> _textures;

        public Rectangle Bounds;
        public bool IsAlive => _hb.CurrentHealth > 0;
        private Rectangle AttackBox => _isAttacking ? _attackBox : Rectangle.Empty;

        public Character
        (
            InputHandler input,
            Rectangle spawn,
            Keys left, Keys right, Keys jump, Keys attack, Keys block,
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

            var atkKey = _facingRight ? "Attack" : "AttackRev";
            var anim = _anims[atkKey];
            _attackTimer = anim.Frames.Count * anim.FrameTime;

            _isAttacking = true;
            _damageDealt = false;   

            // Если игрок смотрит вправо, начинаем с правой границы персонажа, если влево — вычитаем длину атаки из левой границы
            var direction = _facingRight ? 1 : -1;
            var x = direction > 0 ? Bounds.Right : Bounds.Left - AttackRange;
            
            // Хитбокс атаки
            _attackBox = new Rectangle(x, Bounds.Y, AttackRange, Bounds.Height);
        }

        public void TriggerBlock()
        {
            _isBlocking = true;
            _blockDuration = 30;
        }

        public void Update(GameTime gt)
        {
            var dt = (float)gt.ElapsedGameTime.TotalSeconds;

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
            const float baseSpeed = 200f;
            const float airMultiplier = 1.3f;   // ускорение в воздухе
            var speed = baseSpeed;
            
            if (_velocity.Y != 0)
                speed *= airMultiplier;

            if (_input.IsPressed(_left))
            {
                Bounds.X -= (int)(speed * dt);
                _facingRight = false;
            }
            else if (_input.IsPressed(_right))
            {
                Bounds.X += (int)(speed * dt);
                _facingRight = true;
            }
            
            // Границы, за которые не может выйти персонаж
            Bounds.X = MathHelper.Clamp(Bounds.X, 0, MapWidth - Bounds.Width);

            // Гравитация
            var ground = 500 - Bounds.Height;
            _velocity.Y += 800 * dt;
            Bounds.Y += (int)(_velocity.Y * dt);
            if (Bounds.Y >= ground)    
            {
                Bounds.Y = ground;
                _velocity.Y = 0;
                if (_input.IsNewKey(_jump))
                    _velocity.Y = -500;
            }

            // Атака (cooldown)
            if (_attackCooldown > 0)
                _attackCooldown--;

            if (_input.IsNewKey(_attack) && _attackCooldown == 0)
                TriggerAttack();

            // Блок (нажатие)
            if (_input.IsNewKey(_block))
                TriggerBlock();

            // Анимация
            var key = _isAttacking ? (_facingRight ? "Attack" : "AttackRev") : _isBlocking ? (_facingRight ? "Block" : "BlockRev") : (_facingRight ? "Idle" : "IdleRev");

            _am.Play(_anims[key]);
            _am.Update(gt);
        }

        public void Draw(SpriteBatch sb)
        {
            var key = _isAttacking ? (_facingRight ? "Attack" : "AttackRev") : _isBlocking ? (_facingRight ? "Block" : "BlockRev") : (_facingRight ? "Idle" : "IdleRev");
            var tex = _textures[key];
            _am.Draw(sb, tex, Bounds);
        }

        // Проверка на блок
        private void ReceiveDamage(int dmg)
        {
            if (!_isBlocking)
                _hb.TakeDamage(dmg);
        }

        
        //Проверка на получение урона
        public void TryDealDamage(Character target, int dmg)
        {
            if (_isAttacking && !_damageDealt && AttackBox.Intersects(target.Bounds))
            {
                target.ReceiveDamage(dmg);
                _damageDealt = true;
            }
        }

        // Отрисовка ХП-бара
        public void DrawHealthBar(SpriteBatch sb) => _hb.Draw(sb);
    }
}

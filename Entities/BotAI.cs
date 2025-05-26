using System;
using Microsoft.Xna.Framework;

namespace lasthope.Entities
{
    // Алгоритм бота
    public class BotAi
    {
        private readonly Character _bot;
        private readonly Character _target;
        private readonly Random _random = new();

        private int _attackCD, _blockCD;
        public bool Enabled = true;

        private enum State { Patrol, Approach, Attack, Retreat }
        private State _state;
        private float _stateTimer;

        // Скорости передвижения
        private const float WalkSpeed = 150f;
        private const float RunSpeed = 250f;
        
        // Дальность атаки
        private const float AttackRange = 60f;

        // Вероятности различных действий
        private const float PatrolChangeChance = 0.1f; // 10% шанс - просто ходить (патрулировать)
        private const float AttackSkill = 0.95f; // 95% шанс - атаковать игрока
        private const float BlockSkill = 0.85f; // 85% шанс - заблокировать удар игрока
        private const float RetreatChance = 0.4f; // 40% шанс - отступить при атаке игрока

        public BotAi(Character bot, Character target)
        {
            _bot = bot;
            _target = target;
            EnterState(State.Patrol);
        }

        public void Update(GameTime gt)
        {
            if (!Enabled) return;
            var dt = (float)gt.ElapsedGameTime.TotalSeconds;
            var distance = Math.Abs(_target.Bounds.Center.X - _bot.Bounds.Center.X);

            _stateTimer -= dt;
            if (_stateTimer <= 0)
                TransitionNext(distance);

            switch (_state)
            {
                case State.Patrol:
                    Patrol(dt);
                    break;
                case State.Approach:
                    Approach(dt, distance);
                    break;
                case State.Attack:
                    TryAttack(distance);
                    break;
                case State.Retreat:
                    Retreat(dt);
                    break;
            }

            _bot.SetFacing(_target.Bounds.Center.X > _bot.Bounds.Center.X);

            if (_attackCD > 0) _attackCD--;
            if (_blockCD > 0)  _blockCD--;

            // Умное блокирование
            if (_target.IsAttacking && distance < AttackRange * 1.2f && _blockCD == 0 && _random.NextDouble() < BlockSkill)
            {
                _bot.TriggerBlock();
                _blockCD = 50;
            }
        }

        private void TransitionNext(float dist)
        {
            switch (_state)
            {
                case State.Patrol:
                    _state = State.Approach;
                    break;
                case State.Approach:
                    _state = State.Attack;
                    break;
                case State.Attack:
                    if (_random.NextDouble() < RetreatChance)
                        _state = State.Retreat;
                    else
                        _state = State.Approach;
                    break;
                case State.Retreat:
                    _state = State.Approach;
                    break;
            }
            EnterState(_state);
        }

        private void EnterState(State newState)
        {
            _state = newState;
            switch (newState)
            {
                case State.Patrol:
                    _stateTimer = 1f + (float)_random.NextDouble() * 1f;
                    break;
                case State.Approach:
                    _stateTimer = 0.7f + (float)_random.NextDouble() * 0.5f;
                    break;
                case State.Attack:
                    _stateTimer = 1f;
                    break;
                case State.Retreat:
                    _stateTimer = 0.3f + (float)_random.NextDouble() * 0.3f;
                    break;
            }
        }

        private void Patrol(float dt)
        {
            if (_random.NextDouble() < PatrolChangeChance)
                EnterState(State.Approach);
            var direction = _random.NextDouble() < 0.5 ? 1 : -1;
            _bot.Bounds.X += (int)(WalkSpeed * direction * dt);
        }

        private void Approach(float dt, float distance)
        {
            if (distance > AttackRange)
                MoveTowards(dt, RunSpeed);
        }

        private void TryAttack(float distance)
        {
            if (_attackCD == 0 && distance <= AttackRange && _random.NextDouble() < AttackSkill)
            {
                _bot.TriggerAttack();
                _attackCD = 50;
            }
        }

        private void Retreat(float dt)
        {
            var direction = _bot.Bounds.Center.X >= _target.Bounds.Center.X ? 1 : -1;
            _bot.Bounds.X += (int)(WalkSpeed * direction * dt);
        }

        private void MoveTowards(float dt, float speed)
        {
            var direction = _target.Bounds.Center.X > _bot.Bounds.Center.X ? 1 : -1;
            _bot.Bounds.X += (int)(speed * direction * dt);
        }
    }
}

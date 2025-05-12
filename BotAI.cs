using System;
using Microsoft.Xna.Framework;

namespace lasthope.Entities
{
    public class BotAI(Character bot, Character target)
    {
        private readonly Character _bot = bot;
        private readonly Character _target = target;
        private readonly Random _random = new();
        private int _attackCD, _blockCD;
        public bool Enabled = true;

        public void Update(GameTime gt)
        {
            if (!Enabled)
                return;

            float dt = (float)gt.ElapsedGameTime.TotalSeconds;
            int attackRange = 70, blockRange = 60;

            if (_target.Bounds.Center.X < _bot.Bounds.Center.X - attackRange)
                _bot.Bounds.X -= (int)(200 * dt);

            else if (_target.Bounds.Center.X > _bot.Bounds.Center.X + attackRange)
                _bot.Bounds.X += (int)(200 * dt);

            _bot.SetFacing(_target.Bounds.Center.X > _bot.Bounds.Center.X);

            if(_attackCD > 0)
                _attackCD--;

            if(Math.Abs(_bot.Bounds.Center.X - _target.Bounds.Center.X) <= attackRange && _attackCD == 0 )
            { 
                _bot.TriggerAttack(); 
                _attackCD = 100; 
            }

            if (_blockCD > 0)
                _blockCD--;

            if(_random.NextDouble() < 0.05 && Math.Abs(_bot.Bounds.Center.X-_target.Bounds.Center.X) <= blockRange && _blockCD == 0 )
            { 
                _bot.TriggerBlock(); 
                _blockCD = 60; 
            }
        }
    }
}
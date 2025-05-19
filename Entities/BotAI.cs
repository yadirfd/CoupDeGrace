using System;
using Microsoft.Xna.Framework;

namespace lasthope.Entities
{
    // Простая логика для бота
    public class BotAI(Character bot, Character target)
    {
        private readonly Random _random = new();
        private int _attackCD, _blockCD;
        public bool Enabled = true;

        public void Update(GameTime gt)
        {
            if (!Enabled)
                return;

            var dt = (float)gt.ElapsedGameTime.TotalSeconds;
            const int attackRange = 70;
            const int blockRange = 60;

            if (target.Bounds.Center.X < bot.Bounds.Center.X - attackRange)
                bot.Bounds.X -= (int)(200 * dt);

            else if (target.Bounds.Center.X > bot.Bounds.Center.X + attackRange)
                bot.Bounds.X += (int)(200 * dt);

            bot.SetFacing(target.Bounds.Center.X > bot.Bounds.Center.X);

            if(_attackCD > 0)
                _attackCD--;

            if(Math.Abs(bot.Bounds.Center.X - target.Bounds.Center.X) <= attackRange && _attackCD == 0 )
            { 
                bot.TriggerAttack(); 
                _attackCD = 100; 
            }

            if (_blockCD > 0)
                _blockCD--;

            if(_random.NextDouble() < 0.05 && Math.Abs(bot.Bounds.Center.X-target.Bounds.Center.X) <= blockRange && _blockCD == 0 )
            { 
                bot.TriggerBlock(); 
                _blockCD = 60; 
            }
        }
    }
}
using System;
using Microsoft.Xna.Framework;

namespace lasthope.Entities
{
    public class BotAI
    {
        private Character _bot, _target;
        private Random _rnd = new Random();
        private int _atkCD, _blkCD;
        public bool Enabled=true;
        public BotAI(Character bot, Character target){_bot=bot;_target=target;}
        
        public void Update(GameTime gt)
        {
            if (!Enabled)
                return;

            float dt = (float)gt.ElapsedGameTime.TotalSeconds;
            int ar = 50, br = 60;

            if (_target.Bounds.Center.X < _bot.Bounds.Center.X-ar)
                _bot.Bounds.X -= (int)(200*dt);

            else if (_target.Bounds.Center.X > _bot.Bounds.Center.X+ar)
                _bot.Bounds.X += (int)(200*dt);

            _bot.SetFacing(_target.Bounds.Center.X>_bot.Bounds.Center.X);

            if(_atkCD > 0)
                _atkCD--;

            if(Math.Abs(_bot.Bounds.Center.X-_target.Bounds.Center.X) <= ar && _atkCD == 0 )
            { 
                _bot.TriggerAttack(); 
                _atkCD = 60; 
            }

            if (_blkCD > 0)
                _blkCD--;

            if(_rnd.NextDouble() < 0.01 && Math.Abs(_bot.Bounds.Center.X-_target.Bounds.Center.X) <= br && _blkCD == 0 )
            { 
                _bot.TriggerBlock(); 
                _blkCD = 100; 
            }
        }
    }
}
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using lasthope.Input;
using lasthope.Entities;

namespace lasthope
{
    public class CoupDeGrace : Game
    {
        private readonly GraphicsDeviceManager _gdm;
        private SpriteBatch _sb;
        private InputHandler _input;
        private Character _p1, _p2;
        private BotAI _bot;
        private Texture2D _bg;
        private Texture2D _hbTex;

        public CoupDeGrace()
        {
            _gdm = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            _gdm.PreferredBackBufferWidth = 800;
            _gdm.PreferredBackBufferHeight = 600;
            _gdm.ApplyChanges();
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _sb = new SpriteBatch(GraphicsDevice);
            _input = new InputHandler();

            // Фон
            _bg = Content.Load<Texture2D>("backgrounds/background");

            // Текстура HealthBar
            _hbTex = new Texture2D(GraphicsDevice, 150, 15);
            _hbTex.SetData(Enumerable.Repeat(Color.White, 150 * 15).ToArray());

            // Игрок 1: спрайты и анимации
            var texs1 = new Dictionary<string, Texture2D>
            {
                {"Idle", Content.Load<Texture2D>("Characters/Player1")},
                {"IdleRev", Content.Load<Texture2D>("Characters/Player1Reversed")},
                {"Attack", Content.Load<Texture2D>("Characters/Player1DamageHands")},
                {"AttackRev", Content.Load<Texture2D>("Characters/Player1DamageHandsReversed")},
                {"Block", Content.Load<Texture2D>("Characters/Player1Blocking")},
                {"BlockRev", Content.Load<Texture2D>("Characters/Player1BlockingReversed")}
            };

            var anims1 = texs1.ToDictionary(kv => kv.Key, kv => new Animation( [new Rectangle(0, 0, kv.Value.Width, kv.Value.Height)], 0.8f, !kv.Key.StartsWith("Attack")));

            var hb1 = new HealthBar(_hbTex, new Vector2(40, 10), 100);

            _p1 = new Character(_input, new Rectangle(100, 450, 50, 100), Keys.A, Keys.D, Keys.W, Keys.Space, Keys.LeftShift, texs1, anims1, hb1);


            // Игрок 2: спрайты и анимации
            var texs2 = new Dictionary<string, Texture2D>
            {
                {"Idle", Content.Load<Texture2D>("Characters/Player2")},
                {"IdleRev", Content.Load<Texture2D>("Characters/Player2Reversed")},
                {"Attack", Content.Load<Texture2D>("Characters/Player2DamageHands")},
                {"AttackRev", Content.Load<Texture2D>("Characters/Player2DamageHandsReversed")},
                {"Block", Content.Load<Texture2D>("Characters/Player2Blocking")},
                {"BlockRev", Content.Load<Texture2D>("Characters/Player2BlockingReversed")}
            };

            var anims2 = texs2.ToDictionary(kv => kv.Key, kv => new Animation( [new Rectangle(0, 0, kv.Value.Width, kv.Value.Height)], 0.8f, !kv.Key.StartsWith("Attack")));

            var hb2 = new HealthBar(_hbTex, new Vector2(620, 10), 100);

            _p2 = new Character(_input, new Rectangle(600, 450, 50, 100), Keys.Left, Keys.Right, Keys.Up, Keys.RightControl, Keys.RightShift, texs2, anims2, hb2);


            // Запуск бота
            _bot = new BotAI(_p2, _p1);
        }

        protected override void Update(GameTime gt)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            _input.Update();

            //Включение/выключение бота на `
            if (_input.IsNewKey(Keys.OemTilde))
                _bot.Enabled = !_bot.Enabled;

            _p1.Update(gt);
            _p2.Update(gt);
            _bot.Update(gt);


            _p1.TryDealDamage(_p2, 10);
            _p2.TryDealDamage(_p1, 10);

            // Проверка на смерть
            if (!_p1.IsAlive || !_p2.IsAlive)
                Exit();

            base.Update(gt);
        }

        protected override void Draw(GameTime gt)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            _sb.Begin();
            _sb.Draw(_bg, new Rectangle(0, 0, 800, 600), Color.White);
            _p1.Draw(_sb);
            _p2.Draw(_sb);
            _p1.DrawHealthBar(_sb);
            _p2.DrawHealthBar(_sb);
            _sb.End();
            base.Draw(gt);
        }
    }
} 
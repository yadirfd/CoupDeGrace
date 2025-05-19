using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Linq;
using lasthope.Entities;
using lasthope.Models;

namespace lasthope
{
    public class CoupDeGrace : Game
    {
        private readonly GraphicsDeviceManager _gdm;
        private SpriteBatch _sb;
        private InputHandler _input;

        // Состояние игры
        private GameState _state = GameState.Menu;

        // Меню
        private SpriteFont _menuFont;
        private readonly List<string> _menuItems = ["Player vs Player", "Player vs Computer"];
        private int _selectedIndex;

        // Экран победы
        private string _winnerText;

        // Игровые сущности
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
            // Размеры окна
            _gdm.PreferredBackBufferWidth = 800;
            _gdm.PreferredBackBufferHeight = 600;
            _gdm.ApplyChanges();
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _sb = new SpriteBatch(GraphicsDevice);
            _input = new InputHandler();

            // Загрузка фона и полоски здоровья
            _bg = Content.Load<Texture2D>("backgrounds/background");
            _hbTex = new Texture2D(GraphicsDevice, 150, 15);
            _hbTex.SetData(Enumerable.Repeat(Color.White, 150 * 15).ToArray());

            // Шрифт для меню и победы
            _menuFont = Content.Load<SpriteFont>("MenuFont");

            // Инициализация персонажей и бота
            InitializeCharacters();
        }

        private void InitializeCharacters()
        {
            // Игрок 1
            var texs1 = LoadPlayerTextures("Player1");
            var anims1 = CreateAnimations(texs1);
            var hb1 = new HealthBar(_hbTex, new Vector2(40, 10), 100);
            _p1 = new Character(_input, new Rectangle(100, 450, 50, 100), Keys.A, Keys.D, Keys.W, Keys.Space, Keys.LeftShift, texs1, anims1, hb1);

            // Игрок 2
            var texs2 = LoadPlayerTextures("Player2");
            var anims2 = CreateAnimations(texs2);
            var hb2 = new HealthBar(_hbTex, new Vector2(620, 10), 100);
            _p2 = new Character(_input, new Rectangle(600, 450, 50, 100), Keys.Left, Keys.Right, Keys.Up, Keys.RightControl, Keys.RightShift, texs2, anims2, hb2);

            _bot = new BotAI(_p2, _p1);
        }

        private Dictionary<string, Texture2D> LoadPlayerTextures(string prefix)
        {
            return new Dictionary<string, Texture2D>
            {
                {"Idle", Content.Load<Texture2D>($"Characters/{prefix}")},
                {"IdleRev", Content.Load<Texture2D>($"Characters/{prefix}Reversed")},
                {"Attack", Content.Load<Texture2D>($"Characters/{prefix}DamageHands")},
                {"AttackRev", Content.Load<Texture2D>($"Characters/{prefix}DamageHandsReversed")},
                {"Block", Content.Load<Texture2D>($"Characters/{prefix}Blocking")},
                {"BlockRev", Content.Load<Texture2D>($"Characters/{prefix}BlockingReversed")}
            };
        }

        private Dictionary<string, Animation> CreateAnimations(Dictionary<string, Texture2D> texs)
        {
            return texs.ToDictionary(
                kv => kv.Key,
                kv => new Animation(new List<Rectangle> { new Rectangle(0, 0, kv.Value.Width, kv.Value.Height) }, 0.8f, !kv.Key.StartsWith("Attack"))
            );
        }

        protected override void Update(GameTime gt)
        {
            _input.Update();

            switch (_state)
            {
                case GameState.Menu:
                    UpdateMenu();
                    break;
                case GameState.Playing:
                    UpdateGame(gt);
                    break;
                case GameState.GameOver:
                    if (_input.IsNewKey(Keys.Escape) || _input.IsNewKey(Keys.Enter))
                        Exit();
                    break;
            }

            base.Update(gt);
        }

        private void UpdateMenu()
        {
            if (_input.IsNewKey(Keys.Up))
                _selectedIndex = (_selectedIndex - 1 + _menuItems.Count) % _menuItems.Count;
            if (_input.IsNewKey(Keys.Down))
                _selectedIndex = (_selectedIndex + 1) % _menuItems.Count;
            if (_input.IsNewKey(Keys.Enter))
            {
                _bot.Enabled = (_selectedIndex == 1);
                _state = GameState.Playing;
            }
        }

        private void UpdateGame(GameTime gt)
        {
            _p1.Update(gt);
            _p2.Update(gt);
            _bot.Update(gt);

            _p1.TryDealDamage(_p2, 10);
            _p2.TryDealDamage(_p1, 10);

            // Проверка победы
            if (!_p1.IsAlive)
            {
                _winnerText = "Player 2 Won!";
                _state = GameState.GameOver;
            }
            else if (!_p2.IsAlive)
            {
                _winnerText = "Player 1 Won!";
                _state = GameState.GameOver;
            }
        }

        protected override void Draw(GameTime gt)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            _sb.Begin();

            switch (_state)
            {
                case GameState.Menu:
                    DrawMenu();
                    break;
                
                case GameState.Playing:
                    _sb.Draw(_bg, new Rectangle(0, 0, 800, 600), Color.White);
                    _p1.Draw(_sb);
                    _p2.Draw(_sb);
                    _p1.DrawHealthBar(_sb);
                    _p2.DrawHealthBar(_sb);
                    break;
                
                case GameState.GameOver:
                    DrawGameOver();
                    break;
            }

            _sb.End();
            base.Draw(gt);
        }

        private void DrawMenu()
        {
            var center = new Vector2(_gdm.PreferredBackBufferWidth / 2f, _gdm.PreferredBackBufferHeight / 2f);
            for (var i = 0; i < _menuItems.Count; i++)
            {
                var text = _menuItems[i];
                var size = _menuFont.MeasureString(text);
                var pos = center + new Vector2(-size.X / 2, (i - _menuItems.Count / 2f) * 40);
                var color = i == _selectedIndex ? Color.Yellow : Color.White;
                _sb.DrawString(_menuFont, text, pos, color);
            }
        }

        private void DrawGameOver()
        {
            var center = new Vector2(_gdm.PreferredBackBufferWidth / 2f, _gdm.PreferredBackBufferHeight / 2f);
            var size = _menuFont.MeasureString(_winnerText);
            var pos = center - size / 2;
            _sb.DrawString(_menuFont, _winnerText, pos, Color.Red);

            const string prompt = "Press Esc to exit";
            var promptSize = _menuFont.MeasureString(prompt);
            var promptPosition = new Vector2(center.X - promptSize.X / 2, center.Y + size.Y);
            _sb.DrawString(_menuFont, prompt, promptPosition, Color.White);
        }
    }
}

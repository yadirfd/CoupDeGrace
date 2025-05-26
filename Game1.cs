using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Linq;
using lasthope.Entities;
using lasthope.Controllers;
using lasthope.Views;


namespace lasthope
{
    public class CoupDeGrace : Game
    {
        private readonly GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private InputHandler _input;
        
        private GameController _gameController;

        // Текстуры
        private Texture2D _background;
        private Texture2D _healthBarTexture;
        private SpriteFont _font;
        
        // Музыка
        private AudioManager  _audio;
        private MusicManager _music;
        private MuteButtonController _muteCtrl;
        
        
        // Персонажи и бот
        private Character _player1;
        private Character _player2;
        private BotAi _bot;

        public CoupDeGrace()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            // Размеры окна
            _graphics.PreferredBackBufferWidth = 800;
            _graphics.PreferredBackBufferHeight = 600;
            _graphics.ApplyChanges();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _input = new InputHandler();

            // Загрузка графики
            _background = Content.Load<Texture2D>("backgrounds/background_1");
            _healthBarTexture = new Texture2D(GraphicsDevice, 150, 15);
            _healthBarTexture.SetData(Enumerable.Repeat(Color.White, 150 * 15).ToArray());
            
            var whitePixel = new Texture2D(GraphicsDevice, 1, 1);
            whitePixel.SetData([Color.White]);
            
            var menuPvp = Content.Load<Texture2D>("Menu/menu_pvp");
            var menuPvc = Content.Load<Texture2D>("Menu/menu_pvc");
            
            var win1Bg = Content.Load<Texture2D>("GameOverScreen/win_1");
            var win2Bg = Content.Load<Texture2D>("GameOverScreen/win_2");
            
            _font = Content.Load<SpriteFont>("MenuFont");
            
            // Загрузка музыки
            _audio = new AudioManager();
            _muteCtrl = new MuteButtonController(_input, _audio);
            
            var iconOn  = Content.Load<Texture2D>("Icons/speaker_icon");
            var iconOff = Content.Load<Texture2D>("Icons/speaker_muted_icon");
            var mutePosition = new Vector2(10, _graphics.PreferredBackBufferHeight - 60);
            
            _music = new MusicManager();
            _music.Load(Content);

            // Инициализация персонажей и бота
            InitializeCharacters();
            
            // Выбор в меню
            var menuItems = new List<string> { "Player vs Player", "Player vs Computer" };

            // Инициализация views
            var muteView  = new MuteButtonView(_spriteBatch, iconOn, iconOff, mutePosition);
            var menuView = new MenuView(_spriteBatch, menuPvp, menuPvc, muteView);
            var gameView = new GameView(_spriteBatch, _background, _font);
            var gameOverView = new GameOverView(_spriteBatch, win1Bg, win2Bg);
            var pauseView = new PauseView(_spriteBatch, _font, GraphicsDevice.Viewport, whitePixel);    

            // Инициализация контроллеров
            var menuController = new MenuController(menuView, menuItems, _input);
            var playController = new PlayController(gameView, _player1, _player2, _bot);
            var gameOverController = new GameOverController(gameOverView, _input);
            var pauseController = new PauseController(pauseView, _input);

            _gameController = new GameController(_audio, _input, menuController, playController, pauseController, gameOverController);
        }

        private void InitializeCharacters()
        {
            var textures1 = LoadPlayerTextures("Player1");
            var anims1 = CreateAnimations(textures1);
            var hb1 = new HealthBar(_healthBarTexture, new Vector2(40, 40), 100);
            _player1 = new Character(_input, new Rectangle(100, 450, 50, 100), Keys.A, Keys.D, Keys.W, Keys.Space, Keys.LeftShift, textures1, anims1, hb1);

            var textures2 = LoadPlayerTextures("Player2");
            var anims2 = CreateAnimations(textures2);
            var hb2 = new HealthBar(_healthBarTexture, new Vector2(620, 40), 100);
            _player2 = new Character(_input, new Rectangle(600, 450, 50, 100), Keys.Left, Keys.Right, Keys.Up, Keys.RightControl, Keys.RightShift, textures2, anims2, hb2);

            _player2.SetFacing(false);
            
            _bot = new BotAi(_player2, _player1);
        }
        
        // Спрайты анимации
        private Dictionary<string, Texture2D> LoadPlayerTextures(string prefix)
        {
            return new Dictionary<string, Texture2D>
            {
                { "Idle", Content.Load<Texture2D>($"Characters/{prefix}") },
                { "IdleRev", Content.Load<Texture2D>($"Characters/{prefix}Reversed") },
                { "Attack", Content.Load<Texture2D>($"Characters/{prefix}DamageHands") },
                { "AttackRev", Content.Load<Texture2D>($"Characters/{prefix}DamageHandsReversed") },
                { "Block", Content.Load<Texture2D>($"Characters/{prefix}Blocking") },
                { "BlockRev", Content.Load<Texture2D>($"Characters/{prefix}BlockingReversed") }
            };
        }

        private Dictionary<string, Animation> CreateAnimations(Dictionary<string, Texture2D> textures)
        {
            return textures.ToDictionary(
                kvp => kvp.Key,
                kvp => new Animation(new List<Rectangle> { new Rectangle(0, 0, kvp.Value.Width, kvp.Value.Height) }, 0.8f, !kvp.Key.StartsWith("Attack"))
            );
        }

        protected override void Update(GameTime gameTime)
        {
            _input.Update();
            _muteCtrl.Update();
            _gameController.Update(gameTime);
            _music.Update(_gameController.CurrentState);
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            
            _spriteBatch.Begin();
            _gameController.Draw();
            _spriteBatch.End();
            
            base.Draw(gameTime);
        }
    }
}
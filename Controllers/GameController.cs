using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using lasthope.Entities;
using lasthope.Models;
using lasthope.Views;

namespace lasthope.Controllers
{
    public class GameController
    {
        private readonly InputHandler _input; 
        private readonly AudioManager _audio;
        private readonly MenuController _menuCtrl;
        private readonly PlayController _playCtrl;
        private readonly GameOverController _overCtrl;
        private readonly PauseController _pauseCtrl;
        private GameState _state = GameState.Menu;

        public GameController(AudioManager audio, InputHandler input, MenuController menuCtrl, PlayController playCtrl, PauseController pauseCtrl, GameOverController overCtrl)
        {
            _input = input;
            _audio = audio;
            _menuCtrl = menuCtrl;
            _playCtrl = playCtrl;
            _overCtrl = overCtrl;
            _pauseCtrl = pauseCtrl;
        }
        
        public GameState CurrentState => _state;

        // Переключение состояний игры
        public void Update(GameTime gt)
        {
            switch (_state)
            {
                case GameState.Menu:
                    if (_menuCtrl.Update())
                    {
                        _playCtrl.SetBotEnabled(_menuCtrl.IsPvC);
                        _state = GameState.Playing;
                    }
                    break;
                
                case GameState.Playing:
                    if (_input.IsNewKey(Keys.Escape)) 
                        _state = GameState.Paused;

                    else
                    {
                        var winnerIndex = _playCtrl.Update(gt);
                        if (winnerIndex.HasValue)
                        {
                            _overCtrl.SetWinner(winnerIndex.Value);
                            _state = GameState.GameOver;
                        }
                    }
                    break;
                
                case GameState.Paused:
                    if (_pauseCtrl.Update())
                        _state = GameState.Playing;
                    break;
                
                case GameState.GameOver:
                    if (_overCtrl.Update())
                        ExitGame();
                    break;
            }
        }

        public void Draw()
        {
            switch (_state)
            {
                case GameState.Menu:      _menuCtrl.Draw(_audio.IsMuted); break;
                case GameState.Playing:   _playCtrl.Draw(); break;
                case GameState.Paused:    _playCtrl.Draw(); _pauseCtrl.Draw(); break;
                case GameState.GameOver:  _overCtrl.Draw(); break;
            }
        }

        private static void ExitGame() => System.Environment.Exit(0);
    }
}
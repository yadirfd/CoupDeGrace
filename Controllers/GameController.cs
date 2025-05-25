using System;
using Microsoft.Xna.Framework;
using lasthope.Models;

namespace lasthope.Controllers
{
    public class GameController
    {
        private readonly MenuController _menuCtrl;
        private readonly PlayController _playCtrl;
        private readonly GameOverController _overCtrl;
        private GameState _state = GameState.Menu;

        public GameController(MenuController menuCtrl, PlayController playCtrl, GameOverController overCtrl)
        {
            _menuCtrl = menuCtrl;
            _playCtrl = playCtrl;
            _overCtrl = overCtrl;
        }

        // Переключение состояний игры
        public void Update(GameTime gt)
        {
            switch (_state)
            {
                case GameState.Menu:
                    if (_menuCtrl.Update())
                        _state = GameState.Playing;
                    break;
                
                case GameState.Playing:
                    var winner = _playCtrl.Update(gt);
                    if (winner != null)
                    {
                        _overCtrl.SetWinner(winner);
                        _state = GameState.GameOver;
                    }
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
                case GameState.Menu:      _menuCtrl.Draw(); break;
                case GameState.Playing:   _playCtrl.Draw(); break;
                case GameState.GameOver:  _overCtrl.Draw(); break;
            }
        }

        private static void ExitGame() => System.Environment.Exit(0);
    }
}
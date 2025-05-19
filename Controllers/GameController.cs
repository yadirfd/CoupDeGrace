using System;
using Microsoft.Xna.Framework;
using lasthope.Models;

namespace lasthope.Controllers
{
    public class GameController(MenuController menuCtrl, PlayController playCtrl, GameOverController overCtrl)
    {
        private GameState _state = GameState.Menu;


        // Переключение состояний игры
        public void Update(GameTime gt)
        {
            switch (_state)
            {
                case GameState.Menu:
                    if (menuCtrl.Update())
                        _state = GameState.Playing;
                    break;
                
                case GameState.Playing:
                    var winner = playCtrl.Update(gt);
                    if (winner != null)
                    {
                        overCtrl.SetWinner(winner);
                        _state = GameState.GameOver;
                    }
                    break;
                
                case GameState.GameOver:
                    if (overCtrl.Update())
                        ExitGame();
                    break;
            }
        }

        public void Draw()
        {
            switch (_state)
            {
                case GameState.Menu:      menuCtrl.Draw(); break;
                case GameState.Playing:   playCtrl.Draw(); break;
                case GameState.GameOver:  overCtrl.Draw(); break;
            }
        }

        private static void ExitGame() => System.Environment.Exit(0);
    }
}
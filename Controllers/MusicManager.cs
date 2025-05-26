using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Content;
using lasthope.Models;

namespace lasthope.Controllers;

public class MusicManager
{
    private Song _menu, _fight, _gameOver;
    private GameState _lastState;

    // Громкость музыки
    public float Volume
    {
        get => MediaPlayer.Volume;
        set => MediaPlayer.Volume = MathHelper.Clamp(value, 0f, 1f);
    }
    

    public void Load(ContentManager content)
    {
        _menu = content.Load<Song>("Songs/MenuMusic");
        _fight = content.Load<Song>("Songs/FightMusic");
        _gameOver = content.Load<Song>("Songs/GameOverMusic");

        MediaPlayer.IsRepeating = true;
        Volume = 0.5f;
        _lastState = GameState.Menu;
        MediaPlayer.Play(_menu);
    }


    // Проигрыш различной музыки в зависимости от экрана
    public void Update(GameState currentState)
    {
        if (currentState == _lastState)
            return;

        switch (currentState)
        {
            case GameState.Menu:
                MediaPlayer.Play(_menu);
                break;
            case GameState.Playing:
                if (_lastState == GameState.Paused)
                    MediaPlayer.Resume();
                else
                    MediaPlayer.Play(_fight);
                break;
            case GameState.Paused:
                MediaPlayer.Pause();
                break;
            case GameState.GameOver:
                MediaPlayer.Play(_gameOver);
                break;
        }

        _lastState = currentState;
    }
}
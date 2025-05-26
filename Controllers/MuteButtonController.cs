using Microsoft.Xna.Framework.Input;
using lasthope.Entities;

namespace lasthope.Controllers;

public class MuteButtonController
{
    private readonly InputHandler  _input;
    private readonly AudioManager  _audio;

    public MuteButtonController(InputHandler input, AudioManager audio)
    {
        _input = input;
        _audio = audio;
    }

    // Выключение музыки при нажатии на кнопку "М"
    public bool Update()
    {
        if (_input.IsNewKey(Keys.M))
        {
            _audio.ToggleMute();
            return true;
        }
        return false;
    }
}
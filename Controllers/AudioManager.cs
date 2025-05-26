using Microsoft.Xna.Framework.Media;

namespace lasthope.Controllers;

public class AudioManager
{
    // Возможность отключить музыку
    public bool IsMuted
    {
        get => MediaPlayer.IsMuted;
        set => MediaPlayer.IsMuted = value;
    }

    public void ToggleMute()
    {
        IsMuted = !IsMuted;
    }
}
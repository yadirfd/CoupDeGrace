using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace lasthope.Views;

public class PauseView
{
    private readonly SpriteBatch _sb;
    private readonly SpriteFont _font;
    private readonly Viewport _vp;
    private readonly Texture2D _pixel;

    public PauseView(SpriteBatch sb, SpriteFont font, Viewport vp, Texture2D whitePixel)
    {
        _sb = sb;
        _font = font;
        _vp = vp;
        _pixel = whitePixel;
    }

    // Постановка игры на паузу (Затемнение экрана + надпись "Game Paused")
    public void Draw()
    {
        const string pauseText = "Game Paused";
        var size = _font.MeasureString(pauseText);
        var pos = new Vector2((_vp.Width  - size.X) / 2, (_vp.Height - size.Y) / 2);
        
        _sb.Draw(_pixel, destinationRectangle: new Rectangle(0, 0, _vp.Width, _vp.Height), color: new Color(0, 0, 0, 0.5f));
        
        _sb.DrawString(_font, pauseText, pos, Color.Yellow, rotation: 0f, origin: Vector2.Zero, scale: 1f, effects: SpriteEffects.None, layerDepth: 0f);
    }
}
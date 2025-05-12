using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace lasthope.Entities
{
    public class Animation
    {
        public List<Rectangle> Frames { get; }
        public float FrameTime { get; }
        public bool IsLooping { get; }
        public Animation(List<Rectangle> frames, float frameTime, bool isLooping = true)
        {
            Frames = frames;
            FrameTime = frameTime;
            IsLooping = isLooping;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace _999AD
{
    class Animation
    {
        #region DECLARATIONS
        int totalFrames; //number of frames for the animation
        Rectangle[] sourceRectangles;
        int currentFrame=0;
        float timePerFrame;
        float elapsedFrameTime = 0;
        bool loop; //false if the animation should only be played once
        bool keepLastFrameWhenInactive; //if true, the last frame is displayed when the animation ends 
        bool active= true; //becomes false when the animations ends
        #endregion
        #region CONSTRUCTOR
        public Animation(Rectangle _frameLocationOnSpritesheet,
                        int _frameWidth, int _frameHeight, int _totalFrames,
                        float _timePerFrame, bool _loop, bool _keepLastFrameWhenInactive = false)
        {
            totalFrames = _totalFrames;
            timePerFrame = _timePerFrame;
            loop = _loop;
            keepLastFrameWhenInactive = _keepLastFrameWhenInactive;
            sourceRectangles = new Rectangle[totalFrames];
            for (int i = 0; i < totalFrames; i++)
            {
                sourceRectangles[i] = (new Rectangle(_frameLocationOnSpritesheet.X + _frameWidth * (i % (_frameLocationOnSpritesheet.Width / _frameWidth)),
                                                   _frameLocationOnSpritesheet.Y + _frameHeight * (i / (_frameLocationOnSpritesheet.Width / _frameWidth)),
                                                    _frameWidth,
                                                    _frameHeight));
            }
        }
        public Animation(Rectangle[] _sourceRectangles,
                        float _timePerFrame, bool _loop, bool _keepLastFrameWhenInactive = false)
        {
            sourceRectangles = _sourceRectangles;
            totalFrames = sourceRectangles.Length;
            timePerFrame = _timePerFrame;
            loop = _loop;
            keepLastFrameWhenInactive = _keepLastFrameWhenInactive;
        }
        #endregion
        #region PROPERTIES
        public Rectangle Frame
        {
            get { return sourceRectangles[currentFrame]; }
        }
        #endregion
        #region METHODS
        public void Update(float elapsedTime)
        {
            if (!active)
                return;
            elapsedFrameTime += elapsedTime;
            if (elapsedFrameTime >= timePerFrame)
            {
                elapsedFrameTime = 0f;
                currentFrame = (currentFrame + 1) % totalFrames;
                if (!loop && currentFrame == 0)
                {
                    active = false;
                    if (keepLastFrameWhenInactive)
                        currentFrame = totalFrames - 1;
                }
            }
        }
        public float TotalTime()
        {
            return timePerFrame * sourceRectangles.Length;
        }
        public void Reset()
        {
            elapsedFrameTime = 0;
            currentFrame = 0;
        }
        #endregion
    }
}
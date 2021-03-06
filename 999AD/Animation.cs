﻿using Microsoft.Xna.Framework;

namespace _999AD
{
    class Animation
    {
        #region DECLARATIONS
        int totalFrames; //number of frames for the animation
        Rectangle[] sourceRectangles;
        int currentFrame;
        int previousFrame;
        float timePerFrame;
        float elapsedFrameTime;
        bool loop; //false if the animation should only be played once
        bool keepLastFrameWhenInactive; //if true, the last frame is displayed when the animation ends 
        bool active; //becomes false when the animations ends
        #endregion
        #region CONSTRUCTOR
        public Animation(Rectangle _frameLocationOnSpritesheet,
                        int _frameWidth, int _frameHeight, int _totalFrames,
                        float _timePerFrame, bool _loop, bool _keepLastFrameWhenInactive = false)
        {
            currentFrame = 0;
            previousFrame = 0;
            elapsedFrameTime = 0;
            active = true;
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
            currentFrame = 0;
            elapsedFrameTime = 0;
            active = true;
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
        public bool Active
        {
            get { return active; }
        }
        public float TotalTime
        {
            get { return timePerFrame * sourceRectangles.Length; }
        }
        public int FrameIndex
        {
            get { return currentFrame; }
        }
        public int FreviousFrameIndex
        {
            get { return previousFrame; }
        }
        #endregion
        #region METHODS
        public void Update(float elapsedTime)
        {
            if (!active)
                return;
            previousFrame = currentFrame;
            elapsedFrameTime += elapsedTime;
            if (elapsedFrameTime >= timePerFrame)
            {//if the time between two frames has elapsed, change frame
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
        public bool IsLastFrame()
        {
            return currentFrame == totalFrames - 1;
        }
        public void Reset()
        {
            elapsedFrameTime = 0;
            currentFrame = 0;
            active = true;
        }
        public Animation DeepCopy()
        {
            Animation newAnimation = new Animation(sourceRectangles, timePerFrame, loop, keepLastFrameWhenInactive);
            return newAnimation;
        }
        #endregion
    }
}
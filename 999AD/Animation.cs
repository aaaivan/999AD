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
        Texture2D spritesheet;  //spritesheet with all the frames. Can contain different animations.
        Rectangle framesLocationOnSpritesheet; //rectangle containing all and only the frames of one animation
        int frameWidth;
        int frameHeight;
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
        public Animation(Texture2D _spritesheet, Rectangle _frameLocationOnSpritesheet,
                        int _frameWidth, int _frameHeight, int _totalFrames,
                        float _timePerFrame, bool _loop, bool _keepLastFrameWhenInactive=false)
        {
            spritesheet = _spritesheet;
            framesLocationOnSpritesheet = _frameLocationOnSpritesheet;
            frameHeight = _frameHeight;
            frameWidth = _frameWidth;
            totalFrames = _totalFrames;
            timePerFrame = _timePerFrame;
            loop = _loop;
            keepLastFrameWhenInactive = _keepLastFrameWhenInactive;
            sourceRectangles = new Rectangle[totalFrames];
            for (int i=0; i<totalFrames; i++)
            {
                sourceRectangles[i]=(new Rectangle(framesLocationOnSpritesheet.X+ frameWidth * (i % (framesLocationOnSpritesheet.Width / frameWidth)),
                                                   framesLocationOnSpritesheet.Y+ frameHeight * (i / (framesLocationOnSpritesheet.Width / frameWidth)),
                                                    frameWidth,
                                                    frameHeight));
            }
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
        #endregion
    }
}
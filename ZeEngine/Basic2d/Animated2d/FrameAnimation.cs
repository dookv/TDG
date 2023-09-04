#region Includes
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;

using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
#endregion

namespace ZeEngine
{
    public class FrameAnimation
    {
        public bool hasFired;
        public int frames, currentFrame, maxPasses, currentPass, fireFrame;//maxpasses(the amount of times the animation gets fired), 
        public string name;
        public Vector2 sheet, startFrame, sheetFrame, spriteDims;
        public McTimer frameTimer;

        public PassObject FireAction;


        public FrameAnimation(Vector2 SpriteDims, Vector2 sheetDims, Vector2 start, int totalframes, int timePerFrame, int MAXPASSES, string NAME = "")
        {
            spriteDims = SpriteDims;
            sheet = sheetDims;
            startFrame = start;
            sheetFrame = new Vector2(start.X, start.Y);//the same as the start frame
            frames = totalframes;
            currentFrame = 0;
            frameTimer = new McTimer(timePerFrame);//how long each frame shows, animations are 30fps, you could slow it down to 15, this is in ms, so 66 ms is 15 fps
            maxPasses = MAXPASSES;//0 to make it run forever
            currentPass = 0;
            name = NAME;
            FireAction = null;//what function to call when you reached a ceratin point, for example when you swing a sword, midway trough you want it to fire attack function
            hasFired = false;

            fireFrame = 0;
        }

        public FrameAnimation(Vector2 SpriteDims, Vector2 sheetDims, Vector2 start, int totalframes, int timePerFrame, int MAXPASSES, int FIREFRAME, PassObject FIREACTION, string NAME = "")//overloaded constructor, you have the option to pass in a fireaction delgate
        {
            spriteDims = SpriteDims;
            sheet = sheetDims;
            startFrame = start;
            sheetFrame = new Vector2(start.X, start.Y);
            frames = totalframes;
            currentFrame = 0;
            frameTimer = new McTimer(timePerFrame);
            maxPasses = MAXPASSES;
            currentPass = 0;
            name = NAME;
            FireAction = FIREACTION;
            hasFired = false;

            fireFrame = FIREFRAME;//choose a frame to fire an action on
        }

        #region Properties
        public int Frames
        {
            get { return frames; }
        }
        public int CurrentFrame
        {
            get { return currentFrame; }
        }

        public int CurrentPass//readonly prop
        {
            get
            {
                return currentPass;
            }
        }
        public int MaxPasses//readonly prop
        {
            get
            {
                return maxPasses;
            }
        }

        #endregion

        public void Update()
        {

            if (frames > 1)
            {
                frameTimer.UpdateTimer();
                if (frameTimer.Test() && (maxPasses == 0 || maxPasses > currentPass))
                {
                    currentFrame++;
                    if (currentFrame >= frames)
                    {
                        currentPass++;
                    }
                    if (maxPasses == 0 || maxPasses > currentPass)
                    {
                        sheetFrame.X += 1;

                        if (sheetFrame.X >= sheet.X)//if we have 4 sheetframes, when it gets to 5 we want it to loop back to the first frame
                                                    //(also we add 1 sheetframe to y because if we have a sheet with 2x4 frames we want it to continue on the second row)
                        {
                            sheetFrame.X = 0;
                            sheetFrame.Y += 1;
                        }
                        if (currentFrame >= frames)
                        {
                            currentFrame = 0;
                            hasFired = false;
                            sheetFrame = new Vector2(startFrame.X, startFrame.Y);
                        }
                    }
                    frameTimer.Reset();
                }
            }

            if (FireAction != null && fireFrame == currentFrame && !hasFired)//fire the delgate function if it is present for that frame
            {
                FireAction(null);
                hasFired = true;
            }
        }

        public void Reset()//reset all the basic variables
        {
            currentFrame = 0;
            currentPass = 0;
            sheetFrame = new Vector2(startFrame.X, startFrame.Y);
            hasFired = false;
        }

        public bool IsAtEnd()//after you swing a sword, you want to go back to the standing animation, somewhere in your code you ant to be testing for the end of your swing animation this
        {
            if (currentFrame + 1 >= frames)
            {
                return true;
            }
            return false;
        }


        public void Draw(Texture2D myModel, Vector2 dims, Vector2 imageDims, Vector2 screenShift, Vector2 pos, float ROT, Color color, SpriteEffects spriteEffect)
        {
            Globals.spriteBatch.Draw(myModel, new Rectangle((int)((pos.X + screenShift.X)), (int)((pos.Y + screenShift.Y)), (int)Math.Ceiling(dims.X), (int)Math.Ceiling(dims.Y)), new Rectangle((int)(sheetFrame.X * imageDims.X), (int)(sheetFrame.Y * imageDims.Y), (int)imageDims.X, (int)imageDims.Y), color, ROT, imageDims / 2, spriteEffect, 0);
        }

    }



}

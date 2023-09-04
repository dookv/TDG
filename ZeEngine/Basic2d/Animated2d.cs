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
    public class Animated2d : Basic2d
    {

        public Vector2 frames;//each frame of the animated sprite sheet
        public List<FrameAnimation> frameAnimationList = new List<FrameAnimation>();//list of the animations, as many as you need, we name them all with a name
        public Color color;
        public bool frameAnimations;//allows you to use Animated2d just like basic2d, set this to true to enable animations for that sprite (hero example)
        public int currentAnimation = 0;//itteration of the current animation we are running

        public Animated2d(string PATH, Vector2 POS, Vector2 DIMS, Vector2 FRAMES, Color COLOR) : base(PATH, POS, DIMS)
        {
            Frames = new Vector2(FRAMES.X, FRAMES.Y);

            color = COLOR;
        }

        #region Properties
        public Vector2 Frames//properties always have a return type, in this case Vector2
        {
            set//set value to prop
            {
                frames = value;//value is what is coming in to the set, this is required
                if (myModel != null)
                {
                    frameSize = new Vector2(myModel.Bounds.Width / frames.X, myModel.Bounds.Height / frames.Y);
                }
            }
            get//return the value, in this case we are not changing the incoming value
            {
                return frames;
            }
        }
        #endregion


        public override void Update(Vector2 OFFSET)
        {
            if (frameAnimations && frameAnimationList != null && frameAnimationList.Count > currentAnimation)
            {
                frameAnimationList[currentAnimation].Update();//target the update from the frameAnimation List<>
            }

            base.Update(OFFSET);
        }

        public virtual void CreatePerFrameAnimations()
        {
            for (int j = 0; j < Frames.Y; j++)
            {
                for (int i = 0; i < Frames.X; i++)
                {
                    frameAnimationList.Add(new FrameAnimation(new Vector2(frameSize.X, frameSize.Y), new Vector2(1, 1), new Vector2(i, j), 1, 100, 0, ""));//66 ms is 15 frames per second
                }
            }
        }

        public virtual int GetAnimationFromName(string ANIMATIONNAME)//loop trough the animation list and get the animation by name
        {
            for (int i = 0; i < frameAnimationList.Count; i++)
            {
                if (frameAnimationList[i].name == ANIMATIONNAME)
                {
                    return i;
                }
            }

            return -1;//-1 means null in this case
        }

        public virtual void SetAnimationByName(string NAME)
        {
            int tempAnimation = GetAnimationFromName(NAME);

            if (tempAnimation != -1)
            {
                if (tempAnimation != currentAnimation)
                {
                    frameAnimationList[tempAnimation].Reset();
                }

                currentAnimation = tempAnimation;

            }
        }

        public override void Draw(Vector2 screenShift)
        {

            if (frameAnimations && frameAnimationList[currentAnimation].Frames > 0)
            {
                //Globals.spriteBatch.Draw(myModel, new Rectangle((int)(pos.X+screenShift.X), (int)(pos.Y+screenShift.Y), (int)dims.X, (int)dims.Y), new Rectangle((int)((currentFrame.X-1)*dims.X), (int)((currentFrame.Y-1)*dims.Y), (int)(currentFrame.X*dims.X), (int)(currentFrame.Y*dims.Y)), color, rot, new Vector2(myModel.Bounds.Width/2, myModel.Bounds.Height/2), new SpriteEffects(), 0);
                frameAnimationList[currentAnimation].Draw(myModel, dims, frameSize, screenShift, pos, rot, color, new SpriteEffects());

            }
            else
            {
                base.Draw(screenShift);
            }
        }


    }

}

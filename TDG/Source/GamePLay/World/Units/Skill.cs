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
using ZeEngine;
#endregion
namespace TDG
{
    public class Skill
    {
        protected bool active;
        public bool done;
        public Effect2d targetEffect;
        public Animated2d icon;
        public AttackableOBject owner;
        public int selectionType;

        public Skill(AttackableOBject OWNER) //somethng that effects other somethings, could be a sword swing, 
        {
            selectionType = 1;
            active = false;
            done = false;
            targetEffect = new TargetingCircle(new Vector2(0, 0), new Vector2(160, 160));
            owner = OWNER;
        }

        #region Properties

        public bool Active//properties are kind of in between functions and variables, you can only pass values into properties no input parameters
        {
            get 
            {
                return active;
            }
            set
            {
                if (value && !active && targetEffect != null)
                {
                    targetEffect.done = false;
                    GameGlobals.PassEffect(targetEffect);
                }
                active = value;//value is the passed in value?
            }
        }


        #endregion


        public virtual void Update(Vector2 OFFSET, Player ENEMY)// a skill is not the thing that you see, what you see is an animation, or an effect or projjectile or whatever, the skill should only show icon or smtn(not sure)
        {
            if (active && !done)
            {
                Targeting(OFFSET, ENEMY);//targeting() override in example Flamewave

            }


        }

        public virtual void Reset()
        {
            active = false;//Active??
            done = false;
        }


        public virtual void Targeting(Vector2 OFFSET, Player ENEMY)//just in case you forget to set targeting for a slill that inherrits this class
        {
            if (Globals.mouse.LeftClickRelease())
            {
                /*Reset();*/
                Active = false;//use properties where you can
                done = false;

            }

        }
    }
}

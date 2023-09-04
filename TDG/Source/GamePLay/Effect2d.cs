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
    public class Effect2d : Animated2d
    {
        public bool done, noTimer;//you dont edit his, stay protected
        public McTimer timer;


        public Effect2d(string PATH, Vector2 POS, Vector2 DIMS, Vector2 FRAMES, int MSEC) 
            : base(PATH, POS, DIMS, FRAMES, Color.White)
        {
            done = false;
            noTimer = false;//not have the effect finish based on the timer
            timer = new McTimer(MSEC);
        }

        public override void Update(Vector2 OFFSET)//override?
        {
            timer.UpdateTimer();

            if (timer.Test() && !noTimer)
            {
                done = true;
            }
            base.Update(OFFSET);
        }


        public override void Draw(Vector2 OFFSET)//overrides the basic2d draw function, so we have to put the offset as an input parameter
        {
            base.Draw(OFFSET);
        }
    }
}

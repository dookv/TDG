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
    public class Message
    {
        public McTimer timer;
        public Vector2 pos, dims;
        public bool done, lockScreen;
        public Color color;
        public TextZone textZone;


        public Message(Vector2 POS, Vector2 DIMS, string MSG, int MSEC, Color COLOR, bool LOCKSCREEN) 
        {
            pos = POS;
            dims = DIMS;
            lockScreen = LOCKSCREEN;//pause the game when message pops up
            color = COLOR;
            done = false;
            textZone = new TextZone(new Vector2(0, 0), MSG, (int)(dims.X * .9), 22, "fonts\\font", color);
            timer = new McTimer(MSEC);
        }


        public virtual void Update()
        {
            timer.UpdateTimer();
            if (timer.Test())
            {
                done = true;
            }
                                                       //Total timer msec      current timer
            textZone.color = color * (float)( .9f * (float)(timer.MSec - (float)timer.Timer) / (float) timer.MSec);//you have the default color, and we multip[ly it times some number that is smaller than one
                                                                                                                  //(float)(timer.MSec - (float)timer.Timer) / (float) timer.MSec, this shrinks as it goes, so the color fades out
        }


        public virtual void Draw()
        {
            textZone.Draw(new Vector2(pos.X - textZone.dims.X / 2, pos.Y));//center the textzone horizontally(X)
        }
    }
}

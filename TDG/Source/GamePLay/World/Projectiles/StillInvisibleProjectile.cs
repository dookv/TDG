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
    //it doasnt move, draw, run in to something and hit
    public class StillInvisibleProjectile : Projectile2d
    {

        float ticks, currentTick;//float because we are going to be multiplying and devinding them, if you were to use and int and were not carefull to cast them properly you end up concattonation your answers
                                //we'll leave these in float so we dont continuously have to cast them ,

        public StillInvisibleProjectile(Vector2 POS, Vector2 DIMS, AttackableOBject OWNER, Vector2 TARGET, int MSEC) 
            : base("2d\\Projectiles\\Bullet", POS, DIMS, OWNER, TARGET)//we dont need PATH and DIMS because we know what the fireball is going to look like and we know its dimensions. So provide hardcoded variables since they dont change in this case
        {
            ticks = 3;
            currentTick = 0;
            timer = new McTimer(MSEC);
            

        } 

        public override void Update(Vector2 OFFSET, List<AttackableOBject> UNITS)
        {
            base.Update(OFFSET, UNITS);//bas update first or last, sometimes in the middle

            //  timer of timer,  whole timer, 
            if (timer.Timer >= timer.MSec * (currentTick/(ticks - 1)))//tick when you launch, middle and end of the projectile timer, devides the timer into 3 ticks
            {
                for (int i = 0; i < UNITS.Count; i++)
                {
                    if (Globals.GetDistance(UNITS[i].pos, pos) <= dims.X/2)//if unit enters the size of the projectile
                    {
                        UNITS[i].GetHit(owner, 1.0f);
                    }
                }
                currentTick++;
            }


        }

        public override void ChangePosition()
        {
            //override the change position here and dont run any code 
        }


        public override bool HitSomething(List<AttackableOBject> UNITS)
        {
            return false;
        }

        public override void Draw(Vector2 OFFSET)//add effects to the projectile for instance, particles
        {
            //dont draw anything
        }
    }
}

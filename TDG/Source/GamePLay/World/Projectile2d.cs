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
    public class Projectile2d : Basic2d
    {
        public bool done;//when projectile is done

        public McTimer timer;//timer for the projectile (lifespan)

        public float speed;//projectile speed

        public Vector2 direction;//direction we are firing

        public AttackableOBject owner;//owner of ther projectile so you dont get hir by your won projectiles




        public Projectile2d(string PATH, Vector2 POS, Vector2 DIMS, AttackableOBject OWNER, Vector2 TARGET) : base(PATH, POS, DIMS)//In this case TARGET is a vector 2 because its goes to a random vector2 as position. If you were to pass in Unit as TARGET, you say that the projectile will always hit a certain target
        {
            done = false;

            speed = 40.0f;

            owner = OWNER;

            direction = TARGET - owner.pos;//if you subtract a vector from another vector, you get a directional vector. Wen multiplied by speed, it will give us where we are moving to

            direction.Normalize();//makes it of length 1. We want the speed(6) to mean something. So if our target is 10 distance away when multiplied by our speed(6) it goes a lot faster then when our target is 5 distance away. This is why we normalize to 1
                                  //we are moving at 6 units (speed), 60 fps == 6 * 60 = 360 pixels

            rot = Globals.RotateTowards(pos, new Vector2(TARGET.X, TARGET.Y));
            timer = new McTimer(1500);//1.5 sec

        }

        public virtual void Update(Vector2 OFFSET, List<AttackableOBject> UNITS)
        {

            ChangePosition();

            timer.UpdateTimer();
            if(timer.Test())
            {
                done = true;
            }
            if(HitSomething(UNITS))
            {
                done = true;
            }
        }


        public virtual void ChangePosition()
        {
            pos += direction * speed;//pos of the projectile gets set here, so we can use pos

        }


        public virtual bool HitSomething(List<AttackableOBject> UNITS)
        {
            for (int i = 0; i < UNITS.Count; i++)
            {
                //it can only hit it if it is not the owner of the projectile, if lets say you have a ball of healing, cahnge != to == so you can only heal
                if (owner.ownerId != UNITS[i].ownerId && Globals.GetDistance(pos, UNITS[i].pos) < UNITS[i].hitDistance)//if the distance from the projectile to the mob (unit) is smaller than the default given hit distance for all units
                {
                    UNITS[i].GetHit(owner, 1);
                    Globals.soundControl.PlaySound("Hit");
                    return true;
                }
            }

            return false;
        }

        public override void Draw(Vector2 OFFSET)//add effects to the projectile for instance, particles
        {

            //in this case this shader does fuck all because i am using SamplerState.PointClamp in this project because of pixel art
            Globals.normalEffect.Parameters["xSize"].SetValue((float)myModel.Bounds.Width);//in the fx file xSize etc is a float, so that is why we cast them to float here, these are already concatonated to int so we dont need to do it
            Globals.normalEffect.Parameters["ySize"].SetValue((float)myModel.Bounds.Height);
            Globals.normalEffect.Parameters["xDraw"].SetValue((float)((int)dims.X));//we concatonate it to an int, because pixel shaders dont deal with partial pixels (float values)
            Globals.normalEffect.Parameters["yDraw"].SetValue((float)((int)dims.Y));
            Globals.normalEffect.Parameters["filterColor"].SetValue(Color.White.ToVector4());//in the fx file filterColor is a float4 which is a vector4
            Globals.normalEffect.CurrentTechnique.Passes[0].Apply();//this sline sets all the effects

            base.Draw(OFFSET);
        }
    }
}

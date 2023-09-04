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
    public class FlamewaveProjectile : StillInvisibleProjectile
    {

       

        public FlamewaveProjectile(Vector2 POS, AttackableOBject OWNER, Vector2 TARGET, int MSEC) 
            : base(POS, new Vector2(160,160), OWNER, TARGET, MSEC)//we dont need PATH and DIMS because we know what the fireball is going to look like and we know its dimensions. So provide hardcoded variables since they dont change in this case
        {

            GameGlobals.PassEffect(new FlameCircle(new Vector2(POS.X, POS.Y), new Vector2(dims.X, dims.Y), MSEC));//targetEffect.dims.X, targetEffect.dims.Y use this for the dims of the skill
                                                                                                                                               //new FlameCircle(Globals.mouse.newMousePos - OFFSET, new Vector2(targetEffect.dims.X, targetEffect.dims.Y)),
        }

        public override void Update(Vector2 OFFSET, List<AttackableOBject> UNITS)
        {

            base.Update(OFFSET, UNITS);
        }

        public override void ChangePosition()
        {
            
        }


        public override bool HitSomething(List<AttackableOBject> UNITS)
        {
            return false;
        }

        public override void Draw(Vector2 OFFSET)//add effects to the projectile for instance, particles
        {
           
        }
    }
}

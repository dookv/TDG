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
    public class FlameWave : Skill
    {



        public FlameWave(AttackableOBject OWNER) 
            : base(OWNER)//base is not needeed here bc  Skill constructor doesnt take any input parameters, but just for later maybe
        {
            icon = new Animated2d("2d\\UI\\FireIcon", new Vector2(0, 0), new Vector2(40, 40), new Vector2(1, 1), Color.White);
        }

        public override void Targeting(Vector2 OFFSET, Player ENEMY)
        {
            if (Globals.mouse.LeftClickRelease())
            {
                //FlameCircle flamez = new FlameCircle(Globals.mouse.newMousePos - OFFSET, new Vector2(160, 160));
                targetEffect.done = true;
                GameGlobals.PassProjectile(new FlamewaveProjectile(Globals.mouse.newMousePos - OFFSET, owner, new Vector2(0, 0), 1500));//targetEffect.dims.X, targetEffect.dims.Y use this for the dims of the skill
                                               //new FlameCircle(Globals.mouse.newMousePos - OFFSET, new Vector2(targetEffect.dims.X, targetEffect.dims.Y)),
                done = true;
                active = false;


/*                for (int i = 0; i < ENEMY.units.Count; i++)
                {
                    if (Globals.GetDistance(ENEMY.units[i].pos, Globals.mouse.newMousePos - OFFSET) <= targetEffect.dims.X/2)//radius targetEffect(center to ouline, diameter = left to right on X (aka width))
                    {
                        ENEMY.units[i].GetHit(1);
                    }
                }*/
            }
            else
            {
                targetEffect.pos = Globals.mouse.newMousePos - OFFSET;
                
            }
            
        }

    }
}

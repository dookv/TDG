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
    public class Arrow : Projectile2d
    {

        public Arrow(Vector2 POS, AttackableOBject OWNER, Vector2 TARGET) 
            : base("2d\\Projectiles\\Arrow", POS, new Vector2(15,15), OWNER, TARGET)//we dont need PATH and DIMS because we know what the fireball is going to look like and we know its dimensions. So provide hardcoded variables since they dont change in this case
        {
            speed = 15.0f;
            timer = new McTimer(1500);
            

        } 

        public override void Update(Vector2 OFFSET, List<AttackableOBject> UNITS)
        {
            base.Update(OFFSET, UNITS);
        }


        public override void Draw(Vector2 OFFSET)//add effects to the projectile for instance, particles
        {
            base.Draw(OFFSET);
        }
    }
}
